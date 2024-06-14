using Microsoft.AspNetCore.Mvc;

namespace MessagesExchange.Controllers
{
    public class ClientsController : Controller
    {
        public ClientsController()
        {
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
