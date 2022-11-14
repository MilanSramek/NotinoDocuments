using System.Diagnostics.CodeAnalysis;

namespace Documents.Core;

public readonly struct DocumentId : IEquatable<DocumentId>, IParsable<DocumentId>
{
    private readonly string _value;

    private DocumentId(string value) => _value = value
        ?? throw new ArgumentNullException(nameof(value));

    public bool Equals(DocumentId other) => _value.Equals(other._value);
    public override bool Equals(object? obj) => obj is DocumentId id && Equals(id);
    public override int GetHashCode() => _value.GetHashCode();
    public static bool operator ==(DocumentId left, DocumentId right) => left.Equals(right);
    public static bool operator !=(DocumentId left, DocumentId right) => !(left == right);

    public override string ToString() => _value;

    public static DocumentId Parse(string value) => new(value);

    public static DocumentId Parse(string value, IFormatProvider? provider) => new(value);

    public static bool TryParse(
        [NotNullWhen(true)] string? value,
        IFormatProvider? provider,
        [MaybeNullWhen(false)] out DocumentId result)
    {
        if (value is { })
        {
            result = new(value);
            return true;
        }

        result = default;
        return false;
    }
}
