using DI_Proyecyo_Final.Services.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DI_Proyecyo_Final.Model
{
    internal class Propietario
    {
        private int id;
        private string nombre;
        private string apellidos;
        private string nif;
        private DateTime fecha_alta;
        private string email;
        private int telefono;
        Direccion direccion;

        public Propietario() { }

        

        public string Nombre
        {
            get => nombre; 
            set
            {
                char[] caracteresNoPermitidos = { ' ', '\t', '\n', '\r' };
                value.Trim(caracteresNoPermitidos);
                nombre = value.Length>60?value.Substring(0,60):value; // la trunco si ocupamas de lo que admite la BBDD
            }
        }
        public string Apellidos
        {
            get => apellidos;
            set
            {
                char[] caracteresNoPermitidos = { ' ', '\t', '\n', '\r' };
                value.Trim(caracteresNoPermitidos);
                apellidos = value.Length > 60 ? value.Substring(0, 60) : value; // la trunco si ocupamas de lo que admite la BBDD
            }
        }

        public string NIF
        {
            get => nif;
            set
            {                   // retiro caracteres que la gente suele poner en los nif
                char[] caracteresNoPermitidos = {' ', '-' ,'/', '.', ','};
                foreach (char caracter in caracteresNoPermitidos)
                {
                    value = value.Replace(caracter.ToString(), "");
                }
                value= value.Length > 9 ? value.Substring(0, 9) : value; // lo trunco si ocupamas de lo que admite la BBDD
                string patron = @"^[a-zA-Z]\d{7}[a-zA-Z]$|^\d{8}[a-zA-Z]$";
                Regex regex = new Regex(patron);
                nif = regex.IsMatch(value) ? value : "Erróneo"; // si el formato no es válido guardo error (esto no debe llegar a pasar)
            }
        }

        public DateTime Fecha_alta { get => fecha_alta; set => fecha_alta = value; }

        public string Email
        {
            get => email;
            set
            {
                value.Trim();  // quito espacios en blanco en incio y fin de la cadena
                value= value.Length > 100 ? value.Substring(0, 100) : value;   // lo trunco si ocupamas de lo que admite la BBDD
                string patron = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
                Regex regex = new Regex(patron);
                email = regex.IsMatch(value) ? value: "Erróneo"; // si el formato no es válido guardo error (esto no debe llegar a pasar)
            }
        }

        public int Telefono { get => telefono; set => telefono = value; }
        public int Id { get => id; set => id = value; }
        internal Direccion Direccion { get => direccion; set => direccion = value; }

        internal static List<Propietario> obtenerListaPropietarios()
        {
            return PropietarioDataAccess.ObtenerTodosLosPropietarios();
        }
    }






}
