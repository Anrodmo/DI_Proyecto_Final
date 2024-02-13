﻿using DI_Proyecyo_Final.Model;
using DI_Proyecyo_Final.ViewModel;
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
    /// Lógica de interacción para VistaPropiedades.xaml
    /// </summary>
    public partial class VistaPropiedades : Page
    {

        private CollectionView viewPropiedades;
        private OperacionActual opActual = OperacionActual.None;
        private Propiedad propiedadSeleccionada = null;
        private List<Propiedad> listaPropiedades;
        public VistaPropiedades()
        {
            InitializeComponent();
            // se añaden los contextos de datos de los validadores y los notificadores de los textboxes
            var contenedor = new ContenedorDataContext();
            contenedor.CambioEnTexto = new NotificacionCambioEnTextBoxes();
            contenedor.FechaCorrecta = new NotificacionFechaCorrecta();
            DataContext = contenedor;

            agregarEventoATextBoxes(); // agrego  todos los textboxes a un evento comun para que cuando ninguno tenga error
            // se habilite el boton de modificar
            agregarEventoATextBoxesCreacion();  // lo mismo para el boton de crear propiedad

            dataGridPropiedades.Items.Clear();
            listaPropiedades = Propiedad.obtenerListaPropiedades();
            if (listaPropiedades is null)
            {
                txtVentanaEmergente1btn.Text = "Error de conexión";
                miDialogHost1btn.IsOpen = true;
            }
            else
            {   // aqui uso CollectionView porque quiero aplicar filtros al datagrid
                viewPropiedades = (CollectionView)CollectionViewSource.GetDefaultView(listaPropiedades);
                dataGridPropiedades.ItemsSource = viewPropiedades;
            }
        }

        /*==============================================================================================================================*/
        /*                      GESTIÓN DE LISTAR, MODIFICAR, ELIMINAR PROPIEDADES                                                      */
        /*==============================================================================================================================*/


        /// <summary>
        /// Metodo que actualiza la vista al seleccionarse un elemento del datagrid. Llama al método que rellena los campor y 
        /// actualiza la visibilidad de iconos y los que botones que deben estar habilitados y deshabilitados.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridPropiedades_Selected(object sender, EventArgs e)
        {
            habilitarCampos(false);
            this.propiedadSeleccionada = dataGridPropiedades.SelectedItem as Propiedad; // recuperamos el usuario seleccinado
            if (this.propiedadSeleccionada != null)
            {
                rellenarCamposGestion(this.propiedadSeleccionada);  //  rellenamos los campos con su informacion

                // habilitamos botones 
                btnIrAPropietario.IsEnabled = true;
                btnInformesPropiedad.IsEnabled = true;

                btnEditarPropiedad.IsEnabled = true;
                btnGuardarPropiedad.IsEnabled = false;

                btnBorrarPropiedad.IsEnabled = true;
                // deshabilitamos entradas de datos

            }
            // reiniciamos variables de control y campos de contraseña
            this.opActual = OperacionActual.None;

        }

        /// <summary>
        /// Metodo que refresca el DataGrid con la BBDD y limpia todos los campos y cambia el estado de botones e iconos
        /// a su  valor incial
        /// </summary>
        /// <param name="senser"></param>
        /// <param name="e"></param>
        private void btnActualzarPropiedades_onClick(object senser, EventArgs e)
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
            btnIrAPropietario.IsEnabled = false;
            btnInformesPropiedad.IsEnabled = false;
            
            btnEditarPropiedad.IsEnabled = false;
            btnGuardarPropiedad.IsEnabled = false;

            btnBorrarPropiedad.IsEnabled = false;

            dataGridPropiedades.UnselectAll();
            listaPropiedades = Propiedad.obtenerListaPropiedades();
            if (listaPropiedades is null) // recojo el null desde Controlador si hay fallo en conexión
            {
                dataGridPropiedades.Items.Clear();
                txtVentanaEmergente1btn.Text = "Error de conexión";
                miDialogHost1btn.IsOpen = true;
            }
            else
            {
                viewPropiedades = (CollectionView)CollectionViewSource.GetDefaultView(listaPropiedades);
                dataGridPropiedades.ItemsSource = viewPropiedades;
            }

            // ocultamos los iconos de verificacion de campos
           
            // reinciamos las variables de control
            this.propiedadSeleccionada = null;
            this.opActual = OperacionActual.None;
            // eliminamos los errores de los  bindings de nombre y contraseña
            BindingExpression be = txtNIFPropiedadGestion.GetBindingExpression(TextBox.TextProperty);
            Validation.ClearInvalid(be);
            be = txtDescripcionPropiedadGestion.GetBindingExpression(TextBox.TextProperty);
            Validation.ClearInvalid(be);
            be = txtTamañoPropiedadGestion.GetBindingExpression(TextBox.TextProperty);
            Validation.ClearInvalid(be);
            
            
            be = txtCallePropiedadGestion.GetBindingExpression(TextBox.TextProperty);
            Validation.ClearInvalid(be);
            be = txtBloquePropiedadGestion.GetBindingExpression(TextBox.TextProperty);
            Validation.ClearInvalid(be);
            be = txtPisoPropiedadGestion.GetBindingExpression(TextBox.TextProperty);
            Validation.ClearInvalid(be);
            be = txtLocalidadPropiedadGestion.GetBindingExpression(TextBox.TextProperty);
            Validation.ClearInvalid(be);
            be = txtProvinciaPropiedadGestion.GetBindingExpression(TextBox.TextProperty);
            Validation.ClearInvalid(be);
            be = txtCodigoPostalPropiedadGestion.GetBindingExpression(TextBox.TextProperty);
            Validation.ClearInvalid(be);            
        }

        private void vaciarCampos()
        {
            txtNIFPropiedadGestion.Text = "";
            txtDescripcionPropiedadGestion.Text = "";
            txtTamañoPropiedadGestion.Text = "";
            txtObservacionesGestion.Text = "";
            
            cmbTipoPropiedad.SelectedIndex = 0;

            txtCallePropiedadGestion.Text = "";
            txtBloquePropiedadGestion.Text = "";
            txtPisoPropiedadGestion.Text = "";
            txtCodigoPostalPropiedadGestion.Text = "";
            txtLocalidadPropiedadGestion.Text = "";
            txtProvinciaPropiedadGestion.Text = "";

            txtFiltroNIFPropietario.Text = "";
            txtFiltroDescripcion.Text = "";
        }


        private void habilitarCampos(bool habilitar)
        {
            txtNIFPropiedadGestion.IsEnabled = habilitar;
            txtDescripcionPropiedadGestion.IsEnabled = habilitar;
            txtTamañoPropiedadGestion.IsEnabled= habilitar;
            txtObservacionesGestion.IsEnabled = habilitar;

            cmbTipoPropiedad.SelectedIndex = 0;
            cmbTipoPropiedad.IsEnabled= habilitar;

            txtCallePropiedadGestion.IsEnabled = habilitar;
            txtBloquePropiedadGestion.IsEnabled = habilitar;
            txtPisoPropiedadGestion.IsEnabled = habilitar;
            txtCodigoPostalPropiedadGestion.IsEnabled = habilitar;
            txtLocalidadPropiedadGestion.IsEnabled = habilitar;
            txtProvinciaPropiedadGestion.IsEnabled = habilitar;

            txtFiltroNIFPropietario.IsEnabled = habilitar;
            txtFiltroDescripcion.IsEnabled = habilitar;

        }

        private void rellenarCamposGestion(Propiedad propiedad)
        {
            txtNIFPropiedadGestion.Text = propiedad.NIFPropietario;
            txtDescripcionPropiedadGestion.Text = propiedad.Descripcion;
            txtTamañoPropiedadGestion.Text = propiedad.Tamaño.ToString();
            txtObservacionesGestion.Text = propiedad.Observaciones;

            cmbTipoPropiedad.SelectedIndex = (int)(propiedad.TipoPropiedad -1);
            
            txtCallePropiedadGestion.Text = propiedad.Direccion.Calle;
            txtBloquePropiedadGestion.Text = propiedad.Direccion.Bloque;
            txtPisoPropiedadGestion.Text = propiedad.Direccion.Piso;
            txtCodigoPostalPropiedadGestion.Text = propiedad.Direccion.CodPostal;
            txtLocalidadPropiedadGestion.Text = propiedad.Direccion.Localidad;
            txtProvinciaPropiedadGestion.Text = propiedad.Direccion.Provincia;         
        }

        private void cargarCamposEnPropiedadSeleccionado()
        {
            this.propiedadSeleccionada.NIFPropietario = txtNIFPropiedadGestion.Text.Trim();
            this.propiedadSeleccionada.Descripcion = txtDescripcionPropiedadGestion.Text.Trim();
            this.propiedadSeleccionada.Tamaño = int.Parse(txtTamañoPropiedadGestion.Text.Trim());
            this.propiedadSeleccionada.Observaciones = txtObservacionesGestion.Text.Trim();
            this.propiedadSeleccionada.TipoPropiedad = (TipoPropiedad)(cmbTipoPropiedad.SelectedIndex + 1);

            this.propiedadSeleccionada.Direccion.Calle = txtCallePropiedadGestion.Text.Trim();
            this.propiedadSeleccionada.Direccion.Bloque = txtBloquePropiedadGestion.Text.Trim();
            this.propiedadSeleccionada.Direccion.Piso = txtPisoPropiedadGestion.Text.Trim();
            this.propiedadSeleccionada.Direccion.CodPostal = txtCodigoPostalPropiedadGestion.Text.Trim();
            this.propiedadSeleccionada.Direccion.Localidad = txtLocalidadPropiedadGestion.Text.Trim();
            this.propiedadSeleccionada.Direccion.Provincia = txtProvinciaPropiedadGestion.Text.Trim();
        }


        private void txtFiltroDescripcion_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (viewPropiedades != null)
            {
                txtFiltroNIFPropietario.Text = "";
                // Aplica el filtro al nombre de propiedad
                viewPropiedades.Filter = item =>
                {
                    if (item is Propietario propietario)
                    {
                        // Comparación sin distinción entre mayúsculas y minúsculas
                        return propietario.Nombre.IndexOf(txtFiltroDescripcion.Text, StringComparison.OrdinalIgnoreCase) != -1;
                    }
                    return false;
                };
            }
        }

        private void txtFiltroNIFPropietario_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (viewPropiedades != null)
            {
                txtFiltroDescripcion.Text = "";
                // Aplica el filtro al NIf del propiedad
                viewPropiedades.Filter = item =>
                {
                    if (item is Propietario propietario)
                    {
                        // Comparación sin distinción entre mayúsculas y minúsculas
                        return propietario.NIF.IndexOf(txtFiltroNIFPropietario.Text, StringComparison.OrdinalIgnoreCase) != -1;
                    }
                    return false;
                };
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Verifica el estado de validación de todos los campos y que se este editando al propietrio
            btnGuardarPropiedad.IsEnabled = ValidarTodosLosCampos() && opActual.Equals(OperacionActual.UpdatePropiedad);
        }
       

        private bool ValidarTodosLosCampos()
        {          
            return !Validation.GetHasError(txtNIFPropiedadGestion) &&
                   !Validation.GetHasError(txtDescripcionPropiedadGestion) &&
                   !Validation.GetHasError(txtTamañoPropiedadGestion) &&                  
                   !Validation.GetHasError(txtCallePropiedadGestion) &&
                   !Validation.GetHasError(txtBloquePropiedadGestion) &&
                   !Validation.GetHasError(txtPisoPropiedadGestion) &&
                   !Validation.GetHasError(txtCodigoPostalPropiedadGestion) &&
                   !Validation.GetHasError(txtLocalidadPropiedadGestion) &&
                   !Validation.GetHasError(txtProvinciaPropiedadGestion);

        }

        private void agregarEventoATextBoxes()
        {
            // eventos para controlar que este todo cumplimentado para modificar
            txtNIFPropiedadGestion.TextChanged += TextBox_TextChanged;
            txtDescripcionPropiedadGestion.TextChanged += TextBox_TextChanged;
            txtTamañoPropiedadGestion.TextChanged += TextBox_TextChanged;
           
            txtCallePropiedadGestion.TextChanged += TextBox_TextChanged;
            txtBloquePropiedadGestion.TextChanged += TextBox_TextChanged;
            txtPisoPropiedadGestion.TextChanged += TextBox_TextChanged;
            txtCodigoPostalPropiedadGestion.TextChanged += TextBox_TextChanged;
            txtLocalidadPropiedadGestion.TextChanged += TextBox_TextChanged;
            txtProvinciaPropiedadGestion.TextChanged += TextBox_TextChanged;
            // eventos para recibir los cambios de texto para filtrar el datagrid
            txtFiltroNIFPropietario.TextChanged += txtFiltroNIFPropietario_TextChanged;
            txtFiltroDescripcion.TextChanged += txtFiltroDescripcion_TextChanged;
        }


        private void btnIrAPropietario_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnInformesPropiedad_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnEditarPropiedad_Click(object sender, RoutedEventArgs e)
        {
            opActual = OperacionActual.UpdatePropiedad;
            habilitarCampos(true);
        }

        private void btnGuardarPropiedad_Click(object sender, RoutedEventArgs e)
        {
            if (this.propiedadSeleccionada != null)
            {
                this.opActual = OperacionActual.UpdatePropiedad;
                txtVentanaEmergente2btn.Text = "¿ Desea modificar la propiedad "+propiedadSeleccionada.Descripcion
                    +"\nal propetario "+this.propiedadSeleccionada.NIFPropietario+" ?";
                miDialogHost2btn.IsOpen = true;
            }
        }

        private void lanzarModificarPropiedad()
        {
            cargarCamposEnPropiedadSeleccionado(); // leo los txt y los pongo en el usuario
            bool modificadoConExito = this.propiedadSeleccionada.modificarPropiedad();
            if (modificadoConExito) refrescarDataGrid();  // si se modifico correctamente refresco la pestaña de gestion de usuarios
            String mensaje = modificadoConExito ? "Propiedad modificada con éxito" : "Error, no se modifico la propiedad"; // mensaje segun resultado
            lanzarSnackBar(mensaje, 2);  // lanzo el snackbar que informa al usuario.
        }

        

        private void btnBorrarPropiedad_Click(object sender, RoutedEventArgs e)
        {
            if (this.propiedadSeleccionada != null)
            {
                this.opActual = OperacionActual.DeletePropiedad;
                txtVentanaEmergente2btn.Text = "Va a eliminar la propiedad "+propiedadSeleccionada.Descripcion+
                    "\n del propiedad "+propiedadSeleccionada.NIFPropietario+" de forma definitiva."
                   + "\n\n¿ Desea continuar ?";
                miDialogHost2btn.IsOpen = true;
            }

        }

        private void lanzarBorrarPropiedad()
        {
            bool eliminadoConExito = propiedadSeleccionada.borrarPropiedad();

            if (eliminadoConExito) refrescarDataGrid();  // si se elimino correctamente refresco la pestaña de gestion 
            String mensaje = eliminadoConExito ? "Propiedd eliminada con éxito" : "Error, no se eliminó la propiedad"; // mensaje segun resultado
            lanzarSnackBar(mensaje, 2);  // lanzo el snackbar que informa al usuario.
        }


        /*==============================================================================================================================*/
        /*                                            GESTIÓN DE CREAR PROPIETARIOS                                                     */
        /*==============================================================================================================================*/

        private void btnBorrarCamposPropiedad_Click(object sender, EventArgs e)
        {
            borrarCamposCreacionPropiedad();
        }

        private void borrarCamposCreacionPropiedad()
        {
            txtNIFPropiedadCreacion.Text = "";
            txtDescripcionPropiedadCreacion.Text = "";
            txtTamañoPropiedadCreacion.Text = "";
            txtObservacionesPropiedadCreacion.Text = "";

            cmbTipoPropiedadCrear.SelectedIndex = 0;

            txtCallePropiedadCreacion.Text = "";
            txtBloquePropiedadCreacion.Text = "";
            txtPisoPropiedadCreacion.Text = "";
            txtCodigoPostalPropiedadCreacion.Text = "";
            txtLocalidadPropiedadCreacion.Text = "";
            txtProvinciaPropiedadCreacion.Text = "";            
        }



        private void btnCrearPropiedad_Click(object sender, EventArgs e)
        {
            this.opActual = OperacionActual.CreatePropiedad;
            txtVentanaEmergente2btn.Text = "¿ Desea crear la propiedad "+txtDescripcionPropiedadCreacion+"" +
                "\npara el propiedad "+txtNIFPropiedadCreacion+" ?";
            miDialogHost2btn.IsOpen = true;
        }

        private void lanzarCrearPropiedad()
        {
            Propiedad prop = obtenerPropiedadDeFormulario();
            bool creadoConExito = prop.crearPropiedad();

            if (creadoConExito) borrarCamposCreacionPropiedad();  // si se modifico correctamente borro los campos de creacion de propiedad
            String mensaje = creadoConExito ? "Propiedad creada con éxito" : "Error, no se creo la propiedad"; // mensaje segun resultado
            lanzarSnackBar(mensaje, 2);  // lanzo el snackbar que informa al usuario.
        }

        private Propiedad obtenerPropiedadDeFormulario()
        {
            Propiedad propiedad = new Propiedad();
            propiedad.Descripcion = txtDescripcionPropiedadCreacion.Text.Trim();
            propiedad.NIFPropietario = txtNIFPropiedadCreacion.Text.Trim();
            propiedad.Observaciones = txtObservacionesPropiedadCreacion.Text.Trim();

            propiedad.TipoPropiedad = (TipoPropiedad)(cmbTipoPropiedadCrear.SelectedIndex - 1);
            propiedad.Tamaño = int.TryParse(txtTamañoPropiedadCreacion.Text.Trim(), out int telefono) ? telefono : 0;

            propiedad.Direccion = new Direccion();
            propiedad.Direccion.Calle = txtCallePropiedadCreacion.Text.Trim();
            propiedad.Direccion.Bloque = txtBloquePropiedadCreacion.Text.Trim();
            propiedad.Direccion.Piso = txtPisoPropiedadCreacion.Text.Trim();
            propiedad.Direccion.CodPostal = txtCodigoPostalPropiedadCreacion.Text.Trim();
            propiedad.Direccion.Localidad = txtLocalidadPropiedadCreacion.Text.Trim();
            propiedad.Direccion.Provincia = txtProvinciaPropiedadCreacion.Text.Trim();

            return propiedad;
        }

        private void TextBox_TextCrearChanged(object sender, TextChangedEventArgs e)
        {
            // Verifica el estado de validación de todos los campos y que se este editando al propietrio
            btnCrearPropiedad.IsEnabled = ValidarTodosLosCamposCrear();
        }


        private bool ValidarTodosLosCamposCrear()
        {
            return !Validation.GetHasError(txtDescripcionPropiedadCreacion) &&
                   !Validation.GetHasError(txtNIFPropiedadCreacion) &&
                   !Validation.GetHasError(txtTamañoPropiedadCreacion) &&                  
                   
                   !Validation.GetHasError(txtCallePropiedadCreacion) &&
                   !Validation.GetHasError(txtBloquePropiedadCreacion) &&
                   !Validation.GetHasError(txtPisoPropiedadCreacion) &&
                   !Validation.GetHasError(txtCodigoPostalPropiedadCreacion) &&
                   !Validation.GetHasError(txtLocalidadPropiedadCreacion) &&
                   !Validation.GetHasError(txtProvinciaPropiedadCreacion);

        }

        private void agregarEventoATextBoxesCreacion()
        {
            // eventos para controlar que este todo cumplimentado para modificar
            txtDescripcionPropiedadCreacion.TextChanged += TextBox_TextCrearChanged;
            txtNIFPropiedadCreacion.TextChanged += TextBox_TextCrearChanged;
            txtTamañoPropiedadCreacion.TextChanged += TextBox_TextCrearChanged;

            txtCallePropiedadCreacion.TextChanged += TextBox_TextCrearChanged;
            txtBloquePropiedadCreacion.TextChanged += TextBox_TextCrearChanged;
            txtPisoPropiedadCreacion.TextChanged += TextBox_TextCrearChanged;
            txtCodigoPostalPropiedadCreacion.TextChanged += TextBox_TextCrearChanged;
            txtLocalidadPropiedadCreacion.TextChanged += TextBox_TextCrearChanged;
            txtProvinciaPropiedadCreacion.TextChanged += TextBox_TextCrearChanged;
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
            if (opActual.Equals(OperacionActual.DeletePropiedad))
            {
                lanzarBorrarPropiedad();
            }
            else if (opActual.Equals(OperacionActual.UpdatePropiedad))
            {
                lanzarModificarPropiedad();
            }
            else if (opActual.Equals(OperacionActual.CreatePropiedad))
            {
                lanzarCrearPropiedad();
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
        /// Método que realiza las gestiones necesarias en la IU al cambiar de TabItem
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TabControlPropiedades_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                TabItem selectedTab = e.AddedItems[0] as TabItem;

                if ( selectedTab !=null && selectedTab == tabCrearProp)   // para actualziar los errores y vaciar los campos
                {
                    txtNIFPropiedadCreacion.Text = "1";
                    txtDescripcionPropiedadCreacion.Text = "1";
                    txtTamañoPropiedadCreacion.Text = "1";

                    txtCallePropiedadCreacion.Text = "1";
                    txtBloquePropiedadCreacion.Text = "1";
                    txtPisoPropiedadCreacion.Text = "1";
                    txtCodigoPostalPropiedadCreacion.Text = "1";
                    txtLocalidadPropiedadCreacion.Text = "1";
                    txtProvinciaPropiedadCreacion.Text = "1";

                    btnBorrarCamposPropiedad_Click(null, null);
                }
                else if (selectedTab == tabGestionProp)
                {
                    refrescarDataGrid();
                }
            }
        }

        
    }
}
