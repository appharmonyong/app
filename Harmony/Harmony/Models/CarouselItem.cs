
//Modelo de los datos del carousel como imagen, algunas cosas como caption y botones
namespace Harmony.Presentation.Main.Models
{
    public class CarouselItem
    {
        public string ImageUrl { get; set; }
        public string CaptionTitle { get; set; }
        public string CaptionBody { get; set; }
        public bool HasButtons { get; set; }
    }
}
