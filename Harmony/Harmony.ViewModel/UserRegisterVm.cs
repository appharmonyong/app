using Harmony.Common;

namespace Harmony.Bussiness.ViewModel
{
    public class UserRegisterVm
    {
        public string FirstName { get; set; } = string.Empty!;
        public string LastName { get; set; } = string.Empty!;
        public string Email { get; set; } = string.Empty!;
        public string Password { get; set; } = string.Empty!;
        public EUserTypes userType { get; set; }
    }
}