using DI_Proyecyo_Final.Model;
using DI_Proyecyo_Final.Modelo;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DI_Proyecyo_Final.Services.DataAccess
{
    internal class PropietarioDataAccess
    {

        /// <summary>
        /// Método que devuelve un propietario de la BBDD a partir de su id.
        /// </summary>
        /// <param name="idPropietario"> id del propietario del que se quieren obtener los datos</param>
        /// <returns>objeto Usuario con los datos del usuario si existe o null si no existe.</returns>
        public static Propietario ObtenerPropietarioPorId(int idPropietario)
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
                            propietario.Telefono = resConsulta.GetInt32(6);
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


        public static List<Propietario> ObtenerTodosLosPropietarios()
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
                            propietario.Telefono = resConsulta.GetInt32(6);
                            propietario.Id= resConsulta.GetInt32(7);
                            propietario.Direccion = DireccionDataAcces.getDireccion(propietario.Id);
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


        public static bool modificarPropietario(Propietario nuevoPropietario)
        {
            bool exito = false;
            string query = "UPDATE propietarios " +
                           "SET nombre = @nombre, apellidos = @apellidos, nif = @nif, " +
                           "fecha_alta = @fechaAlta, email = @email, telefono = @telefono " +
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


        internal static bool eliminarPropietario(Propietario propietario)
        {
            throw new NotImplementedException();
        }
    }
}
