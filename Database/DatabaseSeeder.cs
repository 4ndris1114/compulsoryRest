using Bogus;
using compulsoryRest.Models;
using MongoDB.Driver;

namespace compulsoryRest.Database;

public class DatabaseSeeder {
    private readonly MongoDbContext _context;

    public DatabaseSeeder(MongoDbContext context) {
        _context = context;
    }

    public void SeedMovies(int count = 10) {
        var movieCollection = _context.Database.GetCollection<Movie>("Movies");

        // Check if the database already has data to avoid duplicate seeding
        if (movieCollection.Find(_ => true).Any()) return;

        var faker = new Faker<Movie>()
            .RuleFor(m => m.Title, f => f.Lorem.Sentence(3))
            .RuleFor(m => m.Genre, f => f.PickRandom(new[] { "Action", "Comedy", "Drama", "Horror", "Sci-Fi" }))
            .RuleFor(m => m.Year, f => f.Date.Past(20).Year)
            .RuleFor(m => m.Rating, f => (double)Math.Round(f.Random.Float(1, 10), 1))
            .RuleFor(m => m.Director, f => f.Name.FullName())
            .RuleFor(m => m.Description, f => f.Lorem.Paragraph());

        var movies = faker.Generate(count);
        movieCollection.InsertMany(movies);
    }
}
