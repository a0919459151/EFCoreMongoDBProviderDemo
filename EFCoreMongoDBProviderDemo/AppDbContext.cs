using EFCoreMongoDBProviderDemo.EFCore.Mongo.Entities;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using MongoDB.EntityFrameworkCore.Extensions;

namespace EFCoreMongoDBProviderDemo;

public class AppDbContext : DbContext
{
    public DbSet<Post> Posts { get; set; }

    public AppDbContext(DbContextOptions options) : base(options)
    {
    }

    // Static create
    public static AppDbContext Create(IMongoDatabase database)
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseMongoDB(database.Client, database.DatabaseNamespace.DatabaseName)
            .Options;

        return new AppDbContext(options);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Post>().ToCollection("posts");
    }
}
