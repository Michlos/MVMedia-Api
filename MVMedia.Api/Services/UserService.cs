using AutoMapper;

using MVMedia.Api.DTOs;
using MVMedia.Api.Models;
using MVMedia.Api.Repositories.Interfaces;
using MVMedia.Api.Services.Interfaces;

namespace MVMedia.Api.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IAuthtenticate _authenticateService;
    private readonly IMapper _mapper;

    public UserService(IUserRepository userRepository, IMapper mapper, IAuthtenticate authtenticate)
    {
        _userRepository = userRepository;
        _authenticateService = authtenticate;
        _mapper = mapper;
    }

    public async Task<UserDTO> Add(UserDTO userDTO)
    {
        var user = _mapper.Map<User>(userDTO);
        
        (var passwordHash, var passwordSalt) = _authenticateService.HashPassword(userDTO.Password);
        //var passwordHash = _authenticateService.HashPassword(userDTO.Password).hash;
        //var passwordSalt = _authenticateService.HashPassword(userDTO.Password).salt;
        user.SetPassword(passwordHash, passwordSalt);
        
        var userAdded = await _userRepository.Add(user);
        return _mapper.Map<UserDTO>(userAdded);
    }

    public async Task<IEnumerable<UserDTO>> GetAllUsers()
    {
        var users = await _userRepository.GetAllUsers();
        return _mapper.Map<IEnumerable<UserDTO>>(users);
    }

    public async Task<UserDTO> GetUser(int id)
    {
        var user = await _userRepository.GetUser(id);
        return _mapper.Map<UserDTO>(user);
    }

    public async Task<UserDTO> Update(UserDTO userDTO)
    {
        var user = _mapper.Map<User>(userDTO);
        var userUpdated = await _userRepository.Update(user);
        return _mapper.Map<UserDTO>(userUpdated);
    }

    public async Task<bool> UserAutorized(int userId)
    {
        var logedUser = await _userRepository.GetUser(userId);
        return logedUser != null && logedUser.IsActive;
    }

    public async Task<bool> IsAdmin(int userId)
    {
        var logedUser = await _userRepository.GetUser(userId);
        return logedUser != null && logedUser.IsAdmin && logedUser.IsActive;
    }

    public async Task<int> GetCompanyId(int userId)
    {
        var logedUser = await _userRepository.GetUser(userId);
        return logedUser != null ? logedUser.CompanyId : 0;
    }
}
