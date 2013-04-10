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
using System.IO;
using NLog;

namespace RecordingBooth
{
    /// <summary>
    /// Interaction logic for StartWindow.xaml
    /// </summary>
    public partial class HostWindow : Window
    {
        private static Logger m_logger = LogManager.GetCurrentClassLogger();

        private RecordingBoothViewModel m_viewModel;
        private int m_adminTouchRadius;
        private int m_adminTouchState;       // Bit mask:  1=top left, 2=top right, 3=bottom left, 4=bottom right
        private Brush m_defaultBackground;

        #region Constructors

        public HostWindow()
        {
            // Hook up the plumbing
            m_viewModel = new RecordingBoothViewModel(this);
            m_adminTouchRadius = Properties.Settings.Default.AdminTouchRadius;
            m_adminTouchState = 0;

            InitializeComponent();

            m_defaultBackground = ScreenLayout.Background;

            SetBackground(CustomSettings.WelcomeBackgroundImage);

            StartScreenInstance.ViewModel = m_viewModel;
            AdminScreenInstance.ViewModel = m_viewModel;
            RegisterScreenInstance.ViewModel = m_viewModel;
            AgreementScreenInstance.ViewModel = m_viewModel;
            RecordingScreenInstance.ViewModel = m_viewModel;
            RecordingCompletedScreenInstance.ViewModel = m_viewModel;
            UserListScreenInstance.ViewModel = m_viewModel;
            UserRecordingsScreenInstance.ViewModel = m_viewModel;
            
#if DEBUG
         //  this.WindowStyle = WindowStyle.SingleBorderWindow;
           //this.Topmost = false;
#endif
        }

        #endregion

        #region Properties

        #endregion

        #region Methods

        public void ViewStartScreen()
        {
            ClearTouchState();

            SetBackground(CustomSettings.WelcomeBackgroundImage);

            ContentPanel.SelectedItem = (TabItem)ContentPanel.FindName("StartTab");

            ((IApplicationScreen) StartScreenInstance).Activate();
        }

        public void ViewAdminScreen()
        {
            ClearTouchState();

            SetBackground(CustomSettings.AdminBackgroundImage);

            ContentPanel.SelectedItem = (TabItem)ContentPanel.FindName("AdminTab");

            ((IApplicationScreen)AdminScreenInstance).Activate();
        }

        public void ViewRegisterScreen()
        {
            ClearTouchState();

            SetBackground(CustomSettings.RegisterBackgroundImage);

            ContentPanel.SelectedItem = (TabItem)ContentPanel.FindName("RegisterTab");

            ((IApplicationScreen)RegisterScreenInstance).Activate();
        }

        public void ViewAgreementScreen()
        {
            ClearTouchState();

            SetBackground(CustomSettings.AgreementBackgroundImage);

            ContentPanel.SelectedItem = (TabItem)ContentPanel.FindName("AgreementTab");

            ((IApplicationScreen)AgreementScreenInstance).Activate();
        }

        public void ViewRecordingScreen()
        {
            ClearTouchState();

            SetBackground(CustomSettings.RecordingBackgroundImage);

            ContentPanel.SelectedItem = (TabItem)ContentPanel.FindName("RecordingTab");

            ((IApplicationScreen)RecordingScreenInstance).Activate();
        }

        public void ViewRecordingCompletedScreen()
        {
            ClearTouchState();

            SetBackground(CustomSettings.PlaybackBackgroundImage);

            ContentPanel.SelectedItem = (TabItem)ContentPanel.FindName("RecordingCompletedTab");

            ((IApplicationScreen)RecordingCompletedScreenInstance).Activate();
        }

        public void ViewUserListScreen()
        {
            ClearTouchState();

            ContentPanel.SelectedItem = (TabItem)ContentPanel.FindName("UserListTab");

            ((IApplicationScreen)UserListScreenInstance).Activate();
        }

        public void ViewUserRecordingsScreen()
        {
            ClearTouchState();

            ContentPanel.SelectedItem = (TabItem)ContentPanel.FindName("UserRecordingsTab");

            ((IApplicationScreen)UserRecordingsScreenInstance).Activate();
        }

        public void SetBackground(string backgroundKey)
        {
            CustomSettings settings = DataModel.Instance.CustomSettings;

            try
            {
                if (settings.ContainsKey(backgroundKey))
                {
                    ImageBrush backgroundBrush = new ImageBrush();
                    backgroundBrush.ImageSource = new BitmapImage(new Uri(settings[backgroundKey], UriKind.RelativeOrAbsolute));
                    backgroundBrush.TileMode = TileMode.FlipXY;
                    ScreenLayout.Background = backgroundBrush;
                    
                }
                else
                {
                    ScreenLayout.Background = m_defaultBackground;
                }
            }
            catch (IOException)
            {
                ScreenLayout.Background = m_defaultBackground;
            }
        }

        private void ClearTouchState()
        {
            m_adminTouchState = 0;
        }

        public void RequestAdminPassword()
        {
            m_viewModel.ViewAdminScreen();
            /*
            ClearTouchState();

            AdminPasswordPopup popup = new AdminPasswordPopup();

            popup.Owner = this;
            popup.ViewModel = m_viewModel;

            popup.ShowDialog();*/
        }

        #endregion

        #region Event handlers

        private void Window_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Point mousePosition = e.GetPosition(this);

            if (mousePosition.X < m_adminTouchRadius)
            {
                if (mousePosition.Y < m_adminTouchRadius)
                {
                    // Touch at top left corner
                    m_adminTouchState |= 1;
                }
                else if ((this.Height - mousePosition.Y) < m_adminTouchRadius)
                {
                    // Touch at bottom left corner
                    m_adminTouchState |= 4;
                }
            }
            else if ((this.Width - mousePosition.X) < m_adminTouchRadius)
            {
                if (mousePosition.Y < m_adminTouchRadius)
                {
                    // Touch at top right corner
                    m_adminTouchState |= 2;
                }
                else if ((this.Height - mousePosition.Y) < m_adminTouchRadius)
                {
                    // Touch at bottom right corner
                    m_adminTouchState |= 8;
                }
            }

            if (15 == m_adminTouchState)
            {
                RequestAdminPassword();
            }
        }

        #endregion

    }
}
