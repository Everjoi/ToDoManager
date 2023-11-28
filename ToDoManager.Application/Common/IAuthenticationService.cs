using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoManager.Application.DTOs;
using ToDoManager.Domain.Entities;

namespace ToDoManager.Application.Common
{
    public interface IAuthenticationService
    {
        string Authenticate(LoginDto login);
        Task<Guid> Register(SignUpDto signup);
        string GenerateToken(User user);
        bool ValidateToken(string token);
    }
}
