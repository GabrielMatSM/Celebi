using System;
using System.Collections.Generic;

namespace Manager.Models;

public partial class Pessoa
{
    public int Pessoaid { get; set; }

    public string Nome { get; set; } = null!;

    public int? Idade { get; set; }

    public string Rg { get; set; } = null!;

    public string Cpf { get; set; } = null!;

    public string? Endereco { get; set; }

    public string? Cidade { get; set; }

    public string? Estado { get; set; }

    public string? Cep { get; set; }

    public string? Contato { get; set; }

    public virtual ICollection<Cliente> Clientes { get; set; } = new List<Cliente>();

    public virtual ICollection<Funcionario> Funcionarios { get; set; } = new List<Funcionario>();
}
