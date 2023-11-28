using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Data.Entity.Core;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ToDoManager.Application.Common;
using ToDoManager.Application.Common.Interfaces.Repository;
using ToDoManager.Application.DTOs;
using ToDoManager.Domain.Entities;
using ToDoManager.Domain.Exceptions;

namespace ToDoManager.Presentation.Controllers
{
    [Authorize]
    public class UserController:Controller
    {
       
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITaskRepository _taskRepository;
        private readonly IUserRepository _userRepository;
        private readonly IAuthenticationService _authenticationService;

        public UserController(IUnitOfWork unitOfWork,
            ITaskRepository taskRepository,
            IUserRepository userRepository,
            IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
            _taskRepository = taskRepository;
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            
        }

 

        [HttpGet]
        public IActionResult MainDashboard()
        {
            return View("/Views/Tasks/MainDashboard.cshtml");
        }




        [HttpGet]
        [Route("/get/tasks")]
        public async Task<IActionResult> GetAllTasks()
        {
            var claims = HttpContext.User.Claims.ToList();
            var claimId = claims.FirstOrDefault(x => x.Type == ClaimTypes.Actor)?.Value.ToString();
            Guid id;
            if(!Guid.TryParse(claimId,out id))
            {
                throw new UserNotFoundException(id.ToString());
            }
            var tasks = await _taskRepository.GetTasksByUserId(id);
            return Ok(tasks);
        }


        [HttpDelete]
        [Route("delete/task/${taskId}")]
        public async Task<IActionResult> DeleteTask(Guid taskId)
        {
            try
            {
                await _taskRepository.DeleteAsync(await _taskRepository.GetByIdAsync(taskId));
                await _unitOfWork.Save(default);
                return Ok("successfully delete");
            }
            catch(EntityException ex)
            {
                return NotFound(ex.Message);
            }
              
        }


        [HttpPost]
        [Route("/create/task")]
        public async Task<IActionResult> CreateTask([FromBody] TaskDto task)
        {
            var claims = HttpContext.User.Claims.ToList();
            var claimId = claims.FirstOrDefault(x => x.Type == ClaimTypes.Actor)?.Value.ToString();
            Guid id;
            if(!Guid.TryParse(claimId,out id))
            {
                throw new UserNotFoundException(id.ToString());
            }
            var newTask = new ToDoManager.Domain.Entities.Task
            {
                Id = Guid.NewGuid(),
                Content = task.Content,
                CreationDate = DateTime.Now,
                Title = task.Title,
                User =await _userRepository.GetByIdAsync(id),
                UserId = id,
                IsComleated = task.IsComleated
            };

            await _taskRepository.AddAsync(newTask);
            await _unitOfWork.Save(default);
            return Ok("create successfully");
        }



        [HttpPut]
        [Route("edit/task")]
        public async Task<IActionResult> EditTask([FromForm] ToDoManager.Domain.Entities.Task task)
        {
            try
            {
                await _taskRepository.UpdateAsync(task);
                await _unitOfWork.Save(default);
                return Ok(task);
            }
            catch(EntityException ex)
            {
                return NotFound(ex.Message);
            }
            
            
        }



    }
}
