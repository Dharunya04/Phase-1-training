using Microsoft.AspNetCore.Mvc;
using ClinicManagementAPI.Context;
using ClinicManagementAPI.Models;
using ClinicManagementAPI.Service;
using ClinicManagementAPI.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace ClinicManagementAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class userController : ControllerBase
    {
        IUserRepo userRepo;
        public userController(IUserRepo repo)
        {
            userRepo = repo;
        }
        [HttpPost("Role")]
        public IActionResult AddRole(Role role)
        {
            userRepo.AddRole(role);
            return Ok("Role added successfully");
        }
        [HttpPost("register")]
        public IActionResult Register(RegisterDto dto)
        {
            userRepo.Add(dto);
            return Ok(new { message = "Registered successfully" });
        }
       [HttpPost("login")]
public IActionResult Login(LoginRequest loginReq)
{
    return userRepo.Login(loginReq);
}
    }

}