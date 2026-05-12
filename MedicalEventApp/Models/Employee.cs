using System;
using System.Collections.Generic;

namespace MedicalEventApp.Models;

public partial class Employee
{
    public int Empid { get; set; }

    public string Role { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public string? Dept { get; set; }

    public string Password { get; set; } = null!;

    public string? Profession { get; set; }

    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    public virtual ICollection<ProjectMember> ProjectMembers { get; set; } = new List<ProjectMember>();
}
