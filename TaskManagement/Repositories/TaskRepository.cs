namespace TaskManagement.Repositories;

using System.Collections.Generic;
using TaskManagement.Data;
using TaskManagement.Models;

public class TaskRepository : ITaskRepository
{
    private readonly List<Task> _tasks = new List<Task>();
    private readonly ApplicationDbContext _dbContext;

    public TaskRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task GetTaskById(int id)
    {
        return _dbContext.Tasks.FirstOrDefault(t => t.Id == id);
    }

    public IEnumerable<Task> GetAllTasks()
    {
        return _dbContext.Tasks.ToList();
    }

    public void CreateTask(Task task)
    {
        _dbContext.Tasks.Add(task);
        _dbContext.SaveChanges();
    }

    public void UpdateTask(Task task)
    {
        var existingTask = _dbContext.Tasks.FirstOrDefault(t => t.Id == task.Id);
        if (existingTask != null)
        {
            existingTask.Title = task.Title;
            existingTask.Description = task.Description;

            _dbContext.SaveChanges();
        }
    }

    public void DeleteTask(Task task)
    {
        _dbContext.Tasks.Remove(task);
        _dbContext.SaveChanges();
    }
}