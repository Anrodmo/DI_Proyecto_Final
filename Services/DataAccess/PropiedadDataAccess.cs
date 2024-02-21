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
    internal class PropiedadDataAccess
    {
        /// <summary>
        /// Método que elimina la propiedad introducida por parámetro.
        /// </summary>
        /// <param name="propiedad"> Objeto porpiedad a eliminar de la BBDD</param>
        /// <returns> True si la operación se realizó con éxito, False en caso contrario </returns>
        internal static bool borrarPropiedad(Propiedad propiedad)
        {
            bool exito = false;
            string query = "DELETE FROM propiedades WHERE id = @id";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(ConexionData.CadenaConexion))
                {
                    connection.Open();
                    using (MySqlCommand comandoBorrar = new MySqlCommand(query, connection))
                    {
                        comandoBorrar.Parameters.AddWithValue("@id", propiedad.Id);

                        int filasAfectadas = comandoBorrar.ExecuteNonQuery();

                        if (filasAfectadas > 0)
                        {
                            if (DireccionDataAcces.borrarDireccion(propiedad.Direccion.Id))
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
        /// Método que crea una propiedad en la BBDD
        /// </summary>
        /// <param name="propiedad"></param>
        /// <returns></returns>
        internal static bool crearPropiedad(Propiedad propiedad)
        {
            int indiceDireccion;
            bool exito = false;
            string query = "INSERT INTO propiedades (propietario, descripcion, tamaño, observaciones,direccion, tipo, uid) " +
                            "VALUES (@propietario, @descripcion, @tamaño, @observaciones, @direccion,@tipo, @uid);";

            // inserto la direccion  y obtengo el ID con el que se creado
            indiceDireccion = DireccionDataAcces.crearDireccion(propiedad.Direccion);

            if (indiceDireccion != -1)  // si no es -1 es que se a creado bien la direccion
            {
                propiedad.Direccion.Id = indiceDireccion; // lo añado al objeto propietario
                try
                {
                    using (MySqlConnection connection = new MySqlConnection(ConexionData.CadenaConexion))
                    {
                        connection.Open();
                        using (MySqlCommand comandoInsertar = new MySqlCommand(query, connection))
                        {
                            comandoInsertar.Parameters.AddWithValue("@propietario", propiedad.IdPropietario);
                            comandoInsertar.Parameters.AddWithValue("@descripcion", propiedad.Descripcion);
                            comandoInsertar.Parameters.AddWithValue("@tamaño", propiedad.Tamaño);
                            comandoInsertar.Parameters.AddWithValue("@observaciones", propiedad.Observaciones);
                            comandoInsertar.Parameters.AddWithValue("@direccion", propiedad.Direccion.Id);
                            comandoInsertar.Parameters.AddWithValue("@tipo", propiedad.TipoPropiedad);
                            comandoInsertar.Parameters.AddWithValue("@uid", Sesion.UsuarioActivo.Id);
                            int filasAfectadas = comandoInsertar.ExecuteNonQuery();
                            if (filasAfectadas > 0)
                            {
                                exito = true;
                            }
                            else // si llego aqui es que se insertó la direccion pero no se ha insertado el propietario
                            {    // borro la dirección
                                DireccionDataAcces.borrarDireccion(propiedad.Direccion.Id);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    MessageBox.Show("Error de conexión con la BBDD", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    DireccionDataAcces.borrarDireccion(propiedad.Direccion.Id);
                }
            }
            return exito;
        }

        /// <summary>
        /// Método que dada una propiedad la modifica en la BBDD con sus nuevos valores
        /// </summary>
        /// <param name="propiedad"> Propeidad con los nuevos valores que se desea modificar </param>
        /// <returns> True si la operación se realizó con éxito, False en caso contrario</returns>
        internal static bool modificarPropiedad(Propiedad propiedad)
        {
            bool exito = false;
            string query = "UPDATE propiedades " +
                           "SET descripcion = @descripcion, tamaño = @tamaño, observaciones = @observaciones, " +
                           "tipo = @tipo, uid = @uid " +
                           "WHERE id = @id;";
            try
            {
                using (MySqlConnection connection = new MySqlConnection(ConexionData.CadenaConexion))
                {
                    connection.Open();
                    using (MySqlCommand comandoModificar = new MySqlCommand(query, connection))
                    {
                        comandoModificar.Parameters.AddWithValue("@id", propiedad.Id);
                        comandoModificar.Parameters.AddWithValue("@descripcion", propiedad.Descripcion);
                        comandoModificar.Parameters.AddWithValue("@tamaño", propiedad.Tamaño);
                        comandoModificar.Parameters.AddWithValue("@observaciones", propiedad.Observaciones);
                        comandoModificar.Parameters.AddWithValue("@tipo", propiedad.TipoPropiedad);                       
                        comandoModificar.Parameters.AddWithValue("@uid", Sesion.UsuarioActivo.Id);

                        int filasAfectadas = comandoModificar.ExecuteNonQuery();

                        if (filasAfectadas > 0)  // si modifico el usuario tambien modifico su direccion
                        {
                            bool modificacionDireccionExitosa = DireccionDataAcces.modificarDireccion(propiedad.Direccion);
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
        /// Método que elimina todas las propiedades del propietario con ID facilitada por parámetro 
        /// </summary>
        /// <param name="propietarioId"> Propietario cuyas propiedades se eliminaran</param>
        /// <returns> True si la operación se realizó con éxito, False en caso contrario </returns>
        internal static bool eliminarPropiedadesPorPropietario(int propietarioId)
        {
            bool exito = false;
            string query = "DELETE FROM propiedades WHERE propietario = @id;";
            try
            {
                using (MySqlConnection connection = new MySqlConnection(ConexionData.CadenaConexion))
                {
                    connection.Open();
                    using (MySqlCommand comandoEliminar = new MySqlCommand(query, connection))
                    {
                        comandoEliminar.Parameters.AddWithValue("@id", propietarioId);
                        int filasAfectadas = comandoEliminar.ExecuteNonQuery();
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

        /// <summary>
        /// Método que actualiza el propietario de la Propiedad en la BBDD al dato del objeto
        /// </summary>
        /// <param name="propiedad"></param>
        /// <returns> True si la operación se realizó con éxito, False en caso contrario </returns>
        internal static bool actualizarPropietario(Propiedad propiedad)
        {
            bool exito = false;
            string query = "UPDATE propiedades " +
                           "SET propietario = @propietario, uid = @uid" +
                           "WHERE id = @id;";
            try
            {
                using (MySqlConnection connection = new MySqlConnection(ConexionData.CadenaConexion))
                {
                    connection.Open();
                    using (MySqlCommand comandoModificar = new MySqlCommand(query, connection))
                    {
                        comandoModificar.Parameters.AddWithValue("@id", propiedad.Id);
                        comandoModificar.Parameters.AddWithValue("@propietario", propiedad.IdPropietario);                      
                        comandoModificar.Parameters.AddWithValue("@uid", Sesion.UsuarioActivo.Id);
                        int filasAfectadas = comandoModificar.ExecuteNonQuery();                           
                        exito = filasAfectadas > 0;                                                 
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
        /// Método que devuelve una lista con todas las propeiddes de la BBDD
        /// </summary>
        /// <returns> List<Propiedad> </returns>
        internal static List<Propiedad> obtenerListaPropiedades()
        {
            List<Propiedad> listaPropiedades = new List<Propiedad>();
            string query = "SELECT propiedades.id, propietario, descripcion, tamaño, observaciones, propiedades.direccion, tipo, nif, " +

                "calle,bloque,piso,localidad,provincia,cod_postal "+
                "FROM propiedades, propietarios, direcciones " +
                "WHERE propiedades.propietario = propietarios.id and propiedades.direccion = direcciones.id;";

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
                            Propiedad propiedad = new Propiedad();
                            propiedad.Id = resConsulta.GetInt32("id");
                            propiedad.IdPropietario = resConsulta.GetInt32("propietario");
                            propiedad.NIFPropietario = resConsulta.GetString("nif");
                            propiedad.Descripcion = resConsulta.GetString("descripcion");
                            propiedad.Tamaño = resConsulta.GetInt32("tamaño");
                            propiedad.Observaciones = resConsulta.GetString("observaciones");
                            propiedad.TipoPropiedad = (TipoPropiedad)Convert.ToInt32(resConsulta["tipo"]);
                            propiedad.Direccion = new Direccion();
                            propiedad.Direccion.Id = resConsulta.GetInt32("direccion");
                            propiedad.Direccion.Calle = resConsulta.GetString("calle");
                            propiedad.Direccion.Bloque = resConsulta.GetString("bloque");
                            propiedad.Direccion.Piso = resConsulta.GetString("piso");
                            propiedad.Direccion.Localidad = resConsulta.GetString("localidad");
                            propiedad.Direccion.Provincia = resConsulta.GetString("provincia");
                            propiedad.Direccion.CodPostal = resConsulta.GetString("cod_postal");
                            //int idDireccion = resConsulta.GetInt32("direccion");
                            //propiedad.Direccion = DireccionDataAcces.getDireccion(idDireccion);
                            listaPropiedades.Add(propiedad);
                        }
                        resConsulta.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                listaPropiedades = null;  // si hay un error de lectura no devuelvo lista                
                Console.WriteLine(ex.ToString());
                MessageBox.Show("Error de conexión con la BBDD", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return listaPropiedades;
        }


        /*    rehecho por usar doble consulta y ralentizar               */

        ///// <summary>
        ///// Método que devuelve una lista con todas las propeiddes de la BBDD
        ///// </summary>
        ///// <returns> List<Propiedad> </returns>
        //internal static List<Propiedad> obtenerListaPropiedades()
        //{
        //    List<Propiedad> listaPropiedades = new List<Propiedad>();
        //    string query = "SELECT propiedades.id, propietario, descripcion, tamaño, observaciones, propiedades.direccion, tipo, nif " +
        //        "FROM propiedades, propietarios " +
        //        "WHERE propiedades.propietario = propietarios.id;";

        //    try
        //    {
        //        using (MySqlConnection connection = new MySqlConnection(ConexionData.CadenaConexion))
        //        {
        //            connection.Open();

        //            using (MySqlCommand comandoConsulta = new MySqlCommand(query, connection))
        //            {
        //                MySqlDataReader resConsulta = comandoConsulta.ExecuteReader();

        //                while (resConsulta.Read())
        //                {
        //                    Propiedad propiedad = new Propiedad();
        //                    propiedad.Id = resConsulta.GetInt32("id");
        //                    propiedad.IdPropietario = resConsulta.GetInt32("propietario");
        //                    propiedad.NIFPropietario = resConsulta.GetString("nif");
        //                    propiedad.Descripcion = resConsulta.GetString("descripcion");
        //                    propiedad.Tamaño = resConsulta.GetInt32("tamaño");
        //                    propiedad.Observaciones = resConsulta.GetString("observaciones");
        //                    propiedad.TipoPropiedad = (TipoPropiedad)Convert.ToInt32(resConsulta["tipo"]);
        //                    int idDireccion = resConsulta.GetInt32("direccion");
        //                    propiedad.Direccion = DireccionDataAcces.getDireccion(idDireccion);
        //                    listaPropiedades.Add(propiedad);
        //                }
        //                resConsulta.Close();
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        listaPropiedades = null;  // si hay un error de lectura no devuelvo lista                
        //        Console.WriteLine(ex.ToString());
        //        MessageBox.Show("Error de conexión con la BBDD", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        //    }
        //    return listaPropiedades;
        //}







    }
   

}

