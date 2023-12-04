using Harmony.Bussiness.Services.Contracts;
using Harmony.Bussiness.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Harmony.Presentation.Main.Controllers
{
    public class AdminController : Controller
    {
        public IUserServices _userServices { get; }

        public AdminController(IUserServices userServices)
        {
            _userServices = userServices;
        }

        public async Task<bool> IsUserLogin()
        {
            try
            {

                int userId = HttpContext.Session.GetInt32("UserId") ?? 0;

                if (userId != 0)
                {
                    var user = await _userServices.GetById(userId);
                    if (user != null) return true;
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


        [HttpGet]
        public async Task<IActionResult> CreateUser()
        {
            if (await IsUserAdmin())
                return View();

            return RedirectToAction("Index", "Home");

        }

        [HttpPost]

        public async Task<IActionResult> CreateUser(UserRegisterVm model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                model.userType = Common.EUserTypes.Administrator;
                await _userServices.Register(model);

                return RedirectToAction("Index", "Home");
            }
            catch (ArgumentException ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("CreateUser");
            }
        }
    }
}

