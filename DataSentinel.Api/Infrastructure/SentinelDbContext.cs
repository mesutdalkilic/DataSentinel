using DataSentinel.Api.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace DataSentinel.Api.Infrastructure;

public class SentinelDbContext : DbContext
{
    public SentinelDbContext(DbContextOptions<SentinelDbContext> options) : base(options) { }

    public DbSet<ScanResult> ScanResults { get; set; }
}