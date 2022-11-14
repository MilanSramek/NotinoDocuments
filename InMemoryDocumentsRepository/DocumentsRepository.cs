using Documents.Core;
using Documents.Repository.Abstractions;

using Microsoft.Extensions.Logging;

using System.Collections.Concurrent;

namespace Documents.Repository.InMemory;

public class InMemoryDocumentsRepository : 
    IReadDocumentsRepository,
    IWriteDocumentsRepository
{
    private readonly ConcurrentDictionary<DocumentId, Document> _documents = new();
    private readonly ILogger _logger;

    public InMemoryDocumentsRepository(ILogger<InMemoryDocumentsRepository> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public ValueTask<Document> GetByIdAsync(DocumentId id)
    {
        if (_documents.TryGetValue(id, out Document? document))
        {
            _logger.LogDebug("Document with Id={id} returned.", id);
            return ValueTask.FromResult(document);
        }

        _logger.LogError("Requested document with Id={id} not found.", id);
        throw new ApplicationException($"Document with Id={id} does not exists.");
    }

    public ValueTask InsertAsync(Document document)
    {
        ArgumentNullException.ThrowIfNull(document);

        if (_documents.TryAdd(document.Id, document))
        {
            _logger.LogDebug("Document with Id={id} inserted.", document.Id);
            return ValueTask.CompletedTask;
        }

        _logger.LogError("Document with Id={id} cannot be inserted due to Id conflict.", document.Id);
        throw new ApplicationException($"Document with Id={document.Id} already exists.");
    }

    public ValueTask UpdateAsync(Document document)
    {
        ArgumentNullException.ThrowIfNull(document);

        if (_documents.TryGetValue(document.Id, out Document? currentDocument)
            && _documents.TryUpdate(document.Id, document, currentDocument))
        {
            _logger.LogDebug("Document with Id={id} updated.", document.Id);
            return ValueTask.CompletedTask;
        }

        _logger.LogError("Document with Id={id} cannot be update because it does not exists.", document.Id);
        throw new ApplicationException($"Document with Id={document.Id} doest not exists.");
    }
}