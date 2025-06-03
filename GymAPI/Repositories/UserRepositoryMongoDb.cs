using Core;
using GymAPI.Repositories.Interfaces;
using MongoDB.Driver;

namespace GymAPI.Repositories;

public class UserRepositoryMongoDb : IUserRepository
{
    private readonly string _connectionString;
    IMongoClient _client;
    IMongoDatabase _database;
    IMongoCollection<User> _userCollection;
    
    public UserRepositoryMongoDb()
    {
        _connectionString = Environment.GetEnvironmentVariable("MONGO_CONNECTION_STRING");
        _client = new MongoClient(_connectionString);
        _database = _client.GetDatabase("Gym");
        _userCollection = _database.GetCollection<User>("Users");
    }

    public async Task<User?> TryLoginAsync(string email, string password)
    {
        var emailFilter = Builders<User>.Filter.Eq("Email", email);
        var passwordFilter = Builders<User>.Filter.Eq("Password", password);
        var userFilter = Builders<User>.Filter.And(emailFilter, passwordFilter);

        return await _userCollection.Find(userFilter).FirstOrDefaultAsync();
    }

    public async Task<bool> RegisterAsync(User user)
    {
        return false;
    }
    

}