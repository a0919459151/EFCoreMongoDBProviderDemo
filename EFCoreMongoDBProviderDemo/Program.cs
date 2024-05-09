using EFCoreMongoDBProviderDemo;
using EFCoreMongoDBProviderDemo.EFCore.Mongo.Entities;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Driver;
using Testcontainers.MongoDb;

var connectionString = await TestContainerStart();

var postId = await InsertOnePost();

await UpdatePostTitle();

await AddPostComment();

await QueryPost(postId);

Console.ReadLine();

async Task<string> TestContainerStart()
{
    var mongoContainer = new MongoDbBuilder()
       .WithImage("mongo:latest")
       .Build();

    await mongoContainer.StartAsync();

    return mongoContainer.GetConnectionString();
}

AppDbContext GetDbContext()
{
    var mongoClient = new MongoClient(connectionString);
    var dbContext = AppDbContext.Create(mongoClient.GetDatabase("mongoDemo"));
    return dbContext;
}

async Task<ObjectId> InsertOnePost()
{
    var dbContext = GetDbContext();

    var post = new Post
    {
        Title = "Hello World",
        Content = "This is a test post",
        CreateAt = DateTime.Now,
        Comments = new List<Comment>
        {
            new Comment
            {
                Author = "John Doe",
                Content = "This is a test comment",
                CreateAt = DateTime.Now
            },
            new Comment
            {
                Author = "Apple Pie",
                Content = "This is another test comment",
                CreateAt = DateTime.Now
            }
        }
    };

    await dbContext.Posts.AddAsync(post);

    await dbContext.SaveChangesAsync();

    return post.Id;
}

async Task UpdatePostTitle()
{
    var dbContext = GetDbContext();

    var post = await dbContext.Posts
        .Where(p => p.Title == "Hello World")
        .FirstOrDefaultAsync();

    if (post is null)
    {
        Console.WriteLine("Post not found");
        return;
    }

    post.Title = "Hello World Updated";

    await dbContext.SaveChangesAsync();
}

async Task AddPostComment()
{
    var dbContext = GetDbContext();

    var post = await dbContext.Posts
        .Where(p => p.Title == "Hello World Updated")
        .FirstOrDefaultAsync();

    if (post is null)
    {
        Console.WriteLine("Post not found");
        return;
    }

    post.Comments.Add(new Comment
    {
        Author = "Joe Chiu",
        Content = "This is a new comment",
        CreateAt = DateTime.Now
    });

    await dbContext.SaveChangesAsync();
}

async Task QueryPost(ObjectId postId)
{
    var dbContext = GetDbContext();

    var post = await dbContext.Posts
        .FindAsync(postId);

    if (post is null)
    {
        Console.WriteLine("Post not found");
        return;
    }

    Console.WriteLine("Post found");
}
