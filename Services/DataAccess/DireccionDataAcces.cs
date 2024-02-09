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
    /// <summary>
    /// Clase que gestiona las interacciones  con la tabla  direcciones de la BBDD
    /// </summary>
    internal class DireccionDataAcces
    {

        /// <summary>
        /// Método que crea unanuevadireccion en la BBDD
        /// </summary>
        /// <param name="usuario"> Objeto direccion con los datos de la  dirección a crear.</param>
        /// <returns> int Id de la dirección creada si la operacion ha tenido éxito. 0 en caso contrario.
        public static int crearDireccion(Direccion direccion)
        {
            int ultimaID = 0;
            string query = "INSERT INTO direcciones (calle, bloque, piso, localidad, provincia, cod_postal, uid) " +
                "VALUES (@calle, @bloque, @piso, @localidad, @provincia, @cod_postal, @uid)";
            try
            {
                using (MySqlConnection connection = new MySqlConnection(ConexionData.CadenaConexion))
                {
                    connection.Open();
                    using (MySqlCommand comandoConsulta = new MySqlCommand(query, connection))
                    {
                        comandoConsulta.Parameters.AddWithValue("@calle", direccion.Calle);
                        comandoConsulta.Parameters.AddWithValue("@bloque", direccion.Bloque);
                        comandoConsulta.Parameters.AddWithValue("@piso", direccion.Piso);
                        comandoConsulta.Parameters.AddWithValue("@localidad", direccion.Localidad);
                        comandoConsulta.Parameters.AddWithValue("@provincia", direccion.Provincia);
                        int codigo_postal = int.Parse(direccion.CodPostal);
                        comandoConsulta.Parameters.AddWithValue("@cod_postal", codigo_postal);
                        comandoConsulta.Parameters.AddWithValue("@uid", Sesion.UsuarioActivo.Id);
                       
                        int resConsulta = comandoConsulta.ExecuteNonQuery();
                        ultimaID = (int)comandoConsulta.LastInsertedId;  // recogo el id con el que se ha creado la direccion en la BBDD
                        
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                MessageBox.Show("Error de conexión con la BBDD", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
             
            return ultimaID = 0;
        }

        /// <summary>
        /// Método que devuelve una direccion de la BBDD a partir de su id.
        /// </summary>
        /// <param name="id"> de la  direccion de la que  se quieren obtener los datos</param>
        /// <returns>objeto Direccion con los datos de la direccion si existe o null si no existe.</returns>
        public static Direccion getDireccion(int id)
        {
            Direccion direccion = null;
            string query = "SELECT calle,bloque,piso,localidad,provincia,cod_postal " +
                "FROM direcciones WHERE id = @id;";
            try
            {
                using (MySqlConnection connection = new MySqlConnection(ConexionData.CadenaConexion))
                {
                    connection.Open();

                    using (MySqlCommand comandoConsulta = new MySqlCommand(query, connection))
                    {
                        comandoConsulta.Parameters.AddWithValue("@id", id);
                        MySqlDataReader resConsulta = comandoConsulta.ExecuteReader();
                        if (resConsulta.HasRows)
                        {       // Si la  consulta ha dado resultado creo y alimento con él un objeto direccion
                            resConsulta.Read();
                            direccion = new Direccion();
                            direccion.Calle = resConsulta.GetString(0);
                            direccion.Bloque = resConsulta.GetString(1);
                            direccion.Piso = resConsulta.GetString(2);
                            direccion.Localidad = resConsulta.GetString(3);
                            direccion.Provincia = resConsulta.GetString(4);
                            direccion.CodPostal = resConsulta.GetString(5);
                            direccion.Id = id;
                        }
                        resConsulta.Close();
                    }

                }
            }
            catch (Exception ex)
            {
                direccion = null;
                Console.WriteLine(ex.ToString());
                MessageBox.Show("Error de conexión con la BBDD", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return direccion;
        }

        public static bool modificarDireccion(int id, Direccion nuevaDireccion)
        {
            bool exito = false;
            string query = "UPDATE direcciones " +
                           "SET calle = @calle, bloque = @bloque, piso = @piso, " +
                           "localidad = @localidad, provincia = @provincia, cod_postal = @codPostal " +
                           "WHERE id = @id;";
            try
            {
                using (MySqlConnection connection = new MySqlConnection(ConexionData.CadenaConexion))
                {
                    connection.Open();

                    using (MySqlCommand comandoModificar = new MySqlCommand(query, connection))
                    {
                        comandoModificar.Parameters.AddWithValue("@id", id);
                        comandoModificar.Parameters.AddWithValue("@calle", nuevaDireccion.Calle);
                        comandoModificar.Parameters.AddWithValue("@bloque", nuevaDireccion.Bloque);
                        comandoModificar.Parameters.AddWithValue("@piso", nuevaDireccion.Piso);
                        comandoModificar.Parameters.AddWithValue("@localidad", nuevaDireccion.Localidad);
                        comandoModificar.Parameters.AddWithValue("@provincia", nuevaDireccion.Provincia);
                        comandoModificar.Parameters.AddWithValue("@codPostal", nuevaDireccion.CodPostal);

                        int filasAfectadas = comandoModificar.ExecuteNonQuery();

                        if (filasAfectadas > 0)
                        {
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



    }
}
