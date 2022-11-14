using Documents.Core;

using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

using System.Diagnostics.CodeAnalysis;

namespace Documents.Business;

internal class DocumentsCache : IDocumentsCache
{
    private readonly IMemoryCache _cache;
    private readonly DocumentsCacheOptions _options;

    public DocumentsCache(IMemoryCache cache,
        IOptionsSnapshot<DocumentsCacheOptions> options)
    {
        _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        _options = options?.Value
            ?? throw new ArgumentNullException(nameof(options));
    }

    public bool TryGetDocument(DocumentId id, [MaybeNullWhen(false)] out Document? document)
    {
        if (_options.Enabled && _cache.TryGetValue(id, out object? cachedDocument))
        {
            document = (Document)cachedDocument!;
            return true;
        }

        document = default;
        return false;
    }

    public void SetDocument(Document document)
    {
        ArgumentNullException.ThrowIfNull(document);

        if (!_options.Enabled)
            return;

        using ICacheEntry entry = _cache.CreateEntry(document.Id);
        entry.Value = document;
        entry.SlidingExpiration = _options.SlidingExpiration;
        entry.AbsoluteExpirationRelativeToNow = _options.AbsoluteExpiration;
    }
}
