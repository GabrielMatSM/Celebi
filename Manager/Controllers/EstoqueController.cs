using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Manager.Models;
using Microsoft.Extensions.WebEncoders.Testing;
using Microsoft.AspNetCore.Components.Web;

namespace Manager.Controllers
{
    public class EstoqueController : Controller
    {
        private readonly PostgresContext _context;

        public EstoqueController(PostgresContext context)
        {
            _context = context;
        }
        public void SaveProduto(FormCollection values)
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
                if (string.IsNullOrEmpty(values["Preco"]) 
                    || !(int.TryParse(values["Preco"], out int a)))
                {
                    throw new Exception("Preço inválido!");
                }
                if (string.IsNullOrEmpty(values["Quantidade"]))
                {
                    oProduto.Quantidadeemestoque = 0;
                }
                else
                {
                    oProduto.Quantidadeemestoque = Convert.ToInt32(values["Quantidade"]);
                }
            }
            catch
            {

            }

        }
        public void TesteAdicao()
        {
            
        }
    }
}
