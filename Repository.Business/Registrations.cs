using Documents.Business.Abstractions;

using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Repository.Business;

namespace Documents.Business;

public static class Registrations
{
    public static IServiceCollection AddBusiness(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        services
            .AddScoped<IDocumentsProvider, DocumentsProvider>()
            .AddScoped<IDocumentsSetter, DocumentsSetter>()
            .AddScoped<IDocumentsCache, DocumentsCache>()
            .AddSingleton<IMemoryCache, MemoryCache>();

        services.AddOptions<DocumentsCacheOptions>()
           .Bind(configuration.GetSection(DocumentsCacheOptions.Section))
           .ValidateDataAnnotations();

        return services;
    }
}
