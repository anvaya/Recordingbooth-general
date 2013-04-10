using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using NLog;

namespace RecordingLibrary
{
    public class RecordingCsvWriter
    {
        private static Logger m_logger = LogManager.GetCurrentClassLogger();

        public static readonly string DATE_TIME_FORMAT = "yyyy/MM/dd hh:mm:ss";

        public static readonly string[] RecordingFieldList =
        {
            "RecordingId",
            "UserId",
            "RecordingTimestamp",
            "FileName"
        };

        private TextWriter m_textWriter;

        #region Constructors

        public RecordingCsvWriter(TextWriter textWriter)
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

            foreach (string fieldName in RecordingFieldList)
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

        public void WriteRecording(Recording recording)
        {
            m_textWriter.Write(recording.RecordingId);
            m_textWriter.Write(',');
            m_textWriter.Write(recording.UserId);
            m_textWriter.Write(',');
            m_textWriter.Write(CsvEncoder.Encode(recording.RecordingTimestamp.ToString(DATE_TIME_FORMAT)));
            m_textWriter.Write(',');
            m_textWriter.Write(CsvEncoder.Encode(recording.FileName));

            m_textWriter.Write(m_textWriter.NewLine);
        }

        #endregion

    }
}
