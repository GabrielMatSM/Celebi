using Microsoft.AspNetCore.Mvc;
using Manager.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Manager.Controllers;

public class EstoqueController : Controller
{
    private readonly PostgresContext _context;
    public EstoqueController(PostgresContext context)
    {
        _context = context;
    }
    #region Views
    public ActionResult Index()
    {
        return (ActionResult)GetProdutoList();
    }

    public Produto? Detail(int? id)
    {
        Produto oproduto;
        if(id != null && id != 0)
        {
            oproduto = _context.Produtos.Where(p => p.Produtoid == id).FirstOrDefault();
            if(oproduto == null)
            {
                oproduto = new Produto();
            }
        }
        else
        {
            oproduto = new Produto();
        }
        return oproduto;
    }
    #endregion

    #region Produto
    //Método para detalhe pra pegar um produto individualmente, caso não venha nenhum é um cadastro novo.
    public Produto? GetProduto(int? id)
    {
        if (id != null)
        {
            Produto? oProduto = _context.Produtos.Find(id);
            if (oProduto == null)
            {
                return null;
            }
            return oProduto;
        }
        return null;
    }
    public object SaveProduto(int id, string descricao, float preco,  int quantidade)
    {
        try
        {   
            bool newRecord = false;
            Produto? oProduto;
            if (id == null || id == 0)
            {
                newRecord = true;
            }
            if (newRecord)
            {
                oProduto = new Produto();
            }
            else
            {
                oProduto = _context.Produtos.Where(f => f.Produtoid == Convert.ToInt32(id)).FirstOrDefault();
                if (oProduto == null) { throw new Exception("O Produto não foi encontrado!"); }
            }
            oProduto.Descricao = descricao;
            if (preco == 0)
            {
                throw new Exception("Preço Inválido");
            }
            else
            {
                oProduto.Preco = (decimal)preco;
            }
            if (quantidade < 0)
            {
                quantidade = 0;
            }
            oProduto.Quantidadeemestoque = quantidade;
   
            if (newRecord)
            {
                _context.Produtos.Add(oProduto);
            }
            _context.SaveChanges();
            
            return new { Success = true, Response = "Produto salvo com sucesso!" };
            //Olhar nos projetos qual q é a classe q usa pra response pq eu n lembro

        }
        catch (Exception ex)
        {
            return new { Success = false, Response = ex.Message };
        }
    } 
    public IActionResult GetProdutoList(int pagina = 1, bool pegarExcluidos = false, string descricao = "")
    {
        int quantidadePorPagina = 50;
        ModelListaProdutos lista = new ModelListaProdutos();
        lista.total = _context.Produtos.Where(l=> pegarExcluidos || l.Ativo == true).Count();
        lista.produtos = _context.Produtos.
            Where(l => pegarExcluidos || l.Ativo == true).
            Skip((pagina - 1) * quantidadePorPagina).
            Take(quantidadePorPagina).
            OrderByDescending(a => a.Produtoid).ToList();
        return View("Index", lista);
    }
    public object DeleteProduto(int id)
    {

        var Produto = _context.Produtos.Where(l => l.Produtoid == id).FirstOrDefault();
        if (Produto == null)
        {
            return new { Success = false, Response = "Produto não encontrado" };
        }
        else
        {
            Produto.Ativo = false;
            _context.SaveChanges();
            return new { Success = true, Response = "Produto removido!" };
        }
    }

    public void AtivaProdutos()
    {
        var produtos = _context.Produtos.ToList();
        foreach(var produto in produtos)
        {
            produto.Ativo = true;
        }
        _context.SaveChanges();
    }
    #endregion

}