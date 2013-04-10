using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using NLog;

namespace RecordingLibrary
{
    public class UserCsvWriter
    {
        private static Logger m_logger = LogManager.GetCurrentClassLogger();

        public static readonly string DATE_TIME_FORMAT = "yyyy/MM/dd hh:mm:ss";

        public static readonly string[] UserFieldList =
        {
            "UserId",
            "CreationDateTime",
            "UserName",
            "AddressLine1",
            "AddressLine2",
            "AdministrativeRegion",
            "PostalRegion",
            "PostalCode",
            "Email",
            "Age",
            "AcceptsTerms"
        };

        private TextWriter m_textWriter;

        #region Constructors

        public UserCsvWriter(TextWriter textWriter)
        {
            m_textWriter = textWriter;
        }

        #endregion

        #region Properties

        public TextWriter TextWriter
        {
            get { return (m_textWriter); }
            // Read-only property
        }

        #endregion

        #region Methods

        public void WriteHeader()
        {
            bool isFirst = true;

            foreach (string fieldName in UserFieldList)
            {
                if (isFirst)
                {
                    isFirst = false;
                }
                else
                {
                    m_textWriter.Write(',');
                }

                m_textWriter.Write(CsvEncoder.Encode(fieldName));
            }

            m_textWriter.Write(m_textWriter.NewLine);
        }

        public void WriteUser(User user)
        {
            m_textWriter.Write(user.UserId);
            m_textWriter.Write(',');
            m_textWriter.Write(CsvEncoder.Encode(user.CreationDateTime.ToString(DATE_TIME_FORMAT)));
            m_textWriter.Write(',');
            m_textWriter.Write(CsvEncoder.Encode(user.UserName));
            m_textWriter.Write(',');
            m_textWriter.Write(CsvEncoder.Encode(user.AddressLine1));
            m_textWriter.Write(',');
            m_textWriter.Write(CsvEncoder.Encode(user.AddressLine2));
            m_textWriter.Write(',');
            m_textWriter.Write(CsvEncoder.Encode(user.AdministrativeRegion));
            m_textWriter.Write(',');
            m_textWriter.Write(CsvEncoder.Encode(user.PostalRegion));
            m_textWriter.Write(',');
            m_textWriter.Write(CsvEncoder.Encode(user.PostalCode));
            m_textWriter.Write(',');
            m_textWriter.Write(CsvEncoder.Encode(user.Email));
            m_textWriter.Write(',');
            m_textWriter.Write(user.Age);
            m_textWriter.Write(',');
            m_textWriter.Write(user.AcceptsTerms ? 'Y' : 'N');

            m_textWriter.Write(m_textWriter.NewLine);
        }

        #endregion

    }
}
