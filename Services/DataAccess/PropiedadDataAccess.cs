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
        internal static bool borrarPropiedad(Propiedad propiedad)
        {
            return true;
        }

        internal static bool crearPropiedad(Propiedad propiedad)
        {
            return true;
        }

        internal static bool modificarPropiedad(Propiedad propiedad)
        {
            return true;
        }

        internal static List<Propiedad> obtenerListaPropiedades()
        {
            List<Propiedad> listaPropiedades = new List<Propiedad>();
            string query = "SELECT propiedades.id, propietario, descripcion, tamaño, observaciones, propiedades.direccion, tipo, nif " +
                "FROM propiedades, propietarios " +
                "WHERE propiedades.propietario = propietarios.id;";

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
                            int idDireccion = resConsulta.GetInt32("direccion");
                            propiedad.Direccion = DireccionDataAcces.getDireccion(idDireccion);
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







    }

}

