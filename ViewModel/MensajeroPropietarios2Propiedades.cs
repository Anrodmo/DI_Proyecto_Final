using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DI_Proyecyo_Final.ViewModel
{
    public class MensajeroPropietarios2Propiedades
    {
        public event EventHandler Propietarios2Propiedades;
        private  static String nif = null;

        public static String Nif { get => nif; set => nif = value; }

        public void OnPropietarios2Propiedades()
        {
            Propietarios2Propiedades?.Invoke(this,EventArgs.Empty);
        }
    }
}
