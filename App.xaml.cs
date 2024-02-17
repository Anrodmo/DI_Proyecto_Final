using DI_Proyecyo_Final.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace DI_Proyecyo_Final
{
    /// <summary>
    /// Lógica de interacción para App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static MensajeroLogIn MensajeriaLoginCompartida { get; } = new MensajeroLogIn();
        public static MensajeroLogOut MensajeroLogOut { get; } = new MensajeroLogOut();
        public static MensajeroPropietarios2Propiedades EventoPropietarios2Propiedades { get; } = new MensajeroPropietarios2Propiedades();
        public static MensajeroPropiedades2Propietarios EventoPropiedades2Propietarios { get; } = new MensajeroPropiedades2Propietarios();
        public static MensajeroAñadirPropiedad EventoAñadirPropiedad { get; } = new MensajeroAñadirPropiedad();
    }
}
