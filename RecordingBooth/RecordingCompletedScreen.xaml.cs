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
    /// Interaction logic for RecordingCompletedScreen.xaml
    /// </summary>
    public partial class RecordingCompletedScreen : UserControl, IApplicationScreen
    {
        private static Logger m_logger = LogManager.GetCurrentClassLogger();

        private IBoothViewModel m_viewModel;
        private PlaybackViewModel m_playbackModel;

        #region Constructors

        public RecordingCompletedScreen()
        {
            m_playbackModel = new PlaybackViewModel();

            InitializeComponent();

            this.DataContext = m_playbackModel;
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
        }

        #endregion

        #region Event handlers

        private void PlaybackScreen_Initialized(object sender, EventArgs e)
        {
            m_logger.Trace("PlaybackScreen_Initialized() called");

            // Check for custom settings
            CustomSettings settings = DataModel.Instance.CustomSettings;

            if (settings.ContainsKey(CustomSettings.PlaybackPlayButtonText))
            {
                TextBlock contentBlock = PlaybackButton.Content as TextBlock;

                if (null != contentBlock)
                {
                    contentBlock.Text = settings[CustomSettings.PlaybackPlayButtonText];
                }
                else
                {
                    PlaybackButton.Content = settings[CustomSettings.PlaybackPlayButtonText];
                }
            }

            if (settings.ContainsKey(CustomSettings.PlaybackPlayButtonColor))
            {
                Color buttonColor = (Color)ColorConverter.ConvertFromString(settings[CustomSettings.PlaybackPlayButtonColor]);

                this.Resources.Add("RedButtonColor", buttonColor);
            }

            if (settings.ContainsKey(CustomSettings.PlaybackReRecordButtonText))
            {
                TextBlock contentBlock = ReRecordButton.Content as TextBlock;

                if (null != contentBlock)
                {
                    contentBlock.Text = settings[CustomSettings.PlaybackReRecordButtonText];
                }
                else
                {
                    ReRecordButton.Content = settings[CustomSettings.PlaybackReRecordButtonText];
                }
            }

            if (settings.ContainsKey(CustomSettings.PlaybackReRecordButtonColor))
            {
                Color buttonColor = (Color)ColorConverter.ConvertFromString(settings[CustomSettings.PlaybackReRecordButtonColor]);

                this.Resources.Add("GoldButtonColor", buttonColor);
            }

            if (settings.ContainsKey(CustomSettings.PlaybackNewRecordingButtonText))
            {
                TextBlock contentBlock = NewRecordingButton.Content as TextBlock;

                if (null != contentBlock)
                {
                    contentBlock.Text = settings[CustomSettings.PlaybackNewRecordingButtonText];
                }
                else
                {
                    NewRecordingButton.Content = settings[CustomSettings.PlaybackNewRecordingButtonText];
                }
            }

            if ((settings.ContainsKey(CustomSettings.PlaybackNewRecordingButtonColor)) && (!this.Resources.Contains("GoldButtonColor")))
            {
                Color buttonColor = (Color)ColorConverter.ConvertFromString(settings[CustomSettings.PlaybackNewRecordingButtonColor]);

                this.Resources.Add("GoldButtonColor", buttonColor);
            }

            if (settings.ContainsKey(CustomSettings.PlaybackFinishButtonText))
            {
                TextBlock contentBlock = FinishButton.Content as TextBlock;

                if (null != contentBlock)
                {
                    contentBlock.Text = settings[CustomSettings.PlaybackFinishButtonText];
                }
                else
                {
                    FinishButton.Content = settings[CustomSettings.PlaybackFinishButtonText];
                }
            }

            if (settings.ContainsKey(CustomSettings.PlaybackFinishButtonColor))
            {
                Color buttonColor = (Color)ColorConverter.ConvertFromString(settings[CustomSettings.PlaybackFinishButtonColor]);

                this.Resources.Add("GreenButtonColor", buttonColor);
            }
        }

        private void PlaybackButton_Click(object sender, RoutedEventArgs e)
        {
            m_logger.Trace("PlaybackButton_Click() called");

            m_playbackModel.PlayRecording();
        }

        private void FinishButton_Click(object sender, RoutedEventArgs e)
        {
            m_logger.Trace("FinishButton_Click() called");

            m_playbackModel.Finish();

            m_viewModel.ViewStartScreen();

        }

        private void ReRecordButton_Click(object sender, RoutedEventArgs e)
        {
            m_logger.Trace("ReRecordButton_Click() called");

            m_playbackModel.DiscardRecording();

            m_viewModel.ViewRecordingScreen();
        }

        private void NewRecordingButton_Click(object sender, RoutedEventArgs e)
        {
            m_logger.Trace("NewRecordingButton_Click() called");

            m_playbackModel.Finish();

            m_viewModel.ViewRecordingScreen();
        }

        #endregion

    }
}
