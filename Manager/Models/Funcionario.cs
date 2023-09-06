using System;
using System.Collections;
using System.Collections.Generic;

namespace Manager.Models;

public partial class Funcionario
{
    public int Funcionarioid { get; set; }

    public int? Pessoaid { get; set; }

    public string Login { get; set; } = null!;

    public string Senha { get; set; } = null!;

    public BitArray Adm { get; set; } = null!;

    public virtual ICollection<Notafiscal> Notafiscals { get; set; } = new List<Notafiscal>();

    public virtual Pessoa? Pessoa { get; set; }
}
