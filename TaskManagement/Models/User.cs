namespace TaskManagement.Models;

using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

public class User : IdentityUser
{
    public int Id { get; set; }
    public string FullName { get; set; }

    // Relationships
    public List<Task> Tasks { get; set; }
}

