using System;
using System.Collections;
using System.Collections.Generic;

namespace Manager.Models;

public partial class Cliente
{
    public int Clienteid { get; set; }

    public int Pessoaid { get; set; }

    public double Limitecredito { get; set; }

    public BitArray Ativo { get; set; } = null!;

    public BitArray Dividaativa { get; set; } = null!;

    public virtual ICollection<Contapendente> Contapendentes { get; set; } = new List<Contapendente>();

    public virtual Pessoa Pessoa { get; set; } = null!;
}
