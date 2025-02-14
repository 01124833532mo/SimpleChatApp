using Microsoft.AspNetCore.Mvc;

namespace SignalRProject.Controllers
{
    public class ChatController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
