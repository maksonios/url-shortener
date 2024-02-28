using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UrlShortener.Data;
using UrlShortener.Models;

namespace UrlShortener.Services;

public class UrlService
{
    private readonly ApplicationDbContext _dbContext;

    public UrlService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<ActionResult<UrlModel>> AddToDb(string originalUrl, Guid userId)
    {
        if (await _dbContext.UrlModels.FirstOrDefaultAsync(x => x.Url == originalUrl) != null)
        {
            throw new ArgumentException("Url already exists");
        }
        
        var shortenedUrl = ShortenUrl(originalUrl);
        
        var model = new UrlModel
        {
            Url = originalUrl,
            ShortUrl = shortenedUrl.shortened,
            CreatedBy = userId,
            CreatedDate = DateTime.Now
        };
        
        if (await _dbContext.UrlModels.FirstOrDefaultAsync(x => x.ShortUrl == model.ShortUrl) != null)
        {
            throw new ArgumentException("This shortened URL already exists");
        }
        
        _dbContext.UrlModels.Add(model);
        await _dbContext.SaveChangesAsync();

        return model;
    }

    private (string shortened, string full) ShortenUrl(string originalUrl)
    {
        using SHA256 sha256 = SHA256.Create();
        byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(originalUrl));
        StringBuilder sb = new StringBuilder();

        foreach (byte b in hashBytes)
        {
            sb.Append(b.ToString("X2"));
        }

        string hash = sb.ToString();

        string shortenedPart = hash.Substring(0, 8);

        var result = (
            shortened: shortenedPart,
            full: $"https://short.url/{shortenedPart}"
        );

        return result;
    }
}