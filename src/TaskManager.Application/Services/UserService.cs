using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using TaskManager.Application.Services.Interfaces;

namespace TaskManager.Application.Services
{
    public class UserService(IHttpContextAccessor httpContextAccessor) : IUserService
    {
        public int GetCurrentUserId()
        {
            return 1; 
        }
    }
}