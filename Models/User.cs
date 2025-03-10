using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace compulsoryRest.Models;

public class User {
    [BsonId]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [Required]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    [BsonElement("email")]
    public string? Email { get; set; }

    [Required]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
    [BsonElement("password")]
    public string? Password { get; set; }

    [BsonElement("createdAt")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
