using ControlzEx.Standard;
using DI_Proyecyo_Final.Services.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DI_Proyecyo_Final.Model
{
    /// <summary>
    /// Referencia a la tabla de tipo_propiedad de la BBDD, los valores de enum coinciden con los id de los tipos de la tabla
    /// </summary>
    public enum TipoPropiedad
    {
        Habitable = 1,  // esta mapeado con el mismo numero de id de la clave foránea de tipos de propiedades
        Local = 2,
        PlazaGaraje = 3,
        Terreno = 4
    }
    internal class Propiedad
    {
        private int id;
        private int idPropietario;
        private string nifPropietario;
        private string descripcion;
        private int tamaño;
        private string observaciones;
        private Direccion direccion;
        private TipoPropiedad tipoPropiedad;

        public Propiedad() { }

        public string Descripcion
        {
            get => descripcion;
            set
            {
                char[] caracteresNoPermitidos = { ' ', '\t', '\n', '\r' };
                value.Trim(caracteresNoPermitidos);
                descripcion = value.Length > 100 ? value.Substring(0, 100) : value; // la trunco si ocupa mas de lo que admite la BBDD
            }
        }

        public int Tamaño { get => tamaño; set => tamaño = value; }

        public string Observaciones
        {
            get => observaciones;
            set
            {               
                value.Trim();
                observaciones = value.Length > 250 ? value.Substring(0, 250) : value; // la trunco si ocupa mas de lo que admite la BBDD
            }
        }

        public TipoPropiedad TipoPropiedad { get => tipoPropiedad; set => tipoPropiedad = value; }
        //internal Propietario Propietario { get => propietario; set => propietario = value; }
        internal Direccion Direccion { get => direccion; set => direccion = value; }

        public string NIFPropietario
        {
            get => nifPropietario;
            set
            {                   // retiro caracteres que la gente suele poner en los nif
                char[] caracteresNoPermitidos = { ' ', '-', '/', '.', ',' };
                foreach (char caracter in caracteresNoPermitidos)
                {
                    value = value.Replace(caracter.ToString(), "");
                }
                value = value.Length > 9 ? value.Substring(0, 9) : value; // lo trunco si ocupamas de lo que admite la BBDD
                string patron = @"^[a-zA-Z]\d{7}[a-zA-Z]$|^\d{8}[a-zA-Z]$";
                Regex regex = new Regex(patron);
                nifPropietario = regex.IsMatch(value) ? value : "Erróneo"; // si el formato no es válido guardo error (esto no debe llegar a pasar)
            }
        }

        public int Id { get => id; set => id = value; }
        public int IdPropietario { get => idPropietario; set => idPropietario = value; }

        internal static List<Propiedad> obtenerListaPropiedades()
        {
            return PropiedadDataAccess.obtenerListaPropiedades();
        }

        internal bool modificarPropiedad()
        {
            return PropiedadDataAccess.modificarPropiedad(this);
        }

        internal bool borrarPropiedad()
        {
            return PropiedadDataAccess.borrarPropiedad(this);
        }

        internal bool crearPropiedad()
        {
            return PropiedadDataAccess.crearPropiedad(this);
        }

        internal bool modificarPropiedadPropietario()
        {
            return PropiedadDataAccess.actualizarPropietario(this);
        }
    }
}
