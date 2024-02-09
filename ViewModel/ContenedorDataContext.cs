using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DI_Proyecyo_Final.ViewModel
{
    /// <summary>
    /// Esta clase es un contenedor para agregar datacontex a los  xqml con los notificadores.
    /// Es neceasio porque solo se puede estableces un datacontex y si quiero asociar más elementos debo crear un contendor con ellos.
    /// </summary>
    internal class ContenedorDataContext
    {
        public NotificacionCambioEnTextBoxes CambioEnTexto { get; set; }
        //public NotificacionFechaCorrecta FechaCorrecta { get; set; }
        public NotificadorToggleButtons notificadorToggleButtons { get; set; }
    }
}
