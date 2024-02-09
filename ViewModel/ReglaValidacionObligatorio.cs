using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DI_Proyecyo_Final.ViewModel
{
    internal class ReglaValidacionObligatorio : ValidationRule
    {
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Valida si el textbox tiene algo introducido  , campos obligatorios
        /// </summary>
        /// <param name="value"></param>
        /// <param name="cultureInfo"></param>
        /// <returns></returns>
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            return string.IsNullOrWhiteSpace((value ?? "").ToString())? 
            new ValidationResult(false, "Este campo es obligatorio."):ValidationResult.ValidResult;
        }

        
    }
}
