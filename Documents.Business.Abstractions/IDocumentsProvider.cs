using Documents.Core;

namespace Repository.Business;

public interface IDocumentsProvider
{
    ValueTask<Document> GetByIdAsync(DocumentId id);
}
