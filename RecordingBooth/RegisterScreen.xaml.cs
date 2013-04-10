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
    /// Interaction logic for RegisterPage.xaml
    /// </summary>
    public partial class RegisterScreen : UserControl, IApplicationScreen
    {
        private static Logger m_logger = LogManager.GetCurrentClassLogger();

        private IBoothViewModel m_viewModel;

        #region Constructors

        public RegisterScreen()
        {
            m_logger.Trace("Constructor called");
            FPS.VirtualKeyboard.Common.LicenseValidator.LicenceKey = @"gTySAwY/Sehq8CcFAhIUoCSEhDA9/f0QUFlDKzN6Ep4wKj3Ott4wuA==";
            //FPS.VirtualKeyboard.Common.LicenseValidator.ShowTrialWindow = false;
            
            InitializeComponent();

            System.Reflection.Assembly assembly = this.GetType().Assembly;

            //FPS.VirtualKeyboard.Common.LicenseValidator.ShowTrialWindow = false;

            virtualKeyboard.DefaultLayout = FPS.VirtualKeyboard.KeyboardLayout.Create(assembly.GetManifestResourceStream("RecordingBooth.Resources.FPSLayout(134809609).xml"));


            virtualKeyboard.SendKey(FPS.VirtualKeyboard.Key.Lang);
            ResourceDictionary resources = new ResourceDictionary();
            resources.Source = new Uri("CustomVirtualKeyboardStyle.xaml", UriKind.Relative);
            Resources.MergedDictionaries.Add(resources);
            virtualKeyboard.ForceUpdateLayout();

            //virtualKeyboard.Style = Application.Current.Resources["custoKeyboardStye"] as Style;
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

            this.UserInputGrid.DataContext = new User();
        }

        #endregion

        #region Event handlers

        private void RegisterScreen_Initialized(object sender, EventArgs e)
        {
            m_logger.Trace("RegisterScreen_Initialized() called");

            // Check for custom settings
            CustomSettings settings = DataModel.Instance.CustomSettings;

            if (settings.ContainsKey(CustomSettings.RegisterNextButtonText))
            {
                NextButton.Content = settings[CustomSettings.RegisterNextButtonText];
            }

            if (settings.ContainsKey(CustomSettings.RegisterNextButtonColor))
            {
                Color buttonColor = (Color)ColorConverter.ConvertFromString(settings[CustomSettings.RegisterNextButtonColor]);

                this.Resources.Add("GreenButtonColor", buttonColor);
            }

            if (settings.ContainsKey(CustomSettings.RegisterBackButtonText))
            {
                BackButton.Content = settings[CustomSettings.RegisterBackButtonText];
            }

            if (settings.ContainsKey(CustomSettings.RegisterBackButtonColor))
            {
                Color buttonColor = (Color)ColorConverter.ConvertFromString(settings[CustomSettings.RegisterBackButtonColor]);

                this.Resources.Add("RedButtonColor", buttonColor);
            }
        }

        private void RegisterScreen_Loaded(object sender, RoutedEventArgs e)
        {
            m_logger.Trace("OnInit() called");

            Keyboard.Focus(UserInputGrid.NameTextBox);
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            m_logger.Trace("NextButton_Click() called");

            User userData = (User)this.UserInputGrid.DataContext;

            DataModel.Instance.CreateUser(userData);

            m_viewModel.ViewAgreementScreen();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            m_logger.Trace("BackButton_Click() called");

            m_viewModel.ViewStartScreen();
        }

        private void IAgreeCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            m_logger.Trace("IAgreeCheckbox_Checked() called");

            NextButton.IsEnabled = true;
        }

        #endregion

    }
}
