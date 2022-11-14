using Documents.Core;
using Documents.Repository.Abstractions;

using FluentAssertions;

using Microsoft.Extensions.Logging;

using Repository.Business;

namespace Documents.Business.Tests
{
    public class Tests
    {

        [Test]
        public async Task GetByIdAsync_NoCache()
        {
            Document document = new()
            {
                Id = DocumentId.Parse("id"),
                Tags = Enumerable.Empty<string>(),
                Data = ""
            };
            Mock<IReadDocumentsRepository> repository = new();
            repository
                .Setup(_ => _.GetByIdAsync(It.IsAny<DocumentId>()))
                .Returns<DocumentId>(_ => ValueTask.FromResult(document));

            Mock<IDocumentsCache> cache = new();

            DocumentsProvider sut = new(
                repository.Object,
                cache.Object,
                Mock.Of<ILogger<DocumentsProvider>>());

            Document result = await sut.GetByIdAsync(DocumentId.Parse("id"));

            result.Should().BeSameAs(document);
            repository.Verify(_ => _.GetByIdAsync(It.IsAny<DocumentId>()), Times.Once);
            Document d;
            cache.Verify(_ => _.TryGetDocument(It.IsAny<DocumentId>(), out d), Times.Once);
        }

        [Test]
        public async Task GetByIdAsync_Cache()
        {
            Document document = new()
            {
                Id = DocumentId.Parse("id"),
                Tags = Enumerable.Empty<string>(),
                Data = ""
            };
            Mock<IReadDocumentsRepository> repository = new();

            Mock<IDocumentsCache> cache = new();
            Document? d;
            cache
                .Setup(_ => _.TryGetDocument(It.IsAny<DocumentId>(), out d))
                .Returns((DocumentId id, out Document d) =>
                {
                    d = document;
                    return true;
                });

            DocumentsProvider sut = new(
                repository.Object,
                cache.Object,
                Mock.Of<ILogger<DocumentsProvider>>());

            Document result = await sut.GetByIdAsync(DocumentId.Parse("id"));

            result.Should().BeSameAs(document);
            repository.Verify(_ => _.GetByIdAsync(It.IsAny<DocumentId>()), Times.Never);
            cache.Verify(_ => _.TryGetDocument(It.IsAny<DocumentId>(), out d), Times.Once);
        }
    }
}