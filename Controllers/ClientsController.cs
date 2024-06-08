using MessagesExchange.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace MessagesExchange.Controllers
{
    public class ClientsController : Controller
    {
        private readonly ILogger<ClientsController> _logger;

        public ClientsController(ILogger<ClientsController> logger)
        {
            _logger = logger;
        }

        public IActionResult Sender()
        {
            return View();
        }

        public IActionResult RealTimeReader()
        {
            return View();
        }

        public IActionResult Reader()
        {
            return View();
        }
    }
}
