using MongoDB.Driver;
using compulsoryRest.Models;

namespace compulsoryRest.Repositories.database;

public class MongoDbContext
{
    private readonly IMongoDatabase _database;

    public MongoDbContext(string mongoDbConnectionString)
    {
        var client = new MongoClient(mongoDbConnectionString);
        _database = client.GetDatabase("Cinema");  // dbname
    }

    public IMongoCollection<Movie> Movies => _database.GetCollection<Movie>("Movies");  // Collection name
}