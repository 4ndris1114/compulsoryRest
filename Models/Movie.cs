using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace compulsoryRest.Models;

public class Movie {
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set;}

    [Required]
    [StringLength(255, MinimumLength = 2, ErrorMessage = "Title must be between 2 and 255 characters")]
    [BsonElement("title")]
    public string Title { get; set; } = "N/A";

    [BsonElement("genre")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "Genre must be between 2 and 50 characters")]
    public string? Genre { get; set; }

    [Required]
    [Range(1900, 2030, ErrorMessage = "Year must be between 1900 and 2030")]
    [BsonElement("year")]
    public int Year { get; set; }

    [Required]
    [Range(0, 10, ErrorMessage = "Rating must be between 0 and 10")]
    [BsonElement("rating")]
    public double Rating { get; set; } = 0;

    [BsonElement("director")]
    [StringLength(255, MinimumLength = 2, ErrorMessage = "Director must be between 2 and 255 characters")]
    public string? Director { get; set; }

    [Required]
    [StringLength(1024, MinimumLength = 2, ErrorMessage = "Description must be between 2 and 1024 characters")]
    [BsonElement("description")]
    public string Description { get; set; } = "N/A";

    [BsonElement("createdAt")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [BsonElement("updatedAt")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
