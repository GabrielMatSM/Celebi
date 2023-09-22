using Manager.Models;
using Microsoft.AspNetCore.Mvc;

namespace Manager.Controllers
{
    public class VendaController : Controller
    {
        private readonly PostgresContext _context;
        public VendaController(PostgresContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult NotaList(int pagina = 1)
        {
            int quantidadePorPagina = 50;
            var total = _context.Notafiscals.Count();
            var listaDeNotas = _context.Notafiscals.OrderByDescending(l => l.Notafiscalid).Skip((pagina - 1) * quantidadePorPagina).Take(quantidadePorPagina).ToList();
            return Ok(new { listaDeNotas, total });
        }
        public IActionResult SaveItemNota(Itempedido item, int notaId)
        { //Inacabado
            try 
            { 
                Itempedido? oItemPedido = _context.Itempedidos.
                    Where(l => l.Produtoid == item.Produtoid && l.Notafiscalid == notaId).
                    FirstOrDefault();
                if (oItemPedido == null)
                {
                    oItemPedido = new Itempedido();
                    _context.Add(oItemPedido);
                }
                oItemPedido.Produtoid = item.Produtoid;
                oItemPedido.Notafiscalid = notaId;
                oItemPedido.Produtoid = item.Produtoid;
                oItemPedido.Quantidade = item.Quantidade;
                _context.SaveChanges();
                return Ok("O item foi salvo com sucesso na nota fiscal!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    
    }
}
