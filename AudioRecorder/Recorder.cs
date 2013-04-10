using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using NAudio.Wave;

namespace AudioRecorder
{
    public class Recorder : IRecorder
    {
        public const int DEFAULT_SAMPLE_RATE = 16000;

        private RecordingState m_recordingState;
        private double m_recordingLevel;
        private WaveFormat m_recordingFormat;
        private PeakSampler m_peakSampler;
        private WaveIn m_waveIn;
        private WaveFileWriter m_fileWriter;
        private TimeSpan m_maxTime;

        public event EventHandler RecordingStopped = delegate { };

        #region Constructors

        public Recorder(double recordingLevel)
        {
            m_recordingState = RecordingState.Stopped;
            m_recordingLevel = recordingLevel;
            m_recordingFormat = new WaveFormat(DEFAULT_SAMPLE_RATE, 1);
            m_peakSampler = new PeakSampler(DEFAULT_SAMPLE_RATE / 10);
            m_waveIn = null;
            m_fileWriter = null;
            m_maxTime = new TimeSpan(0, 20, 0);
        }

        #endregion

        #region Properties

        public RecordingState RecordingState
        {
            get { return (m_recordingState); }
            // Read-only property
        }

        public double RecordingLevel
        {
            get { return (m_recordingLevel); }
            set { m_recordingLevel = value; }
        }

        public TimeSpan RecordingLength
        {
            get
            {
                if (m_fileWriter == null)
                {
                    return TimeSpan.Zero;
                }
                else
                {
                    return TimeSpan.FromSeconds((double)m_fileWriter.Length / m_fileWriter.WaveFormat.AverageBytesPerSecond);
                }
            }
            // Read-only property
        }

        public WaveFormat RecordingFormat
        {
            get { return (m_recordingFormat); }
            set
            {
                m_recordingFormat = value;
                m_peakSampler.SampleSize = m_recordingFormat.SampleRate / 10;
            }
        }

        public PeakSampler PeakMonitor
        {
            get { return (m_peakSampler); }
            // Read-only property
        }

        public TimeSpan MaximumRecordingTime
        {
            get { return (m_maxTime); }
            set { m_maxTime = value; }
        }

        #endregion

        #region Methods

        public void Monitor(int deviceIndex)
        {
            if (m_recordingState != RecordingState.Stopped)
            {
                throw new InvalidOperationException("RecordingState must be Stopped to monitor.  Current state is " + m_recordingState.ToString());
            }

            // Set up the wave input
            m_waveIn = new WaveIn();

            m_waveIn.DeviceNumber = deviceIndex;
            m_waveIn.DataAvailable += waveIn_DataAvailable;
            m_waveIn.RecordingStopped += new EventHandler<NAudio.Wave.StoppedEventArgs>(waveIn_RecordingStopped);
            m_waveIn.WaveFormat = m_recordingFormat;
            m_waveIn.StartRecording();

            // See if there's an input level control on this input source
            VolumeControlUtility.FindVolumeControl(m_waveIn);

            m_recordingState = RecordingState.Monitoring;
        }

        public void StartRecording(string waveFileName)
        {
            if (m_recordingState != RecordingState.Monitoring)
            {
                throw new InvalidOperationException("RecordingState must be Monitoring to record.  Current state is " + m_recordingState.ToString());
            }

            m_fileWriter = new WaveFileWriter(waveFileName, m_recordingFormat);
            m_recordingState = RecordingState.Recording;
        }

        public void StopRecording()
        {
            if ((m_recordingState == RecordingState.Monitoring)
                || (m_recordingState == RecordingState.Recording))
            {
                m_recordingState = RecordingState.StopRequested;

                m_waveIn.StopRecording();
            }
        }

        public void StopMonitoring()
        {
            StopRecording();
        }

        #endregion

        #region Event handlers

        /// <summary>
        /// Called when WaveIn completes a buffer of data.
        /// </summary>
        private void waveIn_DataAvailable(object sender, WaveInEventArgs e)
        {
            byte[] buffer = e.Buffer;
            int bytesRecorded = e.BytesRecorded;

            // Write the buffer to the file
            WriteToFile(buffer, bytesRecorded);

            // Send the samples to the peak sampler
            for (int index = 0; index < e.BytesRecorded; index += 2)
            {
                short sample = (short)((buffer[index + 1] << 8) | buffer[index + 0]);
                float sample32 = sample / 32768.0f;
                m_peakSampler.Add(sample32);
            }
        }

        private void WriteToFile(byte[] buffer, int bytesRecorded)
        {
            long maxFileLength = m_recordingFormat.AverageBytesPerSecond * ((long) m_maxTime.TotalSeconds);

            if ((m_recordingState == RecordingState.Recording)
                || ((m_recordingState == RecordingState.StopRequested) && (null != m_fileWriter)))
            {
                int toWrite = (int)Math.Min(maxFileLength - m_fileWriter.Length, bytesRecorded);
                if (toWrite > 0)
                {
                    m_fileWriter.Write(buffer, 0, bytesRecorded);
                }
                else
                {
                    StopRecording();
                }
            }
        }

        private void waveIn_RecordingStopped(object sender, EventArgs e)
        {
            m_recordingState = RecordingState.Stopped;

            if (null != m_fileWriter)
            {
                m_fileWriter.Dispose();
                m_fileWriter = null;
            }

            RecordingStopped(this, EventArgs.Empty);

            m_waveIn.Dispose();
            m_waveIn = null;
        }

        #endregion

    }
}
