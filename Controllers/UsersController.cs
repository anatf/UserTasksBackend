using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using UserTasksBackend.Models;
using UserTasksBackend.Services;

namespace UserTasksBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        private readonly IUserService _service;

        public UsersController(ILogger<UsersController> logger, IUserService service)
        {
            _logger = logger;
            _service = service;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            try
            {
                var users = await _service.GetUsersAsync();

                if (users == null || users.Count() == 0)
                    return NotFound();

                return Ok(users);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "error in GetUsers");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet]
        [Route("withTaskCount")]
        public async Task<ActionResult<IEnumerable<UserWithTaskCount>>> GetUsersWithTaskCount()
        {
            try
            {
                var users = await _service.GetUsersWithTaskCountAsync();

                if (users == null || users.Count() == 0)
                    return NotFound();

                return Ok(users);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "error in GetUsersWithTaskCount");
                return StatusCode(500, "Internal Server Error");
            }
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            try
            {
                var task = await _service.GetUserAsync(id);

                if (task == null)
                {
                    return NotFound();
                }

                return Ok(task);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "error in GetUser");
                return StatusCode(500, "Internal Server Error");
            }
        }

       

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            try
            {
                await _service.AddUserAsync(user);
                return CreatedAtAction("GetTask", new { id = user.Id }, user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "error in PostUser");
                return StatusCode(500, "Internal Server Error");
            }          
        }
    }
}
