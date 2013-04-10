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
    /// Interaction logic for UserListScreen.xaml
    /// </summary>
    public partial class UserListScreen : UserControl, IApplicationScreen
    {
        private static Logger m_logger = LogManager.GetCurrentClassLogger();

        private IBoothViewModel m_viewModel;

        #region Constructors

        public UserListScreen()
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

            UserCollection users = DataModel.Instance.Library.ListUsers();

            UserList.DataContext = users;
        }

        #endregion

        #region Event handlers

        private void UserList_Initialized(object sender, EventArgs e)
        {
            m_logger.Trace("UserList_Initialized() called");

            // Check for custom settings
            CustomSettings settings = DataModel.Instance.CustomSettings;

            if (settings.ContainsKey(CustomSettings.UserListBackButtonText))
            {
                BackButton.Content = settings[CustomSettings.UserListBackButtonText];
            }

            if (settings.ContainsKey(CustomSettings.UserListBackButtonColor))
            {
                Color buttonColor = (Color)ColorConverter.ConvertFromString(settings[CustomSettings.UserListBackButtonColor]);

                this.Resources.Add("RedButtonColor", buttonColor);
            }
        }

        private void UserList_Loaded(object sender, RoutedEventArgs e)
        {
            m_logger.Trace("UserList_Loaded() called");

            //UserCollection users = DataModel.Instance.Library.ListUsers();

            //UserList.DataContext = users;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            m_logger.Trace("BackButton_Click() called");

            m_viewModel.ViewAdminScreen();
        }

        private void UserList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            m_logger.Trace("UserList_SelectionChanged() called");

            m_logger.Trace("  selectedIndex={0}", UserList.SelectedIndex);

            if (null != UserList.SelectedItem)
            {
                DataModel.Instance.CurrentUser = (User)UserList.SelectedItem;
                m_viewModel.ViewUserRecordingsScreen();
            }

        }

        #endregion

        private void image1_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

    }
}
