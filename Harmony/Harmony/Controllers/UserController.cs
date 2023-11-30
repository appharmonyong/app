using Harmony.Bussiness.Services.Contracts;
using Harmony.Bussiness.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Harmony.Presentation.Main.Controllers
{
    public class UserController : Controller
    {
        //Inicio de mi codigo
        private IUserServices _userServices;
        
        public UserController(IUserServices userServices)
        {
            _userServices = userServices;
        }
        

        public async Task<IActionResult> Index()
        {
            var allUsers= await _userServices.GetCertainProperties();
            return View(allUsers);
        }


        //Final de mi codigo

        // GET: UserController/Delete/5
        public ActionResult Delete(int id)
        {
            var user= _userServices.GetById(id);
            return View(user);
        }

        // POST: UserController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, IFormCollection collection)
        {
            try
            {
                var user= _userServices.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
