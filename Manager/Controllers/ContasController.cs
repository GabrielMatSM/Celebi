using Manager.Models;
using Manager.Util;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Manager.Controllers
{
    public class ContasController : Controller
    {
        private readonly PostgresContext _context;
        public ContasController(PostgresContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var model = GetContapendentes();
            return View(model);
        }
        
        public List<Devedore> GetContapendentes()
        {
            
            var contas = _context.Devedores.ToList();
            return contas;
        }
        public Devedore GetDividaCliente(int clienteid)
        {
            var caloteiro = _context.Devedores.Where(c => c.Clienteid == Convert.ToInt32(clienteid)).FirstOrDefault();
            return caloteiro;        
        }

        public object quitarDividas(int clienteId, decimal valorPago)
        {
            try
            {

                var contasAtivas = _context.Contapendentes.Where(l => l.Clienteid == clienteId && l.Valorpendente > 0).ToList();
                if (contasAtivas != null)
                {
                    foreach(var conta in contasAtivas)
                    {
                        var nota = _context.Notafiscals.Where(n => n.Notafiscalid == conta.Notafiscalid).FirstOrDefault();
                        if (conta.Valorpendente >= valorPago)
                        {
                            nota.Valorpago += valorPago;
                            conta.Valorpendente -= valorPago;
                            if(nota.Valorpago == nota.Valortotal)
                            {
                                nota.Situacaonotaid = (int)SituacaoNotaEnum.PagoTotalmente;
                            }
                            else
                            {
                                nota.Situacaonotaid = (int)SituacaoNotaEnum.PagoParcial;
                            }
                            break;
                        }
                        else
                        {
                            valorPago -= conta.Valorpendente;
                            conta.Valorpendente = 0;
                            nota.Valorpago = nota.Valortotal;
                        }
                    }
                }
                _context.SaveChanges();
                return new { success = true, content = "Pagamento realizado com sucesso" };
            }
            catch(Exception ex)
            {
                return new { success = false, content = ex.Message };
            }
        }
    }
}
