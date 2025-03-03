using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace compulsoryRest.Models;

public class Movie {
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set;}
    [BsonElement("title")]
    public string Title { get; set; } = null!;
    [BsonElement("genre")]
    public string? Genre { get; set; }
    [BsonElement("year")]
    public int Year { get; set; }
    [BsonElement("director")]
    public string? Director { get; set; }
    [BsonElement("description")]
    public string? Description { get; set; }
}
