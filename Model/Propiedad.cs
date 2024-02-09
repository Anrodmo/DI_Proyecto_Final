using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        Solar = 4
    }
    internal class Propiedad
    {
        private Propietario propietario;
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
        internal Propietario Propietario { get => propietario; set => propietario = value; }
        internal Direccion Direccion { get => direccion; set => direccion = value; }
    }
}
