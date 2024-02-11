using DI_Proyecyo_Final.Model;
using DI_Proyecyo_Final.ViewModel;
using MaterialDesignThemes.Wpf;
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
using MaterialDesignColors;
// using System.Web.UI.WebControls;

namespace DI_Proyecyo_Final
{
    /// <summary>
    /// Referencia a la tabla de roles de la BBDD, los valores de enum coinciden con los id de los roles de la tabla
    /// </summary>
    internal enum OperacionActual
    {
        None = 0,
        Crear = 1,
        GuardarNombreUsuario = 2,
        GuardarRol = 3,
        ActualizarEstadoUsuario = 4,
        GuardarContraseñaUsuario = 5,
        UpdatePropietario = 6,
        DeletePropietario = 7,
        CreatePropietario = 8,
    }




    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        private Image logoImage;
        private string imagePathOscuro = "Resources/logo_oscuro.png";
        private string imagePathClaro = "Resources/logo_claro.png";
        private MensajeroLogIn eventoLogIn = App.MensajeriaLoginCompartida;
        private MensajeroLogOut eventoLogOut = App.MensajeroLogOut;

        public MainWindow()
        {
            InitializeComponent();
            ContenedorDataContext contenedorDataContext= new ContenedorDataContext();
            contenedorDataContext.notificadorToggleButtons= new NotificadorToggleButtons();
            DataContext = contenedorDataContext;
            logoImage = FindName("logo") as Image;

            eventoLogIn.LoginExitoso += Mensajeria_LoginExitoso; //  me suscribo al evento de LoginExitoso
            eventoLogOut.LogOut += EventoLogOut_LogOut;

            this.habilitarInterfaz(false);
        }

        /// <summary>
        /// Método que recoge el evento de cierre de sesión y lanza los métodos correspodientes para cerrar la sesión
        /// y cargar la página de login
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EventoLogOut_LogOut(object sender, EventArgs e)
        {
            Sesion.cerrarSesion(); // método para cierre de sesion en el modelo
            this.habilitarInterfaz(false);  // deshabilito la interfaz
            LoadPage(ViewModelSesion.getPathInicioSesion()); // recojo el path de incio de sesion y lo cargo.   
        }

        /// <summary>
        /// Recoje el evento LoginExitoso lanzado por el  modelo login y habilita la interfaz y obtiene del ViewModel la  Page que debe
        /// cargar y la carga.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Mensajeria_LoginExitoso(object sender, EventArgs e)
        {
            // Realiza los cambios necesarios en la ventana principal
            //MessageBox.Show("Login exitoso. Realizando cambios en la ventana principal.");
            this.habilitarInterfaz(true);
            this.LoadPage(ViewModelSesion.getPathCarruselActual());

            // ahora hacemos que el togle refleje el color del tema del usauri oque ha logeado
            DarkModeToggleButton.IsChecked = Sesion.ConfiguracionUsuarioActivo.IsDarkTheme;
            // y establecemos el logo claro/oscuro en consecuencia
            BitmapImage bitmap = new BitmapImage();
            string imagePath = DarkModeToggleButton.IsChecked == true ? imagePathOscuro : imagePathClaro;

            bitmap.BeginInit();
            bitmap.UriSource = new Uri(imagePath, UriKind.Relative);
            bitmap.EndInit();

            logoImage.Source = bitmap;
        }


        /// <summary>
        /// Método que llama a la carga de la pagina de gestión de listaUsuarios.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuUsuarios_OnClick(object sender, RoutedEventArgs e)
        {          
            LoadPage(ViewModelSesion.getPathUsuarios());
            MenuToggleButton.IsChecked = !MenuToggleButton.IsChecked;
        }
        /// <summary>
        /// Método que llama a la carga de la pagina de gestión de propietarios.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuPropietarios_OnClick(object sender, RoutedEventArgs e)
        {
            LoadPage(ViewModelSesion.getPathPropietarios());
            MenuToggleButton.IsChecked = !MenuToggleButton.IsChecked;
        }
        /// <summary>
        /// Método que llama a la carga de la pagina de gestión de propiedades.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuPropiedades_OnClick(object sender, RoutedEventArgs e)
        {
            LoadPage(ViewModelSesion.getPathPropiedades());
            MenuToggleButton.IsChecked = !MenuToggleButton.IsChecked;
        }

        /// <summary>
        /// Método que llama a la carga de la pagina anterior.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuLeftButton_OnClick(object sender, RoutedEventArgs e)
        {
            LoadPage(ViewModelSesion.getPathAnterior());
        }

        /// <summary>
        /// Método que llama a la carga de la pagina posterior.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuRightButton_OnClick(object sender, RoutedEventArgs e)
        {
            LoadPage(ViewModelSesion.getPathPosterior());
        }


        /// <summary>
        /// Método que carga en el frame la pagina facilitada por parámetro
        /// </summary>
        private void LoadPage(string pagePath)
        {           
            frame1.Source = new Uri(pagePath, UriKind.Relative);
            frame1.NavigationUIVisibility = NavigationUIVisibility.Hidden;          
        }


        /// <summary>
        /// Lanza el método que cambia el tema del MaterialDesing en funcion del toggle  button
        /// También cambia el logo de la empresa (claro / oscuro) en función del tema.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuDarkModeButton_Click(object sender, RoutedEventArgs e)
        {          
            ViewModelTheme.ModificarLuzTema(DarkModeToggleButton.IsChecked == true);   
            BitmapImage bitmap = new BitmapImage();
            string imagePath = DarkModeToggleButton.IsChecked==true ? imagePathOscuro : imagePathClaro;
            
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(imagePath, UriKind.Relative);
            bitmap.EndInit();
            
            logoImage.Source = bitmap;
        }

        /// <summary>
        /// Metodo que cambia el tema del Material Desing a Primary DeepPurple, Secondary Lime.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuMoradoButton_Click(object sender, RoutedEventArgs e)
        {
            Color colorPrimario = SwatchHelper.Lookup[MaterialDesignColor.DeepPurple];
            Color colorSecundario = SwatchHelper.Lookup[MaterialDesignColor.Lime];
            ViewModelTheme.CambiarColorPrimario(colorPrimario);
            ViewModelTheme.CambiarColorAcento(colorSecundario);
        }

        /// <summary>
        /// Metodo que cambia el tema del Material Desing a Primary Brown, Secondary Amber.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuMarronButton_Click(object sender, RoutedEventArgs e)
        {
            Color colorPrimario = SwatchHelper.Lookup[MaterialDesignColor.Brown];
            Color colorSecundario = SwatchHelper.Lookup[MaterialDesignColor.YellowA200];
            ViewModelTheme.CambiarColorPrimario(colorPrimario);
            ViewModelTheme.CambiarColorAcento(colorSecundario);
        }

        /// <summary>
        /// Metodo que cambia el tema del Material Desing a Primary BlueGrey, Secondary Yellow.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuAzulGrisButton_Click(object sender, RoutedEventArgs e)
        {
            Color colorPrimario = SwatchHelper.Lookup[MaterialDesignColor.BlueGrey];
            Color colorSecundario = SwatchHelper.Lookup[MaterialDesignColor.LightBlue];
            ViewModelTheme.CambiarColorPrimario(colorPrimario);
            ViewModelTheme.CambiarColorAcento(colorSecundario);
        }

        /// <summary>
        /// Metodo que cambia el tema del Material Desing a Primary Grey, Secondary Teal.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuGrisButton_Click(object sender, RoutedEventArgs e)
        {
            Color colorPrimario = SwatchHelper.Lookup[MaterialDesignColor.Grey];
            Color colorSecundario = SwatchHelper.Lookup[MaterialDesignColor.Teal];
            ViewModelTheme.CambiarColorPrimario(colorPrimario);
            ViewModelTheme.CambiarColorAcento(colorSecundario);
        }

        /// <summary>
        /// Metodo que cambia el tema del Material Desing a Primary Orange, Secondary Yellow.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuNaranjaButton_Click(object sender, RoutedEventArgs e)
        {
            Color colorPrimario = SwatchHelper.Lookup[MaterialDesignColor.Orange500];
            Color colorSecundario = SwatchHelper.Lookup[MaterialDesignColor.Yellow];
            ViewModelTheme.CambiarColorPrimario(colorPrimario);
            ViewModelTheme.CambiarColorAcento(colorSecundario);
        }



        /// <summary>
        /// Abre la ventana emergente de confirmación para  salir de la aplicacion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuSalir_onClick(object sender, RoutedEventArgs e)
        {
            miDialogHost2.IsOpen = true;
        }

        /// <summary>
        /// Abre la ventana emergente de confirmación para cerrar la sesion actual
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuLogOut_onClick(object sender, RoutedEventArgs e)
        {
            miDialogHost3.IsOpen = true;
        }

        /// <summary>
        /// Boton aceptar de la ventana emergente de confimación de salir
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSalir_onClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        /// <summary>
        /// Boton aceptar de la ventana emergente de confimación de cerrar sesion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCerrarSesion_onClick(object sender, RoutedEventArgs e)
        {
            Sesion.cerrarSesion(); // método para cierre de sesion en el modelo
            this.habilitarInterfaz(false);  // deshabilito la interfaz
            LoadPage(ViewModelSesion.getPathInicioSesion()); // recojo el path de incio de sesion y lo cargo.           
        }

        /// <summary>
        /// Abre la ventana emergente de  acerca de
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void acerdaDe_onClick(object sender, RoutedEventArgs e)
        {
            PopBox.IsPopupOpen = false;
            miDialogHost1.IsOpen = true;
        }




        public  void habilitarInterfaz(bool habilitar)
        {
            MenuToggleButton.IsEnabled = habilitar;
            PopBox.IsEnabled = habilitar;
            MenuLeftButton.IsEnabled = habilitar;
            MenuRightButton.IsEnabled = habilitar;
            MenuCerrarsesion.IsEnabled = habilitar;
            if(habilitar && Sesion.UsuarioActivo != null && Sesion.UsuarioActivo.Rol.Equals(Rol.Administrador))
            {
                btnUsuarios.IsEnabled = habilitar;
                btnUsuarios.Visibility= Visibility.Visible;
            }
            else 
            {
                btnUsuarios.IsEnabled = false;
                btnUsuarios.Visibility = Visibility.Collapsed;
            }

        }
    }
}
