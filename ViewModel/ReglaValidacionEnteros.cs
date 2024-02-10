using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DI_Proyecyo_Final.ViewModel
{
    internal class ReglaValidacionEnteros : ValidationRule
    {
        /// <summary>
        /// Valida que el dato no sea anulo y que sea parseable a int
        /// </summary>
        /// <param name="value"></param>
        /// <param name="cultureInfo"></param>
        /// <returns></returns>
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            bool validacionCorrecta = true;
            long result;
            string input = value as string;
            string mensaje = "";

            if (string.IsNullOrEmpty(input))
            {
                validacionCorrecta = false;
                mensaje = "Campo obligatorio";
            }
            else if (!long.TryParse(input, NumberStyles.Integer, CultureInfo.InvariantCulture, out result))
            {
                validacionCorrecta = false;
                mensaje = "Solo admite números";
            }
            else if(long.Parse(input) <1)
            {
                validacionCorrecta = false;
                mensaje = "Número incorrecto";
            }

            return validacionCorrecta ? ValidationResult.ValidResult : new ValidationResult(false, mensaje);
        }


    }
    
    
}
