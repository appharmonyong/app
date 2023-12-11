using Harmony.Bussiness.Services.Contracts;
using Harmony.Bussiness.Services.UserCases;
using Harmony.Bussiness.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;


namespace Harmony.Presentation.Main.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserServices _userService;

        public AccountController(IUserServices userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> LogOut()
        {
            if (HttpContext.Session.Get("UserId") != null)
            {
                HttpContext.Session.Remove("UserId");
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public async Task<IActionResult> Login()
        {
            if (HttpContext.Session.GetInt32("UserId") != null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserLogInVm model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userService.SignIn(model);

                    // Autenticación exitosa, puedes realizar acciones adicionales si es necesario.

                    // Por ejemplo, puedes almacenar información del usuario en la sesión.
                    HttpContext.Session.SetInt32("UserId", user.Id);

                    // Redirigir a la página principal u otra página después del inicio de sesión.
                    return RedirectToAction("Index", "Home");
                }
                catch (ArgumentNullException ex)
                {
                    // Manejar errores, por ejemplo, establecer un mensaje de error en TempData.
                    TempData["Error"] = ex.ParamName;
                    return RedirectToAction("Login");
                }
            }

            // Si llegas aquí, algo salió mal, vuelve a mostrar el formulario de inicio de sesión.
            return View("Login", model);
        }

        
        // Método para manejar el registro
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserRegisterVm model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                model.userType = Common.EUserTypes.Customer;
                await _userService.Register(model);

                TempData["Success"] = "Usuario creado con exito.";
                return View();
            }
            catch (ArgumentException ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("CreateAdmin");
            }
        }
    }
}




