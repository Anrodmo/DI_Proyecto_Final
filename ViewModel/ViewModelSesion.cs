using DI_Proyecyo_Final.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DI_Proyecyo_Final.ViewModel
{
    /// <summary>
    /// Esta clase maneja el acceso a las  distinas Pages.xaml en función del Rol del Usuario Activo, si el usuario no tiene permisos
    /// para acceder a una Page determinada devolvera un acceso a una Page vacía. Es un control adicional a que la Vista tenga habilitados
    /// o visibles elementos del menú. Simplifica el código en el c# tras el xaml y centraliza la gestión de los paths a las Pages.
    /// </summary>
    internal class ViewModelSesion
    {         

        static private List<string> pagePaths = new List<string>
            {
                "VistaLogin.xaml",                   // manejo el ciclo de cargado de páginas según el indice de la lista
                "VistaUsuarios.xaml",                 // me permite cargar el nombre de la pagina a cargar en el frame en
                "VistaPropietarios.xaml",                 // función del índice. Se ha planteado un ciclo circular. 
                "VistaPropiedades.xaml",                  //  --> 0-1-2-3-1-2-3...  (indices dependeran de rol de usuario)
                "VistaVacia.xaml"                         //  <-- 0-3-2-1-3-2-1...  (indices dependeran de rol de usuario)
            };

        static private int indiceVistaVacia = pagePaths.Count() - 1;
        static private int indiceMinimoViews = indiceVistaVacia; // manejo a que views se puede acceder, incialmente hasta incio de sesion solo a VistaVacia
        static private int indiceMaximoViews = indiceVistaVacia; // manejo a que views se puede acceder, incialmente hasta incio de sesion solo a VistaVacia
        static private int indiceActualCarrusel = indiceVistaVacia; // Para almacenar el indice actual de la Page vista.
        static private string pathInicioSesion = pagePaths[0];

        public static int IndiceMinimoViews { get => indiceMinimoViews; }
        public static string PathInicioSesion { get => pathInicioSesion; }
        public static int IndiceMaximoViews { get => indiceMaximoViews; }
        public static int IndiceActualCarrusel { get => indiceActualCarrusel; }
        public static List<string> PagePaths { get => pagePaths; }


        /// <summary>
        /// Metodo que establece las configuraciones inciales de la  clase en funcion del Rol del Usuario Activo de la clase Sesion.
        /// Estó manejará a que vistas se tendrá acceso desde los xaml.
        /// </summary>
        public static void establecerViewModelSesion()
        {
            if(Sesion.UsuarioActivo != null) // si se ha inciado sesion correctamente
            {
                indiceMinimoViews = Sesion.UsuarioActivo.Rol.Equals(Rol.Administrador) ? 1 : 2;
                indiceMaximoViews = pagePaths.Count()-2;
                indiceActualCarrusel = indiceMinimoViews;
            }
            else // si no hay login se establece el  path a la vista vacia coom el único disponible
            {
                indiceActualCarrusel= indiceVistaVacia;
                indiceMaximoViews= indiceVistaVacia;
                indiceMinimoViews= indiceVistaVacia;
            }
            
        }
        
        /// <summary>
        /// Metodo que  devuelve el path a  la Page anterior a la actual, tiene en cuenta la existencia de un login exitoso.
        /// </summary>
        /// <returns></returns>
        public static string getPathAnterior()
        {
            string retorno;
            if (Sesion.UsuarioActivo != null)  // si no hay objeto no hay login
            {
                indiceActualCarrusel--;
                if (indiceActualCarrusel < indiceMinimoViews)  // los indices serán unos u  otros en funcion
                {
                    indiceActualCarrusel = indiceMaximoViews;  // de los privilegios del  usuario activo
                }
                retorno= pagePaths[indiceActualCarrusel];

            }
            else
            {
                retorno = pagePaths[indiceVistaVacia];
            }
            return retorno;
        }

        /// <summary>
        /// Metodo que  devuelve el path a  la Page posterior a la actual, tiene en cuenta la existencia de un login exitoso.
        /// </summary>
        /// <returns></returns>
        public static string getPathPosterior()
        {
            string retorno;
            if (Sesion.UsuarioActivo != null) // si no hay objeto no hay login
            {
                indiceActualCarrusel++;
                if (indiceActualCarrusel > indiceMaximoViews)   // los indices serán unos u  otros en funcion
                {
                    indiceActualCarrusel = indiceMinimoViews;   // de los privilegios del  usuario activo
                }
                retorno = pagePaths[indiceActualCarrusel];

            }
            else
            {
                retorno = pagePaths[indiceVistaVacia];
            }
            return retorno;
        }


        /// <summary>
        /// Método que devuelve el path a la Page de gestión de listaUsuarios para que a cargue la vista. Si no hay usuario activo
        /// o el usuario activo no es adminstrador devuelve path a Page vacía. Se puede usar el método  de la clase Sesion bool
        /// permisoAccesoUsuarios() para comprobar antes.
        /// </summary>
        /// <returns> string con el path a la Page de gestión de listaUsuarios</returns>
        public static string getPathUsuarios()
        {
            indiceActualCarrusel = 1;
            return Sesion.UsuarioActivo != null && Sesion.UsuarioActivo.Rol.Equals(Rol.Administrador) ? pagePaths[1] : pagePaths[indiceVistaVacia];
        }

        /// <summary>
        /// Método que devuelve el path a la Page de gestión de propietarios para que a cargue la vista. Si no hay usuario activo
        /// devuelve path a Page vacía. Se puede usar el método  de la clase Sesion bool 
        /// permisoAccesoPropietarios() para comprobar antes.
        /// </summary>
        /// <returns> string con el path a la Page de gestión de propietarios</returns>
        public static string getPathPropietarios()
        {
            indiceActualCarrusel = 2;
            return Sesion.UsuarioActivo != null ? pagePaths[2] : pagePaths[indiceVistaVacia];
        }

        /// <summary>
        /// Método que devuelve el path a la Page de gestión de propiedades para que a cargue la vista. Si no hay usuario activo
        /// devuelve path a Page vacía. Se puede usar el método  de la clase Sesion bool
        /// permisoAccesoPropiedades() para comprobar antes.
        /// </summary>
        /// <returns> string con el path a la Page de gestión de propiedades</returns>
        public static string getPathPropiedades()
        {
            indiceActualCarrusel = 3;
            return Sesion.UsuarioActivo != null ? pagePaths[3] : pagePaths[indiceVistaVacia];
        }


        /// <summary>
        /// Método que devuelve el path a la Page de incio de sesion para que a cargue la vista. 
        /// </summary>
        /// <returns> string con el path a la Page de gestión de propiedades</returns>
        public static string getPathInicioSesion()
        {
            indiceActualCarrusel = 0;
            return pagePaths[0];
        }

        /// <summary>
        /// Método que devuelve el path a la Page establecida como actual en el carrusel para que a cargue la vista. 
        /// </summary>
        /// <returns> string con el path a la Page de gestión de la vista actual del carrusel</returns>
        public static string getPathCarruselActual()
        {          
            return pagePaths[indiceActualCarrusel];
        }


    }
}
