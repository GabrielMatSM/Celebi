using System;
using System.Collections.Generic;

namespace Manager.Models;

public partial class Pedidocompra
{
    public int Pedidocompraid { get; set; }

    public int? Funcionarioid { get; set; }

    public DateOnly Datacompra { get; set; }

    public double Valortotal { get; set; }

    public DateOnly? Dataentrega { get; set; }

    public string Fornecedor { get; set; } = null!;

    public DateOnly? Dataentregue { get; set; }

    public virtual Funcionario? Funcionario { get; set; }
}
