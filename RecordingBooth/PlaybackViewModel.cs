using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Threading;
using NLog;
using AudioRecorder;
using RecordingLibrary;

namespace RecordingBooth
{
    public class PlaybackViewModel : DependencyObject
    {
        private static Logger m_logger = LogManager.GetCurrentClassLogger();

        public static readonly TimeSpan PLAYBACK_UPDATE_INTERVAL = new TimeSpan(0, 0, 0, 0, 100);       // Update every 0.1 seconds

        private IPlayer m_player;
        private DispatcherTimer m_timer;

        #region Dependency properties

        /// <summary>
        /// Current time within the recording in string format.
        /// </summary>
        public string CurrentTime
        {
            get { return (string)GetValue(CurrentTimeProperty); }
            set { SetValue(CurrentTimeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurrentTime.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentTimeProperty =
            DependencyProperty.Register("CurrentTime", typeof(string), typeof(PlaybackViewModel), new UIPropertyMetadata("0:00"));

        /// <summary>
        /// Current time expressed as a fraction of the length of the recording.
        /// </summary>
        public double CurrentTimeFraction
        {
            get { return (double)GetValue(CurrentTimeFractionProperty); }
            set { SetValue(CurrentTimeFractionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurrentTimeFraction.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentTimeFractionProperty =
            DependencyProperty.Register("CurrentTimeFraction", typeof(double), typeof(PlaybackViewModel), new UIPropertyMetadata(0.0));

        /// <summary>
        /// Volume to connect the volume control.
        /// </summary>
        public double PlaybackVolume
        {
            get { return (double)GetValue(PlaybackVolumeProperty); }
            set { SetValue(PlaybackVolumeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PlaybackVolume.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PlaybackVolumeProperty =
            DependencyProperty.Register("PlaybackVolume",
                typeof(double),
                typeof(PlaybackViewModel),
                new UIPropertyMetadata(0.8, playbackVolume_Changed));

        #endregion

        #region Constructors

        public PlaybackViewModel()
        {
            m_logger.Trace("Constructor called");

            m_player = null;
        }

        #endregion

        #region Properties

        #endregion

        #region Methods

        private void stopPlayback()
        {
            try
            {
              if(m_timer!=null)  m_timer.Stop();
              if (m_player != null)
              {
                  m_player.Stop();
                  m_player.Dispose();
              }
            }
            catch (Exception) { }
        }

        public void PlayRecording()
        {
            m_logger.Trace("PlayRecording() called");

            if (null != DataModel.Instance.CurrentRecording)
            {
                m_player = new Player();

                m_player.LoadFile(DataModel.Instance.CurrentRecording.FilePath);
                m_player.PlaybackStopped += new EventHandler(player_PlaybackStopped);

                // Use a timer to update the screen controls
                m_timer = new DispatcherTimer();
                m_timer.Interval = PLAYBACK_UPDATE_INTERVAL;
                m_timer.Tick += new EventHandler(OnTimerTick);
                m_timer.Start();

                m_player.Play();
            }
        }

        public void ReRecord()
        {
            stopPlayback();
            m_logger.Trace("ReRecord() called");
            DataModel.Instance.DiscardRecording();
        }

        public void DiscardRecording()
        {
            stopPlayback();
            m_logger.Trace("DiscardRecording() called");

            DataModel.Instance.DiscardRecording();
        }

        public void Finish()
        {
            m_logger.Trace("Finish() called");
            stopPlayback();
            DataModel.Instance.SaveRecording();
        }

        #endregion

        #region Event handlers

        private void player_PlaybackStopped(object source, EventArgs e)
        {
            m_timer.Stop();
            m_player.Dispose();
        }

        private void OnTimerTick(object source, EventArgs e)
        {
            try
            {
                CurrentTime = m_player.CurrentTime.ToString(App.TIME_DISPLAY_FORMAT);
                CurrentTimeFraction = m_player.CurrentTime.TotalMilliseconds / m_player.TotalTime.TotalMilliseconds;
            }
            catch (InvalidOperationException ex)
            {
                m_logger.Error("Unexpected exception:  {0}", ex.ToString());
            }
        }

        private static void playbackVolume_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (null != ((PlaybackViewModel)d).m_player)
            {
                ((PlaybackViewModel)d).m_player.Volume = (float)((double)e.NewValue);
            }
        }

        #endregion

    }
}
