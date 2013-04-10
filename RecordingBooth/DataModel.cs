using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Controls;
using NLog;
using RecordingLibrary;
using AudioRecorder;

namespace RecordingBooth
{
    /// <summary>
    /// DataModel is implemented as a singleton class with a public Instance()
    /// method to obtain the one and only instance.  This uses the single-threaded
    /// pattern;  if this class is to be used in a multi-threaded application,
    /// additional checks such as double-check locking will be needed.
    /// </summary>
    public sealed class DataModel
    {
        private static readonly DataModel m_instance = new DataModel();

        private static Logger m_logger = LogManager.GetCurrentClassLogger();

        public const int DEFAULT_MAX_RECORDING_TIME = 20;   // Minutes
 
        private Library m_library;
        private TimeSpan m_maxRecordingTime;
        private string m_organization;
        private User m_currentUser;
        private AudioDevice m_recorder;
        private int m_inputDeviceIndex;
        private Recording m_currentRecording;
        private double m_recordingLevel;
        private CustomSettings m_customSettings;

        #region Constructors

        private DataModel()
        {
            m_library = new Library(String.Empty, String.Empty);
            m_maxRecordingTime = new TimeSpan(0, DEFAULT_MAX_RECORDING_TIME, 0);
            m_organization = "";
            m_currentUser = null;
            m_recorder = new AudioDevice();
            m_inputDeviceIndex = 0;
            m_currentRecording = null;
            m_recordingLevel = 1.0;
            m_customSettings = new CustomSettings();
        }

        #endregion

        #region Singleton access

        public static DataModel Instance
        {
            get
            {
                return m_instance;
            }
        }

        #endregion

        #region Properties

        public Library Library
        {
            get { return (m_library); }
            // Read-only property
        }

        /// <summary>
        /// Maximum recording time.
        /// </summary>
        public TimeSpan MaxRecordingTime
        {
            get { return (m_maxRecordingTime); }
            set { m_maxRecordingTime = value; }
        }

        public string OrganizationName
        {
            get { return (m_organization); }
            set { m_organization = value; }
        }

        public User CurrentUser
        {
            get { return (m_currentUser); }
            set { m_currentUser = value; }
        }

        public AudioDevice AudioDevice
        {
            get { return (m_recorder); }
            // Read-only property
        }

        public int InputDeviceIndex
        {
            get { return (m_inputDeviceIndex); }
            set { m_inputDeviceIndex = value; }
        }

        public Recording CurrentRecording
        {
            get { return (m_currentRecording); }
            set { m_currentRecording = value; }
        }

        public double RecordingLevel
        {
            get { return (m_recordingLevel); }
            set { m_recordingLevel = value; }
        }

        public CustomSettings CustomSettings
        {
            get
            {
                if (!m_customSettings.IsLoaded)
                {
                    m_customSettings = CustomSettings.Load(RecordingBooth.Properties.Settings.Default.CustomSettingsPath);
                }

                return (m_customSettings);
            }

            set { m_customSettings = value; }
        }

        #endregion

        #region Operations

        public void OpenLibrary(string databaseConnectionString, string libraryPath)
        {
            m_logger.Trace("OpenLibrary() called");

            m_library = new Library(databaseConnectionString, libraryPath);
        }

        public void SetLibraryPath(string libraryPath)
        {
            m_logger.Trace("SetLibraryPath() called");

            // Replace the library connection with a new one using the new path
            m_library = new Library(m_library.DatabaseConnectionString, libraryPath);
        }

        public void ResetRecordingBooth()
        {
            m_logger.Trace("ResetRecordingBooth() called");

            m_currentRecording = null;
            m_currentUser = null;
            m_recorder.Reset();
        }

        public void CreateUser(User userInput)
        {
            m_logger.Trace("CreateUser() called");

            StringWriter userContent = new StringWriter();

            userInput.DebugPrint(userContent, "");

            m_logger.Trace(userContent.ToString());

            m_currentUser = userInput;

            m_library.CreateUser(m_currentUser);
        }

        public void AcceptAgreement()
        {
            m_logger.Trace("AcceptAgreement() called");

            m_currentUser.AcceptsTerms = true;

            m_library.UpdateUser(m_currentUser);
        }

        public FlowDocument UpdateAgreement(FlowDocument userAgreement)
        {
            m_logger.Trace("UpdateAgreement() called");

            // Try loading the agreement from the Resources
            FlowDocument fileAgreement = LoadFlowDocument(RecordingBooth.Properties.Settings.Default.UserAgreementPath);

            if (null != fileAgreement)
            {
                userAgreement = fileAgreement;
            }

            if (null != m_currentUser)
            {
                FlowEditSubstitutionCollection substitutions = new FlowEditSubstitutionCollection();

                substitutions.Add(new FlowEditSubstitution("userName", m_currentUser.UserName));
                substitutions.Add(new FlowEditSubstitution("organzationName", m_organization));

                FlowEditor.ReplaceRuns(userAgreement, substitutions);
            }

            return (userAgreement);
        }

        public FlowDocument LoadWelcomeDocument()
        {
            return (LoadFlowDocument(RecordingBooth.Properties.Settings.Default.WelcomeTextPath));
        }

        private FlowDocument LoadFlowDocument(string resourcePath)
        {
            Object content;
            FileStream docStream = new FileStream(resourcePath, FileMode.Open);
            content = XamlReader.Load(docStream);
            docStream.Close();

            if (content is FlowDocument)
            {
                return ((FlowDocument)content);
            }
            else
            {
                return (null);
            }
        }

        public void CreateRecording()
        {
            m_logger.Trace("CreateRecording() called");

            if (null != m_currentUser)
            {
                m_currentRecording = m_library.CreateRecording(m_currentUser.UserId);

                StringWriter recordingContent = new StringWriter();

                m_currentRecording.DebugPrint(recordingContent, "");

                m_logger.Trace(recordingContent.ToString());
            }
        }

        public void SaveRecording()
        {
            m_logger.Trace("SaveRecording() called");

            m_library.SaveRecording(m_currentRecording);
        }

        public void DiscardRecording()
        {
            m_logger.Trace("DiscardRecording() called");

            m_library.DiscardRecording(m_currentRecording);
        }

        #endregion

    }
}
