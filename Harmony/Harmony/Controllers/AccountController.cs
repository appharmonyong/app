using Harmony.Bussiness.Services.Contracts;
using Harmony.Bussiness.Services.UserCases;
using Harmony.Bussiness.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;


namespace Harmony.Presentation.Main.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserServices _userServices;

        public AccountController(IUserServices userService)
        {
            _userServices = userService;
        }

        private async Task UserLoginOrAdmin()
        {
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

        }

        [HttpGet]
        public async Task<IActionResult> LogOut()
        {
            await UserLoginOrAdmin();
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
            await UserLoginOrAdmin();
            if (HttpContext.Session.GetInt32("UserId") != null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserLogInVm model)
        {
            await UserLoginOrAdmin();
            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userServices.SignIn(model);

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
        public async Task<IActionResult> Register()
        {
            await UserLoginOrAdmin();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserRegisterVm model)
        {
            await UserLoginOrAdmin();
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                model.userType = Common.EUserTypes.Customer;
                await _userServices.Register(model);

                TempData["Success"] = "Usuario creado con exito.";
                return RedirectToAction("LogIn", "Account");

            }
            catch (ArgumentException ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("CreateAdmin");
            }
        }

        [HttpGet]
        public async Task<IActionResult> MyAccount()
        {
            await UserLoginOrAdmin();
            int userId = HttpContext.Session.GetInt32("UserId") ?? 0;

            var user = await _userServices.GetById(userId);
            return View(user);
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


        public async Task<IActionResult> UpdateAdmin(UserVm model, string? actionRedirect = "MyAccount")
        {

            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                await _userServices.Update(model.Id, new UserUpdateVm()
                {
                    Id = model.Id,
                    BirthDay = model.BirthDay,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Phone = model.Phone
                });
                TempData["Success"] = "Usuario actualizado con exito.";
                return RedirectToAction(actionRedirect, "Account");
            }
            catch (ArgumentException ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(actionRedirect);
            }

        }

        [HttpPost]
        public async Task<IActionResult> UpdateUser(UserVm user)
        {   
            await UpdateAdmin(user, "MyAccount");
            return RedirectToAction("MyAccount", "Account");


        }

    }
}




