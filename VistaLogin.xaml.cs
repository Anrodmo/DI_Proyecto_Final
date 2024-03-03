using DI_Proyecyo_Final.ViewModel;
using DI_Proyecyo_Final.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Google.Protobuf.WellKnownTypes;
using System.ComponentModel;
using System.Globalization;



namespace DI_Proyecyo_Final
{
    /// <summary>
    /// Lógica de interacción para Login.xaml
    /// </summary>
    public partial class VistaLogin : Page
    {

        private MensajeroLogIn mensajero = App.MensajeriaLoginCompartida; // recojo el manejador de evento para el login correcto.

        public VistaLogin()
        {
            InitializeComponent();         
        }



        /// <summary>
        /// Método que recoge el usuario y contraseña introducido por el usuario par el login y lo transmmite al modelo
        /// para su gestión.
        /// Recoje la respuesta del modelo y según su respuesta lanza el evento OnLoginExitoso() o muestra una ventana de login
        /// incorrecto.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void identificarse_OnClick(object sender, RoutedEventArgs e)
        {
            bool loginCorrecto = false;
            string usuario = txtNombreUsuario.Text;
            if (usuario != null && usuario.Length > 0) // si no hay nombre de usuario ya no hago nada más
            {  // solo  consulto si el login es válido si hay contraseña, si no no.
                if (txtContraseña.SecurePassword != null && txtContraseña.SecurePassword.Length > 0) 
                {        // tranmito al modelo y recojo la respuesta          
                    loginCorrecto = Login.loginUsuario(usuario,txtContraseña.SecurePassword);
                    if (loginCorrecto)  // si es correcto
                    {                      
                        mensajero.OnLoginExitoso(); // <-- evento que se recoge en MainWindow.xaml.cs                       
                    }
                }
            }
            if(!loginCorrecto)
            {               
                txtResultadoLogin.Text = "Usuario y/o contraseña incorrectos";
                resultadoLoginDialogHost.IsOpen = true;
            }
        }

        /// <summary>
        /// Método  que registra el enter en el campo de contraseña y lanza el evento del boton de login
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {               
                identificarse_OnClick(sender, e);
            }
        }



    }
}
