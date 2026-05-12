using System;
using System.Collections.Generic;

namespace MedicalEventApp.Models;

public partial class Er
{
    public string YearMonth { get; set; } = null!;

    public int Triage { get; set; }

    public int ErTotalVisits { get; set; }

    public int TriageVisits { get; set; }

    public int AdmittedVisits { get; set; }

    public int BoardingVisits { get; set; }

    public virtual Ym YearMonthNavigation { get; set; } = null!;
}
