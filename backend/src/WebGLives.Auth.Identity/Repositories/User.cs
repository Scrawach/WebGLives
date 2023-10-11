using Microsoft.AspNetCore.Identity;
using WebGLives.Core.Users;

namespace WebGLives.Auth.Identity.Repositories;

public class User : IdentityUser, IUser
{
    public User() : base() { }
    
    public User(string username) : base(username) { }
}