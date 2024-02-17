using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DI_Proyecyo_Final.ViewModel
{
    public class MensajeroAñadirPropiedad
    {
        public event EventHandler AñadirPropiedad;
        private static String nif = null;

        public static String Nif { get => nif; set => nif = value; }

        public void OnAñadirPropiedad()
        {
            AñadirPropiedad?.Invoke(this, EventArgs.Empty);
        }
    }
}
