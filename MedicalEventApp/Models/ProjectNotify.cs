using System;
using System.Collections.Generic;

namespace MedicalEventApp.Models;

public partial class ProjectNotify
{
    public int Id { get; set; }

    public int? ReceiverId { get; set; }

    public string? Message { get; set; }

    public int? ProjectId { get; set; }

    public bool? IsRead { get; set; }

    public DateOnly? CreateDate { get; set; }

    public virtual Project? Project { get; set; }
}
