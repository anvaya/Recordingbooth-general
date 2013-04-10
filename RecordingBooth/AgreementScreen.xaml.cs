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
using RecordingLibrary;

namespace RecordingBooth
{
    /// <summary>
    /// Interaction logic for AgreementScreen.xaml
    /// </summary>
    public partial class AgreementScreen : UserControl, IApplicationScreen
    {
        private static Logger m_logger = LogManager.GetCurrentClassLogger();

        private IBoothViewModel m_viewModel;

        #region Constructors

        public AgreementScreen()
        {
            m_logger.Trace("Constructor called");

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

            FlowDocument userAgreement = UserAgreementViewer.Document;

            UserAgreementViewer.Document = DataModel.Instance.UpdateAgreement(userAgreement);
        }

        #endregion

        #region Event handlers

        private void AgreementScreen_Initialized(object sender, EventArgs e)
        {
            m_logger.Trace("AgreementScreen_Initialized() called");

            // Check for custom settings
            CustomSettings settings = DataModel.Instance.CustomSettings;

            if (settings.ContainsKey(CustomSettings.AgreementAgreeButtonText))
            {
                AgreeButton.Content = settings[CustomSettings.AgreementAgreeButtonText];
            }

            if (settings.ContainsKey(CustomSettings.AgreementAgreeButtonColor))
            {
                Color buttonColor = (Color)ColorConverter.ConvertFromString(settings[CustomSettings.AgreementAgreeButtonColor]);

                this.Resources.Add("GreenButtonColor", buttonColor);
            }

            if (settings.ContainsKey(CustomSettings.AgreementBackButtonText))
            {
                BackButton.Content = settings[CustomSettings.AgreementBackButtonText];
            }

            if (settings.ContainsKey(CustomSettings.AgreementBackButtonColor))
            {
                Color buttonColor = (Color)ColorConverter.ConvertFromString(settings[CustomSettings.AgreementBackButtonColor]);

                this.Resources.Add("RedButtonColor", buttonColor);
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            m_logger.Trace("BackButton_Click() called");

            m_viewModel.ViewRegisterScreen();
        }

        private void AgreeButton_Click(object sender, RoutedEventArgs e)
        {
            m_logger.Trace("AgreeButton_Click() called");

            DataModel.Instance.AcceptAgreement();

            m_viewModel.ViewRecordingScreen();
        }

        #endregion

    }
}
