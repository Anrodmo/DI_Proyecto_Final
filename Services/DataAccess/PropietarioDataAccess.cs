using DI_Proyecyo_Final.Model;
using DI_Proyecyo_Final.Modelo;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.XPath;

namespace DI_Proyecyo_Final.Services.DataAccess
{
    internal class PropietarioDataAccess
    {

        /// <summary>
        /// Método que devuelve un propietario de la BBDD a partir de su id.
        /// </summary>
        /// <param name="idPropietario"> id del propietario del que se quieren obtener los datos</param>
        /// <returns>objeto Usuario con los datos del usuario si existe o null si no existe.</returns>
        internal static Propietario ObtenerPropietarioPorId(int idPropietario)
        {
            Propietario propietario = null;
            string query = "SELECT id, nombre, apellidos, nif, fecha_alta, email, telefono FROM propietarios WHERE id = @idPropietario;";
            try
            {
                using (MySqlConnection connection = new MySqlConnection(ConexionData.CadenaConexion))
                {
                    connection.Open();
                    using (MySqlCommand comandoConsulta = new MySqlCommand(query, connection))
                    {
                        comandoConsulta.Parameters.AddWithValue("@idPropietario", idPropietario);
                        MySqlDataReader resConsulta = comandoConsulta.ExecuteReader();

                        if (resConsulta.HasRows)
                        {
                            resConsulta.Read();
                            propietario = new Propietario();
                            propietario.Id = resConsulta.GetInt32(0);
                            propietario.Nombre = resConsulta.GetString(1);
                            propietario.Apellidos = resConsulta.GetString(2);
                            propietario.NIF = resConsulta.GetString(3);
                            propietario.Fecha_alta = resConsulta.GetDateTime(4);
                            propietario.Email = resConsulta.GetString(5);
                            propietario.Telefono = resConsulta.GetInt64(6);
                            propietario.Direccion = DireccionDataAcces.getDireccion(propietario.Id);
                        }
                        resConsulta.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                propietario = null; // si hay un error de lectura no devuelvo propietario  
                Console.WriteLine(ex.ToString());
                MessageBox.Show("Error de conexión con la BBDD", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return propietario;
        }


        /// <summary>
        /// Método que devuelve una lista con todos los  propietarios de la BBDD
        /// </summary>
        /// <returns></returns>
        internal static List<Propietario> ObtenerTodosLosPropietarios()
        {
            List<Propietario> listaPropietarios = new List<Propietario>();
            string query = "SELECT id, nombre, apellidos, nif, fecha_alta, email, telefono, direccion FROM propietarios;";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(ConexionData.CadenaConexion))
                {
                    connection.Open();

                    using (MySqlCommand comandoConsulta = new MySqlCommand(query, connection))
                    {
                        MySqlDataReader resConsulta = comandoConsulta.ExecuteReader();

                        while (resConsulta.Read())
                        {
                            Propietario propietario = new Propietario();
                            propietario.Id = resConsulta.GetInt32(0);
                            propietario.Nombre = resConsulta.GetString(1);
                            propietario.Apellidos = resConsulta.GetString(2);
                            propietario.NIF = resConsulta.GetString(3);
                            propietario.Fecha_alta = resConsulta.GetDateTime(4);
                            propietario.Email = resConsulta.GetString(5);
                            propietario.Telefono = resConsulta.GetInt64(6);
                            int idDireccion= resConsulta.GetInt32(7);
                            propietario.Direccion = DireccionDataAcces.getDireccion(idDireccion);
                            listaPropietarios.Add(propietario);
                        }
                        resConsulta.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                listaPropietarios = null;  // si hay un error de lectura no devuelvo lista                
                Console.WriteLine(ex.ToString());
                MessageBox.Show("Error de conexión con la BBDD", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return listaPropietarios;
        }


        /// <summary>
        /// Método que modifica un propietario de la  BBDD
        /// </summary>
        /// <param name="nuevoPropietario"></param>
        /// <returns> True operacion realizada con éxito, Falsecaso contrario </returns>
        internal static bool modificarPropietario(Propietario nuevoPropietario)
        {
            bool exito = false;
            string query = "UPDATE propietarios " +
                           "SET nombre = @nombre, apellidos = @apellidos, nif = @nif, " +
                           "fecha_alta = @fechaAlta, email = @email, telefono = @telefono, uid = @uid " +
                           "WHERE id = @id;";
            try
            {
                using (MySqlConnection connection = new MySqlConnection(ConexionData.CadenaConexion))
                {
                    connection.Open();
                    using (MySqlCommand comandoModificar = new MySqlCommand(query, connection))
                    {
                        comandoModificar.Parameters.AddWithValue("@id", nuevoPropietario.Id);
                        comandoModificar.Parameters.AddWithValue("@nombre", nuevoPropietario.Nombre);
                        comandoModificar.Parameters.AddWithValue("@apellidos", nuevoPropietario.Apellidos);
                        comandoModificar.Parameters.AddWithValue("@nif", nuevoPropietario.NIF);
                        comandoModificar.Parameters.AddWithValue("@fechaAlta", nuevoPropietario.Fecha_alta);
                        comandoModificar.Parameters.AddWithValue("@email", nuevoPropietario.Email);
                        comandoModificar.Parameters.AddWithValue("@telefono", nuevoPropietario.Telefono);
                        comandoModificar.Parameters.AddWithValue("@uid", Sesion.UsuarioActivo.Id);

                        int filasAfectadas = comandoModificar.ExecuteNonQuery();

                        if (filasAfectadas > 0)  // si modifico el usuario tambien modifico su direccion
                        {                           
                            bool modificacionDireccionExitosa = DireccionDataAcces.modificarDireccion(nuevoPropietario.Direccion);
                            if (modificacionDireccionExitosa)
                            {
                                exito = true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                MessageBox.Show("Error de conexión con la BBDD", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return exito;
        }


        /// <summary>
        /// Método que elimina un propietario de la BBDD, elimina tambien su direccion y sus propieadades
        /// </summary>
        /// <param name="propietario"></param>
        /// <returns> True operacion realizada con éxito, Falsecaso contrario</returns>
        /// <exception cref="NotImplementedException"></exception>
        internal static bool eliminarPropietario(Propietario propietario)
        {
            bool exito = false;
            string query = "DELETE FROM propietarios WHERE id = @id";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(ConexionData.CadenaConexion))
                {
                    connection.Open();
                    using (MySqlCommand comandoBorrar = new MySqlCommand(query, connection))
                    {
                        comandoBorrar.Parameters.AddWithValue("@id", propietario.Id);

                        int filasAfectadas = comandoBorrar.ExecuteNonQuery();

                        if (filasAfectadas > 0)
                        {
                            if(DireccionDataAcces.borrarDireccion(propietario.Direccion.Id))
                                exito = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                MessageBox.Show("Error de conexión con la BBDD", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return exito;
        }

        /// <summary>
        /// Mñetodo que crea un propietario y su dirección en  la BBDD
        /// </summary>
        /// <param name="propietario"></param>
        /// <returns> True operacion realizada con éxito, Falsecaso contrario</returns>
        internal static bool crearPropietario(Propietario propietario)
        {
            int indiceDireccion;
            bool exito = false;
            string query = "INSERT INTO propietarios (nombre, apellidos, nif, fecha_alta, email, telefono, direccion, uid) " +
                            "VALUES (@nombre, @apellidos, @nif, @fechaAlta, @email, @telefono, @direccion, @uid);";

            // inserto la direccion  y obtengo el ID con el que se creado
            indiceDireccion = DireccionDataAcces.crearDireccion(propietario.Direccion);

            if(indiceDireccion != -1)  // si no es -1 es que se a creado bien la direccion
            {
                propietario.Direccion.Id = indiceDireccion; // lo añado al objeto propietario
                try
                {
                    using (MySqlConnection connection = new MySqlConnection(ConexionData.CadenaConexion))
                    {
                        connection.Open();
                        using (MySqlCommand comandoInsertar = new MySqlCommand(query, connection))
                        {
                            comandoInsertar.Parameters.AddWithValue("@nombre", propietario.Nombre);
                            comandoInsertar.Parameters.AddWithValue("@apellidos", propietario.Apellidos);
                            comandoInsertar.Parameters.AddWithValue("@nif", propietario.NIF);
                            comandoInsertar.Parameters.AddWithValue("@fechaAlta", propietario.Fecha_alta);
                            comandoInsertar.Parameters.AddWithValue("@email", propietario.Email);
                            comandoInsertar.Parameters.AddWithValue("@telefono", propietario.Telefono);
                            comandoInsertar.Parameters.AddWithValue("@direccion", propietario.Direccion.Id);
                            comandoInsertar.Parameters.AddWithValue("@uid", Sesion.UsuarioActivo.Id);

                            int filasAfectadas = comandoInsertar.ExecuteNonQuery();

                            if (filasAfectadas > 0)
                            {
                                exito = true;
                            }
                            else // si llego aqui es que se insertó la direccion pero no se ha insertado el propietario
                            {    // borro la dirección
                                DireccionDataAcces.borrarDireccion(propietario.Direccion.Id);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    MessageBox.Show("Error de conexión con la BBDD", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    DireccionDataAcces.borrarDireccion(propietario.Direccion.Id);
                }
            }            
            return exito;
            
        }
    }
}
