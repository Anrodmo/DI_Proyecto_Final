using ControlzEx.Standard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DI_Proyecyo_Final.Model
{
    internal class Direccion
    {
        private int id;
        private string calle;
        private string bloque;
        private string piso;
        private string localidad;
        private string provincia;
        private string codPostal;

        public Direccion() { }

        public string Calle { 
            get => calle; 
            set 
            {
                char[] caracteresNoPermitidos = { ' ', '\t', '\n', '\r' };
                value.Trim(caracteresNoPermitidos);
                calle = value.Length > 120 ? value.Substring(0, 120) : value; // la trunco si ocupa mas de lo que admite la BBDD
            } 
        }

        public string Bloque
        {
            get => bloque;
            set
            {
                char[] caracteresNoPermitidos = { ' ', '\t', '\n', '\r' };
                value.Trim(caracteresNoPermitidos);
                bloque = value.Length > 10 ? value.Substring(0, 10) : value; // la trunco si ocupa mas de lo que admite la BBDD
            }
        }

        public string Piso { 
            get => piso; 
            set
            {
                char[] caracteresNoPermitidos = { ' ', '\t', '\n', '\r' };
                value.Trim(caracteresNoPermitidos);
                piso = value.Length > 10 ? value.Substring(0, 10) : value; // la trunco si ocupa mas de lo que admite la BBDD
            }
        }

        public string Localidad { 
            get => localidad; 
            set
            {
                char[] caracteresNoPermitidos = { ' ', '\t', '\n', '\r' };
                value.Trim(caracteresNoPermitidos);
                localidad = value.Length > 60 ? value.Substring(0, 60) : value; // la trunco si ocupa mas de lo que admite la BBDD
            }
        }

        public string Provincia { 
            get => provincia; 
            set
            {
                char[] caracteresNoPermitidos = { ' ', '\t', '\n', '\r' };
                value.Trim(caracteresNoPermitidos);
                provincia = value.Length > 60 ? value.Substring(0, 60) : value; // la trunco si ocupa mas de lo que admite la BBDD
            }
        }

        public string CodPostal { 
            get => codPostal; 
            set
            {
                char[] caracteresNoPermitidos = { ' ', '\t', '\n', '\r' };
                value.Trim(caracteresNoPermitidos);
                value = value.Length > 5 ? value.Substring(0, 5) : value; // la trunco si ocupa mas de lo que admite la BBDD
                string patron = @"^\d+$";
                Regex regex = new Regex(patron);
                codPostal = regex.IsMatch(value) ? value : "00000"; // si el formato no es válido no lo guardo
            }
        }

        public int Id { get => id; set => id = value; }
    }
}
