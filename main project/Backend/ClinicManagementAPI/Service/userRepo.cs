using ClinicManagementAPI.Context;
using ClinicManagementAPI.DTOs;
using ClinicManagementAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

using System.Security.Claims;
using ClinicManagementAPI.Security;

namespace ClinicManagementAPI.Service
{

    
    public class userRepo : IUserRepo
    {
        private AppDbContext context;
        private PasswordHasher<User> passwordhash;
        Microsoft.Extensions.Options.IOptions<JWToptions> jwtOptions;

        public userRepo(AppDbContext dbContext, Microsoft.Extensions.Options.IOptions<JWToptions> _jwtOptions)
        {
            context = dbContext;
            passwordhash = new PasswordHasher<User>();
            jwtOptions = _jwtOptions;

        }
        public void AddRole(Role role)
        {
            context.Roles.Add(role);
            context.SaveChanges();
        }
        public void Add(RegisterDto dto)
        {
            var role = context.Roles.FirstOrDefault(r => r.RoleName == dto.RoleName);
            if (role == null)
            {
                throw new Exception($"Role '{dto.RoleName}' does not exist.");
            }

            var user = new User
            {
                Username = dto.Username,
                Password = dto.Password,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                RoleId = role.RoleId
            };

            if (string.IsNullOrWhiteSpace(dto.Password))
            {
                throw new ArgumentException("Password cannot be null or empty");
            }

            user.Password = passwordhash.HashPassword(user, dto.Password);

            context.Users.Add(user);
            context.SaveChanges();
        }


        // public IActionResult Login(string username, string password)
        // {

        //     var user = context.Users
        //           .Include(u => u.Role)
        //           .FirstOrDefault(u => u.Username == username);

        //     if (user == null)
        //     {
        //         return new UnauthorizedObjectResult("Invalid username or password");
        //     }

        //     var passwordHasher = new PasswordHasher<User>();

        //     var result = passwordHasher.VerifyHashedPassword(user, user.Password, password);

        //     if (result == PasswordVerificationResult.Failed)
        //     {
        //         return new UnauthorizedObjectResult("Invalid username or password");
        //     }

        //     return new OkObjectResult("Login successfully");
        // }
         const string ADMIN = "admin";
        public IActionResult Login(LoginRequest loginReq)
        {
            
            var user = context.Users
                              .Include(u => u.Role)
                              .FirstOrDefault(u => u.Username == loginReq.Name);

            if (user == null)
                return new UnauthorizedObjectResult("User not found");

           
            var passwordHasher = new PasswordHasher<User>();
            var result = passwordHasher.VerifyHashedPassword(user, user.Password, loginReq.Password);
            if (result == PasswordVerificationResult.Failed)
                return new UnauthorizedObjectResult("Invalid username or password");

           var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, user.Username),
        new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString())
    };

    if (user.Role.RoleName.Trim().ToLower() == ADMIN)
    {
        claims.Add(new Claim(ClaimTypes.Role, "Admin"));
    }
    else
    {
        claims.Add(new Claim(ClaimTypes.Role, user.Role.RoleName));
    }

    // Generate JWT token
    var token = jwtService.CreateJwtToken(jwtOptions.Value, claims);
        return new  OkObjectResult(new 
{
    Token = token,
    Role = user.Role.RoleName.ToString()
});
 }
    }
}