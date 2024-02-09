using DI_Proyecyo_Final.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DI_Proyecyo_Final.Services.ManagerXML
{
    internal static class ParserXML
    {
        


        static public bool EscribirConfiguracionEnXml(ConfiguracionUsuarioActivo configuracion, string rutaArchivo)
        {
            try
            {
                // Leer todas las configuraciones existentes
                List<ConfiguracionUsuarioActivo> configuraciones = LeerConfiguracionesDesdeXml(rutaArchivo);

                // Buscar si el usuario ya existe en la lista
                ConfiguracionUsuarioActivo configuracionExistente = configuraciones.FirstOrDefault(c => c.NombreUsuario == configuracion.NombreUsuario);

                if (configuracionExistente != null)
                {
                    // Sobrescribir la configuración si el usuario existe
                    configuracionExistente.ColorPrimario = configuracion.ColorPrimario;
                    configuracionExistente.ColorSecundario = configuracion.ColorSecundario;
                    configuracionExistente.IsDarkTheme = configuracion.IsDarkTheme;
                }
                else
                {
                    // Agregar una nueva configuración si el usuario no existe
                    configuraciones.Add(configuracion);
                }

                // Serializar y escribir en el archivo
                XmlSerializer serializer = new XmlSerializer(typeof(List<ConfiguracionUsuarioActivo>));

                using (StreamWriter writer = new StreamWriter(rutaArchivo))
                {
                    serializer.Serialize(writer, configuraciones);
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al escribir en el archivo XML: {ex.Message}");
                return false;
            }
        }

        static public ConfiguracionUsuarioActivo LeerConfiguracionDesdeXml(string nombreUsuario, string rutaArchivo)
        {
            ConfiguracionUsuarioActivo configuracionLeida = null;
            try
            {
                // Intentar leer el archivo existente
                XmlSerializer serializer = new XmlSerializer(typeof(List<ConfiguracionUsuarioActivo>));

                using (StreamReader reader = new StreamReader(rutaArchivo))
                {

                    List<ConfiguracionUsuarioActivo> configuraciones = (List<ConfiguracionUsuarioActivo>)serializer.Deserialize(reader);

                    ConfiguracionUsuarioActivo configuracionExistente = configuraciones.FirstOrDefault(c => c.NombreUsuario == nombreUsuario);

                    
                    configuracionLeida = configuracionExistente;
                }
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine($"Error al leer desde el archivo XML: {ex.Message}");
                Console.WriteLine($"InnerException: {ex.InnerException?.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al leer desde el archivo XML: {ex.Message}");
                Console.WriteLine($"InnerException: {ex.InnerException?.Message}");

            }

            return configuracionLeida;
        }


        static public List<ConfiguracionUsuarioActivo> LeerConfiguracionesDesdeXml(string rutaArchivo)
        {
            try
            {
                // Intentar leer el archivo existente
                XmlSerializer serializer = new XmlSerializer(typeof(List<ConfiguracionUsuarioActivo>));

                using (StreamReader reader = new StreamReader(rutaArchivo))
                {
                    return (List<ConfiguracionUsuarioActivo>)serializer.Deserialize(reader);
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Archivo XML no encontrado. Se creará uno nuevo.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al leer desde el archivo XML: {ex.Message}");
            }

            // Si hay un error o el archivo no existe, devuelve una lista vacía
            return new List<ConfiguracionUsuarioActivo>();
        }

    }
}
