using System;
using System.Collections.Generic;

namespace Manager.Models;

public partial class Produto
{
    public int Produtoid { get; set; }

    public string? Descricao { get; set; }

    public double Preco { get; set; }

    public int Quantidadeemestoque { get; set; }

    public string? Fornecedor { get; set; }

    public bool? Ativo { get; set; }

    public virtual ICollection<Itempedido> Itempedidos { get; set; } = new List<Itempedido>();
}
