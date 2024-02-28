using Duende.IdentityServer.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UrlShortener.Data;
using UrlShortener.Extensions;
using UrlShortener.Models;
using UrlShortener.Services;

namespace UrlShortener.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class UrlController : ControllerBase
{
    private readonly ApplicationDbContext _dbContext;
    private readonly UrlService _service;

    public UrlController(ApplicationDbContext dbContext, UrlService service)
    {
        _dbContext = dbContext;
        _service = service;
    }
    
    [AllowAnonymous]
    [HttpGet]
    public async Task<IEnumerable<UrlModel>> Get()
    {
        return await _dbContext.UrlModels.ToListAsync();
    }

    [HttpGet, Route("{id}")]
    public async Task<IEnumerable<UrlModel>> Get(int id)
    {
        return await _dbContext.UrlModels.Where(x => x.Id == id).ToListAsync();
    }

    [HttpGet, Route("find/{shortenedUrl}")]
    public async Task<ActionResult> FindOriginalUrl(string shortenedUrl)
    {
        var urlModel = await _dbContext.UrlModels.FirstOrDefaultAsync(x => x.ShortUrl == shortenedUrl);

        if (urlModel == null)
        {
            return NotFound("Shortened URL not found");
        }

        return Ok(urlModel.Url);
    }
    
    [HttpPost]
    public async Task<ActionResult<UrlModel>> Post([FromBody] UrlDto urlDto)
    {
        var originalUrl = urlDto.OriginalUrl;
        var userId = User.Identity.GetSubjectId().ToGuid();

        try
        {
            var model = await _service.AddToDb(originalUrl, userId);
            return Ok(model);
        }

        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }

        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete, Route("{id}")]
    public async Task Delete(int id)
    {
        var url = await _dbContext.UrlModels.SingleAsync(x => x.Id == id);
        _dbContext.Remove(url);
        await _dbContext.SaveChangesAsync();
    }
}