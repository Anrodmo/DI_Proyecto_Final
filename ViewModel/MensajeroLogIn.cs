using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DI_Proyecyo_Final.ViewModel
{
    public class MensajeroLogIn
    {
        public event EventHandler LoginExitoso;
        public void OnLoginExitoso()
        {
            LoginExitoso?.Invoke(this, EventArgs.Empty);
        }

    }
}
