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

            dataGridPropietarios.Items.Clear();
            listaPropietarios = Propietario.obtenerListaPropietarios();
            if (listaPropietarios is null)
            {
                txtVentanaEmergente1btn.Text = "Error de conexión";
                miDialogHost1btn.IsOpen = true;
            }
            else
            {
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
            this.propietarioSeleccionado = dataGridPropietarios.SelectedItem as Propietario; // recuperamos el usuario seleccinado
            rellenarCampos(this.propietarioSeleccionado);  //  rellenamos los campos con su informacion
        
            // habilitamos botones 
            
            // deshabilitamos entradas de datos
            
            // ocultamos iconos de verificacion
            
            // reiniciamos variables de control y campos de contraseña
            
            
            // eliminamos errores de campo de contraseña
            


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
            listaPropietarios = Propietario.obtenerListaPropietarios();
            if (listaPropietarios is null) // recojo el null desde Controlador si hay fallo en conexión
            {
                dataGridPropietarios.Items.Clear();
                txtVentanaEmergente1btn.Text = "Error de conexión";
                miDialogHost1btn.IsOpen = true;
            }
            else
            {
                //dataGridPropietarios.Items.Clear();
                viewPropietarios = (CollectionView)CollectionViewSource.GetDefaultView(listaPropietarios);
                dataGridPropietarios.ItemsSource = viewPropietarios;
            }

            vaciarCampos();   // vaciamos campos y dehabilitamos botones
            btnInformesPropietario.IsEnabled = false;
            btnAñadirPropiedad.IsEnabled = false;
            btnEditarPropietario.IsEnabled = false;
            btnGuardarPropietario.IsEnabled = false;           
            btnBorrarPropietario.IsEnabled = false;
            
            
            
            // ocultamos los iconos de verificacion de campos
           
            // reinciamos las variables de control
           
            // eliminamos los errores de los  bindings de nombre y contraseña
           
        }

        private void rellenarCampos(Propietario propietarioSeleccionado)
        {
            throw new NotImplementedException();
        }

        private void vaciarCampos()
        {

        }

        private void txtFiltroNombre_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (viewPropietarios != null)
            {
                // Aplica el filtro al nombre de usuario
                viewPropietarios.Filter = item =>
                {
                    if (item is Propietario propietario)
                    {
                        // Comparación sin distinción entre mayúsculas y minúsculas
                        //return propietario.Nombre.IndexOf(txtFiltroNombre.Text, StringComparison.OrdinalIgnoreCase) != -1;
                    }
                    return false;
                };
            }
        }

        private void txtFiltroNIF_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (viewPropietarios != null)
            {
                // Aplica el filtro al nombre de usuario
                viewPropietarios.Filter = item =>
                {
                    if (item is Propietario propietario)
                    {
                        // Comparación sin distinción entre mayúsculas y minúsculas
                        //return propietario.NIF.IndexOf(txtFiltroNIF.Text, StringComparison.OrdinalIgnoreCase) != -1;
                    }
                    return false;
                };
            }
        }


        private void btnInformesPropietaro_Click(object sender, EventArgs e)
        {

        }

        private void btnEditarPropietario_Click(object sender, EventArgs e)
        {

        }

        private void btnGuardarPropietario_Click(object sender, EventArgs e)
        {

        }

        private void btnAñadirPropiedadPropietario_Click(object sender, EventArgs e)
        {

        }

        private void btnBorrarPropietario_Click(object sender, EventArgs e)
        {

        }

        private void btnIrAPropiedades_Click(object sender, EventArgs e)
        {

        }




        /*==============================================================================================================================*/
        /*                                            GESTIÓN DE CREAR PROPIETARIOS                                                     */
        /*==============================================================================================================================*/







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
                    
                }
            }
        }


        


    }
}
