# URL Shortener — Clean Architecture · .NET 8 · SQL Server · Analytics

A production-ready URL shortening service built with **Clean Architecture**, **CQRS**, and **Vertical Slicing** in ASP.NET Core 8. Tracks every click with geo-location, device, and browser data, and exposes analytics reports per user.

![URL Shortener](https://i.ibb.co/T0MNNnn/url-shortener.png)

---

## Table of Contents

- [Features](#features)
- [Architecture](#architecture)
- [Project Structure](#project-structure)
- [Technology Stack](#technology-stack)
- [Database Schema](#database-schema)
- [API Reference](#api-reference)
- [Analytics & Reporting](#analytics--reporting)
- [Design Patterns](#design-patterns)
- [Getting Started](#getting-started)
- [Running Tests](#running-tests)

---

## Features

- **URL Shortening** — generates unique 6-character base-62 short codes
- **Click Tracking** — records IP address, country, device type, and browser for each visit
- **Anti-abuse** — limits each IP to 4 tracked visits per link per day
- **Geo-location** — resolves country from IP via [ipinfo.io](https://ipinfo.io)
- **Device & Browser Detection** — parses User-Agent strings with UAParser
- **Analytics Reports** — top links, top countries, top devices, top browsers, and daily click aggregations
- **User Management** — create, update, and delete users; each link is associated with a user
- **Paginated Endpoints** — all list endpoints support page/pageSize query parameters
- **Swagger UI** — interactive API documentation included

---

## Architecture

The solution follows **Clean Architecture** with a **Vertical Slicing** folder structure inside the Application layer. Dependencies flow strictly inward: the Domain layer has zero external dependencies.

```
┌─────────────────────────────────────────────┐
│                  API Layer                  │  ← Controllers, Program.cs
├─────────────────────────────────────────────┤
│            Application Layer               │  ← CQRS Handlers, DTOs, Services
├─────────────────────────────────────────────┤
│           Infrastructure Layer             │  ← EF Core, Repositories, Migrations
├─────────────────────────────────────────────┤
│              Domain Layer                  │  ← Entities, Enums, Repository Interfaces
└─────────────────────────────────────────────┘
```

**Dependency rule:** API → Application → Domain ← Infrastructure

---

## Project Structure

```
ShortenedLinks.sln
│
├── ShortenedLinks.Domain/
│   ├── Entities/
│   │   ├── User.cs
│   │   ├── Link.cs
│   │   └── LinkStatistic.cs
│   ├── Enums/
│   │   └── PeriodType.cs          (Day | Week | Month)
│   └── Interfaces/Repositories/
│       ├── IGenericRepository<T>.cs
│       ├── ILinkRepository.cs
│       ├── IUserRepository.cs
│       └── ILinkStatisticRepository.cs
│
├── ShortenedLinks.Application/
│   ├── Features/                   (Vertical Slicing)
│   │   ├── Links/
│   │   │   ├── Commands/           (CreateLink, DeleteLink)
│   │   │   └── Queries/            (GetAllLinks, GetByIdLink)
│   │   ├── Users/
│   │   │   ├── Commands/           (CreateUser, UpdateUser, DeleteUser)
│   │   │   └── Queries/            (GetAllUsers, GetByIdUser)
│   │   ├── LinksStatistics/
│   │   │   ├── Commands/           (RegisterLinkStatistic)
│   │   │   └── Queries/            (GetTopBrowsers, GetTopCountries,
│   │   │                            GetTopDevices, GetTopLinks,
│   │   │                            GetMonthlyClicksByDay)
│   │   └── ShortLink/
│   │       └── Queries/            (GetByShortLink)
│   ├── DTO/                        (per-entity DTOs)
│   ├── Interfaces/                 (service contracts)
│   ├── Services/                   (LinkShortener, Validation, GeoLocation, DeviceInfo)
│   └── Mapper/
│       └── AutoMapperProfile.cs
│
├── ShortenedLinks.Infrastructure/
│   ├── Persistence/
│   │   └── ShortenedLinksDbContext.cs
│   ├── Repositories/               (Generic, Link, User, LinkStatistic)
│   ├── Migrations/
│   └── IoC/
│       └── Dependencies.cs
│
├── ShortenedLinks.API/
│   ├── Controllers/
│   │   ├── LinkController.cs
│   │   ├── UserController.cs
│   │   ├── ShortLinkController.cs
│   │   └── LinkStats.cs
│   └── Program.cs
│
└── ShortenedLinks.Tests/
    └── ApplicationTest/Features/
        ├── LinksTest/
        ├── LinksStatisticTest/
        └── ShortLinkTest/
```

---

## Technology Stack

| Category | Technology | Version |
|---|---|---|
| Runtime | .NET / ASP.NET Core | 8.0 |
| ORM | Entity Framework Core | 8.0.8 |
| Database | SQL Server | 2019+ |
| CQRS / Mediator | MediatR | 12.4.0 |
| Object Mapping | AutoMapper | 12.0.1 |
| API Docs | Swashbuckle / Swagger | 6.4.0 |
| User-Agent Parsing | UAParser | 3.1.47 |
| Unit Testing | xUnit | 2.5.3 |
| Mocking | Moq | 4.20.70 |
| Code Coverage | Coverlet | 6.0.0 |
| Geo-location | ipinfo.io API | — |

---

## Database Schema

```
Users
├── Id          int PK
├── Email       nvarchar (unique)
└── Username    nvarchar (unique)

Links
├── Id             int PK
├── OriginalLink   nvarchar
├── ShortenedLink  nvarchar (unique)
├── CreatedAt      datetime2
└── UserId         int FK → Users.Id

LinkStatistics
├── Id          int PK
├── LinkId      int FK → Links.Id (cascade delete)
├── VisitDate   datetime2
├── VisitorIp   nvarchar
├── Country     nvarchar
├── Device      nvarchar
└── Browser     nvarchar
```

---

## API Reference

### Links — `/api/link`

| Method | Endpoint | Description |
|---|---|---|
| `GET` | `/api/link?page=1&pageSize=10` | Paginated list of links with usernames |
| `GET` | `/api/link/{id}` | Single link details |
| `POST` | `/api/link` | Create a shortened link |
| `DELETE` | `/api/link/{id}` | Delete a link |

**POST `/api/link` — request body**
```json
{
  "originalLink": "https://example.com/very/long/path",
  "userId": 1
}
```

**GET `/api/link` — response**
```json
{
  "status": true,
  "value": [
    {
      "id": 1,
      "originalLink": "https://example.com/very/long/path",
      "shortenedLink": "lqee02",
      "createdAt": "21/08/2024"
    }
  ]
}
```

---

### Short Link Redirect — `/api/shortlink`

| Method | Endpoint | Description |
|---|---|---|
| `GET` | `/api/shortlink/{code}` | Resolve short code → original URL and register the visit |

This endpoint does two things atomically:
1. Returns the original URL so the client can redirect.
2. Records the visit (IP, country, device, browser) if the IP has fewer than 4 visits today.

**Response**
```json
{
  "status": true,
  "value": {
    "id": 1,
    "originalLink": "https://example.com/very/long/path",
    "shortenedLink": "lqee02",
    "userId": 1,
    "username": "john_doe"
  }
}
```

![Short Link](https://i.ibb.co/VJRzTzh/image.png)

---

### Users — `/api/user`

| Method | Endpoint | Description |
|---|---|---|
| `GET` | `/api/user?page=1&pageSize=10` | Paginated user list |
| `GET` | `/api/user/{id}` | Single user |
| `POST` | `/api/user` | Create user |
| `PUT` | `/api/user/{id}` | Update user |
| `DELETE` | `/api/user/{id}` | Delete user |

---

### Link Statistics — `/api/linkstats`

| Method | Endpoint | Description |
|---|---|---|
| `GET` | `/api/linkstats/toplinks/{userId}?period=Month` | Top links by click count |
| `GET` | `/api/linkstats/monthlyclicks/{userId}` | Clicks grouped by day for the current month |
| `GET` | `/api/linkstats/topdevices/{userId}` | Top devices (monthly) |
| `GET` | `/api/linkstats/topbrowsers/{userId}` | Top browsers (monthly) |
| `GET` | `/api/linkstats/topcountries/{userId}` | Top countries (monthly) |

![Analytics](https://i.ibb.co/F75BFWJ/image.png)

---

## Analytics & Reporting

Each visit to a short link stores:

```json
{
  "id": 1,
  "linkId": 3,
  "visitDate": "2024-08-21T14:38:21.93",
  "visitorIp": "189.203.3.245",
  "device": "Desktop",
  "country": "MX",
  "browser": "Chrome"
}
```

Available reports:

| Report | Description |
|---|---|
| **Top Links** | Links ranked by click count for Day / Week / Month |
| **Monthly Clicks** | Daily click totals for the current month |
| **Top Devices** | Device types ranked by monthly click count |
| **Top Browsers** | Browsers ranked by monthly click count |
| **Top Countries** | Countries ranked by monthly click count |

![Links](https://i.ibb.co/q7r0s8b/image.png)

---

## Design Patterns

| Pattern | Where |
|---|---|
| **Clean Architecture** | 4-layer separation (Domain / Application / Infrastructure / API) |
| **CQRS** | MediatR commands & queries in the Application layer |
| **Mediator** | MediatR decouples controllers from handlers |
| **Repository** | `IGenericRepository<T>` + specialised repositories |
| **Dependency Injection** | Built-in .NET DI, configured in `Dependencies.cs` |
| **DTO** | Separate request/response models per feature |
| **AutoMapper** | Centralized mapping profile (`AutoMapperProfile.cs`) |
| **Service Layer** | Domain services for short-code generation, validation, geo-location, device detection |
| **Vertical Slicing** | Feature folders inside Application (Links, Users, LinksStatistics, ShortLink) |

---

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8)
- SQL Server (Express or higher)

### 1 — Clone the repository

```bash
git clone https://github.com/draquio/Shortened-Link-Clean-Architecture-Dot-Net.git
cd Shortened-Link-Clean-Architecture-Dot-Net
```

### 2 — Configure the database connection

Edit `ShortenedLinks.API/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "Connection": "Server=YOUR_SERVER; Database=ShortenedLinksDb; Trusted_Connection=True; TrustServerCertificate=True;"
  }
}
```

### 3 — Apply migrations

```bash
dotnet ef database update --project ShortenedLinks.Infrastructure --startup-project ShortenedLinks.API
```

### 4 — Run the API

```bash
dotnet run --project ShortenedLinks.API
```

### 5 — Open Swagger UI

Navigate to `https://localhost:{port}/swagger` in your browser.

---

## Running Tests

```bash
dotnet test
```

The test suite covers the Application layer using **xUnit** and **Moq**:

| Test Class | Scenarios Covered |
|---|---|
| `CreateLinkCommandTests` | Validation failure, null result, successful creation |
| `GetAllLinksQueryTests` | Paginated results, empty list, database error |
| `RegisterLinkStatisticCommandTests` | Visit registered, IP threshold exceeded, service error |
| `GetByShortLinkQueryTests` | Found, not found, error |
| `GetTopBrowsersQueryTests` | Aggregated results, user not found, error |
| `GetTopCountriesQueryTests` | Aggregated results, user not found, error |
| `GetTopDevicesQueryTests` | Aggregated results, user not found, error |
| `GetTopLinksQueryTests` | Top links by period, user not found, error |
| `GetMonthlyClicksByDayTests` | Daily aggregation, user not found, error |

---

## License

This project is open-source and available under the [MIT License](LICENSE).

---

Developed by **Ing. Sergio Mercado**
