using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace RecordingLibrary
{
    public class Recording
    {
        public const int MAXIMUM_RECORDING_TIME = 20;       // Minutes

        public const int UNASSIGNED_ID = -1;

        private int m_recordingId;
        private int m_userId;
        private DateTime m_dateTime;
        private string m_fileName;
        private string m_filePath;

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public Recording()
        {
            m_recordingId = UNASSIGNED_ID;
            m_userId = UNASSIGNED_ID;
            m_dateTime = DateTime.Now;
            m_fileName = "";
            m_filePath = "";
        }

        /// <summary>
        /// Create a new recording for a user
        /// </summary>
        /// <param name="userId"></param>
        public Recording(int userId, string libraryPath, string fileName)
        {
            m_recordingId = UNASSIGNED_ID;
            m_userId = userId;
            m_dateTime = DateTime.Now;
            m_fileName = fileName;
            m_filePath = Path.Combine(libraryPath, fileName);
        }

        /// <summary>
        /// Restore a Recording from data
        /// </summary>
        /// <param name="recordingId"></param>
        /// <param name="userId"></param>
        /// <param name="dateTime"></param>
        /// <param name="fileName"></param>
        internal Recording(int recordingId, int userId, DateTime dateTime, string libraryPath, string fileName)
        {
            m_recordingId = recordingId;
            m_userId = userId;
            m_dateTime = dateTime;
            m_fileName = fileName;
            m_filePath = Path.Combine(libraryPath, fileName);
        }

        #endregion

        #region Properties

        public int RecordingId
        {
            get { return (m_recordingId); }
            set { m_recordingId = value; }
        }

        public int UserId
        {
            get { return (m_userId); }
            set { m_userId = value; }
        }

        public DateTime RecordingTimestamp
        {
            get { return (m_dateTime); }
            set { m_dateTime = value; }
        }

        public string FileName
        {
            get { return (m_fileName); }
            set { m_fileName = value; }
        }

        public string FilePath
        {
            get { return (m_filePath); }
            set { m_filePath = value; }
        }

        #endregion

        #region Operations

        public void DebugPrint(TextWriter output, string indent)
        {
            string childIndent = indent + "  ";
            string userIdString = "UNASSIGNED_ID";
            string recordingIdString = "UNASSIGNED_ID";

            if (m_userId != UNASSIGNED_ID)
            {
                userIdString = m_userId.ToString();
            }

            if (m_recordingId != UNASSIGNED_ID)
            {
                recordingIdString = m_recordingId.ToString();
            }

            output.WriteLine("{0}Recording:", indent);
            output.WriteLine("{0}RecordingId={1}", childIndent, recordingIdString);
            output.WriteLine("{0}UserId={1}", childIndent, userIdString);
            output.WriteLine("{0}CreationDateTime={1}", childIndent, m_dateTime);
            output.WriteLine("{0}FileName=\"{1}\"", childIndent, m_fileName);
            output.WriteLine("{0}FilePath=\"{1}\"", childIndent, m_filePath);
        }

        #endregion

    }
}
