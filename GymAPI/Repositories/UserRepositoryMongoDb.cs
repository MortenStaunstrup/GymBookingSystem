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

    public async Task<int> RegisterAsync(User user)
    {
        var filter = Builders<User>.Filter.Eq("Email", user.Email);
        var existingUser =  await _userCollection.Find(filter).FirstOrDefaultAsync();

        if (existingUser != null)
        {
            Console.WriteLine($"User with email {user.Email} already exists!");
            return 1; // user already exists
        }

        try
        {
            await _userCollection.InsertOneAsync(user);
            Console.WriteLine($"User {user.UserId} {user.Email} created");
            return 2; // user created
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error occured: {e.Message}");
            return 3; // server error
        }
        
    }

    public async Task<int> GetMaxId()
    {
        var filter = Builders<User>.Filter.Empty;
        var sort = Builders<User>.Sort.Descending(x => x.UserId);
        var result = await _userCollection.Find(filter).Sort(sort).FirstOrDefaultAsync();

        if (result == null)
            return 0;
        return result.UserId;
    }
    

}