using System;
using System.Collections.Generic;

namespace MedicalEventApp.Models;

public partial class ProjectMethod
{
    public int ProjectId { get; set; }

    public string Method { get; set; } = null!;

    public string? Step { get; set; }

    public string? Content { get; set; }

    public int? IsCompleted { get; set; }

    public DateOnly? LastUpdate { get; set; }

    public virtual Project Project { get; set; } = null!;
}
