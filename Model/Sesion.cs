using DI_Proyecyo_Final.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DI_Proyecyo_Final.Model
{
    /// <summary>
    /// Clase estática que gestiona la sesión activa en la aplicación.
    /// </summary>
    static public class Sesion
    {

        static private Usuario usuarioActivo; //  almaceno el usuario que se ha logeado con éxito
        static private ConfiguracionUsuarioActivo configuracionUsuairoActivo;   // objeto con la configuracion  del usuario                                  

        public static Usuario UsuarioActivo{get => usuarioActivo;}  // no se puede setear, solo obtener
        internal static ConfiguracionUsuarioActivo ConfiguracionUsuarioActivo { get => configuracionUsuairoActivo; set => configuracionUsuairoActivo = value; }

        public static void inciarSesion(Usuario usuario)
        {
            usuarioActivo=usuario;
            ViewModelSesion.establecerViewModelSesion(); // Cada vez que se establece el usuario activo se configura
                                // el ViewModelSesion que maneja a que vistas se tiene acceso en funcion del usuario.
            ConfiguracionUsuarioActivo = new ConfiguracionUsuarioActivo(); // creamos una configuración
            ConfiguracionUsuarioActivo.leerYEstablecerConfiguracion(UsuarioActivo.NombreUsuario); // buscamos y asignamos la configuracion del disco
        }

        public static void cerrarSesion()
        {
            usuarioActivo = null;
            ViewModelSesion.establecerViewModelSesion();// si se cierra la  sesión se actualiza el ViewModelSesion para que
             // solo facilite Page vacia.
            ConfiguracionUsuarioActivo.guardarConfiguracionEnDisco();
        }

        

        

    }
   
}
