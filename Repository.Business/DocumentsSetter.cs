using Documents.Business.Abstractions;
using Documents.Core;
using Documents.Repository.Abstractions;

namespace Documents.Business
{
    internal class DocumentsSetter : IDocumentsSetter
    {
        private readonly IWriteDocumentsRepository _repository;
        private readonly IDocumentsCache _cache;

        public DocumentsSetter(IWriteDocumentsRepository repository,
            IDocumentsCache cache)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        }

        public async ValueTask InsertAsync(Document document)
        {
            ArgumentNullException.ThrowIfNull(document);
            await _repository.InsertAsync(document);
            _cache.SetDocument(document);
        }

        public async ValueTask UpdateAsync(Document document)
        {
            ArgumentNullException.ThrowIfNull(document);
            await _repository.UpdateAsync(document);
            _cache.SetDocument(document);
        }
    }
}
