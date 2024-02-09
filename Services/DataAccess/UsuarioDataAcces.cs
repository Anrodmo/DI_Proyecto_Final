using DI_Proyecyo_Final.Modelo;
using DI_Proyecyo_Final.Model;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Diagnostics;

namespace DI_Proyecyo_Final.Services.DataAccess
{
    /// <summary>
    /// Clase que gestiona las interacciones  con la tabla  listaUsuarios de la BBDD
    /// </summary>
    internal class UsuarioDataAcces
    {


        /// <summary>
        /// Método que crea un nuevo usuario en la BBDD
        /// </summary>
        /// <param name="usuario"> Objeto Usuario con los datos del usuario a crear.</param>
        /// <param name="hashContraseña"> Hash de la  contraseña del nuevo usuario.</param>
        /// <returns> True: Usuario creado con éxito | False: Caso contrario.</returns>
        public static bool crearUsuario(Usuario usuario, string hashContraseña)
        {
            bool operacionCorrecta = false;
            string query = "INSERT INTO usuarios (nombre, contraseña, rol, uid, activo) VALUES (@nombre, @contraseña, @rol, @uid, @activo)";
            try
            {
                using(MySqlConnection connection = new MySqlConnection(ConexionData.CadenaConexion))
                {
                    connection.Open();
                    using (MySqlCommand comandoConsulta = new MySqlCommand(query, connection))
                    {
                        comandoConsulta.Parameters.AddWithValue("@nombre", usuario.NombreUsuario);
                        comandoConsulta.Parameters.AddWithValue("@contraseña", hashContraseña);
                        comandoConsulta.Parameters.AddWithValue("@rol", usuario.Rol);
                        comandoConsulta.Parameters.AddWithValue("@uid", Sesion.UsuarioActivo.Id); // id del usuario activo -> el que ha hecho login
                        comandoConsulta.Parameters.AddWithValue("@activo", true);
                        int resConsulta = comandoConsulta.ExecuteNonQuery();
                        operacionCorrecta = resConsulta == 1;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                MessageBox.Show("Error de conexión con la BBDD", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return operacionCorrecta;
        }

        /// <summary>
        /// Método que devuelve un usario de la BBDD a partir de su nombre de usuario.
        /// Util para obtener el usuario tras un login correcto paa establecerlo en la sesion activa
        /// </summary>
        /// <param name="nombreUsuario"> Nombre del usuario del que  se quieren obtener los datos</param>
        /// <returns>objeto Usuario con los datos del usuario si existe o null si no existe.</returns>
        public static Usuario getUsuario(String nombreUsuario)
        {
            Usuario usuario = null;
            string query = "SELECT id,nombre,rol FROM usuarios WHERE nombre like @nombre;"; 
            try
            {
                using (MySqlConnection connection = new MySqlConnection(ConexionData.CadenaConexion))
                {
                    connection.Open();

                    using (MySqlCommand comandoConsulta = new MySqlCommand(query, connection))
                    {
                        comandoConsulta.Parameters.AddWithValue("@nombre",nombreUsuario);
                        MySqlDataReader resConsulta = comandoConsulta.ExecuteReader();
                        if (resConsulta.HasRows)
                        {       // Si la  consulta ha dado resultado creo y alimento con él un objeto usuario
                            resConsulta.Read();
                            usuario = new Usuario();
                            usuario.Id = resConsulta.GetInt16(0);
                            usuario.NombreUsuario = resConsulta.GetString(1);
                            int rol = resConsulta.GetInt16(2);
                            if (rol == 1)
                            {
                                usuario.Rol = Rol.Administrador;
                            }else if (rol == 2)
                            {
                                usuario.Rol = Rol.Usuario;
                            }
                        }
                        resConsulta.Close();
                    }

                }
            }catch (Exception ex) 
            {
                usuario = null; // si hay un error de lectura no devuelvo usuario
                Console.WriteLine(ex.ToString());
                MessageBox.Show("Error de conexión con la BBDD", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }          
            return usuario;
        }

        /// <summary>
        /// Método que comrpueba si existe un nombre de usuario en la BBDD.
        /// Util para comprobar el nombre antes de crear un usuario.
        /// </summary>
        /// <param name="nombreUsuario"> Nombre del usuario del que se quieren obtener los datos</param>
        /// <returns>True: Existe el nombre de usuario en BDDD o exception. False: Si no existe nombre en la BBDD</returns>
        public static bool existeNombreUsuario(string nombreUsuario)
        {
            bool existeUsuario = true;  // ante la duda el usuario existe y no se puede crear uno igual
            string query = "SELECT nombre FROM usuarios WHERE nombre like @nombre;";
            try
            {
                using (MySqlConnection connection = new MySqlConnection(ConexionData.CadenaConexion))
                {
                    connection.Open();
                    using (MySqlCommand comandoConsulta = new MySqlCommand(query, connection))
                    {
                        comandoConsulta.Parameters.AddWithValue("@nombre", nombreUsuario);
                        MySqlDataReader resConsulta = comandoConsulta.ExecuteReader();
                        if (!resConsulta.HasRows)
                        {
                            existeUsuario = false; // solo si devuelve que no hay devuelvo false.
                        }
                        resConsulta.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                MessageBox.Show("Error de conexión con la BBDD", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return existeUsuario;
        }

        /// <summary>
        /// Lista todos los listaUsuarios en la tabla listaUsuarios de la BBDD. No devuelve el campo contraseña.
        /// </summary>
        /// <returns>Una List<ModeloLibro> con todos los libors de la BBDD</returns>
        public static List<Usuario> listarTodosLosUsuariosEnBBDD()
        {
            List<Usuario> listado = new List<Usuario>();
            string query = "SELECT id,nombre,rol,uid,ultima_modificacion,activo FROM usuarios;";

            using (MySqlConnection miConexion = new MySqlConnection(ConexionData.CadenaConexion))
            {
                miConexion.Open();
                using(MySqlCommand cmd = new MySqlCommand(query, miConexion))
                {
                    MySqlDataReader resConsulta;
                    try
                    {
                        resConsulta = cmd.ExecuteReader();
                        if (resConsulta.HasRows) { 
                            while (resConsulta.Read()) 
                            {
                                listado.Add(new Usuario()
                                {
                                    Id = Convert.ToInt32(resConsulta["id"]),
                                    NombreUsuario = resConsulta["nombre"].ToString(),
                                    Rol = (Rol)Convert.ToInt32(resConsulta["rol"]),
                                    Uid = Convert.ToInt32(resConsulta["uid"]),                                   
                                    Ultima_modificacion = Convert.ToDateTime(resConsulta["ultima_modificacion"]),
                                    Activo = Convert.ToBoolean(resConsulta["activo"])
                                }); ;
                            }
                        }
                        resConsulta.Close();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                        MessageBox.Show("Error de conexión con la BBDD", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        listado = null;
                    }

                }
            }                                          
            return listado;
        }

        /// <summary>
        /// Método que dado  un id de usuario y un rol, actualzia el  rol de ese usuario.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="nombreUsuario"></param>
        /// <returns></returns>
        public static bool actualizarRolUsuario(int id, Rol rol)
        {
            bool operacionCorrecta = false;
            string query = "UPDATE usuarios SET rol = @rol WHERE id = @id";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(ConexionData.CadenaConexion))
                {
                    connection.Open();

                    using (MySqlCommand comandoConsulta = new MySqlCommand(query, connection))
                    {
                        comandoConsulta.Parameters.AddWithValue("@rol", rol);
                        comandoConsulta.Parameters.AddWithValue("@id", id);

                        int resConsulta = comandoConsulta.ExecuteNonQuery();
                        operacionCorrecta = resConsulta == 1;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                MessageBox.Show("Error de conexión con la BBDD", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return operacionCorrecta;
        }

        /// <summary>
        /// Método que dado  un id de usuario y un nombre actualzia el  nombre de ese usuario.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="nombreUsuario"></param>
        /// <returns></returns>
        public static bool actualizarNombreUsuario(int id, string nombreUsuario)
        {
            bool operacionCorrecta = false;
            string query = "UPDATE usuarios SET nombre = @nombreUsuario WHERE id = @id";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(ConexionData.CadenaConexion))
                {
                    connection.Open();

                    using (MySqlCommand comandoConsulta = new MySqlCommand(query, connection))
                    {
                        comandoConsulta.Parameters.AddWithValue("@nombreUsuario", nombreUsuario);
                        comandoConsulta.Parameters.AddWithValue("@id", id);

                        int resConsulta = comandoConsulta.ExecuteNonQuery();
                        operacionCorrecta = resConsulta == 1;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                MessageBox.Show("Error de conexión con la BBDD", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return operacionCorrecta;
        }

        /// <summary>
        /// Metodo que actualiza el esdo de un usuario en la BBDD an base a su ID.
        /// </summary>
        /// <param name="id">  id  del usuario a modificar</param>
        /// <param name="estadoActivo"> Estado que se va a modificar True - Activo , False - Inactivo</param>
        /// <returns> True - Sila opración con la BBDD se ha  realizado con éxito. False en caso contrario.</returns>
        public static bool actualizarEstadoActivo(int id, bool estadoActivo)
        {
            bool operacionCorrecta = false;
            string query = "UPDATE usuarios SET activo = @estadoActivo WHERE id = @id";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(ConexionData.CadenaConexion))
                {
                    connection.Open();

                    using (MySqlCommand comandoConsulta = new MySqlCommand(query, connection))
                    {
                        comandoConsulta.Parameters.AddWithValue("@estadoActivo", estadoActivo);
                        comandoConsulta.Parameters.AddWithValue("@id", id);

                        int resConsulta = comandoConsulta.ExecuteNonQuery();
                        operacionCorrecta = resConsulta == 1;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                MessageBox.Show("Error de conexión con la BBDD", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return operacionCorrecta;
        }


        /// <summary>
        /// Método que actualiza la contraseña de usuario.
        /// </summary>
        /// <param name="id"> id del usuario a actualziar contraserña</param>
        /// <param name="hashContraseña"> hash Sha512 de la contraseña a actualizar</param>
        /// <returns></returns>
        public static bool actualizarContraseña( int id, string hashContraseña)
        {
            bool operacionCorrecta = false;
            string query = "UPDATE usuarios SET contraseña = @hashContraseña WHERE id = @id";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(ConexionData.CadenaConexion))
                {
                    connection.Open();

                    using (MySqlCommand comandoConsulta = new MySqlCommand(query, connection))
                    {
                        comandoConsulta.Parameters.AddWithValue("@hashContraseña", hashContraseña);
                        comandoConsulta.Parameters.AddWithValue("@id", id);

                        int resConsulta = comandoConsulta.ExecuteNonQuery();
                        operacionCorrecta = resConsulta == 1;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                MessageBox.Show("Error de conexión con la BBDD", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return operacionCorrecta;
        }

    }
}
