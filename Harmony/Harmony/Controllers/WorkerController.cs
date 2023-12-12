using Harmony.Bussiness.Services.Contracts;
using Harmony.Bussiness.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Harmony.Presentation.Main.Controllers
{
    public class WorkerController : Controller
    {
        private readonly IUserServices _userServices;

        public WorkerController(IUserServices userServices)
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
        public async Task<IActionResult> CreateWorker()
        {
            if (await IsUserAdmin())
                return View();

            return RedirectToAction("Index", "Home");

        }

        [HttpPost]
        public async Task<IActionResult> CreateWorker(UserRegisterVm model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                model.userType = Common.EUserTypes.Worker;
                await _userServices.Register(model);

                return RedirectToAction("Index", "Home");
            }
            catch (ArgumentException ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("CreateWorker");
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditWorker(int id)
        {
            if (await IsUserAdmin())
            {
                var fetchWorkerUpdate = await _userServices.GetById(id);
                return View(fetchWorkerUpdate);
            }
                
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> EditWorker(int id, UserUpdateVm model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                await _userServices.Update(id, model);

                return RedirectToAction("Index", "Home");
            } catch (ArgumentException ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("CreateWorker");
            }
        }


        [HttpPost]
        public async Task<IActionResult> DeleteWorker(int id)
        {
            if (await IsUserAdmin())
            {
                await _userServices.Delete(id);
            }

            return RedirectToAction("Index", "Home");
        }

        //public async Task<IActionResult> LookupWorker(string name)
        //{
        //    await _userServices.(name);

        //    return View(name);
        //}
        
    }
}
