namespace Documents.Core;

public class Document
{
    public required DocumentId Id { get; init; }
    public required IEnumerable<string> Tags { get; init; }
    public required string Data { get; init; }
}