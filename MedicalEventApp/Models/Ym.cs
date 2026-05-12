using System;
using System.Collections.Generic;

namespace MedicalEventApp.Models;

public partial class Ym
{
    public string Yearmonth { get; set; } = null!;

    public virtual Inf? Inf { get; set; }
}
