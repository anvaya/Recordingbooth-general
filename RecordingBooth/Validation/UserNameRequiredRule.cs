using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Windows.Controls;

namespace RecordingBooth.Validation
{
    public class UserNameRequiredRule : ValidationRule
    {
        public UserNameRequiredRule()
        {
            ;
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string stringValue = value as string;

            if ((null == stringValue) || (0 == stringValue.Length))
            {
                return new ValidationResult(false, "Name is required");
            }
            else
            {
                return new ValidationResult(true, null);
            }
        }

    }
}
