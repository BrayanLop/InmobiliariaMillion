using InmobiliariaMillion.Aplicacion.Servicios.Interfaces;
using InmobiliariaMillion.Application.Servicios;
using InmobiliariaMillion.Dominio.Interfaces.Repositorios;
using InmobiliariaMillion.Infrastructura.Configuration;
using InmobiliariaMillion.Infrastructura.Interfaces;
using InmobiliariaMillion.Infrastructura.Repositorio;
using InmobiliariaMillion.Infrastructura.Servicios;
using InmobiliariaMillion.Infrastructure.Repositorio;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace InmobiliariaMillion.Infrastructura
{
    public static class InyeccionDependencias
    {
        public static IServiceCollection AddInfrastructura(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            var databaseName = configuration["MongoDB:DatabaseName"];
            
            services.AddSingleton<IMongoDatabase>(provider =>
                MongoDbConfiguracion.ConfigurarBaseDatos(connectionString, databaseName)
            );
            
            services.AddScoped<IPropiedadRepository, PropiedadRepository>();
            services.AddScoped<IPropietarioRepository, PropietarioRepository>();
            services.AddScoped<IImagenPropiedadRepository, ImagenPropiedadRepository>();
            services.AddScoped<ITrazabilidadPropiedadRepository, TrazabilidadPropiedadRepository>();
            services.AddScoped<IArchivoServicio, ArchivoServicio>();

            return services;
        }

        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IPropiedadServicio, PropiedadServicio>();
            services.AddScoped<IPropietarioServicio, PropietarioServicio>();
            services.AddScoped<IImagenPropiedadServicio, ImagenPropiedadServicio>();
            services.AddScoped<ITrazabilidadPropiedadServicio, TrazabilidadPropiedadServicio>();

            return services;
        }
    }
}