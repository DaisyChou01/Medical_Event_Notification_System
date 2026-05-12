using System;
using System.Collections.Generic;

namespace MedicalEventApp.Models;

public partial class TimeTable
{
    public string YearQuater { get; set; } = null!;

    public string YearMonth { get; set; } = null!;

    public string DataDate { get; set; } = null!;

    public int MonthlyDays { get; set; }

    public string IsWorkingDay { get; set; } = null!;
}
