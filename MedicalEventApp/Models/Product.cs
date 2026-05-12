using System;
using System.Collections.Generic;

namespace MedicalEventApp.Models;

public partial class Product
{
    public int Pid { get; set; }

    public string Pname { get; set; } = null!;

    public int Price { get; set; }

    public string? Image { get; set; }

    public string? Author { get; set; }

    public string? Publisher { get; set; }

    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();
}
