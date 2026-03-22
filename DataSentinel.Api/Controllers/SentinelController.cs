using Microsoft.AspNetCore.Mvc;
using DataSentinel.Api.Infrastructure;
using DataSentinel.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace DataSentinel.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SentinelController : ControllerBase
{
    private readonly IFileQueue _queue;
    private readonly SentinelDbContext _context;
    private readonly string _storagePath;

    public SentinelController(IFileQueue queue, SentinelDbContext context)
    {
        _queue = queue;
        _context = context;
        _storagePath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "Uploads");

        if (!Directory.Exists(_storagePath))
            Directory.CreateDirectory(_storagePath);
    }

    [HttpPost("analyze")]
    public async Task<IActionResult> Analyze(IFormFile file)
    {
        if (file == null || file.Length == 0) return BadRequest("Dosya yok.");

        var extension = System.IO.Path.GetExtension(file.FileName).ToLower();
        if (extension != ".pdf") return BadRequest("Sadece PDF.");

        var fileName = $"{Guid.NewGuid()}_{file.FileName}";
        var filePath = System.IO.Path.Combine(_storagePath, fileName);

        using (var stream = new System.IO.FileStream(filePath, System.IO.FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        _queue.Enqueue(filePath);
        return Ok(new { message = "Kuyruğa alındı.", fileName });
    }

    [HttpGet("reports")]
    public async Task<IActionResult> GetReports()
    {
        return Ok(await _context.ScanResults.OrderByDescending(r => r.CreatedAt).ToListAsync());
    }
}