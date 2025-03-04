using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace compulsoryRest.Models;

public class Movie {
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set;}

    [Required]
    [StringLength(255, MinimumLength = 2)]
    [BsonElement("title")]
    public string Title { get; set; } = "N/A";

    [BsonElement("genre")]
    public string? Genre { get; set; }

    [Required]
    [Range(1900, 2030)]
    [BsonElement("year")]
    public int Year { get; set; }

    [Required]
    [Range(0, 10)]
    [BsonElement("rating")]
    public double Rating { get; set; } = double.NaN;

    [BsonElement("director")]
    public string? Director { get; set; }

    [Required]
    [StringLength(1024, MinimumLength = 2)]
    [BsonElement("description")]
    public string Description { get; set; } = "N/A";

    [Required]
    [BsonElement("createdAt")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Required]
    [BsonElement("updatedAt")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
