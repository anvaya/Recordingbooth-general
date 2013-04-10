using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using NLog;

namespace RecordingBooth
{
    /// <summary>
    /// Interaction logic for RecordingScreen.xaml
    /// </summary>
    public partial class RecordingScreen : UserControl, IApplicationScreen
    {
        private static Logger m_logger = LogManager.GetCurrentClassLogger();

        private IBoothViewModel m_viewModel;
        private RecordingViewModel m_recordingModel;
        private WindowsMicrophoneMuteLibrary.WindowsMicMute micMute = null;

        #region Constructors

        public RecordingScreen()
        {
            InitializeComponent();

            micMute = new WindowsMicrophoneMuteLibrary.WindowsMicMute(); 

            RecordingIndicator.Visibility = System.Windows.Visibility.Hidden;
            TimeWarning.Visibility = System.Windows.Visibility.Hidden;
            StopButton.IsEnabled = false;

            m_recordingModel = new RecordingViewModel();

            m_recordingModel.NearEndWarning += LastMinuteWarning;
            m_recordingModel.TimeExpired += TimeExpired;

            this.DataContext = m_recordingModel;
        }

        #endregion

        #region Properties

        public IBoothViewModel ViewModel
        {
            get { return (m_viewModel); }
            set { m_viewModel = value; }
        }

        #endregion

        #region Methods

        public void Activate()
        {
            m_logger.Trace("Activate() called");
            
            StopButton.IsEnabled = false;

            img_stoprecord.Visibility = System.Windows.Visibility.Hidden;
            img_record.Visibility = System.Windows.Visibility.Visible;

            TimeWarning.Visibility = System.Windows.Visibility.Hidden;

            m_recordingModel.RecordedTime = "0:00";
            m_recordingModel.RecordedTimeFraction = 0.0;
            
            m_recordingModel.BeginMonitoring();
        }

        #endregion

        #region Event handlers

        private void RecordingScreen_Initialized(object sender, EventArgs e)
        {
            m_logger.Trace("RecordingScreen_Initialized() called");

            // Check for custom settings
            CustomSettings settings = DataModel.Instance.CustomSettings;

            if (settings.ContainsKey(CustomSettings.RecordingStartButtonText))
            {
                TextBlock contentBlock = StartButton.Content as TextBlock;

                if (null != contentBlock)
                {
                    contentBlock.Text = settings[CustomSettings.RecordingStartButtonText];
                }
                else
                {
                    StartButton.Content = settings[CustomSettings.RecordingStartButtonText];
                }
            }

            if (settings.ContainsKey(CustomSettings.RecordingStartButtonColor))
            {
                Color buttonColor = (Color)ColorConverter.ConvertFromString(settings[CustomSettings.RecordingStartButtonColor]);

                this.Resources.Add("GreenButtonColor", buttonColor);
            }

            if (settings.ContainsKey(CustomSettings.RecordingStopButtonText))
            {
                TextBlock contentBlock = StopButton.Content as TextBlock;

                if (null != contentBlock)
                {
                    contentBlock.Text = settings[CustomSettings.RecordingStopButtonText];
                }
                else
                {
                    StopButton.Content = settings[CustomSettings.RecordingStopButtonText];
                }
            }

            if (settings.ContainsKey(CustomSettings.RecordingStopButtonColor))
            {
                Color buttonColor = (Color)ColorConverter.ConvertFromString(settings[CustomSettings.RecordingStopButtonColor]);

                this.Resources.Add("DarkRedButtonColor", buttonColor);
            }

            if (settings.ContainsKey(CustomSettings.RecordingBackButtonText))
            {
                TextBlock contentBlock = BackButton.Content as TextBlock;

                if (null != contentBlock)
                {
                    contentBlock.Text = settings[CustomSettings.RecordingBackButtonText];
                }
                else
                {
                    BackButton.Content = settings[CustomSettings.RecordingBackButtonText];
                }
            }

            if (settings.ContainsKey(CustomSettings.RecordingBackButtonColor))
            {
                Color buttonColor = (Color)ColorConverter.ConvertFromString(settings[CustomSettings.RecordingBackButtonColor]);

                this.Resources.Add("RedButtonColor", buttonColor);
            }
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            m_logger.Trace("StartButton_Click() called");

            img_stoprecord.Visibility = System.Windows.Visibility.Visible;
            //img_record.Visibility = System.Windows.Visibility.Hidden;
            img_record.IsEnabled = false;

            micMute.UnMuteMic();

            StartButton.IsEnabled = false;
            StopButton.IsEnabled = true;
            RecordingIndicator.Visibility = System.Windows.Visibility.Visible;

            m_recordingModel.BeginRecording();
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            m_logger.Trace("StopButton_Click() called");
            img_record.IsEnabled = true;
            stopRecording();

            //Set Microphone to Mute
            micMute.MuteMic();

            m_viewModel.ViewRecordingCompletedScreen();
        }

        private void stopRecording()
        {
            RecordingIndicator.Visibility = System.Windows.Visibility.Hidden;
            StartButton.IsEnabled = true;
            StopButton.IsEnabled = true;

            img_stoprecord.Visibility = System.Windows.Visibility.Hidden;
            img_record.Visibility = System.Windows.Visibility.Visible;


            m_recordingModel.StopRecording();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            m_logger.Trace("BackButton_Click() called");

            StartButton.IsEnabled = true;
            StopButton.IsEnabled = false;

            m_recordingModel.StopRecording();

            m_viewModel.ViewRegisterScreen();
        }

        private void LastMinuteWarning(object sender, EventArgs e)
        {
            m_logger.Trace("LastMinuteWarning() called");

            TimeWarning.Visibility = System.Windows.Visibility.Visible;
        }

        private void TimeExpired(object sender, EventArgs e)
        {
            m_logger.Trace("TimeExpired() called");

            TimeWarning.Visibility = System.Windows.Visibility.Hidden;

            stopRecording();
        }

        #endregion

    }
}
