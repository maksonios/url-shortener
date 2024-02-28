namespace UrlShortener.Models;

public class UrlModel
{
    public int Id { get; set; }
    public string Url { get; set; } = null!;
    public string ShortUrl { get; set; } = null!;
    public Guid CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.Now;
}