using Harmony.Bussiness.Services.Contracts;
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
        public IActionResult Login()
        {
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
                    HttpContext.Session.Set("UserId", BitConverter.GetBytes(user.Id));

                    // Redirigir a la página principal u otra página después del inicio de sesión.
                    return RedirectToAction("Index", "Home");
                }
                catch (ArgumentNullException ex)
                {
                    // Manejar errores, por ejemplo, establecer un mensaje de error en TempData.
                    TempData["Error"] = ex.Message;
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
                // Lógica de validación del modelo
                if (!ModelState.IsValid)
                {
                    // Manejar errores de validación y volver a mostrar el formulario
                    return View(model);
                }

                // Lógica de registro de usuario
                var newUser = await _userService.Register(model);

                // Autenticar al nuevo usuario si es necesario

                // Redirigir a la página principal u otra página después del registro exitoso.
                return RedirectToAction("Index", "Home");
            }
            catch (ArgumentException ex)
            {
                // Manejar errores, por ejemplo, establecer un mensaje de error en TempData.
                TempData["Error"] = ex.Message;
                return RedirectToAction("Register");
            }
        }
    }
}




