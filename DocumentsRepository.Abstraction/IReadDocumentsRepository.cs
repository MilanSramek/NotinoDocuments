using Documents.Core;

namespace Documents.Repository.Abstractions;

public interface IReadDocumentsRepository
{
    ValueTask<Document> GetByIdAsync(DocumentId id);
}