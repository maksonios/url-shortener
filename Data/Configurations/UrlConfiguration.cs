using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UrlShortener.Models;

namespace UrlShortener.Data.Configurations;

public static class UrlConfiguration
{
    public static void Configure(this EntityTypeBuilder<UrlModel> builder)
    {
        builder.ToTable("Urls");
        
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        
        builder.Property(x => x.Url).HasMaxLength(1000);
        builder.Property(x => x.ShortUrl).HasMaxLength(200);
        
        builder.HasIndex(x => x.Url);
        builder.HasIndex(x => x.ShortUrl).IsUnique();
    }
}