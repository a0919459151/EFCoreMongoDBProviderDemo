using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace EFCoreMongoDBProviderDemo.EFCore.Mongo.Entities;

public class Post
{
    [BsonElement("_id")]
    public ObjectId Id { get; set; }

    [BsonElement("title")]
    public string? Title { get; set; }

    [BsonElement("content")]
    public string? Content { get; set; }

    [BsonElement("createAt")]
    public DateTime? CreateAt { get; set; }

    [BsonElement("comments")]
    public List<Comment> Comments { get; set; } = new List<Comment>();
}

public class Comment
{
    [BsonElement("author")]
    public string? Author { get; set; }

    [BsonElement("content")]
    public string? Content { get; set; }

    [BsonElement("createAt")]
    public DateTime? CreateAt { get; set; }
}