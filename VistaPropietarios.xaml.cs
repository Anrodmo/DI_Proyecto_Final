using DI_Proyecyo_Final.Model;
using DI_Proyecyo_Final.ViewModel;
using Google.Protobuf.WellKnownTypes;
using MahApps.Metro.Controls.Dialogs;
using MaterialDesignThemes.Wpf;
using Org.BouncyCastle.Utilities;
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

namespace DI_Proyecyo_Final
{
    



    /// <summary>
    /// Lógica de interacción para VistaPropietarios.xaml
    /// </summary>
    public partial class VistaPropietarios : Page
    {
        private CollectionView viewPropietarios;
        private OperacionActual opActual = OperacionActual.None;
        private Propietario propietarioSeleccionado = null;
        private List<Propietario> listaPropietarios ;


        public VistaPropietarios()
        {
            InitializeComponent();

            // se añaden los contextos de datos de los validadores y los notificadores de los textboxes
            var contenedor = new ContenedorDataContext();
            contenedor.CambioEnTexto = new NotificacionCambioEnTextBoxes();
            contenedor.FechaCorrecta = new NotificacionFechaCorrecta();
            DataContext = contenedor;

            agregarEventoATextBoxes(); // agrego  todos los textboxes a un evento comun para que cuando ninguno tenga error
                                       // se habilite el boton de modificar
            agregarEventoATextBoxesCreacion();  // lo mismo para el boton de crear propietario

            dataGridPropietarios.Items.Clear();
            listaPropietarios = Propietario.obtenerListaPropietarios();
            if (listaPropietarios is null)
            {
                txtVentanaEmergente1btn.Text = "Error de conexión";
                miDialogHost1btn.IsOpen = true;
            }
            else
            {   // aqui uso CollectionView porque quiero aplicar filtros al datagrid
                viewPropietarios = (CollectionView)CollectionViewSource.GetDefaultView(listaPropietarios);
                dataGridPropietarios.ItemsSource = viewPropietarios;
            }


        }

        


        /*==============================================================================================================================*/
        /*                      GESTIÓN DE LISTAR, MODIFICAR, ELIMINAR PROPIETARIOS                                                     */
        /*==============================================================================================================================*/



        /// <summary>
        /// Metodo que actualiza la vista al seleccionarse un elemento del datagrid. Llama al método que rellena los campor y 
        /// actualiza la visibilidad de iconos y los que botones que deben estar habilitados y deshabilitados.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridPropietarios_Selected(object sender, EventArgs e)
        {
            habilitarCampos(false);
            this.propietarioSeleccionado = dataGridPropietarios.SelectedItem as Propietario; // recuperamos el usuario seleccinado
            if (this.propietarioSeleccionado != null)
            {
                rellenarCamposGestion(this.propietarioSeleccionado);  //  rellenamos los campos con su informacion

                // habilitamos botones 
                btnIrAPropiedades.IsEnabled = true;
                btnInformesPropietario.IsEnabled = true;

                btnAñadirPropiedad.IsEnabled = true;
                btnEditarPropietario.IsEnabled = true;
                btnGuardarPropietario.IsEnabled = false;
                btnBorrarPropietario.IsEnabled = true;
                // deshabilitamos entradas de datos

            }
            // reiniciamos variables de control y campos de contraseña
            this.opActual = OperacionActual.None;
         
        }

        /// <summary>
        /// Metodo que refresca el DataGrid con la BBD y limpia todos los campos y cambia el estado de botones e iconos
        /// a su  valor incial
        /// </summary>
        /// <param name="senser"></param>
        /// <param name="e"></param>
        private void btnActualzarPropietarios_onClick(object senser, EventArgs e)
        {
            refrescarDataGrid();
        }

        /// <summary>
        /// Metodo que refresca el datgrid con la BBDD, vacia todos los campos y desabilita los botones
        /// </summary>
        private void refrescarDataGrid()
        {
            // vaciamos campos y dehabilitamos botones y campos
            vaciarCampos();   
            habilitarCampos(false);
            btnIrAPropiedades.IsEnabled = false;
            btnInformesPropietario.IsEnabled = false;

            btnAñadirPropiedad.IsEnabled = false;
            btnEditarPropietario.IsEnabled = false;
            btnGuardarPropietario.IsEnabled = false;

            btnBorrarPropietario.IsEnabled = false;

            dataGridPropietarios.UnselectAll();
            listaPropietarios = Propietario.obtenerListaPropietarios();
            if (listaPropietarios is null) // recojo el null desde Controlador si hay fallo en conexión
            {
                dataGridPropietarios.Items.Clear();
                txtVentanaEmergente1btn.Text = "Error de conexión";
                miDialogHost1btn.IsOpen = true;
            }
            else
            {
                
                viewPropietarios = (CollectionView)CollectionViewSource.GetDefaultView(listaPropietarios);
                dataGridPropietarios.ItemsSource = viewPropietarios;
            }

            // ocultamos los iconos de verificacion de campos

            // reinciamos las variables de control
            this.propietarioSeleccionado = null;
            this.opActual = OperacionActual.None;
            // eliminamos los errores de los  bindings de nombre y contraseña

        }

        private void rellenarCamposGestion(Propietario propietario)
        {
            txtNIFPropietarioGestion.Text = propietario.NIF;
            txtNombrePropietarioGestion.Text=propietario.Nombre;
            txtApellidosPropietarioGestion.Text = propietario.Apellidos;
            txtEmailPropietarioGestion.Text =propietario.Email;
            txtTelefonoPropietarioGestion.Text = propietario.Telefono.ToString();
            dpickerFechaAlta.SelectedDate = propietario.Fecha_alta;
            txtCallePropietarioGestion.Text = propietario.Direccion.Calle;
            txtBloquePropietarioGestion.Text = propietario.Direccion.Bloque;
            txtPisoPropietarioGestion.Text = propietario.Direccion.Piso;
            txtCodigoPostalPropietarioGestion.Text = propietario.Direccion.CodPostal;
            txtLocalidadPropietarioGestion.Text = propietario.Direccion.Localidad;
            txtProvinciaPropietarioGestion.Text = propietario.Direccion.Provincia;
        }

        private void vaciarCampos()
        {
            txtNIFPropietarioGestion.Text = "";
            txtNombrePropietarioGestion.Text = "";
            txtApellidosPropietarioGestion.Text = "";
            txtEmailPropietarioGestion.Text = "";
            txtTelefonoPropietarioGestion.Text = "";
            dpickerFechaAlta.SelectedDate = null;
            txtCallePropietarioGestion.Text = "";
            txtBloquePropietarioGestion.Text = "";
            txtPisoPropietarioGestion.Text = "";
            txtCodigoPostalPropietarioGestion.Text = "";
            txtLocalidadPropietarioGestion.Text = "";
            txtProvinciaPropietarioGestion.Text = "";

            txtFiltroNIF.Text = "";
            txtFiltroNombre.Text = "";
        }

        private void cargarCamposEnPropietrioSeleccionado()
        {
            this.propietarioSeleccionado.NIF = txtNIFPropietarioGestion.Text.Trim();
            this.propietarioSeleccionado.Nombre = txtNombrePropietarioGestion.Text.Trim();
            this.propietarioSeleccionado.Apellidos = txtApellidosPropietarioGestion.Text.Trim();
            this.propietarioSeleccionado.Email= txtEmailPropietarioGestion.Text.Trim();
            this.propietarioSeleccionado.Telefono = int.Parse(txtTelefonoPropietarioGestion.Text.Trim());
            this.propietarioSeleccionado.Fecha_alta = (DateTime)dpickerFechaAlta.SelectedDate;
            this.propietarioSeleccionado.Direccion.Calle = txtCallePropietarioGestion.Text;
            this.propietarioSeleccionado.Direccion.Bloque = txtBloquePropietarioGestion.Text;
            this.propietarioSeleccionado.Direccion.Piso = txtPisoPropietarioGestion.Text;
            this.propietarioSeleccionado.Direccion.CodPostal = txtCodigoPostalPropietarioGestion.Text;
            this.propietarioSeleccionado.Direccion.Localidad = txtLocalidadPropietarioGestion.Text;
            this.propietarioSeleccionado.Direccion.Provincia = txtProvinciaPropietarioGestion.Text;
        }

        private void habilitarCampos(bool habilitar)
        {
            txtNIFPropietarioGestion.IsEnabled = habilitar;
            txtNombrePropietarioGestion.IsEnabled = habilitar;
            txtApellidosPropietarioGestion.IsEnabled = habilitar;
            txtEmailPropietarioGestion.IsEnabled = habilitar;
            txtTelefonoPropietarioGestion.IsEnabled = habilitar;
            dpickerFechaAlta.IsEnabled = habilitar;
            txtCallePropietarioGestion.IsEnabled = habilitar;
            txtBloquePropietarioGestion.IsEnabled = habilitar;
            txtPisoPropietarioGestion.IsEnabled = habilitar;
            txtCodigoPostalPropietarioGestion.IsEnabled = habilitar;
            txtLocalidadPropietarioGestion.IsEnabled = habilitar;
            txtProvinciaPropietarioGestion.IsEnabled = habilitar;

        }
       

        private void txtFiltroNombre_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (viewPropietarios != null)
            {
                txtFiltroNIF.Text = "";
                // Aplica el filtro al nombre de propietario
                viewPropietarios.Filter = item =>
                {
                    if (item is Propietario propietario)
                    {
                        // Comparación sin distinción entre mayúsculas y minúsculas
                        return propietario.Nombre.IndexOf(txtFiltroNombre.Text, StringComparison.OrdinalIgnoreCase) != -1;
                    }
                    return false;
                };
            }
        }

        private void txtFiltroNIF_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (viewPropietarios != null)
            {
                txtFiltroNombre.Text = "";
                // Aplica el filtro al NIf del propietario
                viewPropietarios.Filter = item =>
                {
                    if (item is Propietario propietario)
                    {
                        // Comparación sin distinción entre mayúsculas y minúsculas
                        return propietario.NIF.IndexOf(txtFiltroNIF.Text, StringComparison.OrdinalIgnoreCase) != -1;
                    }
                    return false;
                };
            }
        }

        private void btnIrAPropiedades_Click(object sender, EventArgs e)
        {

        }

        private void btnInformesPropietaro_Click(object sender, EventArgs e)
        {

        }

        private void btnAñadirPropiedadPropietario_Click(object sender, EventArgs e)
        {

        }

        private void btnEditarPropietario_Click(object sender, EventArgs e)
        {
            habilitarCampos(true);
            opActual = OperacionActual.UpdatePropietario;
        }

        private void btnGuardarPropietario_Click(object sender, EventArgs e)
        {
            if (this.propietarioSeleccionado != null)
            {
                this.opActual = OperacionActual.UpdatePropietario;
                txtVentanaEmergente2btn.Text = "Se va a proceder a actualizar los datos del usuario seleccinado." +
                    "\n\n¿ Desea continuar ?";
                miDialogHost2btn.IsOpen = true;
            }
        }

        private void lanzarModificarUsuario()
        {
            cargarCamposEnPropietrioSeleccionado(); // leo los txt y los pongo en el usuario
            bool modificadoConExito = propietarioSeleccionado.modificarPropietario();
            if (modificadoConExito) refrescarDataGrid();  // si se modifico correctamente refresco la pestaña de gestion de usuarios
            String mensaje = modificadoConExito ? "Propietario modificado con éxito" : "Error, no se modifico al propietario"; // mensaje segun resultado
            lanzarSnackBar(mensaje, 2);  // lanzo el snackbar que informa al usuario.
        }

       
        private void btnBorrarPropietario_Click(object sender, EventArgs e)
        {
            if (this.propietarioSeleccionado != null)
            {
                this.opActual = OperacionActual.DeletePropietario;
                txtVentanaEmergente2btn.Text = "¡¡ CUIDADO !!\n\nEliminar al propietario eliminará también " +
                    "todas sus propiedades.\n\nEsta operación no puede deshacerse."
                   + "\n\n¿ Desea eliminar definitivamente al propietario "+this.propietarioSeleccionado.Nombre+
                   " "+this.propietarioSeleccionado.Apellidos+" ?";                
                miDialogHost2btn.IsOpen = true;
            }
        }

        private void lanzarBorrarPropietario()
        {
            bool eliminadoConExito = propietarioSeleccionado.borrarPropietario();
            
            if (eliminadoConExito) refrescarDataGrid();  // si se modifico correctamente refresco la pestaña de gestion de usuarios
            String mensaje = eliminadoConExito ? "Propietario eliminado con éxito" : "Error, no se eliminó al propietario"; // mensaje segun resultado
            lanzarSnackBar(mensaje, 2);  // lanzo el snackbar que informa al usuario.
        }


        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Verifica el estado de validación de todos los campos y que se este editando al propietrio
            btnGuardarPropietario.IsEnabled = ValidarTodosLosCampos() && opActual.Equals(OperacionActual.UpdatePropietario);
        }

        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            // Verifica el estado de validación de todos los campos y que se este editando al propietrio
            btnGuardarPropietario.IsEnabled = ValidarTodosLosCampos() && opActual.Equals(OperacionActual.UpdatePropietario);
        }

        private bool ValidarTodosLosCampos()
        {            
            return !Validation.GetHasError(txtNIFPropietarioGestion) &&
                   !Validation.GetHasError(txtNombrePropietarioGestion) &&
                   !Validation.GetHasError(txtApellidosPropietarioGestion) &&
                   !Validation.GetHasError(txtEmailPropietarioGestion) &&
                   !Validation.GetHasError(txtTelefonoPropietarioGestion) &&
                   !Validation.GetHasError(dpickerFechaAlta) &&
                   !Validation.GetHasError(txtCallePropietarioGestion) &&
                   !Validation.GetHasError(txtBloquePropietarioGestion) &&
                   !Validation.GetHasError(txtPisoPropietarioGestion) &&
                   !Validation.GetHasError(txtCodigoPostalPropietarioGestion) &&
                   !Validation.GetHasError(txtLocalidadPropietarioGestion) &&
                   !Validation.GetHasError(txtProvinciaPropietarioGestion);
           
        }

        private void agregarEventoATextBoxes()
        {
            // eventos para controlar que este todo cumplimentado para modificar
            txtNIFPropietarioGestion.TextChanged += TextBox_TextChanged;
            txtNombrePropietarioGestion.TextChanged += TextBox_TextChanged;
            txtApellidosPropietarioGestion.TextChanged += TextBox_TextChanged;
            txtEmailPropietarioGestion.TextChanged += TextBox_TextChanged;
            txtTelefonoPropietarioGestion.TextChanged += TextBox_TextChanged;
            dpickerFechaAlta.SelectedDateChanged += DatePicker_SelectedDateChanged;
            txtCallePropietarioGestion.TextChanged += TextBox_TextChanged;
            txtBloquePropietarioGestion.TextChanged += TextBox_TextChanged;
            txtPisoPropietarioGestion.TextChanged += TextBox_TextChanged;
            txtCodigoPostalPropietarioGestion.TextChanged += TextBox_TextChanged;
            txtLocalidadPropietarioGestion.TextChanged += TextBox_TextChanged;
            txtProvinciaPropietarioGestion.TextChanged += TextBox_TextChanged;
            // eventos para recibir los cambios de texto para filtrar el datagrid
            txtFiltroNIF.TextChanged += txtFiltroNIF_TextChanged;
            txtFiltroNombre.TextChanged += txtFiltroNombre_TextChanged;
        }


        /*==============================================================================================================================*/
        /*                                            GESTIÓN DE CREAR PROPIETARIOS                                                     */
        /*==============================================================================================================================*/

        private void btnBorrarCamposPropietario_Click(object sender, EventArgs e)
        {
            txtNIFPropietarioCreacion.Text = "";
            txtApellidosPropietarioCreacion.Text = "";
            txtNombrePropietarioCreacion.Text = "";
            txtEmailPropietarioCreacion.Text = "";
            txtTelefonoPropietarioCreacion.Text = "";
            txtCallePropietarioCreacion.Text = "";
            txtBloquePropietarioCreacion.Text = "";
            txtPisoPropietarioCreacion.Text = "";
            txtCodigoPostalPropietarioCreacion.Text = "";
            txtLocalidadPropietarioCreacion.Text = "";
            txtProvinciaPropietarioCreacion.Text = "";          
            dpickerFechaAltaCreacion.SelectedDate = DateTime.Now;
        }



        private void btnCrearPropietario_Click(object sender, EventArgs e)
        {

        }

        private void lanzarCrearPropietario()
        {

        }

        private Propietario obtenerPropietarioDeFormulario()
        {
            Propietario propietario = new Propietario();
            propietario.Nombre = txtNombrePropietarioCreacion.Text.Trim();
            propietario.NIF = txtNIFPropietarioCreacion.Text.Trim();
            propietario.Apellidos = txtApellidosPropietarioCreacion.Text.Trim();
            propietario.Email = txtEmailPropietarioCreacion.Text.Trim();
            propietario.Fecha_alta = (DateTime)dpickerFechaAltaCreacion.SelectedDate;            
            propietario.Telefono = long.TryParse(txtTelefonoPropietarioCreacion.Text.Trim(), out long telefono) ? telefono : 0;

            propietario.Direccion = new Direccion();
            propietario.Direccion.Calle = txtCallePropietarioCreacion.Text.Trim();
            propietario.Direccion.Bloque = txtBloquePropietarioCreacion.Text.Trim();
            propietario.Direccion.Piso = txtPisoPropietarioCreacion.Text.Trim();
            propietario.Direccion.CodPostal = txtCodigoPostalPropietarioCreacion.Text.Trim();
            propietario.Direccion.Localidad = txtLocalidadPropietarioCreacion.Text.Trim();
            propietario.Direccion.Provincia = txtProvinciaPropietarioCreacion.Text.Trim();

            return propietario;
        }

        private void TextBox_TextCrearChanged(object sender, TextChangedEventArgs e)
        {
            // Verifica el estado de validación de todos los campos y que se este editando al propietrio
            btnCrearPropietario.IsEnabled = ValidarTodosLosCamposCrear();
        }

        private void DatePickerCrear_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            // Verifica el estado de validación de todos los campos y que se este editando al propietrio
            btnCrearPropietario.IsEnabled = ValidarTodosLosCamposCrear();
        }

        private bool ValidarTodosLosCamposCrear()
        {
            return !Validation.GetHasError(txtNIFPropietarioCreacion) &&
                   !Validation.GetHasError(txtNombrePropietarioCreacion) &&
                   !Validation.GetHasError(txtApellidosPropietarioCreacion) &&
                   !Validation.GetHasError(txtEmailPropietarioCreacion) &&
                   !Validation.GetHasError(txtTelefonoPropietarioCreacion) &&
                   !Validation.GetHasError(dpickerFechaAltaCreacion) &&
                   !Validation.GetHasError(txtCallePropietarioCreacion) &&
                   !Validation.GetHasError(txtBloquePropietarioCreacion) &&
                   !Validation.GetHasError(txtPisoPropietarioCreacion) &&
                   !Validation.GetHasError(txtCodigoPostalPropietarioCreacion) &&
                   !Validation.GetHasError(txtLocalidadPropietarioCreacion) &&
                   !Validation.GetHasError(txtProvinciaPropietarioCreacion);

        }

        private void agregarEventoATextBoxesCreacion()
        {
            // eventos para controlar que este todo cumplimentado para modificar
            txtNIFPropietarioCreacion.TextChanged += TextBox_TextCrearChanged;
            txtNombrePropietarioCreacion.TextChanged += TextBox_TextCrearChanged;
            txtApellidosPropietarioCreacion.TextChanged += TextBox_TextCrearChanged;
            txtEmailPropietarioCreacion.TextChanged += TextBox_TextCrearChanged;
            txtTelefonoPropietarioCreacion.TextChanged += TextBox_TextCrearChanged;
            dpickerFechaAltaCreacion.SelectedDateChanged += DatePickerCrear_SelectedDateChanged;
            txtCallePropietarioCreacion.TextChanged += TextBox_TextCrearChanged;
            txtBloquePropietarioCreacion.TextChanged += TextBox_TextCrearChanged;
            txtPisoPropietarioCreacion.TextChanged += TextBox_TextCrearChanged;
            txtCodigoPostalPropietarioCreacion.TextChanged += TextBox_TextCrearChanged;
            txtLocalidadPropietarioCreacion.TextChanged += TextBox_TextCrearChanged;
            txtProvinciaPropietarioCreacion.TextChanged += TextBox_TextCrearChanged;
            
        }

        /*==============================================================================================================================*/
        /*                                                     MÉTODOS COMUNES                                                          */
        /*==============================================================================================================================*/

        private void btnCancelar_onClick(object sender, RoutedEventArgs e)
        {          
            opActual = OperacionActual.None;           
        }

        /// <summary>
        /// Método que recoge la acción del dialoghost de aceptar, llama al método adecuado en funaión de la operación que se esté realizando.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAceptar_onClick(object sender, RoutedEventArgs e)
        {
            if (opActual.Equals(OperacionActual.DeletePropietario))
            {
                lanzarBorrarPropietario();
            }else if (opActual.Equals(OperacionActual.UpdatePropietario))
            {
                lanzarModificarUsuario();
            }


            opActual = OperacionActual.None;
        }

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
        /// Método que realiza las getiones necesarias en la IU al cambiar de TabItem
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                TabItem selectedTab = e.AddedItems[0] as TabItem;

                if (selectedTab == tabCrear)   // para actualziar los errores y vaciar los campos
                {                 
                    txtNIFPropietarioCreacion.Text = "1";
                    txtApellidosPropietarioCreacion.Text = "1";
                    txtNombrePropietarioCreacion.Text = "1";
                    txtEmailPropietarioCreacion.Text= "fff@ggg.com";
                    txtTelefonoPropietarioCreacion.Text = "1";
                    txtCallePropietarioCreacion.Text = "1";
                    txtBloquePropietarioCreacion.Text = "1";
                    txtPisoPropietarioCreacion.Text = "1";
                    txtCodigoPostalPropietarioCreacion.Text = "1";
                    txtLocalidadPropietarioCreacion.Text = "1";
                    txtProvinciaPropietarioCreacion.Text = "1";
                    btnBorrarCamposPropietario_Click(null, null);
                    dpickerFechaAltaCreacion.SelectedDate = DateTime.Now;

                }
            }
        }


        


    }
}
