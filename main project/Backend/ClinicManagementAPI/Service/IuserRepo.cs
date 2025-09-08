using ClinicManagementAPI.DTOs;
using ClinicManagementAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagementAPI.Service;

public interface IUserRepo
{
    void AddRole(Role role);
    void Add(RegisterDto dto);
    
    IActionResult Login(LoginRequest loginReq);
    

}