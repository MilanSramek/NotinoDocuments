using Documents.Business;
using Documents.Core;
using Documents.Repository.Abstractions;

using Microsoft.Extensions.Logging;

namespace Repository.Business;

internal class DocumentsProvider : IDocumentsProvider
{
    private readonly IReadDocumentsRepository _repository;
    private readonly IDocumentsCache _cache;
    private readonly ILogger _logger;

    public DocumentsProvider(
        IReadDocumentsRepository repository,
        IDocumentsCache cache,
        ILogger<DocumentsProvider> logger)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async ValueTask<Document> GetByIdAsync(DocumentId id)
    {
        _logger.LogDebug("Document with Id='{id}' requested.", id);

        if (_cache.TryGetDocument(id, out Document? cachedDocument))
        {
            _logger.LogDebug("Document with Id='{id}' retrieved from cache.", id);
            return cachedDocument!;
        }

        Document document = await _repository.GetByIdAsync(id);
        _logger.LogDebug("Document with Id='{id}' retrieved from repository.", id);

        return document;
    }
}
