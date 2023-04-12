using Shared;
using Shared.DTOs;

namespace Application.LogicInterfaces;

public interface IPostLogic
{
    Task<Post> CreateAsync(PostCreationDto dto);
    Task<IEnumerable<Post>> GetAsync(SearchPostParametersDto searchParameters);
    Task<Post?> GetByIdAsync(int id);
    Task DeleteAsync(int id);
}