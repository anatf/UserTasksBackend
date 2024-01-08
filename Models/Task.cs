using System;
using System.Collections.Generic;

namespace UserTasksBackend.Models;

public partial class Task
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string Description { get; set; } = null!;

    public DateTime DueDate { get; set; }

    public bool Completed { get; set; }
}
