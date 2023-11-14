﻿using System;
using System.Collections.Generic;

namespace Manager.Models;

public partial class Contapendente
{
    public int Contapendenteid { get; set; }

    public int Notafiscalid { get; set; }

    public DateTime Dataprevistadepagamento { get; set; }

    public decimal Valorpendente { get; set; }

    public int Clienteid { get; set; }

    public virtual Cliente Cliente { get; set; } = null!;

    public virtual Notafiscal Notafiscal { get; set; } = null!;
}
