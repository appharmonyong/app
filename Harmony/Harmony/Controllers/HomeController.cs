using Harmony.Bussiness.Services.Contracts;
using Harmony.Bussiness.Services.UserCases;
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
        private readonly IUserServices _userServices;

        private readonly IWebHostEnvironment _env;

        public HomeController(ILogger<HomeController> logger, IUserServices authServices, IWebHostEnvironment env)
        {
            _logger = logger;
            _userServices = authServices;
            _env = env;
        }

        public async Task<bool> IsUserLogin()
        {
            try
            {

                int userId = HttpContext.Session.GetInt32("UserId") ?? 0;

                if (userId != 0)
                {
                    var user = await _userServices.GetById(userId);
                    return user != null;
                }
            }
            catch
            {
            }

            return false;
        }



        public async Task<bool> IsUserAdmin()
        {
            if (await IsUserLogin())
            {

                int userId = HttpContext.Session.GetInt32("UserId") ?? 0;

                var user = await _userServices.GetById(userId);

                return user.UserType == Common.EUserTypes.Administrator;
            }

            return false;
        }


        public async Task<IActionResult> Index()
        {
            
            var users = await _userServices.Get();

            //Insertar las fotos que irán en el carousel aquí
            var carouselItems = new List<CarouselItem>
            {
                new CarouselItem {ImageUrl = "https://vibes.okdiario.com/wp-content/uploads/2023/07/pension-seguridad-social-amas.jpg", CaptionTitle = "Emplead@s", CaptionBody = "Nuestros empleados verificados", HasButtons = true},

                new CarouselItem {ImageUrl = "https://i.ibb.co/tBKNfzT/harmony1.jpg", CaptionTitle = "Servicios", CaptionBody = "Ofrecemos los mejores servicios", HasButtons = false},
                new CarouselItem {ImageUrl = "https://i.ibb.co/D4qC3vQ/harmony1.jpg", CaptionTitle = "Harmony", CaptionBody = "Cuenta con nosotros", HasButtons = false}
            };

            //Carga los datos para mandarlo a vista
            var viewModel = new IndexViewModel
            {
                Users = (List<UserVm>)users,
                CarouselItems = carouselItems
            };

            if (await IsUserLogin())
            {

            TempData["IsUserLogIn"] = "Auth";
            } else
            {
                TempData["IsUserLogIn"] = null;
            }


            if (await IsUserAdmin())
            {

                TempData["IsUserAdmin"] = "IsUserAdmin";
            }
            else
            {
                TempData["IsUserAdmin"] = null;
            }

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