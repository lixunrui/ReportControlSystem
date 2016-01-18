using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.Windows.Controls;

namespace ReportControlSystem
{
    internal class DecimalValidation : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            try
            {
                decimal decimalValue = (decimal)value;
            }
            catch (System.Exception ex)
            {
                return new ValidationResult(false, "Please insert a decimal value");
            }
            return new ValidationResult(true, null);
        }
    }
}
