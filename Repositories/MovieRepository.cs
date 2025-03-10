using MongoDB.Driver;
using compulsoryRest.Models;
using compulsoryRest.Database;
using MongoDB.Bson;

namespace compulsoryRest.Repositories;

public class MovieRepository : BaseRepository<Movie>
{
    public MovieRepository(MongoDbContext dbContext)
        : base(dbContext, "Movies") {}

    override
    public async Task UpdateAsync(string id, Movie entity) {
        var filter = Builders<Movie>.Filter.Eq("_id", ObjectId.Parse(id));

        var update = Builders<Movie>.Update
            .Set("title", entity.Title)
            .Set("genre", entity.Genre)
            .Set("year", entity.Year)
            .Set("rating", entity.Rating)
            .Set("director", entity.Director)
            .Set("description", entity.Description)
            .Set("updatedAt", DateTime.UtcNow); // Automatically update timestamp

        await getCollection().UpdateOneAsync(filter, update);
    }
        
    public async Task<IEnumerable<Movie>> GetMoviesByGenreAsync(string genre) {
        var filter = Builders<Movie>.Filter.Eq(m => m.Genre, genre);
        return await getCollection().Find(filter).ToListAsync();
    }

    public async Task<IEnumerable<Movie>> GetMoviesByYearAsync(int year) {
        var filter = Builders<Movie>.Filter.Eq(m => m.Year, year);
        return await getCollection().Find(filter).ToListAsync();
    }
}