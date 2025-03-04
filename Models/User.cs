using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace compulsoryRest.Models;

public class User {
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [Required]
    [StringLength(50, MinimumLength = 2)]
    [BsonElement("name")]
    public string? Name { get; set; }

    [Required]
    [EmailAddress]
    [BsonElement("email")]
    public string? Email { get; set; }

    [Required]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
    [BsonElement("passwordHash")]
    public string? PasswordHash { get; set; } // Store hashed passwords!

    [Required]
    [BsonElement("createdAt")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Required]
    [BsonElement("updatedAt")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
