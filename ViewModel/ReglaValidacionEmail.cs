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
    internal class ReglaValidacionEmail : ValidationRule
    {
        

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            bool validaciónCorrecta = false;
            string mensaje = "sin definir";
            string input = (value ?? "").ToString();
            string patron = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            Regex regex = new Regex(patron);

            // Es un campo obligatorioa
            if (string.IsNullOrWhiteSpace(input))
            {
                mensaje = "Campo obligatorio";
            }
            else if (regex.IsMatch(input))
            {
                return ValidationResult.ValidResult;
            }
            else
            {
                mensaje = "Formato no válido";
            }

            return validaciónCorrecta? ValidationResult.ValidResult: new ValidationResult(false, mensaje);
        }
    }
}

