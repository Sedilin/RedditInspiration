using Application.DaoInterfaces;
using Application.LogicInterfaces;
using Shared;
using Shared.DTOs;

namespace Application.Logic;

public class UserLogic : IUserLogic
{
    private readonly IUserDao userDao;

    public UserLogic(IUserDao userDao)
    {
        this.userDao = userDao;
    }


    public async Task<User> CreateAsync(UserCreationDto userCreationDto)
    {
        User? existing = await userDao.GetByUsernameAsync(userCreationDto.UserName);
        if (existing != null)
        {
            throw new Exception("Username already taken!");
        }

        ValidateData(userCreationDto);

        User toCreate = new User
        {
            UserName = userCreationDto.UserName,
            Password = userCreationDto.Password,
            Type = "User"
        };

        User created = await userDao.CreateAsync(toCreate);
        return created;
    }

    public Task<IEnumerable<User>> GetAsync(SearchUserParametersDto searchParameters)
    {
        return userDao.GetAsync(searchParameters);
    }

    public Task<User?> GetUserByCredentials(UserLoginDto userCredentials)
    {
        return userDao.GetUserByCredentials(userCredentials);
    }

    private static void ValidateData(UserCreationDto userToCreate)
    {
        string userName = userToCreate.UserName;

        if (userName.Length < 3)
        {
            throw new Exception("Username must be at least 3 characters!");
        }

        if (userName.Length > 15)
        {
            throw new Exception("Username must be less than 16 characters!");
        }
    }
}