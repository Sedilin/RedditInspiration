using Shared;
using Shared.DTOs;

namespace HttpClients.ClientInterfaces;

public interface IUserService
{
    Task<User> CreateAsync(UserCreationDto dto);
    Task<IEnumerable<User>> GetUsers(string? usernameContains = null);
}