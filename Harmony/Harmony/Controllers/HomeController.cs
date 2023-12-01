using Harmony.Bussiness.Services.Contracts;
using Harmony.Bussiness.ViewModel;
using Harmony.Presentation.Main.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Harmony.Presentation.Main.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserServices _authServices;


        public HomeController(ILogger<HomeController> logger, IUserServices authServices)
        {
            _logger = logger;
            _authServices = authServices;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _authServices.Get();

            //Insertar las fotos que irán en el carousel aquí
            var carouselItems = new List<CarouselItem>
            {
                new CarouselItem {ImageUrl = "https://vibes.okdiario.com/wp-content/uploads/2023/07/pension-seguridad-social-amas.jpg", CaptionTitle = "Emplead@s", CaptionBody = "Nuestros empleados verificados", HasButtons = true},
                new CarouselItem {ImageUrl = "https://whaticket.com/wp-content/uploads/2022/06/clientes-satisfechos-1-1024x577.png", CaptionTitle = "Servicios", CaptionBody = "Ofrecemos los mejores servicios", HasButtons = false},
                new CarouselItem {ImageUrl = "https://plus.unsplash.com/premium_photo-1664910908445-c902d8b80b8c?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxzZWFyY2h8MTJ8fHB1ZWJsbyUyMGphcG9uZXN8ZW58MHx8MHx8fDE%3D&w=1000&q=80", CaptionTitle = "Harmony", CaptionBody = "Cuenta con nosotros", HasButtons = false}
            };

            //Carga los datos para mandarlo a vista
            var viewModel = new IndexViewModel
            {
                Users = (List<UserVm>)users,
                CarouselItems = carouselItems
            };

            return View(viewModel);
        }

        [Authorize]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}