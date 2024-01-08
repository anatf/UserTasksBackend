using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserTasksBackend.Models;
using UserTasksBackend.Services;

namespace UserTasksBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ILogger<TasksController> _logger;
        private readonly ITaskService _service;

        public TasksController(ILogger<TasksController> logger, ITaskService service)
        {
            _logger = logger;
            _service = service;
        }

        // GET: api/Tasks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Models.Task>>> GetTasks()
        {
            try
            {
                var tasks = await _service.GetTasksAsync();

                if (tasks == null || tasks.Count()==0)
                    return NotFound();

                return Ok(tasks);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "error in GetTasks");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet]
        [Route("withUserNames")]
        public async Task<ActionResult<IEnumerable<TaskWithUserName>>> GetTasksWithUserNames()
        {
            try
            {
                var tasks = await _service.GetTasksWithUserNamesAsync();

                if (tasks == null || tasks.Count() == 0)
                    return NotFound();

                return Ok(tasks);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "error in GetTasksWithUserNames");
                return StatusCode(500, "Internal Server Error");
            }
        }

        // GET: api/Tasks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Models.Task>> GetTask(int id)
        {
            try
            {
                var task = await _service.GetTaskAsync(id);

                if (task == null)
                {
                    return NotFound();
                }

                return Ok(task);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "error in GetTask");
                return StatusCode(500, "Internal Server Error");
            }            
        }

        // POST: api/Tasks
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Models.Task>> PostTask(Models.Task task)
        {
            try
            {
                if (await _service.UserHasLessThan10TasksAsync(task.UserId)==false)
                    return Ok(new { success = false, message = "Tasks limit exceeded for the given user" });

                if (await _service.IsTaskWithGivenDescriptionExist(task))
                    return Ok(new { success = false, message = "Task with a given description already exists for that user" });

                await _service.AddTaskAsync(task);
                return CreatedAtAction("GetTask", new { id = task.Id }, task);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "error in PostTask");
                return StatusCode(500, "Internal Server Error");
            }
        }


        // GET: api/Tasks/overdue
        [HttpGet]
        [Route("overdue")]
        public async Task<ActionResult<IEnumerable<Models.Task>>> GetOverdueTasks(DateTime date)
        {
            try
            {
                var tasks = await _service.GetOverdueTasksAsync(date);

                if (tasks == null || tasks.Count() == 0)
                    return NotFound();

                return Ok(tasks);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "error in GetOverdueTasks");
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}
