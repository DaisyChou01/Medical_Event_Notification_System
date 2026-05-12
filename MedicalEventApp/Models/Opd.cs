using System;
using System.Collections.Generic;

namespace MedicalEventApp.Models;

public partial class Opd
{
    public string OpdDate { get; set; } = null!;

    public int Chartno { get; set; }

    public string Name { get; set; } = null!;

    public int Age { get; set; }

    public string Account { get; set; } = null!;

    public string IsFirstTime { get; set; } = null!;

    public int DeptId { get; set; }

    public string Opddept { get; set; } = null!;

    public string OpdsubDept { get; set; } = null!;

    public string OpddocName { get; set; } = null!;
}
