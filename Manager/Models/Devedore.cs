using System;
using System.Collections.Generic;

namespace Manager.Models;

public partial class Devedore
{
    public string? Nome { get; set; }

    public decimal? Valortotal { get; set; }

    public decimal? Valorpendente { get; set; }

    public int? Clienteid { get; set; }
}
