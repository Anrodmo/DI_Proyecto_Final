using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DI_Proyecyo_Final.ViewModel
{
    internal class ReglaValidacionFechaObligatoria : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            //if (value is DateTime fecha) // Si el valor es una fecha
            //{
            //    if (fecha < DateTime.Now)
            //    {
            //        // La fecha es válida si es mayor o igual a la fecha actual.
            //        return ValidationResult.ValidResult;
            //    }
            //}

            //// La fecha no es válida, devuelve un error.
            //return new ValidationResult(false, "Fecha no válida.");

            // solo admitimos fechas nulas (es opcional) o que  sean válidad y no futuras

            return (value is null ) ?
                new ValidationResult(false, "Fecha no válida."): ValidationResult.ValidResult;
        }

    }
    
}
