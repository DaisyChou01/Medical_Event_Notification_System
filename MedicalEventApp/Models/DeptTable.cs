using System;
using System.Collections.Generic;

namespace MedicalEventApp.Models;

public partial class DeptTable
{
    public int DeptId { get; set; }

    public string DeptName { get; set; } = null!;

    public int? ParentId { get; set; }

    public int DeptLevel { get; set; }
}
