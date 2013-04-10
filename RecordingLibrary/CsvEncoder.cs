using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace RecordingLibrary
{
    public class CsvEncoder
    {

        public static string Encode(string data)
        {
            StringBuilder answer = new StringBuilder();

            // Check for ',' or '"'
            Regex regex = new Regex("[,\"]");
            if (regex.IsMatch(data))
            {
                // Found one - need to quote the string
                answer.Append('"');

                // Scan for '"'
                foreach (char c in data)
                {
                    if (c == '"')
                    {
                        // Found a '"' - need to repeat it
                        answer.Append("\"\"");
                    }
                    else
                    {
                        // Otherwise innocuous character
                        answer.Append(c);
                    }
                }

                // Close the quoted string
                answer.Append('"');

                return (answer.ToString());
            }
            else
            {
                // Nothing to do - just return the original string
                return (data);
            }
        }

    }
}
