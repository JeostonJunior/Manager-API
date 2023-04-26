using AutoMapper;
using Manager.API.Utilities;
using Manager.API.ViewModels;
using Manager.Core.Exceptions;
using Manager.Services.DTOS;
using Manager.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Manager.API.Controllers
{
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UserController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [HttpPost("/api/v1/users/create")]
        [Authorize]
        public async Task<IActionResult> CreateUserAsync([FromBody] CreateUserViewModel user)
        {
            try
            {
                var userDTO = _mapper.Map<UserDTO>(user);

                var userCreated = await _userService.Create(userDTO);

                return Ok(new ResultViewModel
                {
                    Message = "New User successfully created",
                    Success = true,
                    Data = userCreated
                });
            }
            catch (DomainExceptions ex)
            {
                return BadRequest(Responses.DomainErrorMessage(ex.Message, ex.Errors));
            }
            catch (Exception)
            {
                return StatusCode(500, Responses.ApplicationErrorMessage());
            }
        }

        [HttpPut("/api/v1/users/update")]
        [Authorize]
        public async Task<IActionResult> UpdateUserAsync([FromBody] UpdateUserViewModel updateUserViewModel)
        {
            try
            {
                var userDTO = _mapper.Map<UserDTO>(updateUserViewModel);

                var userUpdated = await _userService.Update(userDTO);

                return Ok(new ResultViewModel
                {
                    Message = "The User has been updated",
                    Success = true,
                    Data = userUpdated
                });
            }
            catch (DomainExceptions ex)
            {
                return BadRequest(Responses.DomainErrorMessage(ex.Message, ex.Errors));
            }
            catch (Exception)
            {
                return StatusCode(500, Responses.ApplicationErrorMessage());
            }
        }

        [HttpDelete("/api/v1/users/delete/{id:long}")]
        [Authorize]
        public async Task<IActionResult> UserDeleteAsync(long id)
        {
            try
            {               
                await _userService.Remove(id);

                return Ok(new ResultViewModel
                {
                    Message = "The User has been deleted",
                    Success = true,
                    Data = null
                });
            }
            catch (DomainExceptions ex)
            {
                return BadRequest(Responses.DomainErrorMessage(ex.Message, ex.Errors));
            }
            catch (Exception)
            {
                return StatusCode(500, Responses.ApplicationErrorMessage());
            }
        }

        [HttpGet("/api/v1/users/{id:long}")]
        [Authorize]
        public async Task<IActionResult> UserGetAsync(long id)
        {
            try
            {
                var userExists = await _userService.Get(id);               

                return Ok(new ResultViewModel
                {
                    Message = "User successfully retrieved",
                    Success = true,
                    Data = userExists
                });
            }
            catch (DomainExceptions ex)
            {
                return NotFound(Responses.DomainErrorMessage(ex.Message, ex.Errors));
            }            
            catch (Exception)
            {
                return StatusCode(500, Responses.ApplicationErrorMessage());
            }
        }

        [HttpGet("/api/v1/users/search-by-name/{name}")]
        [Authorize]
        public async Task<IActionResult> UserGetAsync(string name)
        {
            try
            {
                var userExists = await _userService.SearchByName(name);

                return Ok(new ResultViewModel
                {
                    Message = "User successfully retrieved",
                    Success = true,
                    Data = userExists
                });
            }
            catch (DomainExceptions ex)
            {
                return NotFound(Responses.DomainErrorMessage(ex.Message, ex.Errors));
            }
            catch (Exception)
            {
                return StatusCode(500, Responses.ApplicationErrorMessage());
            }
        }

        [HttpGet("/api/v1/users/get-by-email/{email}")]
        [Authorize]
        public async Task<IActionResult> UserGetEmailAsync(string email)
        {
            try
            {
                var userExists = await _userService.GetByEmail(email);

                return Ok(new ResultViewModel
                {
                    Message = "User successfully retrieved",
                    Success = true,
                    Data = userExists
                });
            }
            catch (DomainExceptions ex)
            {
                return NotFound(Responses.DomainErrorMessage(ex.Message, ex.Errors));
            }
            catch (Exception)
            {
                return StatusCode(500, Responses.ApplicationErrorMessage());
            }
        }

        [HttpGet("/api/v1/users/search-by-email/{email}")]
        [Authorize]
        public async Task<IActionResult> UserSerachEmailAsync(string email)
        {
            try
            {
                var userExists = await _userService.SearchByEmail(email);

                return Ok(new ResultViewModel
                {
                    Message = "User successfully retrieved",
                    Success = true,
                    Data = userExists
                });
            }
            catch (DomainExceptions ex)
            {
                return NotFound(Responses.DomainErrorMessage(ex.Message, ex.Errors));
            }
            catch (Exception)
            {
                return StatusCode(500, Responses.ApplicationErrorMessage());
            }
        }

        [HttpGet("/api/v1/users/all-users")]
        [Authorize]
        public async Task<IActionResult> UserGetAllAsync()
        {
            try
            {
                var userExists = await _userService.Get();

                return Ok(new ResultViewModel
                {
                    Message = "Successfully retrieved Users",
                    Success = true,
                    Data = userExists
                });
            }
            catch (DomainExceptions ex)
            {
                return NotFound(Responses.DomainErrorMessage(ex.Message, ex.Errors));
            }
            catch (Exception)
            {
                return StatusCode(500, Responses.ApplicationErrorMessage());
            }
        }
    }
}
