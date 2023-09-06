using System;
using System.Collections.Generic;

namespace Manager.Models;

public partial class Itempedido
{
    public int Itempedidoid { get; set; }

    public int Produtoid { get; set; }

    public int Notafiscalid { get; set; }

    public int Quantidade { get; set; }

    public double Precototal { get; set; }

    public virtual Notafiscal Notafiscal { get; set; } = null!;

    public virtual Produto Produto { get; set; } = null!;
}
