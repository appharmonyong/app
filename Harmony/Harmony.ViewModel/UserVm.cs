using Harmony.Common;
using Microsoft.AspNetCore.Mvc;

namespace Harmony.Bussiness.ViewModel
{
   
    public class UserVm
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty!;
        public string LastName { get; set; } = string.Empty!;
        public string UserName { get; set; } = string.Empty!;
        public string Email { get; set; } = string.Empty!;
        public string Phone { get; set; } = string.Empty!;
        public DateTime? BirthDay { get; set; } 
        public EUserTypes UserType { get; set; }

    }
}