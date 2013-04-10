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
    /// Interaction logic for AdminPasswordPopup.xaml
    /// </summary>
    public partial class AdminPasswordPopup : Window
    {
        private static Logger m_logger = LogManager.GetCurrentClassLogger();

        private IBoothViewModel m_viewModel;

        private static readonly int ATTEMPT_LIMIT = 4;

        private int m_attemptCount;
        private string m_password;

        #region Constructors

        public AdminPasswordPopup()
        {
            m_attemptCount = 0;
            m_password = RecordingBooth.Properties.Settings.Default.AdminPassword;

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

        #endregion

        #region Event Handlers

        private void AdminPassword_Loaded(object sender, RoutedEventArgs e)
        {
            m_logger.Trace("AdminPassword_Loaded() called");

            CustomSettings settings = DataModel.Instance.CustomSettings;

            if (settings.ContainsKey(CustomSettings.AdminPopupBackgroundColor))
            {
                Color backgroundColor = (Color)ColorConverter.ConvertFromString(settings[CustomSettings.AdminPopupBackgroundColor]);

                this.Background = new SolidColorBrush(backgroundColor);

                m_logger.Trace("  Setting background color to {0}", backgroundColor.ToString());
            }
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            if (m_attemptCount++ < ATTEMPT_LIMIT)
            {
                if (m_password == this.passwordTextBox.Password)
                {
                    m_viewModel.ViewAdminScreen();

                    this.DialogResult = true;
                }
                else
                {
                    invalidPasswordLabel.Visibility = Visibility.Visible;
                }
            }
            else
            {
                this.DialogResult = true;
            }
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void passwordTextBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            invalidPasswordLabel.Visibility = Visibility.Hidden;
        }

        #endregion

    }
}
