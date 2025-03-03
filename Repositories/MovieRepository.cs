using MongoDB.Driver;
using compulsoryRest.Models;
using compulsoryRest.Repositories;
using compulsoryRest.Database;

namespace compulsoryRest.Repositories;

public class MovieRepository : BaseRepository<Movie> {
    private readonly IMongoCollection<Movie> _movieCollection;

    public MovieRepository(MongoDbContext dbContext) 
        : base(dbContext, "Movies") {
        _movieCollection = dbContext.Database.GetCollection<Movie>("Movies");
    }
    public async Task<IEnumerable<Movie>> GetMoviesByGenreAsync(string genre) {
        var filter = Builders<Movie>.Filter.Eq(m => m.Genre, genre);
        return await _movieCollection.Find(filter).ToListAsync();
    }

    public async Task<IEnumerable<Movie>> GetMoviesByYearAsync(int year) {
        var filter = Builders<Movie>.Filter.Eq(m => m.Year, year);
        return await _movieCollection.Find(filter).ToListAsync();
    }
}