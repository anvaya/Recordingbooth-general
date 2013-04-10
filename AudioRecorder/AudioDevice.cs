using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLog;

namespace AudioRecorder
{
    public class AudioDevice
    {
        private static Logger m_logger = LogManager.GetCurrentClassLogger();

        private AudioFile m_currentFile;

        #region Constructors

        public AudioDevice()
        {
            m_currentFile = null;
        }

        #endregion

        #region Properties

        public AudioFile CurrentFile
        {
            get { return (m_currentFile); }
            set { m_currentFile = value; }
        }

        #endregion

        #region Methods

        public void Reset()
        {
            m_logger.Trace("Reset() called");

            m_currentFile = null;
        }

        public void CreateRecording(string filePath)
        {
            m_logger.Trace("CreateRecording() called");

            AudioFile audioFile = new AudioFile(filePath);

        }

        public void Record()
        {
            m_logger.Trace("Record() called");

        }

        public void Play()
        {
            m_logger.Trace("Play() called");

        }

        public void Stop()
        {
            m_logger.Trace("Stop() called");

        }

        public void Rewind()
        {
            m_logger.Trace("Rewind() called");

        }

        public void FastForward()
        {
            m_logger.Trace("FastForward() called");

        }

        public void GoToStart()
        {
            m_logger.Trace("GoToStart() called");

        }

        public void GoToEnd()
        {
            m_logger.Trace("GoToEnd() called");

        }

        #endregion

    }
}
