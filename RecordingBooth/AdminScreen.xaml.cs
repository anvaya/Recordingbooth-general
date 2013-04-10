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
    /// Interaction logic for AdminPage.xaml
    /// </summary>
    public partial class AdminScreen : UserControl, IApplicationScreen
    {
        private static Logger m_logger = LogManager.GetCurrentClassLogger();

        private IBoothViewModel m_viewModel;
        private AdminViewModel m_adminModel;

        #region Constructors

        public AdminScreen()
        {
            m_logger.Trace("AdminScreen() constructor");

            m_adminModel = new AdminViewModel();

            InitializeComponent();

            this.DataContext = m_adminModel;
        }

        #endregion

        #region Properties

        public IBoothViewModel ViewModel
        {
            get { return (m_viewModel); }
            set { m_viewModel = value; }
        }

        public AdminViewModel AdminViewModel
        {
            get { return (m_adminModel); }
            set { m_adminModel = value; }
        }

        #endregion

        #region Methods

        public void Activate()
        {
            m_logger.Trace("Activate() called");
        }

        public void ConfirmExit()
        {
            ExitConfirmPopup popup = new ExitConfirmPopup();

            popup.Owner = App.Current.MainWindow;

            popup.ShowDialog();
        }

        #endregion

        #region Event handlers

        private void AdminScreen_Initialized(object sender, EventArgs e)
        {
            m_logger.Trace("AdminScreen_Initialized() called");

            // Check for custom settings
            CustomSettings settings = DataModel.Instance.CustomSettings;

            if (settings.ContainsKey(CustomSettings.AdminUpdateButtonText))
            {
                TextBlock contentBlock = updateButton.Content as TextBlock;

                if (null != contentBlock)
                {
                    contentBlock.Text = settings[CustomSettings.AdminUpdateButtonText];
                }
                else
                {
                    updateButton.Content = settings[CustomSettings.AdminUpdateButtonText];
                }
            }

            if ((settings.ContainsKey(CustomSettings.AdminUpdateButtonColor)) && (!this.Resources.Contains("GreenButtonColor")))
            {
                Color buttonColor = (Color)ColorConverter.ConvertFromString(settings[CustomSettings.AdminUpdateButtonColor]);

                this.Resources.Add("GreenButtonColor", buttonColor);
            }

            if (settings.ContainsKey(CustomSettings.AdminViewUsersButtonText))
            {
                TextBlock contentBlock = viewUsersButton.Content as TextBlock;

                if (null != contentBlock)
                {
                    contentBlock.Text = settings[CustomSettings.AdminViewUsersButtonText];
                }
                else
                {
                    viewUsersButton.Content = settings[CustomSettings.AdminViewUsersButtonText];
                }
            }

            if ((settings.ContainsKey(CustomSettings.AdminViewUsersButtonColor)) && (!this.Resources.Contains("GreenButtonColor")))
            {
                Color buttonColor = (Color)ColorConverter.ConvertFromString(settings[CustomSettings.AdminViewUsersButtonColor]);

                this.Resources.Add("GreenButtonColor", buttonColor);
            }

            if (settings.ContainsKey(CustomSettings.AdminExportButtonText))
            {
                TextBlock contentBlock = ExportButton.Content as TextBlock;

                if (null != contentBlock)
                {
                    contentBlock.Text = settings[CustomSettings.AdminExportButtonText];
                }
                else
                {
                    ExportButton.Content = settings[CustomSettings.AdminExportButtonText];
                }
            }

            if ((settings.ContainsKey(CustomSettings.AdminExportButtonColor)) && (!this.Resources.Contains("GreenButtonColor")))
            {
                Color buttonColor = (Color)ColorConverter.ConvertFromString(settings[CustomSettings.AdminExportButtonColor]);

                this.Resources.Add("GreenButtonColor", buttonColor);
            }

            if (settings.ContainsKey(CustomSettings.AdminBackButtonText))
            {
                TextBlock contentBlock = backButton.Content as TextBlock;

                if (null != contentBlock)
                {
                    contentBlock.Text = settings[CustomSettings.AdminBackButtonText];
                }
                else
                {
                    backButton.Content = settings[CustomSettings.AdminBackButtonText];
                }
            }

            if (settings.ContainsKey(CustomSettings.AdminBackButtonColor))
            {
                Color buttonColor = (Color)ColorConverter.ConvertFromString(settings[CustomSettings.AdminBackButtonColor]);

                this.Resources.Add("RedButtonColor", buttonColor);
            }

            if (settings.ContainsKey(CustomSettings.AdminExitButtonText))
            {
                TextBlock contentBlock = exitButton.Content as TextBlock;

                if (null != contentBlock)
                {
                    contentBlock.Text = settings[CustomSettings.AdminExitButtonText];
                }
                else
                {
                    exitButton.Content = settings[CustomSettings.AdminExitButtonText];
                }
            }

            if (settings.ContainsKey(CustomSettings.AdminExitButtonColor))
            {
                Color buttonColor = (Color)ColorConverter.ConvertFromString(settings[CustomSettings.AdminExitButtonColor]);

                this.Resources.Add("DarkRedButtonColor", buttonColor);
            }
        }

        private void AdminScreen_Loaded(object sender, RoutedEventArgs e)
        {
            m_logger.Trace("AdminScreen_Loaded() called");

            m_adminModel.Load(DataModel.Instance);
        }

        private void updateButton_Click(object sender, RoutedEventArgs e)
        {
            m_logger.Trace("updateButton_Click() called");

            m_adminModel.Save(DataModel.Instance);
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            m_logger.Trace("backButton_Click() called");
            
            m_viewModel.ViewStartScreen();
        }

        private void viewUsersButton_Click(object sender, RoutedEventArgs e)
        {
            m_logger.Trace("viewUsersButton_Click() called");

            m_viewModel.ViewUserListScreen();
        }

        private void exitButton_Click(object sender, RoutedEventArgs e)
        {
            m_logger.Trace("exitButton_Click() called");

            MessageBoxResult result = MessageBox.Show("This will close Recording Booth Application\nAre you Sure?", "Confirm Close", MessageBoxButton.OKCancel, MessageBoxImage.Question, MessageBoxResult.Cancel);
            if (result == MessageBoxResult.OK)
            {
                App.Current.Shutdown(0);
            }            

            //ConfirmExit();
        }

        private void ExportButton_Click(object sender, RoutedEventArgs e)
        {
            m_logger.Trace("ExportButton_Click() called");

            m_adminModel.ExportData();
        }

        private void ClearUsersButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBoxResult result = MessageBox.Show("This will clear all User and Recording Records\nAre you Sure?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
                if (result == MessageBoxResult.Yes)
                {
                    DataModel.Instance.Library.ClearUserDatabase();
                }
            }
            catch (Exception ex) 
            { 

            }
        }

        #endregion

    }
}
