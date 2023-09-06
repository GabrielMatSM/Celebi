using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Manager.Models;
using Microsoft.Extensions.WebEncoders.Testing;

namespace Manager.Controllers
{
    public class EstoqueController : Controller
    {
        private readonly PostgresContext _context;

        public EstoqueController(PostgresContext context)
        {
            _context = context;
        }
        public void TesteLeitura()
        {
            var teste = _context.Produtos.ToList();
        }
        public void TesteAdicao()
        {
            
        }

        
    }
}
