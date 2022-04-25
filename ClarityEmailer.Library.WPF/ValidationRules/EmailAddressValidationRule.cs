using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace ClarityEmailer.Library.WPF;
public class EmailAddressValidationRule : ValidationRule
{
    public override ValidationResult Validate(object value, CultureInfo cultureInfo)
    {
        if (value is not string str)
        {
            return new ValidationResult(false, "E-mail address can not be blank");
        }

        if (!Regex.IsMatch(str, @$"[^@ \t\r\n]+@[^@ \t\r\n]+\.[^@ \t\r\n]+"))
        {
            return new ValidationResult(false, "Please enter a valid e-mail address");
        }

        return ValidationResult.ValidResult;
    }
}
