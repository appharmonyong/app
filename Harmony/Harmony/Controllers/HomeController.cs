using Harmony.Bussiness.Services.Contracts;
using Harmony.Bussiness.Services.UserCases;
using Harmony.Bussiness.ViewModel;
using Harmony.Presentation.Main.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;

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
                new CarouselItem {ImageUrl = "https://i.ibb.co/D4qC3vQ/harmony1.jpg", CaptionTitle = "Emplead@s", CaptionBody = "Nuestros empleados verificados", HasButtons = true},

                new CarouselItem {ImageUrl = "https://i.ibb.co/tBKNfzT/harmony1.jpg", CaptionTitle = "Servicios", CaptionBody = "Ofrecemos los mejores servicios", HasButtons = false},
                new CarouselItem {ImageUrl = "https://i.ibb.co/D4qC3vQ/harmony1.jpg", CaptionTitle = "Harmony", CaptionBody = "Cuenta con nosotros", HasButtons = false},
                new CarouselItem {ImageUrl = "https://i.ibb.co/tBKNfzT/harmony1.jpg", CaptionTitle = "Servicios", CaptionBody = "Ofrecemos los mejores servicios", HasButtons = false},
            };

            //Carga los datos para mandarlo a vista
            var viewModel = new IndexViewModel
            {
                Users = (List<UserVm>)users,
                CarouselItems = carouselItems,
                Employees = await GetEmployees(),
                EEmployees = await GetEmployees(),
            };

            if (await IsUserLogin())
            {

                TempData["IsUserLogIn"] = "Auth";
            }
            else
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


        private async Task<Employee> GetEmployee()
        {
            List<string> descripciones = new List<string>
        {
            "Persona amante de los animales.",
            "Apasionado por la música y la naturaleza.",
            "Entusiasta del deporte y la vida activa.",
            "Aficionado a la lectura y la cultura.",
            "Amante de la tecnología y la innovación.",
            "Adicto a los viajes y la exploración.",
            "Defensor de la justicia social y los derechos humanos.",
            "Aventurero en busca de nuevos desafíos.",
            "Creativo y apasionado por las artes.",
            "Entusiasta del cine y las series de televisión.",
            "Amante de la buena comida y la gastronomía.",
            "Inspirado por el aprendizaje continuo y la educación.",
            "Comprometido con el medio ambiente y la sostenibilidad.",
            "Explorador de nuevas tecnologías y tendencias.",
            "Defensor de la igualdad y la diversidad.",
            "Emprendedor en constante búsqueda de oportunidades.",
            "Fiel amante de los deportes extremos.",
            "Apasionado por la fotografía y la captura de momentos.",
            "Comprometido con la ayuda humanitaria y la caridad.",
            "Explorador de la espiritualidad y la meditación."
        };

            using (HttpClient client = new HttpClient())
            {
                string apiUrl = "https://randomuser.me/api/";

                HttpResponseMessage response = await client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {



                    try
                    {

                        string json = await response.Content.ReadAsStringAsync();
                        Root result = JsonConvert.DeserializeObject<Root>(json);
                        List<Employee> employees = result.results.ToList();
                        if (employees.Count > 0)
                        {
                            var employee = employees.First();

                            employee.Description = GetDescripcion(descripciones);

                            return employee;
                        }

                    }
                    catch (Exception ex)
                    {

                    }
                }
                return await GetEmployee();

            }
        }

        public async Task<List<Employee>> GetEmployees()
        {

            List<Employee> employees = new List<Employee>();

            while (employees.Count < 6)
            {

                employees.Add(await GetEmployee());
            }

            return employees;

        }

        static string GetDescripcion(List<string> descripciones)
        {
            int DESCRIPTION_LENGTH = descripciones.Count;
            StringBuilder descripcion = new StringBuilder();
            Random random = new Random();

            while (descripcion.ToString().Length <= 250)
            {
                int indiceAleatorio = random.Next(0, DESCRIPTION_LENGTH);
                descripcion.Append(" " + descripciones[indiceAleatorio]);
            }

            return descripcion.ToString();
        }
    }
}