using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using NLog;
using AudioRecorder;
using RecordingLibrary;

namespace RecordingBooth
{
    public class RecordingViewModel : DependencyObject
    {
        private static Logger m_logger = LogManager.GetCurrentClassLogger();

        public static readonly TimeSpan NEAR_LIMIT_WARNING_TIME = new TimeSpan(0, 1, 0);

        private IRecorder m_recorder;
        private Recording m_recording;
        private TimeSpan m_warningTime;
        private bool m_warningIssued;
        private bool m_endIssued;

        /// <summary>
        /// Delegate for recording time events.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public delegate void TimeWarningEventHandler(Object sender, EventArgs args);

        /// <summary>
        /// Raised when time remaining is less than limit.
        /// </summary>
        public event TimeWarningEventHandler NearEndWarning;

        /// <summary>
        /// Raised when time has expired.
        /// </summary>
        public event TimeWarningEventHandler TimeExpired;

        #region Dependency properties

        /// <summary>
        /// Last peak reading.
        /// </summary>
        public float PeakSample
        {
            get { return (float)GetValue(PeakSampleProperty); }
            set { SetValue(PeakSampleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for InputLevel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PeakSampleProperty =
            DependencyProperty.Register("PeakSample", typeof(float), typeof(RecordingViewModel), new UIPropertyMetadata(0.0f));

        /// <summary>
        /// Microphone input level setting.
        /// </summary>
        public double MicrophoneLevel
        {
            get { return (double)GetValue(MicrophoneLevelProperty); }
            set { SetValue(MicrophoneLevelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MicrophoneLevel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MicrophoneLevelProperty =
            DependencyProperty.Register("MicrophoneLevel", typeof(double), typeof(RecordingViewModel), new UIPropertyMetadata(0.0));

        /// <summary>
        /// Length of the recording, formatted as a string.
        /// </summary>
        public string RecordedTime
        {
            get { return (string)GetValue(RecordedTimeProperty); }
            set { SetValue(RecordedTimeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RecordedTime.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RecordedTimeProperty =
            DependencyProperty.Register("RecordedTime", typeof(string), typeof(RecordingViewModel), new UIPropertyMetadata("00:00"));

        /// <summary>
        /// Fraction of the allowed time that has been used.
        /// </summary>
        public double RecordedTimeFraction
        {
            get { return (double)GetValue(RecordedTimeFractionProperty); }
            set { SetValue(RecordedTimeFractionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RecordedTimeFraction.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RecordedTimeFractionProperty =
            DependencyProperty.Register("RecordedTimeFraction", typeof(double), typeof(RecordingViewModel), new UIPropertyMetadata(0.0));

        ///// <summary>
        ///// This becomes <code>true</code> in the last minute of recording time.
        ///// </summary>
        //public System.Windows.Visibility IsLastRecordingMinute
        //{
        //    get { return (System.Windows.Visibility)GetValue(LastRecordingMinuteProperty); }
        //    set { SetValue(LastRecordingMinuteProperty, value); }
        //}

        //// Using a DependencyProperty as the backing store for LastRecordingMinute.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty LastRecordingMinuteProperty =
        //    DependencyProperty.Register("LastRecordingMinute",
        //        typeof(System.Windows.Visibility),
        //        typeof(RecordingViewModel),
        //        new UIPropertyMetadata(System.Windows.Visibility.Visible));

        #endregion

        #region Constructors

        public RecordingViewModel()
        {
            m_logger.Trace("RecordingViewModel() constructor called");

            m_warningIssued = false;
            m_endIssued = false;

            // Create the audio recorder
            m_recorder = new Recorder(DataModel.Instance.RecordingLevel);

            // Connect the event handlers to monitor the recording
            m_recorder.RecordingStopped += recorder_Stopped;
            m_recorder.PeakMonitor.SampleCountReached += recorder_PeakSample;
        }

        #endregion

        #region Properties

        #endregion

        #region Methods

        public void BeginMonitoring()
        {
            m_recorder.Monitor(DataModel.Instance.InputDeviceIndex);
        }

        public void BeginRecording()
        {
            m_logger.Trace("BeginRecording() called");

            m_warningIssued = false;
            m_endIssued = false;

            // Use 1 minute or 10% of total time for the warning, whichever is less
            m_warningTime = new TimeSpan(DataModel.Instance.MaxRecordingTime.Ticks / 10);
            if (NEAR_LIMIT_WARNING_TIME < m_warningTime)
            {
                m_warningTime = NEAR_LIMIT_WARNING_TIME;
            }

            DataModel.Instance.CreateRecording();

            m_recording = DataModel.Instance.CurrentRecording;

            if (null != m_recording)
            {              
                
                m_recorder.StartRecording(m_recording.FilePath);
            }
            else
            {
                m_logger.Error("Could not create recording");
            }
        }

        public void StopRecording()
        {
            m_logger.Trace("StopRecording() called");

            m_recorder.StopRecording();
        }

        public void StopMonitoring()
        {
            m_logger.Trace("StopMonitoring() called");

            m_recorder.StopMonitoring();
        }

        protected virtual void OnNearEndWarning(EventArgs e)
        {
            if (NearEndWarning != null)
                NearEndWarning(this, e);
        }

        protected virtual void OnTimeExpired(EventArgs e)
        {
            if (TimeExpired != null)
                TimeExpired(this, e);
        }

        #endregion

        #region Event handlers

        /// <summary>
        /// Recorder has finally finished.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void recorder_Stopped(object sender, EventArgs e)
        {
            m_logger.Trace("recorder_Stopped() called");

            ;
        }

        /// <summary>
        /// Sample received from the peak monitor.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void recorder_PeakSample(object sender, PeakSampleEventArgs e)
        {
            // Don't want to trace this function - it runs 10 times a second

            // Update peak meter
            float currentPeak = Math.Max(e.MaxSample, Math.Abs(e.MinSample));
            PeakSample = currentPeak * 100.0f;

            // Update recording time display
            TimeSpan currentTime = m_recorder.RecordingLength;
            TimeSpan timeLimit = DataModel.Instance.MaxRecordingTime;

            if (currentTime < timeLimit)
            {
                // RecordedTime = String.Format("{0:D2}:{1:D2}.{2:D3}", currentTime.Minutes, currentTime.Seconds, currentTime.Milliseconds);
                RecordedTime = String.Format("{0:D2}:{1:D2}", currentTime.Minutes, currentTime.Seconds);
                RecordedTimeFraction = ((double)currentTime.TotalMilliseconds) / ((double)timeLimit.TotalMilliseconds);

                // Check for time warning
                TimeSpan timeLeft = timeLimit.Subtract(currentTime);
                if (timeLeft < m_warningTime)
                {
                    if (!m_warningIssued)
                    {
                        OnNearEndWarning(EventArgs.Empty);
                        m_warningIssued = true;
                    }
                }
            }
            else
            {
                if (!m_endIssued)
                {
                    OnTimeExpired(EventArgs.Empty);
                    m_endIssued = true;
                }
            }
        }

        #endregion

    }
}
