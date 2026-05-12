using System;
using System.Collections.Generic;

namespace MedicalEventApp.Models;

public partial class Project
{
    public int ProjectId { get; set; }

    public string ProjectName { get; set; } = null!;

    public int CreatorId { get; set; }

    public string? Status { get; set; }

    public string? Indicators { get; set; }

    public string? Summary { get; set; }

    public string? Plan { get; set; }

    public string? Do { get; set; }

    public string? Check { get; set; }

    public string? Action { get; set; }

    public DateOnly? CreateDate { get; set; }

    public DateOnly? DueDate { get; set; }

    public int? ApprovedById { get; set; }

    public DateOnly? ApprovedDate { get; set; }

    public int? Progress { get; set; }

    public DateOnly? LastUpdate { get; set; }

    public string? Note { get; set; }

    public string? FileName { get; set; }

    public DateOnly? UploadDate { get; set; }

    public byte[]? Photo { get; set; }

    public virtual ICollection<ProjectMember> ProjectMembers { get; set; } = new List<ProjectMember>();

    public virtual ICollection<ProjectNotify> ProjectNotifies { get; set; } = new List<ProjectNotify>();
}
