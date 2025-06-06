using Core;
using GymAPI.Repositories.Interfaces;
using MongoDB.Driver;

namespace GymAPI.Repositories;

public class MemberShipRepositoryMongoDb : IMemberShipRepository
{
    private readonly string _connectionString;
    private readonly IMongoClient _client;
    private readonly IMongoDatabase _database;
    private readonly IMongoCollection<MemberShip> _membershipCollection;

    public MemberShipRepositoryMongoDb()
    {
        _connectionString = Environment.GetEnvironmentVariable("MONGO_CONNECTION_STRING");
        _client = new MongoClient(_connectionString);
        _database = _client.GetDatabase("Gym");
        _membershipCollection = _database.GetCollection<MemberShip>("Memberships");
    }
    
    public async Task<List<MemberShip>?> GetAllAsync()
    {
        var filter = Builders<MemberShip>.Filter.Empty;
        return await _membershipCollection.Find(filter).ToListAsync();
    }

    public async Task<int> AddMemberShipAsync(MemberShip memberShip)
    {
        try
        {
            await _membershipCollection.InsertOneAsync(memberShip);
            return 1;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error occured inserting new membership: {e.Message}");
            return 2;
        }
    }

    public async Task<MemberShip?> GetByIdAsync(int id)
    {
        var filter = Builders<MemberShip>.Filter.Eq("_id", id);
        return await _membershipCollection.Find(filter).FirstOrDefaultAsync();
    }

    public async Task<int> GetMaxIdAsync()
    {
        var filter  =  Builders<MemberShip>.Filter.Empty;
        var sort = Builders<MemberShip>.Sort.Descending(x => x.MemberShipId);
        var result = await _membershipCollection.Find(filter).Sort(sort).Limit(1).FirstOrDefaultAsync();
        return result?.MemberShipId ?? 0;
    }
}