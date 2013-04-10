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
    /// Interaction logic for UserRecordingsScreen.xaml
    /// </summary>
    public partial class UserRecordingsScreen : UserControl, IApplicationScreen
    {
        private static Logger m_logger = LogManager.GetCurrentClassLogger();

        private IBoothViewModel m_viewModel;

        #region Constructors

        public UserRecordingsScreen()
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

            User displayUser = DataModel.Instance.CurrentUser;

            UserInformationGrid.DataContext = displayUser;

            RecordingCollection recordings = DataModel.Instance.Library.ListUserRecordings(displayUser.UserId);

            RecordingList.DataContext = recordings;

            m_logger.Trace("  VerticalScrollBarVisibility={0}", RecordingList.VerticalScrollBarVisibility.ToString());
        }

        #endregion

        #region Event handlers

        private void UserRecordingsScreen_Initialized(object sender, EventArgs e)
        {
            m_logger.Trace("UserRecordingsScreen_Initialized() called");

            // Check for custom settings
            CustomSettings settings = DataModel.Instance.CustomSettings;

            if (settings.ContainsKey(CustomSettings.RecordingsListBackButtonText))
            {
                BackButton.Content = settings[CustomSettings.RecordingsListBackButtonText];
            }

            if (settings.ContainsKey(CustomSettings.RecordingsListBackButtonColor))
            {
                Color buttonColor = (Color)ColorConverter.ConvertFromString(settings[CustomSettings.RecordingsListBackButtonColor]);

                this.Resources.Add("RedButtonColor", buttonColor);
            }
        }

        private void UserRecordingsScreen_Loaded(object sender, RoutedEventArgs e)
        {
            m_logger.Trace("UserRecordings_Loaded() called");

            //User displayUser = DataModel.Instance.CurrentUser;

            //UserInformationGrid.DataContext = displayUser;

            //RecordingCollection recordings = DataModel.Instance.Library.ListUserRecordings(displayUser.UserId);

            //RecordingList.DataContext = recordings;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            m_logger.Trace("BackButton_Click() called");

            m_viewModel.ViewUserListScreen();
        }

        #endregion

    }
}
