using System;
using System.Collections.Generic;

namespace MedicalEventApp.Models;

public partial class ViewOpdDoctorMonthly
{
    public string YearMonth { get; set; } = null!;

    public string YearQuater { get; set; } = null!;

    public int DeptId { get; set; }

    public string Opddept { get; set; } = null!;

    public int? ParentId { get; set; }

    public string OpddocName { get; set; } = null!;

    public int? TotalVisits { get; set; }

    public int? FirstVisits { get; set; }

    public decimal? FirstVisitRate { get; set; }
}
