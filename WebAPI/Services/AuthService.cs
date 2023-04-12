using System.ComponentModel.DataAnnotations;
using Application.DaoInterfaces;
using Application.LogicInterfaces;
using Shared;
using Shared.DTOs;

namespace WebAPI.Services;

public class AuthService : IAuthService
{
    private readonly IUserLogic userLogic;

    public AuthService(IUserLogic userLogic)
    {
        this.userLogic = userLogic;
    }
    
    public Task<User> ValidateUser(string username, string password)
    {
        UserLoginDto loginDto = new UserLoginDto
        {
            Password = password,
            Username = username
        };

        User? existing = userLogic.GetUserByCredentials(loginDto).Result;
        
        if (existing == null)
        {
            throw new Exception("User not found");
        }
        if (!existing.Password.Equals(password))
        {
            throw new Exception("Password mismatch");
        }
        return Task.FromResult(existing);
    }

    public Task RegisterUser(User user)
    {
        if (string.IsNullOrEmpty(user.UserName))
        {
            throw new ValidationException("Username cannot be null");
        }

        if (string.IsNullOrEmpty(user.Password))
        {
            throw new ValidationException("Password cannot be null");
        }
        // Do more user info validation here
        
        // save to persistence instead of list

        userLogic.CreateAsync(new UserCreationDto(user.UserName, user.Password));
        
        return Task.CompletedTask;
    }
}