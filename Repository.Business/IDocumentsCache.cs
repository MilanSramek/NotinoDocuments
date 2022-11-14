using Documents.Core;

using System.Diagnostics.CodeAnalysis;

namespace Documents.Business;

public interface IDocumentsCache
{
    void SetDocument(Document document);
    bool TryGetDocument(DocumentId id, [MaybeNullWhen(false)] out Document? document);
}