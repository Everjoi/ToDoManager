using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ToDoManager.Application.Common;
using ToDoManager.Application.Common.Interfaces.Repository;
using ToDoManager.Application.DTOs;
using ToDoManager.Domain.Entities;
using ToDoManager.Domain.Exceptions;
using ToDoManager.Infrastructure.Options;

namespace ToDoManager.Infrastructure.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;


        public AuthenticationService(IConfiguration configuration,IUnitOfWork unitOfWork,IUserRepository userRepository)
        {
            _configuration = configuration;
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
        }


     

        public bool ValidateToken(string token)
        {
            var parameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = AuthOption.ISSUER,
                ValidAudience = AuthOption.AUDIENCE,
                IssuerSigningKey = AuthOption.GetSymmetricSecurityKey()
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken validatedToken;
            try
            {
                tokenHandler.ValidateToken(token,parameters,out validatedToken);
            }
            catch(Exception)
            {
                return false;
            }
            return true;
        }


        public string Authenticate(LoginDto login)
        {
            var user = _userRepository.GetUserByEmail(login.Email).Result;
            if(user == null)
                throw new UserNotFoundException(login.Email);

            if(!VerifyPassword(login.Password,user.Password))
                throw new PasswordNotMatchExceptions();

            return GenerateToken(user);
        }    

        public string GenerateToken(User user)
        {
            
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name,user.UserName.ToString()),
                    new Claim(ClaimTypes.Actor,user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email.ToString()),
                    
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(AuthOption.GetSymmetricSecurityKey(),SecurityAlgorithms.HmacSha256)

            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }      

        public async Task<Guid> Register(SignUpDto user)
        {
            var exeUser = await _userRepository.GetUserByEmail(user.Email);
            if(exeUser != null)
                throw new ExistUserException(exeUser.Email);
            if(user.Password != user.ConfirmPassword)
                throw new PasswordNotMatchExceptions();

            var newUser = new User
            {
                Id = Guid.NewGuid(),
                UserName = user.UserName,
                Email = user.Email,
                Password = HashPassword(user.Password)
            };
            await _userRepository.AddAsync(newUser);
            await _unitOfWork.Save(default);
            return newUser.Id;
        }

        private bool VerifyPassword(string password,string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password,hashedPassword);
        }

        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
    }
}
