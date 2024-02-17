using DI_Proyecyo_Final.Model;
using DI_Proyecyo_Final.Services.DataAccess;
using DI_Proyecyo_Final.ViewModel;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace DI_Proyecyo_Final
{

    

    /// <summary>
    /// Lógica de interacción para VistaUsuarios.xaml
    /// </summary>
    public partial class VistaUsuarios : Page
    {

        private List<Usuario> listaUsuarios;
        private OperacionActual opActual =0; // aqui guardo la operación que se está realizando

        public VistaUsuarios()
        {
            InitializeComponent();
            // se añaden los contextos de datos de los validadores y los notificadores de los textboxes
            var contenedor = new ContenedorDataContext();
            contenedor.CambioEnTexto = new NotificacionCambioEnTextBoxes();

            this.DataContext = contenedor;

            // creo bindings para los campos de contraseña de crear
            Binding binding = new Binding("PasswordBoxAssist.PasswordProperty");
            txtContraseña1.SetBinding(PasswordBoxAssist.PasswordProperty, binding);
            Binding binding2 = new Binding("PasswordBoxAssist.PasswordProperty");
            txtContraseña2.SetBinding(PasswordBoxAssist.PasswordProperty, binding);
            comprobarTxtContraseñas("Campo obligatorio");

            // y para los de gestion de usuario también
            Binding binding3 = new Binding("PasswordBoxAssist.PasswordProperty");
            txtContraseñaGestion1.SetBinding(PasswordBoxAssist.PasswordProperty, binding);
            Binding binding4 = new Binding("PasswordBoxAssist.PasswordProperty");
            txtContraseñaGestion2.SetBinding(PasswordBoxAssist.PasswordProperty, binding);
            //comprobarTxtContraseñas("Campo obligatorio");

            // lo suscribo al evento para controlar si se modifica tras haber validado el nombre de listaUsuarios con la BBDD
            txtNombreUsuario.TextChanged += TxtNombreUsuario_TextChanged;

            // lo mimso pero en la vista de listar,modificar
            txtNombreUsuarioGestion.TextChanged += TxtNombreUsuarioGestion_TextChanged;

            dataGridUsuarios.Items.Clear();
            listaUsuarios = Usuario.obtenerListadoUsuarios();
            if (listaUsuarios is null) // recojo el null desde Controlador si hay fallo en conexión
            {
                txtVentanaEmergente1btn.Text = "Error de conexión";
                miDialogHost1btn.IsOpen = true;
            }
            else
            {
                dataGridUsuarios.ItemsSource = listaUsuarios;
            }

        }




        /*==============================================================================================================================*/
        /*                                          GESTIÓN DE LA CREACIÓN DE USUARIOS                                                  */
        /*==============================================================================================================================*/

        private bool nombreUsuarioCorrecto = false;
        private bool contraseñaCorrecta = false;

        /// <summary>
        /// Método que incializa la interfaz de la página cuando se presiona el botón btnBorrarCampos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBorrarCampos_Click(object sender, RoutedEventArgs e)
        {
            limpiarCamposAñadirUsuario();

        }

        /// <summary>
        /// Método que incializa la interfaz de la página cuando se presiona el botón btnBorrarCampos
        /// </summary>
        private void limpiarCamposAñadirUsuario()
        {
            txtNombreUsuario.Text = string.Empty;
            txtContraseña1.Password = string.Empty;
            txtContraseña2.Password = string.Empty;
            btnCrearUsuario.IsEnabled = false;
            iconPassCheck1.Kind = PackIconKind.Check;
            iconPassCheck2.Kind = PackIconKind.Check;
            iconUsernameCheck.Kind = PackIconKind.Check;
            iconUsernameCheck.Foreground = new SolidColorBrush(Colors.Gray);
            iconPassCheck1.Foreground = new SolidColorBrush(Colors.Gray);
            iconPassCheck2.Foreground = new SolidColorBrush(Colors.Gray);
            cmbRol.SelectedIndex = 1;
        }

        /// <summary>
        /// Método que valida con  que el nombre de usuario del txtNombreUsuario sea único.
        /// Si esválido actualiza el icono en consecuencia  y chequea si la contraseña también es correcta para
        /// habilitar el boton de crear usuario.
        /// Si no es válido lanza el mesaje de error y actualzxiael icono en consecuancia.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnValidarCampos_Click(object sender, RoutedEventArgs e)
        {
            BindingExpression bindingExpression = txtNombreUsuario.GetBindingExpression(TextBox.TextProperty);
            if (bindingExpression != null && !bindingExpression.HasValidationError)
            {
                if (nombreUsuarioCorrecto = txtNombreUsuario.Text.Length < 5)
                {
                    errorEnTxtNombreUsuario("El nombre de usuario debe tener al menos 5 caracteres.");
                }
                else if (nombreUsuarioCorrecto = Usuario.nombreUsuarioUnico(txtNombreUsuario.Text)  )  
                {
                    iconUsernameCheck.Kind = PackIconKind.Check;
                    iconUsernameCheck.Foreground = new SolidColorBrush(Colors.LightGreen);
                    if (contraseñaCorrecta)
                        btnCrearUsuario.IsEnabled = true;
                }
                else
                {
                    errorEnTxtNombreUsuario("Este nombre de usuario ya existe.");
                }

            }

        }

        /// <summary>
        /// Método privado que envía al txtNombreUsuario el mensaje de error correspodiente y actualiza su icono en consecuencia
        /// </summary>
        /// <param name="mensajeError"> Mensaje que debe  aparecer como error para  el usuario</param>
        private void errorEnTxtNombreUsuario(string mensajeError)
        {
            BindingExpression bindingExpression = txtNombreUsuario.GetBindingExpression(TextBox.TextProperty);
            bindingExpression.UpdateSource();
            Validation.MarkInvalid(bindingExpression, new ValidationError(new ReglaValidacionObligatorio(), bindingExpression, mensajeError, null));
            iconUsernameCheck.Kind = PackIconKind.Close;
            iconUsernameCheck.Foreground = new SolidColorBrush(Colors.Red);
            btnCrearUsuario.IsEnabled = false;

        }

        /// <summary>
        /// Comprueba que el usuario y contraseña sean válidos y lanza ventana de confirmación para la creación de un usuario nuevo.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCrearUsuario_Click(object sender, RoutedEventArgs e) 
        {
            // esta comprobación es un refuerzo de seguridad a la lógica de desabilitar el boton de crear uaurio,si se llega hasta  aqui deberia ser siempre true
            if (nombreUsuarioCorrecto && contraseñaCorrecta) 
            {
                txtVentanaEmergente2btn.Text = "Nombre de usuario: " + txtNombreUsuario.Text + "\nRol : " +  ((ComboBoxItem)cmbRol.SelectedItem).Content
                   + "\n\n¿ Desea dar de alta a este usuario en la aplicación ?";
                opActual = OperacionActual.Crear;
                miDialogHost2btn.IsOpen = true;
            }
            else
            {
                txtVentanaEmergente1btn.Text = "Información de usuario incorrecta.\nNo se ha creado el Usuario";
                miDialogHost1btn.IsOpen = true;
            }
        }

        /// <summary>
        /// Método que realiza el lanzmaiento de la creación de usuario, Se utiliza despues qde que el usuario confirme que quiere crear el usuario.
        /// </summary>
        private void lanzarCreaciondeUsuario()
        {
            Rol rol = (Rol)(cmbRol.SelectedIndex + 1); // recojo el rol
            Usuario nuevoUsuario = new Usuario(txtNombreUsuario.Text, rol);  // creo el Usuario
            bool creacioncorrecta = nuevoUsuario.añadirUsuarioBBDD(txtContraseña1.SecurePassword); // lanzo la creación al modelo
            if (creacioncorrecta) limpiarCamposAñadirUsuario();  // si se creó correctamente borro los campos
            String mensaje = creacioncorrecta ? "Usuario creado con éxito" : "Error en la creación del usuario"; // mensaje segun resultado
            lanzarSnackBar(mensaje, 2);  // lanzo el snackbar que infomra al usuario.
        }

        /// <summary>
        /// Métodoque recoje el evento de cambio de contraseña para ambos txtContraseña1/2 y actua en consecuancia en  funcionde si  coinciden o no.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PassChangedEvent(object sender, RoutedEventArgs e)
        {
            string mensajeError = "";
            if (txtContraseña1.SecurePassword.Length > 0 && txtContraseña2.SecurePassword.Length > 0 &&
               (contraseñaCorrecta = ComparadorSecureString.AreSecureStringsEqual(txtContraseña1.SecurePassword, txtContraseña2.SecurePassword)))
            {
                iconPassCheck1.Kind = PackIconKind.Check;
                iconPassCheck2.Kind = PackIconKind.Check;
                iconPassCheck1.Foreground = new SolidColorBrush(Colors.LightGreen);
                iconPassCheck2.Foreground = new SolidColorBrush(Colors.LightGreen);
                comprobarTxtContraseñas("");
                if (nombreUsuarioCorrecto)
                    btnCrearUsuario.IsEnabled = true;
            }
            else
            {
                iconPassCheck1.Kind = PackIconKind.Close;
                iconPassCheck2.Kind = PackIconKind.Close;
                iconPassCheck1.Foreground = new SolidColorBrush(Colors.Red);
                iconPassCheck2.Foreground = new SolidColorBrush(Colors.Red);
                contraseñaCorrecta = false;
                btnCrearUsuario.IsEnabled = false;
                mensajeError = "Las contraseñas no coinciden";
                if (txtContraseña1.SecurePassword.Length == 0 && txtContraseña2.SecurePassword.Length == 0)
                    mensajeError = "La contraseña no puede estar vacía";
                comprobarTxtContraseñas(mensajeError);
            }


        }

        /// <summary>
        /// Método privado que envía a los  txtContraseña1/2 el mensaje de error correspodiente y actualiza su icono en consecuencia
        /// </summary>
        /// <param name="mensajeError"> Mensaje que debe  aparecer como error para  el usuario</param>
        private void comprobarTxtContraseñas(string error)
        {
            BindingExpression bindingExpression1 = BindingOperations.GetBindingExpression(txtContraseña1, (PasswordBoxAssist.PasswordProperty));
            if (bindingExpression1 != null && error.Length>0)
            {
                ValidationError validationError = new ValidationError(new ReglaValidaciónContraseña(), bindingExpression1, error, null);
                Validation.MarkInvalid(bindingExpression1, validationError);
            }
            else if(bindingExpression1 != null)
            {
                Validation.ClearInvalid(bindingExpression1);
            }
            BindingExpression bindingExpression2 = BindingOperations.GetBindingExpression(txtContraseña2, (PasswordBoxAssist.PasswordProperty));
            if (bindingExpression2 != null && error.Length > 0)
            {
                ValidationError validationError = new ValidationError(new ReglaValidaciónContraseña(), bindingExpression2, error, null);
                Validation.MarkInvalid(bindingExpression2, validationError);
            }
            else if (bindingExpression2 != null)
            {
                Validation.ClearInvalid(bindingExpression2);
            }
        }

        /// <summary>
        /// Método que ante un cambio de nombre de usuario invalida la comprobación con la BBDD y actualiza la vista en  consecuencia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxtNombreUsuario_TextChanged(object sender, TextChangedEventArgs e)
        {
            btnCrearUsuario.IsEnabled = false;
            iconUsernameCheck.Kind = PackIconKind.Check;
            iconUsernameCheck.Foreground = new SolidColorBrush(Colors.LightGray);
            nombreUsuarioCorrecto = false;

        }


        /*==============================================================================================================================*/
        /*                      GESTIÓN DE LISTAR, MODIFICAR, ELIMINAR USUARIOS                                                         */
        /*==============================================================================================================================*/

        private bool nombreUsuarioGestionvalidado = false;
        private bool contraseñaUsuarioGestionCorrecta = false;
        private bool requiereIniciodeSesion = false;
        private Usuario usuarioSelecionado = null;
        private MensajeroLogOut eventoLogOut = App.MensajeroLogOut;
       
        /// <summary>
        /// Metodo que actualiza la vista al seleccionarse un elemento del datagrid. Llama al método que rellena los campor y 
        /// actualiza la visibilidad de iconos y los que botones que deben estar habilitados y deshabilitados.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridUsuarios_Selected(object sender, EventArgs e) 
        {
            this.usuarioSelecionado = dataGridUsuarios.SelectedItem as Usuario; // recuperamos el usuario seleccinado
            rellenarCampos(this.usuarioSelecionado);  //  rellenamos los campos con su informacion
                        // habilitamos botones 
            btnInformesUsuario.IsEnabled = true;
            btnEditarNombreUsuario.IsEnabled = true;
            btnCambiarContraseña.IsEnabled = true;
            btnGuardarContraseña.IsEnabled = false;
            btnGuardarNombre.IsEnabled = false;
            btnGuardarRol.IsEnabled = false;
            btnValidarNombreUsusario.IsEnabled = false;
                        // deshabilitamos entradas de datos
            txtContraseñaGestion1.IsEnabled = false;
            txtContraseñaGestion2.IsEnabled = false;
            txtNombreUsuarioGestion.IsEnabled = false;
            cmbRolGestion.IsEnabled = false;
                        // ocultamos iconos de verificacion
            iconPassCheckGestion1.Visibility = Visibility.Hidden;
            iconPassCheckGestion2.Visibility = Visibility.Hidden;
            iconUsernameCheckGestion.Visibility = Visibility.Hidden;
                        // reiniciamos variables de control y campos de contraseña
            nombreUsuarioGestionvalidado = false;
            contraseñaUsuarioGestionCorrecta = false;
            txtContraseñaGestion1.Password = "";
            txtContraseñaGestion2.Password = "";
                        // eliminamos errores de campo de contraseña
            BindingExpression be = txtContraseñaGestion1.GetBindingExpression(PasswordBoxAssist.PasswordProperty);
            Validation.ClearInvalid(be);
            be = txtContraseñaGestion2.GetBindingExpression(PasswordBoxAssist.PasswordProperty);
            Validation.ClearInvalid(be);
           

        }

        /// <summary>
        /// Metodo que refresca el DataGrid con la BBD y limpia todos los campos y cambia el estado de botones e iconos
        /// a su  valor incial
        /// </summary>
        /// <param name="senser"></param>
        /// <param name="e"></param>
        private void btnActualzarGestion_onClick(object senser, EventArgs e)
        {
            refrescarDataGrid();
        }

        /// <summary>
        /// Metodo que incia el proceso el estado del usuario a activo + inactgivo según corresponda.
        /// Lanza ventana de confirmación.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkEstado_onClick(object sender, EventArgs e)
        {
            if(usuarioSelecionado.Id == 1)
            {
                txtVentanaEmergente1btn.Text = "Este usuario es el administrador por defecto.\nNo se puede deshabilitar";
                miDialogHost1btn.IsOpen = true;
                chkEstado.IsChecked= true;
            }
            else if(usuarioSelecionado.Id == Sesion.UsuarioActivo.Id)
            {
                txtVentanaEmergente2btn.Text = "Va a marcar como inactivo a su usuario,\nrealizar esta operación cerrará la sesión actual" +
                    "y le imperdirá\ninciar sesión hasta que otro adminstrador reactive su cuenta de usuario." +
                    "\n\n ¿ Desea marcar como inactivo el usuario actual y cerrar sesión? ";
                opActual = OperacionActual.ActualizarEstadoUsuario;
                miDialogHost2btn.IsOpen = true;
                this.requiereIniciodeSesion = true;
            }
            else
            {
                txtVentanaEmergente2btn.Text = "Estado anterior de usuario: " + (this.usuarioSelecionado.Activo?"Activo":"Inactivo") +
                  "\nNuevo estado de usuario: " +  (chkEstado.IsChecked== true ? "Activo" : "Inactivo") 
                   + "\n\n¿ Desea actualizar el estado del usuario " + this.usuarioSelecionado.NombreUsuario + " ?";
                opActual = OperacionActual.ActualizarEstadoUsuario;
                miDialogHost2btn.IsOpen = true;
            }

        }

        /// <summary>
        /// Método que lanza el cambio de estado  (activo/inactivo) del usuario seleccionado
        /// </summary>
        private void lanzarActualizarEstado()
        {
            bool actualziacionCorrecta = this.usuarioSelecionado.actualizarActivo(chkEstado.IsChecked == true); // lanzo el update al modelo

            if (actualziacionCorrecta) refrescarDataGrid();  // si se modifico correctamente refresco la pestaña de gestion de usuarios
            String mensaje = actualziacionCorrecta ? "El estado del usuario ha sido modificado con éxito" : "Error, el estado del usuario no se ha modificado"; // mensaje segun resultado
            lanzarSnackBar(mensaje, 2);  // lanzo el snackbar que informa al usuario.
            if (actualziacionCorrecta && this.requiereIniciodeSesion)
            {
                this.requiereIniciodeSesion = false;
                this.eventoLogOut.LanzarLogOut();
            }
        }


        private  void btnInformesUsuario_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Método que habilita los campos y botones encesarios para actualziar el nombre de usuario
        /// y el rol.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEditarNombreUsuario_Click(object sender, EventArgs e)
        {
            btnValidarNombreUsusario.IsEnabled = true;
            btnGuardarRol.IsEnabled=true;
            txtNombreUsuarioGestion.IsEnabled = true;
            cmbRolGestion.IsEnabled=true;
            iconUsernameCheckGestion.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Método que verifica con la BBDD si existe el  nombre de usuario en txtNombreUsuarioGestion.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnValidarNombreUsusario_Click(object sender, EventArgs e)
        {         
            BindingExpression bindingExpression = txtNombreUsuarioGestion.GetBindingExpression(TextBox.TextProperty);
            if (bindingExpression != null && !bindingExpression.HasValidationError)
            {
                if (nombreUsuarioGestionvalidado = txtNombreUsuarioGestion.Text.Length < 5)
                {
                    errorEnTxtNombreUsuarioGestion("El nombre de usuario debe tener al menos 5 caracteres.");
                }
                else if ( this.usuarioSelecionado.NombreUsuario.Equals(txtNombreUsuarioGestion.Text)  ||  // si no lo cambia lo damos por bueno
                    (nombreUsuarioGestionvalidado = Usuario.nombreUsuarioUnico(txtNombreUsuarioGestion.Text) ) )  // si no mioramos que no este repetido en BBDD
                {
                    iconUsernameCheckGestion.Kind = PackIconKind.Check;
                    iconUsernameCheckGestion.Foreground = new SolidColorBrush(Colors.LightGreen);
                    btnGuardarNombre.IsEnabled = true;
                }
                else
                {
                    errorEnTxtNombreUsuarioGestion("Este nombre de usuario ya existe.");
                }

            }           

        }

        /// <summary>
        /// Metodo que gestiona el evento de cambio de texto en txtNombreUsuarioGestion para actualizar iconos y botones
        /// en consecuencia.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxtNombreUsuarioGestion_TextChanged(object sender, TextChangedEventArgs e)
        {
            btnGuardarNombre.IsEnabled = false;
            iconUsernameCheckGestion.Kind = PackIconKind.Check;
            iconUsernameCheckGestion.Foreground = new SolidColorBrush(Colors.LightGray);
            nombreUsuarioGestionvalidado = false;
        }

        /// <summary>
        /// Mñetodo que tras comprobar que el  nobmre se ha validado abre dialogo para confirmar el cambio de nombre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGuardarNombreUsuario_Click(object sender, EventArgs e)
        {
            if (nombreUsuarioGestionvalidado)
            {
                txtVentanaEmergente2btn.Text = "Antiguo nombre de usuario: " + this.usuarioSelecionado.NombreUsuario +
                    "\nNuevo nombre de usuario: " + txtNombreUsuarioGestion.Text
                    +"\n\n\t AVISO: Cambiar el  nombre de un usario provoca que todos los logs de BBDD cambién a este nuevo nombre."
                    +"\n\t Si el usuario va a ser  utilizado por otro trabajador es recomendable que establezca el usuario actual a INACTIVO"
                    +"\n\t y cree un nuevo usuario para el  nuevo trabajador "
                   + "\n\n¿ Desea actualizar el nombre de usuario ?";
                opActual = OperacionActual.GuardarNombreUsuario;
                miDialogHost2btn.IsOpen = true;
            }          

        }

        /// <summary>
        /// Método que lanza la actualziación de nombre de usuario al modelo, y según la respuesta lanza una snakbar y refresca la vista
        /// </summary>
        private void lanzarActualziarNombreUsuario()
        {
            bool creacioncorrecta =  this.usuarioSelecionado.actualizarNombre(txtNombreUsuarioGestion.Text); // lanzo el update al modelo
           
            if (creacioncorrecta) refrescarDataGrid();  // si se modifico correctamente refresco la pestaña de gestion de usuarios
            String mensaje = creacioncorrecta ? "Nombre de usuario modificado con éxito" : "Error, el nombre no se ha modificado"; // mensaje segun resultado
            lanzarSnackBar(mensaje, 2);  // lanzo el snackbar que informa al usuario.
        }

        /// <summary>
        /// Método que en caso de ser el rol del usuario actualizable lanza una ventana dialogo de confirmación.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGuardarRolUsuario_Click(Object sender, EventArgs e)
        {
            if(  this.usuarioSelecionado.Rol !=  (Rol)(cmbRolGestion.SelectedIndex + 1) )
            {
                if (this.usuarioSelecionado.Id == 1)
                {
                    txtVentanaEmergente1btn.Text = "Este usuario es el administrador por defecto.\nNo se puede cambiar su rol de administrador";
                    miDialogHost1btn.IsOpen = true;
                    cmbRolGestion.SelectedIndex = 0;
                }
                else if (usuarioSelecionado.Id == Sesion.UsuarioActivo.Id)
                {
                    txtVentanaEmergente2btn.Text = "Va a quitar a su usario el rol de administrador,\nrealizar esta operación cerrará la sesión actual" +
                        "y le imperdirá\nacceder a la gestión de usuarios hasta que otro adminstrador le otrogue permiso para ello." +
                        "\n\n ¿ Desea revocar los privilegios de adminsitrador de su cuenta y cerrar sesión? ";
                    opActual = OperacionActual.GuardarRol;
                    miDialogHost2btn.IsOpen = true;
                    this.requiereIniciodeSesion = true;
                }
                else
                {
                    txtVentanaEmergente2btn.Text = "Antiguo rol de usuario: " + this.usuarioSelecionado.Rol +
                   "\nNuevo rol de usuario: " + ((ComboBoxItem)cmbRolGestion.SelectedItem).Content
                    + "\n\n¿ Desea actualizar el rol del usuario "+this.usuarioSelecionado.NombreUsuario+" ?";
                    opActual = OperacionActual.GuardarRol;
                    miDialogHost2btn.IsOpen = true;
                }
                
            }
        }

        /// <summary>
        /// Método que lanza el cambio de rol del usuario al modelo.
        /// </summary>
        private void lanzarActualizarRol()
        {
            bool actualizaciónCorrecta = this.usuarioSelecionado.actualizarRol( (Rol)(cmbRolGestion.SelectedIndex + 1) );

            if (actualizaciónCorrecta) refrescarDataGrid();  // si se modifico correctamente refresco la pestaña de gestion de usuarios
            String mensaje = actualizaciónCorrecta ? "Rol de usuario modificado con éxito" : "Error, el rol no se ha modificado"; // mensaje segun resultado
            lanzarSnackBar(mensaje, 2);  // lanzo el snackbar que informa al usuario.
            if (actualizaciónCorrecta && this.requiereIniciodeSesion)
            {
                this.requiereIniciodeSesion = false;
                this.eventoLogOut.LanzarLogOut();
            }
        }

        /// <summary>
        /// Metodo que habilita en la vista los elementos necesarios apra cmabiar la contraseña:
        /// Textboxes e iconos de verificación.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCambiarContraseña_Click(object sender, EventArgs e)
        {
            txtContraseñaGestion1.IsEnabled = true;
            txtContraseñaGestion2.IsEnabled = true;
            iconPassCheckGestion1.Visibility =Visibility.Visible;
            iconPassCheckGestion2.Visibility =Visibility.Visible;         
        }

        /// <summary>
        /// Evento que lanza el dialogo de confirmación para el cambio de contraseña.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGuardarContraseñaUsuario_Click(object sender, EventArgs e)
        {
            if (contraseñaUsuarioGestionCorrecta)
            {
                txtVentanaEmergente2btn.Text = "¿ Desea actualizar la contraseña del usuario " + this.usuarioSelecionado.NombreUsuario + " ?";
                opActual = OperacionActual.GuardarContraseñaUsuario;
                miDialogHost2btn.IsOpen = true;
            }
        }

        /// <summary>
        /// Método que lanza al modelo la operación de actualizar contraseña.
        /// </summary>
        private void lanzarActualizarContraseña()
        {
            bool actualizaciónCorrecta = this.usuarioSelecionado.actualizarContraseña(txtContraseñaGestion1.SecurePassword);

            if (actualizaciónCorrecta) refrescarDataGrid();  // si se modifico correctamente refresco la pestaña de gestion de usuarios
            String mensaje = actualizaciónCorrecta ? "Contraseña del usuario modificado con éxito" : "Error, la contraseña no se ha modificado"; // mensaje segun resultado
            lanzarSnackBar(mensaje, 2);  // lanzo el snackbar que informa al usuario.
        }

        /// <summary>
        /// Método que rellena todos los campos de la prestana de gextión de usuarios con los datos
        /// del Usuario facilitado
        /// </summary>
        /// <param name="usuario"></param>
        private void rellenarCampos(Usuario usuario)
        {
            if (usuario != null) 
            {
                txtNombreUsuarioGestion.Text = usuario.NombreUsuario;
                cmbRolGestion.SelectedIndex = (int)(usuario.Rol-1);
                chkEstado.IsEnabled = true;
                chkEstado.IsChecked = usuario.Activo;
                chkEstado.Content = usuario.Activo ? "Usuario activo" : "Usuario inactivo";
                lblFechaModificación.Content=usuario.Ultima_modificacion.ToString("g");
                lblIdModificación.Content = obtenerNombreUsuario(usuario.Uid);
                
            }
        }

        /// <summary>
        /// Metodo que vacía todos los campos del la pestaña de gestionar  usuarios.
        /// </summary>
        private void vaciarCampos()
        {
            txtNombreUsuarioGestion.Text = "";
            txtNombreUsuarioGestion.IsEnabled = false;
            cmbRolGestion.SelectedItem = null;
            cmbRolGestion.IsEnabled=false;
            chkEstado.IsEnabled = false;
            chkEstado.IsChecked = false;
            chkEstado.Content = "Usuario inactivo";
            lblFechaModificación.Content = "";
            lblIdModificación.Content = "";
            txtContraseñaGestion1.Password = "";
            txtContraseñaGestion1.IsEnabled = false;
            txtContraseñaGestion2.Password = "";
            txtContraseñaGestion2.IsEnabled = false;
        }

        /// <summary>
        /// Metodo que obtiene de la lista que alimenta el datagrid el  nombre del usuario dada su id, no consulta con la BBDD
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private string obtenerNombreUsuario(int id)
        {
            string nombre = "";
            bool encontrado=false;
            for (int i = 0; i < listaUsuarios.Count && !encontrado; i++)
            {
                if (listaUsuarios[i].Id == id)
                {
                    nombre = listaUsuarios[i].NombreUsuario;
                    encontrado = true;
                }                   
            }
            return nombre;
        }

        /// <summary>
        /// Metodo que actualzia el textbox del nombre de usuario de gestion con el error facilitado. Tambien pone el icono de error
        /// </summary>
        /// <param name="mensajeError"></param>
        private void errorEnTxtNombreUsuarioGestion(string mensajeError)
        {
            BindingExpression bindingExpression = txtNombreUsuarioGestion.GetBindingExpression(TextBox.TextProperty);
            bindingExpression.UpdateSource();
            Validation.MarkInvalid(bindingExpression, new ValidationError(new ReglaValidacionObligatorio(), bindingExpression, mensajeError, null));
            iconUsernameCheckGestion.Kind = PackIconKind.Close;
            iconUsernameCheckGestion.Foreground = new SolidColorBrush(Colors.Red);
            btnCrearUsuario.IsEnabled = false;

        }

        /// <summary>
        /// Metodo que refresca el datgrid con la BBDD, vacia todos los campos y desabilita los botones
        /// </summary>
        private  void refrescarDataGrid()
        {
            listaUsuarios = Usuario.obtenerListadoUsuarios();
            if (listaUsuarios is null) // recojo el null desde Controlador si hay fallo en conexión
            {
                dataGridUsuarios.Items.Clear();
                txtVentanaEmergente1btn.Text = "Error de conexión";
                miDialogHost1btn.IsOpen = true;
            }
            else
            {
                dataGridUsuarios.ItemsSource = listaUsuarios;
            }
            vaciarCampos();   // vaciamos campos y dehabilitamos botones
            btnInformesUsuario.IsEnabled = false;
            btnEditarNombreUsuario.IsEnabled = false;
            btnCambiarContraseña.IsEnabled = false;
            btnGuardarContraseña.IsEnabled = false;
            btnGuardarNombre.IsEnabled = false;
            btnGuardarRol.IsEnabled = false;
            btnValidarNombreUsusario.IsEnabled = false;
                            // ocultamos los iconos de verificacion de campos
            iconPassCheckGestion1.Visibility = Visibility.Hidden;
            iconPassCheckGestion2.Visibility = Visibility.Hidden;
            iconUsernameCheckGestion.Visibility = Visibility.Hidden;
                            // reinciamos las variables de control
            this.contraseñaUsuarioGestionCorrecta = false;
            this.nombreUsuarioGestionvalidado = false;
            this.usuarioSelecionado = null;
                            // eliminamos los errores de los  bindings de nombre y contraseña
            BindingExpression be = txtNombreUsuarioGestion.GetBindingExpression(TextBox.TextProperty);
            Validation.ClearInvalid(be);
            be = txtContraseñaGestion1.GetBindingExpression(PasswordBoxAssist.PasswordProperty);
            Validation.ClearInvalid(be);
            be = txtContraseñaGestion2.GetBindingExpression(PasswordBoxAssist.PasswordProperty);
            Validation.ClearInvalid(be);
        }

        /// <summary>
        /// Métodoque recoje el evento de cambio de contraseña para ambos txtContraseña1/2 y actua en consecuancia en  funcionde si  coinciden o no.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PassGestionChangedEvent(object sender, RoutedEventArgs e)
        {
            string mensajeError = "";
            if (txtContraseñaGestion1.SecurePassword.Length > 0 && txtContraseñaGestion2.SecurePassword.Length > 0 &&
                (contraseñaUsuarioGestionCorrecta =
                    ComparadorSecureString.AreSecureStringsEqual(txtContraseñaGestion1.SecurePassword, txtContraseñaGestion2.SecurePassword)))
            {
                iconPassCheckGestion1.Kind = PackIconKind.Check;
                iconPassCheckGestion2.Kind = PackIconKind.Check;
                iconPassCheckGestion1.Foreground = new SolidColorBrush(Colors.LightGreen);
                iconPassCheckGestion2.Foreground = new SolidColorBrush(Colors.LightGreen);
                comprobarTxtContraseñasGestion("");               
                btnGuardarContraseña.IsEnabled = true;
            }
            else
            {

                iconPassCheckGestion1.Kind = PackIconKind.Close;
                iconPassCheckGestion2.Kind = PackIconKind.Close;
                iconPassCheckGestion1.Foreground = new SolidColorBrush(Colors.Red);
                iconPassCheckGestion2.Foreground = new SolidColorBrush(Colors.Red);
                contraseñaUsuarioGestionCorrecta = false;
                btnGuardarContraseña.IsEnabled = false;
                mensajeError = "Las contraseñas no coinciden";
                if (txtContraseñaGestion1.SecurePassword.Length == 0 && txtContraseñaGestion2.SecurePassword.Length == 0)
                    mensajeError = "La contraseña no puede estar vacía";
                comprobarTxtContraseñasGestion(mensajeError);
            }
        }

        /// <summary>
        /// Método privado que envía a los  txtContraseña1/2 el mensaje de error correspodiente y actualiza su icono en consecuencia
        /// En ete casoactua sobre los campos de contraseña de gestion de usuarios
        /// </summary>
        /// <param name="mensajeError"> Mensaje que debe  aparecer como error para  el usuario</param>
        private void comprobarTxtContraseñasGestion(string error)
        {
            BindingExpression bindingExpression1 = BindingOperations.GetBindingExpression(txtContraseñaGestion1, (PasswordBoxAssist.PasswordProperty));
            if (bindingExpression1 != null && error.Length > 0)
            {
                ValidationError validationError = new ValidationError(new ReglaValidaciónContraseña(), bindingExpression1, error, null);
                Validation.MarkInvalid(bindingExpression1, validationError);
            }
            else if (bindingExpression1 != null)
            {
                Validation.ClearInvalid(bindingExpression1);
            }
            BindingExpression bindingExpression2 = BindingOperations.GetBindingExpression(txtContraseñaGestion2, (PasswordBoxAssist.PasswordProperty));
            if (bindingExpression2 != null && error.Length > 0)
            {
                ValidationError validationError = new ValidationError(new ReglaValidaciónContraseña(), bindingExpression2, error, null);
                Validation.MarkInvalid(bindingExpression2, validationError);
            }
            else if (bindingExpression2 != null)
            {
                Validation.ClearInvalid(bindingExpression2);
            }
        }

        /*==============================================================================================================================*/
        /*                                                      MÉTODOS COMUNES                                                         */
        /*==============================================================================================================================*/


        /// <summary>
        /// Método que lanza la snackBar con el mensaje y durante los segundos facilitados
        /// </summary>
        /// <param name="mensaje"></param>
        /// <param name="segundos"></param>
        private void lanzarSnackBar(String mensaje, int segundos)
        {
            SnackbarUsuarios.MessageQueue?.Enqueue(mensaje, null, null, null, false, true, TimeSpan.FromSeconds(segundos));
        }

        /// <summary>
        /// Método que 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancelar_onClick(object sender, RoutedEventArgs e) 
        {
            if (opActual.Equals(OperacionActual.ActualizarEstadoUsuario))
            {     // si se cancela una actulización de estado revierto el chkbox a su estado anterior
                chkEstado.IsChecked = !chkEstado.IsChecked;
            }
            opActual = OperacionActual.None;
            this.requiereIniciodeSesion = false;
        }

        /// <summary>
        /// Método que recoge la acción del dialoghost de aceptar, llama al método adecuado en funaión de la operación que se esté realizando.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAceptar_onClick(object sender, RoutedEventArgs e)
        {
            if (opActual.Equals(OperacionActual.Crear))
            {
                lanzarCreaciondeUsuario();
            }else if (opActual.Equals(OperacionActual.GuardarNombreUsuario))
            {
                lanzarActualziarNombreUsuario();
            }else if (opActual.Equals(OperacionActual.GuardarRol))
            {
                lanzarActualizarRol();
            }else if (opActual.Equals(OperacionActual.ActualizarEstadoUsuario))
            {
                lanzarActualizarEstado();
            }
            else if (opActual.Equals(OperacionActual.GuardarContraseñaUsuario))
            {
                lanzarActualizarContraseña();
            }

            opActual = OperacionActual.None;
        }
       
        /// <summary>
        /// Método que realiza las getiones necesarias en la IU al cambiar de TabItem
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {               
                TabItem selectedTab = e.AddedItems[0] as TabItem; 

                if (selectedTab == tabCrear )   // para actualziar los errores y vaciar los campos
                {
                    txtContraseña1.Password = "1";
                    txtContraseña2.Password = "1";
                    txtNombreUsuario.Text = "1";
                    btnBorrarCampos_Click(null,null);                   
                }
            }
        }

    }
}

