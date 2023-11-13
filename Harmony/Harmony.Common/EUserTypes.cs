using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harmony.Common
{
    public enum EUserTypes
    {
        [Description("Administrador")]
        Administrator = 1,
        [Description("Cliente")]
        Customer,
        [Description("Trabajador")]
        Worker,

    }
}
