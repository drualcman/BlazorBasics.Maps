namespace Microsoft.Extensions.DependencyInjection;
public static class DependencyContainer
{
    public static IServiceCollection AddGeoService(this IServiceCollection services) =>
        services.AddGeoService(null);

    public static IServiceCollection AddGeoService(this IServiceCollection services,
        Action<GeolocationRequestOptions> setupOptions)
    {
        GeolocationRequestOptions options = new();
        setupOptions?.Invoke(options);

        services.AddSingleton(options);
        services.AddScoped<IExtendedGeolocationService, GeolocationService>();
        services.AddScoped<IGeolocationService>(provider =>
            provider.GetRequiredService<IExtendedGeolocationService>());
        return services;
    }
}
