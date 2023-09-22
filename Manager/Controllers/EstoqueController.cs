using Microsoft.AspNetCore.Mvc;
using Manager.Models;


namespace Manager.Controllers;

public class EstoqueController : Controller
{
    private readonly PostgresContext _context;
    public EstoqueController(PostgresContext context)
    {
        _context = context;
    }
    #region Views
    public ActionResult ProdutoList()
    {
        return View();
    }

    public ActionResult ProdutoDetail()
    {

        return PartialView();
    }
    #endregion

    #region Produto
    public Produto? GetProduto(int? id)
    {
        Produto oProduto = _context.Produtos.Find(id);
        if (oProduto == null)
        {
            throw new Exception("Produto não encontrado");
        }
        return oProduto;
    }

    [HttpPost]
    public JsonResult SaveProduto(FormCollection values)
    {
        JsonResult response;
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

            response = new JsonResult(Ok("Produto salvo com sucesso!"));
            //Olhar nos projetos qual q é a classe q usa pra response pq eu n lembro

        }
        catch (Exception ex)
        {
            response = new JsonResult(NotFound(ex.Message));
        }
        return response;
    }
    public IActionResult ProdutoList(int pagina, bool pegarExcluidos = false)
    {
        int quantidadePorPagina = 50;
        using (var PostgresContext = new PostgresContext())
        {
            var total = _context.Produtos.Where(l=> pegarExcluidos || l.Ativo == true).Count();
            List<Produto> produtos = _context.Produtos.
                Where(l => pegarExcluidos || l.Ativo == true).
                Skip((pagina - 1) * quantidadePorPagina).
                Take(quantidadePorPagina).
                OrderByDescending(a => a.Produtoid).ToList();
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
    #endregion

}