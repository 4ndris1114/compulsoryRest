using MongoDB.Driver;
using compulsoryRest.Models;

namespace compulsoryRest.Database;

public class MongoDbContext
{
    private readonly IMongoClient _client;
    public IMongoDatabase Database { get; }

    public MongoDbContext(string mongoDbConnectionString)
    {
        _client = new MongoClient(connectionString);
        Database = _client.GetDatabase("Cinema");  // dbname
    }
}