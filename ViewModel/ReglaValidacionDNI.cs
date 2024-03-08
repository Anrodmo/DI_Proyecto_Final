using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DI_Proyecyo_Final.ViewModel
{
    internal class ReglaValidacionDNI : ValidationRule
    {
        

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            
            bool validaciónCorrecta = false;
            string mensaje = "";
            string input = (value ?? "").ToString();
            string patron = @"^[a-zA-Z]\d{7}[a-zA-Z]$|^\d{8}[a-zA-Z]$";
            Regex regex = new Regex(patron);

            // Es un campo obligatorioa
            if (string.IsNullOrWhiteSpace(input))
            {
                mensaje = "Campo obligatorio";
            }
            else if (regex.IsMatch(input))
            {
                validaciónCorrecta = true;
            }
            else
            {
                mensaje = "Formato no válido";
            }

            return validaciónCorrecta ? ValidationResult.ValidResult : new ValidationResult(false, mensaje);
        }
    }
}
