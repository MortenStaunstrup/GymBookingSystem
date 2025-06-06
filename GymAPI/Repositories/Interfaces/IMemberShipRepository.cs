using Core;

namespace GymAPI.Repositories.Interfaces;

public interface IMemberShipRepository
{
    Task<List<MemberShip>?> GetAllAsync();
    Task<int> AddMemberShipAsync(MemberShip memberShip);
    Task<MemberShip?> GetByIdAsync(int id);
    Task<int> GetMaxIdAsync();
}