using MVMedia.Api.DTOs;

namespace MVMedia.Api.Services.Interfaces;

public interface IUserService
{
    Task<UserDTO> Add(UserDTO userDTO);
    Task<UserDTO> Update(UserDTO userDTO);
    Task<UserDTO> GetUser(int id);
    Task<IEnumerable<UserDTO>> GetAllUsers();
    Task<bool> UserAutorized(int userId);
    Task<bool> IsAdmin(int userId);

    Task<int> GetCompanyId(int userId);
}
