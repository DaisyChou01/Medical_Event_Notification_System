using System;
using System.Collections.Generic;

namespace MedicalEventApp.Models;

public partial class Inpt
{
    public string YearMonth { get; set; } = null!;

    public string Department { get; set; } = null!;

    public int Beds { get; set; }

    public int Visits { get; set; }

    public int BedDays { get; set; }

    public decimal OccRate { get; set; }

    public decimal Alos { get; set; }

    public virtual Ym YearMonthNavigation { get; set; } = null!;
}
