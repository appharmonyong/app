﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harmony.Bussiness.ViewModel
{
    public class UserUpdateVm
    {
        public int Id { get; set; }
        public string FirstName { get; set; } 
        public string LastName { get; set; }
        public string? Phone { get; set; }
        public DateTime? BirthDay { get; set; }
    }
}
