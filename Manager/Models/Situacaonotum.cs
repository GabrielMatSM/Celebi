using System;
using System.Collections.Generic;

namespace Manager.Models;

public partial class Situacaonotum
{
    public int Situacaonotaid { get; set; }

    public string Descricao { get; set; } = null!;

    public virtual ICollection<Notafiscal> Notafiscals { get; set; } = new List<Notafiscal>();
}
