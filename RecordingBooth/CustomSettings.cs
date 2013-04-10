using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Xml;
using NLog;

namespace RecordingBooth
{
    public class CustomSettings : Dictionary<string, string>
    {
        private static Logger m_logger = LogManager.GetCurrentClassLogger();

        // Welcome/Start screen
        public static readonly string WelcomeStartButtonText = "WelcomeStartButtonText";
        public static readonly string WelcomeStartButtonColor = "WelcomeStartButtonColor";
        public static readonly string WelcomeBackgroundImage = "WelcomeBackgroundImage";

        // Register screen
        public static readonly string RegisterNextButtonText = "RegisterNextButtonText";
        public static readonly string RegisterNextButtonColor = "RegisterNextButtonColor";
        public static readonly string RegisterBackButtonText = "RegisterBackButtonText";
        public static readonly string RegisterBackButtonColor = "RegisterBackButtonColor";
        public static readonly string RegisterBackgroundImage = "RegisterBackgroundImage";

        // Agreement screen
        public static readonly string AgreementAgreeButtonText = "AgreementAgreeButtonText";
        public static readonly string AgreementAgreeButtonColor = "AgreementAgreeButtonColor";
        public static readonly string AgreementBackButtonText = "AgreementBackButtonText";
        public static readonly string AgreementBackButtonColor = "AgreementBackButtonColor";
        public static readonly string AgreementBackgroundImage = "AgreementBackgroundImage";

        // Recording screen
        public static readonly string RecordingStartButtonText = "RecordingStartButtonText";
        public static readonly string RecordingStartButtonColor = "RecordingStartButtonColor";
        public static readonly string RecordingStopButtonText = "RecordingStopButtonText";
        public static readonly string RecordingStopButtonColor = "RecordingStopButtonColor";
        public static readonly string RecordingBackButtonText = "RecordingBackButtonText";
        public static readonly string RecordingBackButtonColor = "RecordingBackButtonColor";
        public static readonly string RecordingBackgroundImage = "RecordingBackgroundImage";

        // Playback/Review screen
        public static readonly string PlaybackPlayButtonText = "PlaybackPlayButtonText";
        public static readonly string PlaybackPlayButtonColor = "PlaybackPlayButtonColor";
        public static readonly string PlaybackReRecordButtonText = "PlaybackReRecordButtonText";
        public static readonly string PlaybackReRecordButtonColor = "PlaybackReRecordButtonColor";
        public static readonly string PlaybackNewRecordingButtonText = "PlaybackNewRecordingButtonText";
        public static readonly string PlaybackNewRecordingButtonColor = "PlaybackNewRecordingButtonColor";
        public static readonly string PlaybackFinishButtonText = "PlaybackFinishButtonText";
        public static readonly string PlaybackFinishButtonColor = "PlaybackFinishButtonColor";
        public static readonly string PlaybackBackgroundImage = "PlaybackBackgroundImage";

        // Admin screen
        public static readonly string AdminUpdateButtonText = "AdminUpdateButtonText";
        public static readonly string AdminUpdateButtonColor = "AdminUpdateButtonColor";
        public static readonly string AdminViewUsersButtonText = "AdminViewUsersButtonText";
        public static readonly string AdminViewUsersButtonColor = "AdminViewUsersButtonColor";
        public static readonly string AdminExportButtonText = "AdminExportButtonText";
        public static readonly string AdminExportButtonColor = "AdminExportButtonColor";
        public static readonly string AdminBackButtonText = "AdminBackButtonText";
        public static readonly string AdminBackButtonColor = "AdminBackButtonColor";
        public static readonly string AdminExitButtonText = "AdminExitButtonText";
        public static readonly string AdminExitButtonColor = "AdminExitButtonColor";
        public static readonly string AdminPopupBackgroundColor = "AdminPopupBackgroundColor";
        public static readonly string AdminBackgroundImage = "AdminBackgroundImage";

        // User List screen
        public static readonly string UserListBackButtonText = "UserListBackButtonText";
        public static readonly string UserListBackButtonColor = "UserListBackButtonColor";

        // Recordings List screen
        public static readonly string RecordingsListBackButtonText = "RecordingsListBackButtonText";
        public static readonly string RecordingsListBackButtonColor = "RecordingsListBackButtonColor";

        private Boolean m_isLoaded;

        #region Constructors

        public CustomSettings()
        {
            m_isLoaded = false;
        }

        #endregion

        #region Properties

        public bool IsLoaded
        {
            get { return (m_isLoaded); }
            set { m_isLoaded = value; }
        }

        #endregion

        #region Methods

        public void Save(string path)
        {
            m_logger.Trace("Save(\"{0}\") called", path);

            XmlWriterSettings xmlSettings = new XmlWriterSettings();
            xmlSettings.Encoding = new UTF8Encoding(false);
            xmlSettings.Indent = true;

            using (XmlWriter xmlWriter = XmlWriter.Create(path, xmlSettings))
            {
                xmlWriter.WriteStartDocument();
                xmlWriter.WriteStartElement("CustomSettings");

                foreach (KeyValuePair<string, string> pair in this)
                {
                    xmlWriter.WriteStartElement(pair.Key);
                    xmlWriter.WriteValue(pair.Value);
                    xmlWriter.WriteEndElement();
                }

                xmlWriter.WriteEndElement();

                xmlWriter.WriteEndDocument();
            }
        }

        public static CustomSettings Load(string path)
        {
            m_logger.Trace("Load(\"{0}\") called", path);

            CustomSettings answer = new CustomSettings();

            XmlDocument xmlDoc = new XmlDocument();

            try
            {
                xmlDoc.Load(path);

                foreach (XmlNode itemNode in xmlDoc.SelectNodes("/CustomSettings/*"))
                {
                    string key = itemNode.LocalName;
                    string value = ProcessSpecialCharacters(itemNode.InnerText);

                    m_logger.Trace("  key=\"{0}\", value=\"{1}\"", key, value);

                    answer.Add(key, value);
                }

                answer.IsLoaded = true;
            }
            catch (Exception ex)
            {
                m_logger.Error("Cannot load custom settings", ex);       // Yes, it's sloppy, but skip any errors and justs return the empty dictionary
            }

            return (answer);
        }

        private static string ProcessSpecialCharacters(string rawText)
        {
            Regex regex = new Regex(@"\\n");                // Search for \n
            string answer = regex.Replace(rawText, "\n");   // Replace with a newline

            return (answer);
        }

        public void DebugPrint(TextWriter writer, string indent)
        {
            string childIndent = indent + "  ";

            writer.WriteLine("{0}CustomSettings:", indent);
            foreach (KeyValuePair<string, string> pair in this)
            {
                writer.WriteLine("{0}key=\"{1}\", value=\"{2}\"", childIndent, pair.Key, pair.Value);
            }
        }

        #endregion

    }
}
