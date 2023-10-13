using Microsoft.AspNetCore.Identity;
using WebGLives.Core;
using WebGLives.Core.Users;

namespace WebGLives.Auth.Identity;

public class User : IdentityUser<int>, IUser
{
    public User() : base() { }
    
    public User(string username) : base(username) { }
    
    public List<Game> Games { get; set; }
}