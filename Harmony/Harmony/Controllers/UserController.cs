using Harmony.Bussiness.Services.Contracts;
using Harmony.Bussiness.ViewModel;
using Harmony.Common.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Harmony.Presentation.Main.Controllers
{
    public class UserController : Controller
    {
        //Inicio de mi codigo
        private IUserServices _userServices;
        private IBase _base;
        private IDeleteFlagEntity _deleteFlagEntity;
        
        public UserController(IUserServices userServices)
        {
            _userServices = userServices;
        }
        

        public async Task<ActionResult> Index()
        {
            var allUsers= await _userServices.GetCertainProperties();
            return View(allUsers);
        }

        //El objetivo de esta accion es que cuando se presione el boton se envíe en conjunto con la id de la entidad.
        //Entonces se procede a definir el tipo de cuenta de usuario.
        //Pero se deberia retornar una vista parcial o similar con un mensaje de que el usuario ha sido elimando con exito.
        [HttpPost]
        public async Task<ActionResult> DeleteUsers(int id)
        {
            await _userServices.GetEType(id);
            return View();
        }

        //Final de mi codigo

    }
}
