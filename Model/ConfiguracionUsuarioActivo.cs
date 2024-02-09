using DI_Proyecyo_Final.Services.ManagerXML;
using DI_Proyecyo_Final.ViewModel;
using MaterialDesignColors;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace DI_Proyecyo_Final.Model
{
    public class ConfiguracionUsuarioActivo
    {
        private static string rutaArchivo = "..\\..\\Resources\\config.xml";

        public Color ColorPrimario { get; set; }
        public Color ColorSecundario { get; set; }
        public bool IsDarkTheme { get; set; }
        public string NombreUsuario { get; set; }

        /// <summary>
        /// Establece commo configuración la configuración por defecto de la aplicacion
        /// </summary>
        public ConfiguracionUsuarioActivo()
        {
            this.IsDarkTheme = false;
            ColorPrimario = SwatchHelper.Lookup[MaterialDesignColor.Brown];
            ColorSecundario = SwatchHelper.Lookup[MaterialDesignColor.Amber];
        }


        

        public void leerYEstablecerConfiguracion(string nombreUsuario)
        {
            this.NombreUsuario = nombreUsuario; // asignamos ya el  nombre de usuario
            ConfiguracionUsuarioActivo confEnDisco = ParserXML.LeerConfiguracionDesdeXml(nombreUsuario, rutaArchivo);
            if (confEnDisco != null)
            {   // si hay una  configuracion en disco la asignamos a la configuración activa
                this.ColorPrimario=confEnDisco.ColorPrimario;
                this.ColorSecundario = confEnDisco.ColorSecundario;
                this.IsDarkTheme = confEnDisco.IsDarkTheme;               
            }
            reflejarConfiguraciónEnVista(); // y reflejamos en vista la configuracion correspondiente
        }

        public void guardarConfiguracionEnDisco()
        {
            this.ColorPrimario = ViewModelTheme.ColorPrimmario;
            this.ColorSecundario= ViewModelTheme.ColorSecundario;
            this.IsDarkTheme = ViewModelTheme.EsTemaOscuro;
            ParserXML.EscribirConfiguracionEnXml(this,rutaArchivo);
        }

        


        /// <summary>
        /// Método que refleja en la Vista la configuración almacenada en el objeto.
        /// </summary>
        private void reflejarConfiguraciónEnVista()
        {
            ViewModelTheme.CambiarColorPrimario(ColorPrimario);
            ViewModelTheme.CambiarColorAcento(ColorSecundario);
            ViewModelTheme.ModificarLuzTema(IsDarkTheme);
        }

        /// <summary>
        /// Método que crea una ruta de forma segura gestionando las barras según el SO.
        /// </summary>
        /// <param name="nombreArchivo">Nombre del archivo</param>
        /// <param name="carpeta"></param>
        /// <returns></returns>
        static string ObtenerRutaArchivo(string nombreArchivo, string carpeta)
        {           
            string directorioEjecutable = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            return Path.Combine(directorioEjecutable, carpeta, nombreArchivo);
        }



    }
}
