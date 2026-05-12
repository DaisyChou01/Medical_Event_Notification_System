using System;
using System.Collections.Generic;

namespace MedicalEventApp.Models;

public partial class ProjectMember
{
    public int ProjectId { get; set; }

    public int TeamMid { get; set; }

    public int Id { get; set; }

    public virtual Project Project { get; set; } = null!;

    public virtual Employee TeamM { get; set; } = null!;
}
