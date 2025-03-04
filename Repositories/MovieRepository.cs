using MongoDB.Driver;
using compulsoryRest.Models;
using compulsoryRest.Database;

namespace compulsoryRest.Repositories;

public class MovieRepository : BaseRepository<Movie>
{
    public MovieRepository(MongoDbContext dbContext)
        : base(dbContext, "Movies") {}
        
    public async Task<IEnumerable<Movie>> GetMoviesByGenreAsync(string genre) {
        var filter = Builders<Movie>.Filter.Eq(m => m.Genre, genre);
        return await getCollection().Find(filter).ToListAsync();
    }

    public async Task<IEnumerable<Movie>> GetMoviesByYearAsync(int year) {
        var filter = Builders<Movie>.Filter.Eq(m => m.Year, year);
        return await getCollection().Find(filter).ToListAsync();
    }
}