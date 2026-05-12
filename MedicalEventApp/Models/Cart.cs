using System;
using System.Collections.Generic;

namespace MedicalEventApp.Models;

public partial class Cart
{
    public int Cartid { get; set; }

    public int Memberid { get; set; }

    public int Amount { get; set; }

    public int Pid { get; set; }

    public virtual Employee Member { get; set; } = null!;

    public virtual Product PidNavigation { get; set; } = null!;
}
