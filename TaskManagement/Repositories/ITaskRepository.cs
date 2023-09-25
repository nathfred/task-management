namespace TaskManagement.Repositories;

using System.Collections.Generic;
using TaskManagement.Models;

public interface ITaskRepository
{
    Task GetTaskById(int id);
    IEnumerable<Task> GetAllTasks();
    void CreateTask(Task task);
    void UpdateTask(Task task);
    void DeleteTask(Task task);
}