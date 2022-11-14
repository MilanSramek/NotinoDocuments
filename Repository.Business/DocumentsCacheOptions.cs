namespace Documents.Business;

internal class DocumentsCacheOptions
{
    public const string Section = "DocumentsCaching";

    public bool Enabled { get; set; } = true;
    public TimeSpan? SlidingExpiration { get; set; } = TimeSpan.FromSeconds(3);
    public TimeSpan? AbsoluteExpiration { get; set; }
}
