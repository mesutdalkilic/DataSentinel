using DataSentinel.Api.Infrastructure;
using DataSentinel.Api.Workers;
using DataSentinel.Api.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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


// 2. MIDDLEWARE
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


// Statik dosyalarý (Dashboard) aktifleþtir
app.UseDefaultFiles(); // index.html'i otomatik bulur
app.UseStaticFiles();  // wwwroot klasörünü açar


app.UseAuthorization();
app.MapControllers();

//Properties / launchSettings.json içinde applicationUrl'i 5000 yapmýþtýk yine de burada 5000 portunu belirtelim kesin olsun
app.Run("http://localhost:5000");