using MaterialDesignColors;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using MaterialDesignColors.ColorManipulation;
using System.Runtime.CompilerServices;

namespace DI_Proyecyo_Final.ViewModel
{
    /// <summary>
    /// Clase que permite ajustar toda la paleta de colores del Material Desing a partir de un color primario y/ o secundario
    /// </summary>
    public static class ViewModelTheme
    {
        static private Color colorPrimmario;
        static private Color colorSecundario;
        static private bool esTemaOscuro;

        public static Color ColorPrimmario { get => colorPrimmario; set => colorPrimmario = value; }
        public static Color ColorSecundario { get => colorSecundario; set => colorSecundario = value; }
        public static bool EsTemaOscuro { get => esTemaOscuro; set => esTemaOscuro = value; }


        /// <summary>
        /// Metodo que establece toda la paleta de colores primarios (Light, Mid, Dark) para los temas light y dark del Material Desing
        /// a partir del color facilitado por parámetro
        /// </summary>
        /// <param name="color"> color que se quiere establecer como primario</param>
        public static void CambiarColorPrimario(Color color)
        {
            colorPrimmario = color;
            PaletteHelper paletteHelper = new PaletteHelper();  
            ITheme theme = paletteHelper.GetTheme();

            theme.PrimaryLight = new ColorPair(color.Lighten());
            theme.PrimaryMid = new ColorPair(color);
            theme.PrimaryDark = new ColorPair(color.Darken());

            paletteHelper.SetTheme(theme);
        }

        /// <summary>
        /// Metodo que establece toda los colores de acento para los temas light y dark del Material Desing
        /// a partir del color facilitado por parámetro
        /// </summary>
        /// <param name="color"> color que se quiere establecer como acento</param>
        public static void CambiarColorAcento(Color color)
        {
            colorSecundario= color;
            PaletteHelper paletteHelper = new PaletteHelper();
            ITheme theme = paletteHelper.GetTheme();

            theme.SecondaryLight = new ColorPair(color.Lighten());
            theme.SecondaryMid = new ColorPair(color);
            theme.SecondaryDark = new ColorPair(color.Darken());

            paletteHelper.SetTheme(theme);
        }

        /// <summary>
        /// metodo que setea el tema de Material Design en claro u oscuro según parametro de entrada.
        /// </summary>
        /// <param name="isDarkTheme"></param>
        public static void ModificarLuzTema(bool isDarkTheme)
        {
            EsTemaOscuro= isDarkTheme;
            PaletteHelper paletteHelper = new PaletteHelper();
            ITheme theme = paletteHelper.GetTheme();
            if (isDarkTheme)
            {
                theme.SetBaseTheme(MaterialDesignThemes.Wpf.Theme.Dark);
            }
            else
            {
                theme.SetBaseTheme(MaterialDesignThemes.Wpf.Theme.Light);
            }
            paletteHelper.SetTheme(theme);
        }
    }
}
