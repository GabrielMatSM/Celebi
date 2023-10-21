using Manager.Models;
using Manager.Util;
using Microsoft.AspNetCore.Mvc;

namespace Manager.Controllers
{
    public class CaixaController : Controller
    {
        private readonly PostgresContext _context;
        public CaixaController(PostgresContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public int CriarNotaFiscal(int FuncionarioId, float ValorPago = 0)
        {
            Notafiscal oNota = new Notafiscal();
            oNota.Funcionarioid = FuncionarioId;
            oNota.Situacaonotaid = (int)SituacaoNotaEnum.NaoPago;
            oNota.Data = DateTime.Today;
            oNota.Valorpago = 0;
            oNota.Valortotal = 0;
            _context.Notafiscals.Add(oNota);
            _context.SaveChanges();
            return oNota.Notafiscalid;

        }
        
        public IActionResult AdicionarItemNota(int idNota, int itemId, int quantidade)
        {
            try
            {
                Produto? produto = _context.Produtos.Where(l => l.Produtoid == itemId).FirstOrDefault();
                if (produto == null)
                {
                    throw new Exception("Produto não encontrado!");
                }
                if (produto.Quantidadeemestoque < quantidade)
                {
                    throw new Exception("Quantidade maior do que temos em estoque!");
                }
                Itempedido oItem = new Itempedido();
                oItem.Notafiscalid = idNota;
            }
            catch
            {

            }
            return Ok();
            
        }
        
        public IActionResult AdicionarItem(int notaID, Itempedido item)
        {
           var Nota = _context.Notafiscals.Where(n => n.Notafiscalid == notaID).FirstOrDefault();
           if (Nota == null) { return NotFound("A Nota Fiscal correspondente não foi encontrada."); }
           item.Notafiscalid = Nota.Notafiscalid;
            return Ok();
        }    
        public IActionResult RemoverItem(int ID)
        {
            using(PostgresContext db = new PostgresContext())
            {
                return Ok();
            }
        }
    }

}
