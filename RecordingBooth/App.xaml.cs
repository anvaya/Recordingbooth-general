using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using NLog;

namespace RecordingBooth
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static Logger m_logger = LogManager.GetCurrentClassLogger();

        public const string TIME_DISPLAY_FORMAT = "%m\\:ss";

        #region Properties

        #endregion

        #region Operations

        private void App_Startup(object sender, StartupEventArgs e)
        {
            m_logger.Info("-------- App_Startup() called --------");

            DispatcherUnhandledException += App_DispatcherUnhandledException;

            DataModel.Instance.OrganizationName = RecordingBooth.Properties.Settings.Default.OrganizationName;
            DataModel.Instance.MaxRecordingTime = RecordingBooth.Properties.Settings.Default.MaxRecordingTime;
            DataModel.Instance.RecordingLevel = RecordingBooth.Properties.Settings.Default.RecordingLevel;

            string databaseConnection = RecordingBooth.Properties.Settings.Default.RecordingsDataConnectionString;
            string libraryPath = RecordingBooth.Properties.Settings.Default.RecordingsLibraryPath;

            DataModel.Instance.OpenLibrary(databaseConnection, libraryPath);

            m_logger.Trace("  OrganizationName=\"{0}\", MaxRecordingTime={1}", DataModel.Instance.OrganizationName, DataModel.Instance.MaxRecordingTime);
        }

        private void App_SessionEnding(object sender, SessionEndingCancelEventArgs e)
        {
            m_logger.Info("App_SessionEnding() called");
        }

        private void App_Exit(object sender, ExitEventArgs e)
        {
            m_logger.Info("App_Exit() called");
            m_logger.Trace("  OrganizationName=\"{0}\", MaxRecordingTime={1}", DataModel.Instance.OrganizationName, DataModel.Instance.MaxRecordingTime);
        }

        private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            m_logger.Error("Unhandled exception:  {0}", e.Exception.ToString());
        }

        #endregion

    }
}
