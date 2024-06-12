using System;
using System.Collections.Generic;

namespace TTP_2_4.Models;

public partial class Task
{
    public int Taskid { get; set; }

    public int Userid { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public DateTime Duedate { get; set; }

    public virtual User User { get; set; } = null!;
}
