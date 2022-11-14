using Documents.Business.Abstractions;
using Documents.Core;

using Microsoft.AspNetCore.Mvc;

using Repository.Business;

namespace Documents.API;

[ApiController]
[Route("[controller]")]
public class DocumentsController
{
    private static Document Map(DocumentModel document) => new()
    {
        Id = DocumentId.Parse(document.Id),
        Tags = document.Tags ?? Enumerable.Empty<string>(),
        Data = document.Data ?? string.Empty
    };

    [HttpGet("/{documentId}")]
    public ValueTask<Document> Get(string documentId,
        [FromServices] IDocumentsProvider documentsProvider)
    {
        return documentsProvider.GetByIdAsync(DocumentId.Parse(documentId));
    }

    [HttpPost("/")]
    public ValueTask Post(
        DocumentModel document,
        [FromServices] IDocumentsSetter documentsSetter)
    {
        return documentsSetter.InsertAsync(Map(document));
    }

    [HttpPut("/")]
    public ValueTask Put(
        DocumentModel document,
        [FromServices] IDocumentsSetter documentsSetter)
    {
        return documentsSetter.UpdateAsync(Map(document));
    }
}
