using Microsoft.AspNetCore.Mvc;
using TaskManagement.Repositories;

[Route("api/tasks")]
[ApiController]
public class TaskController : ControllerBase
{
    private readonly ITaskRepository _taskRepository;

    public TaskController(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    // GET: api/tasks
    [HttpGet]
    public ActionResult<IEnumerable<TaskManagement.Models.Task>> GetTasks()
    {
        var tasks = _taskRepository.GetAllTasks();
        return Ok(tasks);
    }

    // GET: api/tasks/{id}
    [HttpGet("{id}")]
    public ActionResult<TaskManagement.Models.Task> GetTask(int id)
    {
        var task = _taskRepository.GetTaskById(id);

        if (task == null)
            return NotFound(new { error = "Task not found." });

        return Ok(task);
    }

    // POST: api/tasks
    [HttpPost]
    public ActionResult<TaskManagement.Models.Task> CreateTask(TaskManagement.Models.Task task)
    {
        if (task == null)
            return BadRequest();

        _taskRepository.CreateTask(task);

        return CreatedAtAction("GetTask", new { id = task.Id }, task);
    }

    // PUT: api/tasks/{id}
    [HttpPut("{id}")]
    public ActionResult<TaskManagement.Models.Task> UpdateTask(int id, TaskManagement.Models.Task task)
    {
        if (_taskRepository.GetTaskById(id) == null)
            return NotFound(new { error = "Task id invalid." });

        var existingTask = _taskRepository.GetTaskById(id);

        if (existingTask == null)
            return NotFound(new { error = "Task not found." });

        _taskRepository.UpdateTask(task);

        return Ok(new { message = "Task updated successfully" });
    }

    // DELETE: api/tasks/{id}
    [HttpDelete("{id}")]
    public ActionResult<TaskManagement.Models.Task> DeleteTask(int id)
    {
        var task = _taskRepository.GetTaskById(id);

        if (task == null)
            return NotFound(new { error = "Task id invalid." });

        _taskRepository.DeleteTask(task);

        return Ok(task);
    }
}
