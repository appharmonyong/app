//Modelo que combina los datos del carousel con los de usuario que ya estaban en el proyecto base
using System.Collections.Generic;
using Harmony.Bussiness.ViewModel;
using Harmony.Presentation.Main.Models;

namespace Harmony.Presentation.Main.Models
{
    public class IndexViewModel
    {
        public List<UserVm> Users { get; set; }
        public List<CarouselItem> CarouselItems { get; set; }
    }
}
