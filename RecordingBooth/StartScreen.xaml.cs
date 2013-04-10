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
    /// Interaction logic for StartScreen.xaml
    /// </summary>
    public partial class StartScreen : UserControl, IApplicationScreen
    {
        private static Logger m_logger = LogManager.GetCurrentClassLogger();

        private IBoothViewModel m_viewModel;

        #region Constructors

        public StartScreen()
        {
            m_viewModel = null;

            InitializeComponent();
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

            DataModel.Instance.ResetRecordingBooth();
        }

        #endregion

        #region Event handlers

        private void StartScreen_Initialized(object sender, EventArgs e)
        {
            m_logger.Trace("StartScreen_Initialized() called");

            // Check for custom settings
            CustomSettings settings = DataModel.Instance.CustomSettings;

            if (settings.ContainsKey(CustomSettings.WelcomeStartButtonText))
            {
                StartButton.Content = settings[CustomSettings.WelcomeStartButtonText];
            }

            if (settings.ContainsKey(CustomSettings.WelcomeStartButtonColor))
            {
                Color buttonColor = (Color)ColorConverter.ConvertFromString(settings[CustomSettings.WelcomeStartButtonColor]);

                this.Resources.Add("GreenButtonColor", buttonColor);
            }
        }

        private void StartScreen_Loaded(object sender, RoutedEventArgs e)
        {
            m_logger.Trace("StartScreen_Loaded() called");

            // Check for a "Welcome" document
            FlowDocument welcomeText = DataModel.Instance.LoadWelcomeDocument();

            // Did we find one?
            if (null != welcomeText)
            {
                // Yes - adjust the display and show the text
                WelcomeTextViewer.Document = welcomeText;
                WelcomeTextViewer.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            m_logger.Trace("StartButton_Click() called");

            m_viewModel.ViewRegisterScreen();
        }

        #endregion

    }
}
