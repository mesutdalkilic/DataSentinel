using DataSentinel.Api.Infrastructure;
using DataSentinel.Api.Workers;
using DataSentinel.Api.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDbContext<SentinelDbContext>(options => options.UseSqlite("Data Source=sentinel.db"));

// KAYITLARIN DOÐRULUÐUNDAN EMÝN OLALIM
builder.Services.AddSingleton<IFileQueue, FileQueue>();
builder.Services.AddHostedService<ProcessingWorker>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<SentinelDbContext>();
    db.Database.EnsureCreated();
}

app.MapControllers();
app.Run();