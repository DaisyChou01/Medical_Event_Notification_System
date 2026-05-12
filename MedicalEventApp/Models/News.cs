using System;
using System.Collections.Generic;

namespace MedicalEventApp.Models;

public partial class News
{
    public int Id { get; set; }

    public string Topic { get; set; } = null!;

    public DateOnly Date { get; set; }
}
