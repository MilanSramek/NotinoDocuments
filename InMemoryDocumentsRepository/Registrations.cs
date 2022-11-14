using Documents.Repository.Abstractions;

using Microsoft.Extensions.DependencyInjection;

namespace Documents.Repository.InMemory;

public static class Registrations
{
    public static IServiceCollection AddInMemoryDocumentsRepository(
        this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);
        return services
            .AddSingleton<InMemoryDocumentsRepository>()
            .AddSingleton<IReadDocumentsRepository>(provider => 
                provider.GetRequiredService<InMemoryDocumentsRepository>())
             .AddSingleton<IWriteDocumentsRepository>(provider =>
                provider.GetRequiredService<InMemoryDocumentsRepository>());
    }
}
