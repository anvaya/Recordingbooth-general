using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace AudioRecorder
{
    public class AudioFile
    {
        private string m_filePath;

        #region Constructors

        public AudioFile(string filePath)
        {
            m_filePath = filePath;
        }

        #endregion

        #region Properties

        public string FilePath
        {
            get { return (m_filePath); }
            set { m_filePath = value; }
        }

        #endregion

        #region Methods

        public void DebugPrint(TextWriter output, string indent)
        {
            string childIndent = indent + "  ";

            output.WriteLine("{0}AudioFile:", indent);
            output.WriteLine("{0}FilePath=\"{1}\"", childIndent, m_filePath);
        }

        #endregion

    }
}
