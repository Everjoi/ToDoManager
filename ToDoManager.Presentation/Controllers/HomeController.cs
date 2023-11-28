using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Net.Http;
using ToDoManager.Application.Common;
using ToDoManager.Application.Common.Interfaces.Repository;
using ToDoManager.Application.DTOs;
using ToDoManager.Domain.Exceptions;
using ToDoManager.Presentation.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace ToDoManager.Presentation.Controllers
{
    [AllowAnonymous]
    public class HomeController:Controller
    {
        private readonly IAuthenticationService _authenticationService;

        public HomeController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }


        [HttpGet]
        public IActionResult LogIn()
        {
            return View();
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromForm] LoginDto login)
        {
            try
            {
                var token = _authenticationService.Authenticate(login);
                HttpContext.Response.Cookies.Append(".AspNetCore.Application.Id", token, 
                new CookieOptions
                {
                    MaxAge = TimeSpan.FromMinutes(60)
                });

                return RedirectToAction("MainDashboard","User");
            }
            catch(UserNotFoundException ex)
            {
                return NotFound($"User not found {ex.Message}");
            } 
            catch (PasswordNotMatchExceptions ex)
            {    
                return Unauthorized($"Password do not match ({ex.Message})");
            }
            catch(Exception ex)
            {    
                return StatusCode(500,$"Internal Server Error: {ex.Message}");
            }     
        }


        [HttpPost]
        [Route("home/signup")]
        public async Task<IActionResult> Register([FromForm] SignUpDto signUp)
        {
            try
            {
                var id = await _authenticationService.Register(signUp);
                return RedirectToAction("Login");
            }
            catch(PasswordNotMatchExceptions ex)
            {
                return Unauthorized($"Password do not match ({ex.Message})");
            }
            catch(ExistUserException ex)
            {
                return BadRequest(ex.Message);
            }
            catch(Exception ex)
            {
                return StatusCode(500,$"Internal Server Error: {ex.Message}");
            }
        }
        
    }
}