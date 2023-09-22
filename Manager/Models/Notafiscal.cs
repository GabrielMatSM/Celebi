using System;
using System.Collections.Generic;

namespace Manager.Models;

public partial class Notafiscal
{
    public int Notafiscalid { get; set; }

    public int? Funcionarioid { get; set; }

    public DateOnly Data { get; set; }

    public double Valortotal { get; set; }

    public int? Situacaonotaid { get; set; }

    public double Valorpago { get; set; }

    public virtual ICollection<Contapendente> Contapendentes { get; set; } = new List<Contapendente>();

    public virtual Funcionario? Funcionario { get; set; }

    public virtual ICollection<Itempedido> Itempedidos { get; set; } = new List<Itempedido>();

    public virtual Situacaonotum? Situacaonota { get; set; }
}
