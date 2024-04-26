using Microsoft.AspNetCore.Mvc;
using Notifications.ClientUI.Models;
using Notifications.Core.Interfaces;

namespace Notifications.ClientUI.Controllers
{
    public class ClientController(
        ILogger<ClientController> logger,
        IClientService clientService
        ) : Controller
    {
        private readonly ILogger<ClientController> _logger = logger;
        private readonly IClientService _clientService = clientService;

        public IActionResult Index()
        {
            return RedirectToAction("Login");
        }

        public IActionResult Register()
        {
            RegisterModel model = new() { Name = "" };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
               return View(model);
            }

            await _clientService.RegisterClient(model.Name);
            return RedirectToAction("Login");
        }

        public IActionResult Login()
        {
            RegisterModel model = new() { Name = "" };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var client = await _clientService.GetClient(model.Name);
            if (client == null)
            {
                return View(new RegisterModel() { Name = ""});
            }

            Response.Cookies.Append("clientId", client.Id.ToString());
            Response.Cookies.Append("clientName", client.Name);


            return View();
        }
    }
}
