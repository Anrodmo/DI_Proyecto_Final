using DI_Proyecyo_Final.Services.DataAccess;
using Google.Protobuf.WellKnownTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Web.SessionState;

namespace DI_Proyecyo_Final.Model


{

    /// <summary>
    /// Referencia a la tabla de roles de la BBDD, los valores de enum coinciden con los id de los roles de la tabla
    /// </summary>
    public enum Rol
    {
        Administrador=1,  // esta mapeado con el mismo numero de id de la clave foránea de  roles
        Usuario=2
    }


    /// <summary>
    /// Clase que representa un usuario de la aplicación y un registro de la tabla usuarios de la BBDD.
    /// Dispone de los métodos para lanzar el CRUD del usuario. (no se puede eliminar, solo marcar como  inactivo)
    /// </summary>
    public class Usuario
    {
        private string nombreUsuario;
        public int Id { get; set; }
        
        public Rol Rol { get; set; }

        public int Uid { get; set; }

        public DateTime Ultima_modificacion { get; set; }

        public bool Activo { get; set; }

        public Usuario() { }

        public  Usuario(String nombre, Rol rol) {
            this.nombreUsuario = nombre;
            this.Rol = rol; 
        }

        public string NombreUsuario
        {
            get => nombreUsuario;
            set
            {
                char[] caracteresNoPermitidos = { ' ', '\t', '\n', '\r' };
                value.Trim(caracteresNoPermitidos);
                nombreUsuario = value.Length > 50 ? value.Substring(0, 50) : value; // la trunco si ocupa mas de lo que admite la BBDD
            }
        }


        /// <summary>
        /// Método que crea el usuario en la  BBDD con con la contraseña facilitada.
        /// </summary>
        /// <param name="hash"></param>
        /// <returns> True -> usuario creado con éxito. False -> En caso contrario.</returns>
        public bool añadirUsuarioBBDD(SecureString contraseña)
        {
            String hash = Login.CalcularHashSHA512(contraseña);
            return UsuarioDataAcces.crearUsuario(this, hash);
        }

        /// <summary>
        /// Metodo  que lanza el  cambio de nombre del usuario en la  BBDD
        /// </summary>
        /// <param name="nombre"> Nuevo nombre.  </param>
        /// <returns> Si la actualización se ha llevado a cabo con éxito en la BBDD</returns>
        public bool actualizarNombre(string nombre)
        {
            char[] caracteresNoPermitidos = { ' ', '\t', '\n', '\r' };
            nombre.Trim(caracteresNoPermitidos);
            nombre = nombre.Length > 50 ? nombre.Substring(0, 50) : nombre; // la trunco si ocupa mas de lo que admite la BBDD

            return UsuarioDataAcces.actualizarNombreUsuario(this.Id, nombre);
        }

        /// <summary>
        /// Método que lanza el cambio de rol del usuario en la BBDD.
        /// </summary>
        /// <param name="rol"> Nuevo rol del usuario.</param>
        /// <returns> Si la actualización se ha llevado a cabo con éxito en la BBDD</returns>
        public bool actualizarRol(Rol rol)
        {
            return UsuarioDataAcces.actualizarRolUsuario(this.Id, rol);
        }

        /// <summary>
        /// Método que actualiza el estado del usuario activo/inactivo
        /// </summary>
        /// <param name="activo"> Nuevo estado </param>
        /// <returns> Si la actualización se ha llevado a cabo con éxito en la BBDD </returns>
        public bool actualizarActivo(bool activo)
        {
            return UsuarioDataAcces.actualizarEstadoActivo(this.Id, activo);
        }

        /// <summary>
        /// Método que lanza la actualziación en  la BBDD de la contraseña del usuario.
        /// </summary>
        /// <param name="contraserña"> Secure String con la nueva cantraseña para el usuario</param>
        /// <returns> True si la operación se ha llevado  con éxito en la BBDD, False en caso  contrario.</returns>
        public bool actualizarContraseña(SecureString contraserña)
        {
            string hash = Login.CalcularHashSHA512(contraserña);
            return UsuarioDataAcces.actualizarContraseña(this.Id,hash);
        }

        /// <summary>
        /// Método que llama a la BBDD para comprobar si existe el nombre de usuario facilitado  por parámetro.
        /// </summary>
        /// <param name="nombre"></param>
        /// <returns> </returns>
        static public bool nombreUsuarioUnico(string nombre)
        {
            char[] caracteresNoPermitidos = { ' ', '\t', '\n', '\r' };
            nombre.Trim(caracteresNoPermitidos);
            nombre = nombre.Length > 50 ? nombre.Substring(0, 50) : nombre; // la trunco si ocupa mas de lo que admite la BBDD

            return nombre.Length > 0 ?  !UsuarioDataAcces.existeNombreUsuario(nombre) : false;
        }

        /// <summary>
        /// Método que devuelve una lista con todos los listaUsuarios de la BBDD
        /// </summary>
        /// <returns> Lista de objetos Usuario, null si ha habido error en la conexión con la BBDD </returns>
        static public  List<Usuario> obtenerListadoUsuarios()
        {
            return UsuarioDataAcces.listarTodosLosUsuariosEnBBDD();
        }



        
    }
}
