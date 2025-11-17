namespace Microsoft.Extensions.DependencyInjection;
public static class DependencyContainer
{
    public static IServiceCollection AddGeoService(this IServiceCollection services)
    {
        services.AddScoped<IGeolocationService, GeolocationService>();
        return services;
    }
}