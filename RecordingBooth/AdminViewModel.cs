using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Forms;
using System.Linq;
using System.Text;
using System.Globalization;
using NLog;

namespace RecordingBooth
{
    public class AdminViewModel : DependencyObject
    {
        private static Logger m_logger = LogManager.GetCurrentClassLogger();

        public const string MAX_TIME_FORMAT = "%m\\:ss";

        #region Dependency properties

        /// <summary>
        /// Edit field for organization name
        /// </summary>
        public string OrganizationName
        {
            get { return (string)GetValue(OrganizationNameProperty); }
            set { SetValue(OrganizationNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for OrganizationName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OrganizationNameProperty =
            DependencyProperty.Register("OrganizationName", typeof(string), typeof(AdminViewModel), new UIPropertyMetadata(""));

        /// <summary>
        /// Edit field for MaxRecordingTime
        /// </summary>
        public string MaxRecordingTime
        {
            get { return (string)GetValue(MaxRecordingTimeProperty); }
            set { SetValue(MaxRecordingTimeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MaxRecordingTime.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaxRecordingTimeProperty =
            DependencyProperty.Register("MaxRecordingTime", typeof(string), typeof(AdminViewModel), new UIPropertyMetadata("20:00"));

        /// <summary>
        /// Path to use for recording files
        /// </summary>
        public string LibraryPath
        {
            get { return (string)GetValue(LibraryPathProperty); }
            set { SetValue(LibraryPathProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LibraryPath.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LibraryPathProperty =
            DependencyProperty.Register("LibraryPath", typeof(string), typeof(AdminViewModel), new UIPropertyMetadata("c:\\RecordingLibrary"));

        /// <summary>
        /// Edit buffer for setting recording level
        /// </summary>
        public double RecordingLevel
        {
            get { return (double)GetValue(RecordingLevelProperty); }
            set { SetValue(RecordingLevelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RecordingLevel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RecordingLevelProperty =
            DependencyProperty.Register("RecordingLevel", typeof(double), typeof(AdminViewModel), new UIPropertyMetadata(1.0));

        

        #endregion


        #region Constructors

        public AdminViewModel()
        {
            m_logger.Trace("Constructor called");
        }

        #endregion

        #region Properties

        #endregion

        #region Methods

        public void Activate()
        {
            m_logger.Trace("Activate() called");
        }

        public void Load(DataModel dataModel)
        {
            m_logger.Trace("Load() called");

            this.OrganizationName = dataModel.OrganizationName;

            TimeSpan maxTime = dataModel.MaxRecordingTime;
            this.MaxRecordingTime = maxTime.ToString(MAX_TIME_FORMAT);
            this.LibraryPath = dataModel.Library.LibraryPath;
            this.RecordingLevel = dataModel.RecordingLevel;

            m_logger.Trace("  OrganizationName=\"{0}\", MaxRecordingTime={1}", this.OrganizationName, this.MaxRecordingTime);
        }

        public void Save(DataModel dataModel)
        {
            m_logger.Trace("Save() called");

            dataModel.OrganizationName = this.OrganizationName;
            dataModel.RecordingLevel = this.RecordingLevel;

            TimeSpan testTime = dataModel.MaxRecordingTime;
            if (TimeSpan.TryParseExact(this.MaxRecordingTime, MAX_TIME_FORMAT, CultureInfo.CurrentCulture, out testTime))
            {
                dataModel.MaxRecordingTime = testTime;
            }
            dataModel.SetLibraryPath(this.LibraryPath);

            m_logger.Trace("  dataModel.OrganizationName=\"{0}\", dataModel.MaxRecordingTime={1}", dataModel.OrganizationName, dataModel.MaxRecordingTime);

            // Save to settings
            RecordingBooth.Properties.Settings.Default.OrganizationName = dataModel.OrganizationName;
            RecordingBooth.Properties.Settings.Default.MaxRecordingTime = dataModel.MaxRecordingTime;
            RecordingBooth.Properties.Settings.Default.RecordingsLibraryPath = dataModel.Library.LibraryPath;
            RecordingBooth.Properties.Settings.Default.RecordingLevel = dataModel.RecordingLevel;

            RecordingBooth.Properties.Settings.Default.Save();
        }

        public void ExportData()
        {
            ExportUserList();
            ExportRecordingsList();
        }

        public void ExportUserList()
        {
            SaveFileDialog fileDlg = new SaveFileDialog();

            fileDlg.AddExtension = true;
            fileDlg.CheckPathExists = true;
            fileDlg.DefaultExt = ".csv";
            fileDlg.FileName = "UserList.csv";
            fileDlg.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
            fileDlg.FilterIndex = 1;
            fileDlg.OverwritePrompt = true;
            fileDlg.Title = "Save User List";

            if (fileDlg.ShowDialog() == DialogResult.OK)
            {
                DataModel.Instance.Library.ExportUserList(fileDlg.FileName);
            }
        }

        public void ExportRecordingsList()
        {
            SaveFileDialog fileDlg = new SaveFileDialog();

            fileDlg.AddExtension = true;
            fileDlg.CheckPathExists = true;
            fileDlg.DefaultExt = ".csv";
            fileDlg.FileName = "RecordingList.csv";
            fileDlg.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
            fileDlg.FilterIndex = 1;
            fileDlg.OverwritePrompt = true;
            fileDlg.Title = "Save Recording List";

            if (fileDlg.ShowDialog() == DialogResult.OK)
            {
                DataModel.Instance.Library.ExportRecordingList(fileDlg.FileName);
            }
        }

        #endregion

    }
}
