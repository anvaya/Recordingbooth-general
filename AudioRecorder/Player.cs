using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NAudio.Wave;

namespace AudioRecorder
{
    public class Player : IPlayer
    {
        private WaveOut m_waveOut;
        private WaveStream m_inputStream;
        private PlaybackState m_playbackState;
        private float m_volume;

        #region Events

        public event EventHandler PlaybackStopped = delegate { };

        #endregion

        #region Constructors

        public Player()
        {
            m_waveOut = null;
            m_inputStream = null;
            m_playbackState = PlaybackState.Stopped;
            m_volume = 1.0f;
        }

        #endregion

        #region Properties

        public TimeSpan TotalTime
        {
            get { return (m_inputStream.TotalTime); }
        }

        public TimeSpan CurrentTime
        {
            get { return (m_inputStream.CurrentTime); }
            set { m_inputStream.CurrentTime = value; }
        }

        public PlaybackState PlaybackState
        {
            get { return (m_playbackState); }
            private set { m_playbackState = value; }
        }

        public float Volume
        {
            get { return (m_volume); }
            set
            {
                m_volume = value;
                if (null != m_waveOut)
                {
                    m_waveOut.Volume = m_volume;
                }
            }
        }


        #endregion

        #region Methods

        public void LoadFile(string filePath)
        {
            CloseWaveOut();
            CloseInputStream();
            m_inputStream = new WaveFileReader(filePath);
        }

        public void Play()
        {
            CreateWaveOut();
            if (m_waveOut.PlaybackState == PlaybackState.Stopped)
            {
                m_inputStream.Position = 0;
                m_waveOut.Play();
            }
        }

        private void CreateWaveOut()
        {
            if (m_waveOut == null)
            {
                m_waveOut = new WaveOut();
                m_waveOut.Init(m_inputStream);
                m_waveOut.PlaybackStopped += new EventHandler<NAudio.Wave.StoppedEventArgs>(waveOut_PlaybackStopped);
                m_waveOut.Volume = m_volume;
            }
        }

        public void waveOut_PlaybackStopped(object sender, EventArgs e)
        {
            m_playbackState = PlaybackState.Stopped;
            PlaybackStopped(sender, e);
        }

        public void Stop()
        {
            m_waveOut.Stop();
            m_inputStream.Position = 0;
        }

        public void Dispose()
        {
            CloseWaveOut();
            CloseInputStream();
        }

        private void CloseInputStream()
        {
            if (m_inputStream != null)
            {
                m_inputStream.Dispose();
                m_inputStream = null;
            }
        }

        private void CloseWaveOut()
        {
            if (m_waveOut != null)
            {
                m_waveOut.Dispose();
                m_waveOut = null;
            }
        }

        #endregion

    }
}
