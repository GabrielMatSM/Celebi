
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.IIS.Core;
using Microsoft.EntityFrameworkCore;

namespace Manager.Controllers;

public class EstoqueController : Controller
{
    //private readonly CelebiContext _context;
    public IActionResult Index()
    {
        return View();
    }
    /*public EstoqueController(CelebiContext context)
    {
        _context = context;
    }
    public void SalvaProduto (FormCollection values)
    {
        bool newRecord = false;
        Produto produto;
        if (values["Produtoid"].ToString() == string.Empty)
        {
            newRecord = true;
        }
        if (newRecord)
        {
            produto = new Produto();
        }
        else
        {
            produto = _context.Produtos.Find(values["ProdutoID"]);
            if (produto == null) { throw new Exception("ID não encontrado"); }
        }
        produto.Descricao = values["Descricao"];
        produto.Preco = 3.5;
        _context.SaveChanges();
        
    }
    public void RemoverProduto(int? id)
    {        
        if(id == null)
        {
            throw new Exception("Forneça um ID válido");
        }
        var produtoParaRemocao = _context.Produtos.FirstOrDefault(f => f.Produtoid == id);
        if(produtoParaRemocao == null)
        {
            throw new Exception("O produto não foi encontrado");
        }
    }
    public List<Produto> RetornaProduto(int? id, int? limit, int? start)
    {
        if (id == null)
        { 
            start = start != null ? start : 0;
            limit = limit != null ? limit : 25;
            return _context.Produtos.Skip((int)start).Take((int)limit).ToList();
        }
        else
        {
            var produtoProcurado = _context.Produtos.Where(p => p.Produtoid == id).ToList();
            if (produtoProcurado != null) return produtoProcurado;
            else
            {
                throw new Exception($"Não foi encontrado nenhum Produto com o ID {id}");
            }
        }
    }*/
}