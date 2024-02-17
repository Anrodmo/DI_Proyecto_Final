using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DI_Proyecyo_Final.ViewModel
{
    public class MensajeroPropiedades2Propietarios
    {
        public event EventHandler Propiedades2Propietarios;
        private static String nif = null;

        public static String Nif { get => nif; set => nif = value; }

        public void OnPropiedades2Propietarios()
        {
            Propiedades2Propietarios?.Invoke(this, EventArgs.Empty);
        }
    }
}
