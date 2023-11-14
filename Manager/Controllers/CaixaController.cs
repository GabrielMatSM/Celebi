using Manager.Models;
using Manager.Util;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.Text.Json.Nodes;

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

        public object AdicionaItemNota(int produtoId, int quantidade, int notaFiscalId)
        {
            try
            {
                var produto = _context.Produtos.Find(produtoId);

                if (produto == null) { throw new Exception("Produto não encontrado!"); }
                var itemPedido = new Itempedido();
                itemPedido.Quantidade = quantidade;
                itemPedido.Produtoid = produto.Produtoid;
                itemPedido.Precototal = produto.Preco * quantidade;
                itemPedido.Notafiscalid = notaFiscalId;
                if(produto.Quantidadeemestoque < quantidade) {
                    produto.Quantidadeemestoque = 0;
                }
                else produto.Quantidadeemestoque -= quantidade;

               _context.Itempedidos.Add(itemPedido);
               _context.SaveChanges();

                return new { success = true, content = "Item cadastrado com sucesso" };
                
            }
            catch (Exception ex)
            {
                return new { success = false, content = ex.Message };
            }

        }
        public int CriarNotaFiscal(float ValorPago = 0)
        {
            Notafiscal oNota = new Notafiscal();
            oNota.Situacaonotaid = (int)SituacaoNotaEnum.NaoPago;
            oNota.Data = DateTime.Now;
            oNota.Valorpago = 0;
            oNota.Valortotal = 0;
            _context.Notafiscals.Add(oNota);
            _context.SaveChanges();
            return oNota.Notafiscalid;
        }

        public void CancelaNota(int notaId)
        {
            var nota = _context.Notafiscals.Find(notaId);
            if (nota != null)
            {
                var idItens = _context.Itempedidos.Where(i => i.Notafiscalid == nota.Notafiscalid).Select(i=> i.Itempedidoid).ToList();
                foreach(var id in idItens)
                {
                    var item = _context.Itempedidos.Find(id);
                    if(item!= null )_context.Itempedidos.Remove(item);
                }
                var pendencia = _context.Contapendentes.Where(c => c.Notafiscalid == nota.Notafiscalid).FirstOrDefault();
                if(pendencia != null)
                _context.Contapendentes.Remove(pendencia);
                _context.Notafiscals.Remove(nota);
                _context.SaveChanges();
            }
        }
        public object FinalizarCompra(List<ItemCaixa> itens, string cpf, decimal valorPago, bool parcelado)
        {
            int notaFiscalId = 0;
            try
            {
                int clienteId = 0;
                Pessoa? pessoa;
                if (parcelado)
                {
                    pessoa = _context.Pessoas.Where(l => l.Cpf == cpf).FirstOrDefault();
                    if (pessoa == null) throw new Exception("Cliente não cadastrado");
                    clienteId = _context.Clientes.Where(l => l.Pessoaid == pessoa.Pessoaid).Select(c => c.Clienteid).FirstOrDefault();
                    DateTime maioridade = Convert.ToDateTime(pessoa.Datanascimento).AddYears(18);
                    bool maiorDeIdade = maioridade < DateTime.Now;
                    if(!maiorDeIdade)
                    {
                        throw new Exception("Apenas maiores de idade podem comprar parcelado");
                    }
                }
                notaFiscalId = CriarNotaFiscal();
                Notafiscal? nota = _context.Notafiscals.Find(notaFiscalId);
                if (nota == null)
                {
                    throw new Exception("Não foi possível criar a nota");
                }
                foreach (var item in itens)
                {
                    AdicionaItemNota(item.Id, item.Quantidade, notaFiscalId);
                }
                decimal valorTotal = _context.Itempedidos.Where(l => l.Notafiscalid == notaFiscalId).Sum(l => l.Precototal);
                decimal valorPendente = valorTotal - valorPago;
                nota.Valortotal = valorTotal;
                if (!parcelado)
                {
                    if(valorPendente > 0)
                    {
                        throw new Exception("Pagamento insuficiente");
                    }
                    nota.Valortotal = valorTotal;
                    nota.Valorpago = valorPago;
                    nota.Situacaonotaid = (int)SituacaoNotaEnum.PagoTotalmente;
                    
                }
                else 
                {
                    
                    nota.Situacaonotaid = valorPendente == valorTotal ? (int)SituacaoNotaEnum.NaoPago : (int)SituacaoNotaEnum.PagoParcial;
                    Contapendente novaDivida = new Contapendente();
                    novaDivida.Notafiscalid = notaFiscalId;
                    novaDivida.Clienteid = clienteId;
                    novaDivida.Valorpendente = valorPendente;
                    novaDivida.Dataprevistadepagamento = DateTime.Now.AddMonths(1);
                    _context.Add(novaDivida);
                }
                _context.SaveChanges();
                return new { success = true, result = "Compra finalizada com sucesso!" };
            }
           
            catch(Exception ex)
            {
                if (notaFiscalId != 0)
                    CancelaNota(notaFiscalId);
                return new { success = false, result = ex.Message }; 
            }
        }
        public object CadastrarCliente(string? nomeCompleto="",  string rg="", string cpf = "", string endereco = "", string cidade = "",
            string cep = "", string? contato="", DateTime? dataNascimento = null)
        {

            try
            {
                bool clienteJaCadastrado = _context.Pessoas.Where(p => p.Cpf == cpf).FirstOrDefault() != null;
                if(clienteJaCadastrado)
                throw new Exception("Cliente já cadastrado");
                Pessoa oPessoa = new Pessoa();
                oPessoa.Nome = nomeCompleto;
                oPessoa.Cpf = cpf;
                oPessoa.Cep = cep;
                oPessoa.Endereco = endereco;
                oPessoa.Cidade = cidade;
                oPessoa.Rg = rg;
                oPessoa.Contato = contato;
                oPessoa.Datanascimento = dataNascimento;
                _context.Add(oPessoa);
                _context.SaveChanges();
                Cliente oCliente = new Cliente();
                oCliente.Pessoaid = oPessoa.Pessoaid;
                oCliente.Dividaativa = false;
                oCliente.Ativo = true;
                oCliente.Limitecredito = 1000;
                _context.Add(oCliente);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return new { success = false, result = ex.Message };
            }
            return new { success = true, result = "Cliente cadastrado com sucesso" };
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
