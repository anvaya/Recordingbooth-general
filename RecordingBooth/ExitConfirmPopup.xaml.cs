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
using System.Windows.Shapes;
using NLog;

namespace RecordingBooth
{
    /// <summary>
    /// Interaction logic for ExitConfirmPopup.xaml
    /// </summary>
    public partial class ExitConfirmPopup : Window
    {
        private static Logger m_logger = LogManager.GetCurrentClassLogger();

        #region Constructors

        public ExitConfirmPopup()
        {
            InitializeComponent();
        }

        #endregion

        #region Properties

        #endregion

        #region Methods

        #endregion

        #region Event Handlers

        private void ExitConfirm_Loaded(object sender, RoutedEventArgs e)
        {
            m_logger.Trace("ExitConfirm_Loaded() called");

            CustomSettings settings = DataModel.Instance.CustomSettings;

            if (settings.ContainsKey(CustomSettings.AdminPopupBackgroundColor))
            {
                Color backgroundColor = (Color)ColorConverter.ConvertFromString(settings[CustomSettings.AdminPopupBackgroundColor]);

                this.Background = new SolidColorBrush(backgroundColor);

                m_logger.Trace("  Setting background color to {0}", backgroundColor.ToString());
            }
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            m_logger.Trace("cancelButton_Click() called");

            this.DialogResult = false;
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            m_logger.Trace("okButton_Click() called");

            this.DialogResult = true;

            App.Current.Shutdown(0);
        }

        #endregion

    }
}
