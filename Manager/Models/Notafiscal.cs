using System;
using System.Collections.Generic;

namespace Manager.Models;

public partial class Notafiscal
{
    public int Notafiscalid { get; set; }

    public int? Funcionarioid { get; set; }

    public DateTime Data { get; set; }

    public decimal Valortotal { get; set; }

    public int? Situacaonotaid { get; set; }

    public decimal Valorpago { get; set; }

    public virtual ICollection<Contapendente> Contapendentes { get; set; } = new List<Contapendente>();

    public virtual Funcionario? Funcionario { get; set; }

    public virtual ICollection<Itempedido> Itempedidos { get; set; } = new List<Itempedido>();

    public virtual Situacaonotum? Situacaonota { get; set; }
}
