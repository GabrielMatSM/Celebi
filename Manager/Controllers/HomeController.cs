using Manager.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using System.Diagnostics;
namespace Manager.Controllers;



public class HomeController : Controller
{

    private readonly PostgresContext _context;
    public HomeController(PostgresContext context)
    {
        this._context = context;
    }

    public ActionResult Login()
    {
        return View();
    }
    public ActionResult Index() 
    {
        return View();
    }
    
    public object SetLogin(string user, string password)
    {
        var funcionario = _context.Funcionarios.Where(l => l.Login == user && l.Senha == password).FirstOrDefault();
        if(funcionario == null)
        {

            
            return new { success = false, response = "Usuário não encontrado" };
        }
        else
        {
            Response.Cookies.Append("logon", "true");
            return new { success = true, response = "Autenticado com sucesso" };
        }
    }
}
