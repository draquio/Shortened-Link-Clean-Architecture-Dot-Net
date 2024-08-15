using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ShortenedLinks.Application.Features.Users.Queries.GetAll;
using ShortenedLinks.Application.Interfaces;
using ShortenedLinks.Application.Mapper;
using ShortenedLinks.Application.Services.DeviceInfo;
using ShortenedLinks.Application.Services.GeoLocation;
using ShortenedLinks.Application.Services.Links;
using ShortenedLinks.Application.Services.Validation;
using ShortenedLinks.Domain.Entities;
using ShortenedLinks.Domain.Interfaces.Repositories;
using ShortenedLinks.Infrastructure.Persistence;
using ShortenedLinks.Infrastructure.Repositories;

namespace ShortenedLinks.Infrastructure.IoC
{
    public static class Dependencies
    {
        public static void InjectDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            // DB
            services.AddDbContext<ShortenedLinksDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("Connection"));
            });

            // HTTP Client
            services.AddHttpClient();

            // Repositories
            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ILinkRepository, LinkRepository>();
            services.AddScoped<ILinkStatisticRepository, LinkStatisticRepository>();

            // Services
            services.AddScoped<LinkShortenerService>();
            services.AddScoped<ValidationService>();
            services.AddScoped<IGeoLocationService, GeoLocationService>();
            services.AddScoped<IDeviceInfoService, DeviceInfoService>();

            // Mapper
            services.AddAutoMapper(typeof(AutoMapperProfile));

            // MediatR
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(
                typeof(GetAllUsersQueryHandler).Assembly
            ));
        }
    }
}
