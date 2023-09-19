using Microsoft.AspNetCore.Mvc;
using Manager.Models;


namespace Manager.Controllers;

public class EstoqueController : Controller
{
    private readonly PostgresContext _context;

    public ActionResult ProdutoList()
    {
        return View();
    }

    public ActionResult ProdutoDetail()
    {

        return PartialView();
    }

    public Produto? GetProduto(int? id)
    {
        Produto oProduto = _context.Produtos.Find(id);
        if (oProduto == null)
        {
            throw new Exception("Produto não encontrado");
        }
        return oProduto;
    }
    public EstoqueController(PostgresContext context)
    {
        _context = context;
    }
    [HttpPost]
    public string SaveProduto(FormCollection values)
    {
        try
        {
            bool newRecord = false;
            Produto oProduto;
            if (string.IsNullOrEmpty(values["ProdutoID"]))
            {
                newRecord = true;
            }
            if (newRecord)
            {
                oProduto = new Produto();
            }
            else
            {
                oProduto = _context.Produtos.Where(f => f.Produtoid == Convert.ToInt32(values["ProdutoID"])).FirstOrDefault();
                if (oProduto == null) { throw new Exception("O Produto não foi encontrado!"); }
            }
            oProduto.Descricao = values["Descricao"];
            oProduto.Fornecedor = values["Fornecedor"];
            if (!string.IsNullOrEmpty(values["Preco"]))
            {
                throw new Exception("Preço Inválido");
            }
            else
            {
                oProduto.Preco = Convert.ToDouble(values["Preco"]);
            }
            if (string.IsNullOrEmpty(values["Quantidade"]))
            {
                oProduto.Quantidadeemestoque = 0;
            }
            else
            {
                oProduto.Quantidadeemestoque = Convert.ToInt32(values["Quantidade"]);
            }
            _context.Produtos.Add(oProduto);
            _context.SaveChanges();
            return "Produto salvo com sucesso!";
            //Olhar nos projetos qual q é a classe q usa pra response pq eu n lembro

        }
        catch (Exception ex)
        {
            return $"Ocorreu o seguinte erro: {ex.Message}!";
        }

    }
    public IActionResult ProdutoList(int pagina)
    {
        int quantidadePorPagina = 50;
        using (var PostgresContext = new PostgresContext())
        {
            var total = _context.Produtos.Count();
            List<Produto> produtos = _context.Produtos.Skip((pagina - 1) * quantidadePorPagina).Take(quantidadePorPagina)
                .OrderBy(a => a.Produtoid).ToList();
            return Ok(new { produtos, total });
        }

    }
    public IActionResult DeleteProduto(int id)
    {

        var Produto = _context.Produtos.Where(l => l.Produtoid == id).FirstOrDefault();
        if (Produto == null)
        {
            return NotFound();
        }
        else
        {
            Produto.Ativo = false;
            _context.SaveChanges();
            return NoContent();
        }
    }
}