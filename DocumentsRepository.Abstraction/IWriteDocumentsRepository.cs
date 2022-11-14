using Documents.Core;

namespace Documents.Repository.Abstractions;

public interface IWriteDocumentsRepository
{
    ValueTask InsertAsync(Document document);
    ValueTask UpdateAsync(Document document);
}
