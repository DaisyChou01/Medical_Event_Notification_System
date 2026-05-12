using System;
using System.Collections.Generic;

namespace MedicalEventApp.Models;

public partial class Inf
{
    public string YearMonth { get; set; } = null!;

    public DateOnly Date { get; set; }

    public string PatientId { get; set; } = null!;

    public string Department { get; set; } = null!;

    public string AttendingPhysician { get; set; } = null!;

    public string InfectionSite { get; set; } = null!;

    public string Pathogen { get; set; } = null!;

    public int BedDaysContext { get; set; }

    public virtual Ym YearMonthNavigation { get; set; } = null!;
}
