using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Windows.Controls;
using RecordingLibrary;

namespace RecordingBooth.Validation
{
    public class MaxRecordingTimeRule : ValidationRule
    {
        private TimeSpan m_min;
        private TimeSpan m_max;

        public MaxRecordingTimeRule()
        {
            m_min = new TimeSpan(0, 1, 0);
            m_max = new TimeSpan(0, Recording.MAXIMUM_RECORDING_TIME, 0);
        }

        public TimeSpan Min
        {
            get { return m_min; }
            set { m_min = value; }
        }

        public TimeSpan Max
        {
            get { return m_max; }
            set { m_max = value; }
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            TimeSpan testTime = new TimeSpan(0);

            if (!TimeSpan.TryParseExact(value as string, AdminViewModel.MAX_TIME_FORMAT, CultureInfo.CurrentCulture, out testTime))
            {
                return new ValidationResult(false, "Incorrect time format");
            }

            if ((testTime < Min) || (testTime > Max))
            {
                return new ValidationResult(false,
                  "Please enter a value in the range "
                  + Min.ToString(AdminViewModel.MAX_TIME_FORMAT)
                  + " to "
                  + Max.ToString(AdminViewModel.MAX_TIME_FORMAT)
                  + ".");
            }
            else
            {
                return new ValidationResult(true, null);
            }
        }

    }
}
