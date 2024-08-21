# Shortened Links + Clean Architecture + SQL Server + Analytics

## Características del sistema.
- Implementación de Clean Architecture + estructura de Vertical Slicing architecture.
- Patrón CQRS
- Patrón Mediador
- Patrón de Inyección de dependencias
- Patrón de capa Servicio para la lógica de negocio.
- Patrón Repositorio para el manejo de la base de datos.
- Patrón DTO para crear, listar y actualizar datos (cada modelo tiene sus DTOs).
- AutoMapper para la mapear de manera más eficiente los DTOs a modelos y separar la lógica de mapeo del servicio.
- Interfaces de Servicio y repositorio.
- Cors activada para patición desde cualquier url.
- Testing

![](https://i.ibb.co/T0MNNnn/url-shortener.png)
---

## Generación de Enlaces Cortos
El sistema permite generar nuevos enlaces únicos a través de una petición POST al controlador Link, así también se puede obtener un enlace por ID, por una lista paginada y/o eliminar un enlace por ID.

![](https://i.ibb.co/q7r0s8b/image.png)
---

## Endpoint de enlace corto
El sistema cuenta con un endpoint específico el cual se usa para acceder al enlace corto y redirección a la url original, este endpoint tiene 2 funciones:
1. Recuperar la información del enlace corto mediante su código para redireccionar al enlace original.
```
{
    "id": 1,
    "originalLink": "https://website.com/", <- Enlace al que se debe redirigir
    "shortenedLink": "lqee02",
    "userId": 1,
    "username": "Username"
}
```
2. Registrar la visita/click en el enlace guardando en la base de datos para usarse como analítica.
```
{
    "id": 1,
    "linkId": 3,
    "visitDate": "2024-08-21 14:38:21.9300000",
    "visitorIp": "189.203.3.245",
    "device": "Desktop",
    "country": "MX",
    "browser": "Chrome",
}
```
Para esto, internamente se usa servicios propios para obtener navegador y dispositivo, y servicios externos como ipinfo.io para obtener el país.

![](https://i.ibb.co/VJRzTzh/image.png)
---
# Reporte de visitas/clicks
Éste permite generar reporte de las visitas en los enlaces:
1. Top Links.- Este endpoint permite generar reporte de los enlaces con más clicks del día, semana y/o mes.
2. Monthly Clicks.- Permite generar reporte de estadísticas del mes ordenados por día, es decir cada día con la cantidad de clicks.
3. Top Devices.- Reporte mensual de los dispositivos mas usados para acceder a los enlaces (mayor a menor).
4. Top Browsers.- Reporte mensual de los navegadores mas usados para acceder a los enlaces (mayor a menor).
5. Top Countries.- Reporte mensual de los países desde donde se accedieron los enlaces (mayor a menor).

![](https://i.ibb.co/F75BFWJ/image.png)
---
# Dependencias
- Entity Framework
- Entity Framework SQLServer
- Entity Framework Core
- Entity Framework Tools
- Entity Framework Desing
- AutoMapper
- MediatR
- MediatR dependency injection
- UAParser
- Moq (testing)

Desarrollado por Ing. Sergio Mercado.