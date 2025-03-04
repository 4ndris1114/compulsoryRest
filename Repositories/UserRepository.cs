using compulsoryRest.Models;
using compulsoryRest.Database;
using MongoDB.Driver;

namespace compulsoryRest.Repositories;
public class UserRepository : BaseRepository<User> {
    private readonly IMongoCollection<User> _users;

    public UserRepository(MongoDbContext dbContext) : base(dbContext, "Users") {
        _users = dbContext.Database.GetCollection<User>("Users");
    }

    public async Task<User?> GetByEmailAsync(string email) {
        return await _users.Find(u => u.Email == email).FirstOrDefaultAsync();
    }
}
