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
        private void clearMessages()
        {

            TempData["Success"] = null;
            TempData["Error"] = null;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            clearMessages();
            if (await IsUserAdmin())
            {

                int userId = HttpContext.Session.GetInt32("UserId") ?? 0;

                var user = await _userServices.GetById(userId);
                return View(user);
            }

            return RedirectToAction("Index", "Home");
        }


        [HttpGet]
        public async Task<IActionResult> UsersList()
        {   
            if (await IsUserAdmin())
                return View(await _userServices.Get());


            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> CreateAdmin()
        {
            if (await IsUserAdmin())
                return View();

            return RedirectToAction("Index", "Home");

        }

        [HttpPost]

        public async Task<IActionResult> CreateAdmin(UserRegisterVm model)
        {
            clearMessages();
            if (await IsUserAdmin())
            {
                try
                {
                    if (!ModelState.IsValid)
                    {
                        return View(model);
                    }
                    model.userType = Common.EUserTypes.Administrator;
                    await _userServices.Register(model);

                    TempData["Success"] = "Usuario creado con exito.";
                    return View();
                }
                catch (ArgumentException ex)
                {
                    TempData["Error"] = ex.Message;
                    return RedirectToAction("CreateAdmin");
                }
            }


            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateAdmin(UserVm model, string? actionRedirect = "Index")
        {
            clearMessages();
            if (await IsUserAdmin())
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
                    return RedirectToAction(actionRedirect, "Admin");
                }
                catch (ArgumentException ex)
                {
                    TempData["Error"] = ex.Message;
                    return RedirectToAction(actionRedirect);
                }
            }


            return RedirectToAction(actionRedirect, "Admin");
        }


        [HttpGet]
        public async Task<IActionResult> DeleteAdmin(int id)
        {
            try
            {

                if (await IsUserAdmin())
                {
                    await _userServices.Delete(id);
                    TempData["Success"] = "Usuario eliminado con exito.";
                    Thread.Sleep(3000);
                }

            }
            catch (ArgumentException ex)
            {
                TempData["Error"] = ex.Message;
            }
            return RedirectToAction("UsersList", "Admin");
        }

        [HttpGet]
        public async Task<IActionResult> UpdateUser(int id)
        {


            if (await IsUserAdmin())
            {
                UserVm user = await _userServices.GetById(id);
                return View(user);
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUser(UserVm user)
        {


            if (await IsUserAdmin())
            {
                await UpdateAdmin(user, "UsersList");
                return RedirectToAction("UsersList", "Admin");
            }

            return RedirectToAction("Index", "Admin");
        }
    }
}

