using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DI_Proyecyo_Final.ViewModel
{
    public class MensajeroLogOut
    {
        public event EventHandler LogOut;
        public void LanzarLogOut()
        {
            LogOut?.Invoke(this, EventArgs.Empty);
        }
    }
}
