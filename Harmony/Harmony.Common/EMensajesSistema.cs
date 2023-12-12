using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harmony.Common
{
    public enum EMensajesSistema
    {
        [Description("Usuario no existente")]
        USUARIO_NO_EXISTENTE,

        [Description("Usuario existente. Intente con otro nombre de usuario o correo.")]
        USUARIO_EXISTENTE,

        [Description("Usuario desabilitado o eliminado.")]
        USUARIO_DESHABILITADO_ELIMINADO

    }
}
