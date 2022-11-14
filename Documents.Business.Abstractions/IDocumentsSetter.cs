using Documents.Core;

namespace Documents.Business.Abstractions;

public interface IDocumentsSetter
{
    ValueTask InsertAsync(Document document);
    ValueTask UpdateAsync(Document document);
}
