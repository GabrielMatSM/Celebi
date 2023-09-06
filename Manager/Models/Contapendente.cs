using System;
using System.Collections.Generic;

namespace Manager.Models;

public partial class Contapendente
{
    public int Contapendenteidint { get; set; }

    public int Notafiscalid { get; set; }

    public DateOnly Dataprevistadepagamento { get; set; }

    public double Valorpendente { get; set; }

    public int Clienteid { get; set; }

    public virtual Cliente Cliente { get; set; } = null!;

    public virtual Notafiscal Notafiscal { get; set; } = null!;
}
