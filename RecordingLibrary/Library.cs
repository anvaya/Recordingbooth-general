using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using NLog;
using RecordingLibrary.Persistence;

namespace RecordingLibrary
{
    public class Library
    {
        private static Logger m_logger = LogManager.GetCurrentClassLogger();

        public const string TEMP_FILENAME = "tempfile.wav";
        public const string INDEX_FILENAME = "RecordingIndex.dat";
        public const string FILENAME_TEMPLATE = "Recording_{0:D5}.wav";

        private string m_databaseConnectionString;
        private string m_libraryPath;
        private RecordingStore m_persistentStore;

        #region Constructors

        public Library(string databaseConnectionString, string libraryPath)
        {
            m_databaseConnectionString = databaseConnectionString;
            m_libraryPath = libraryPath;
            m_persistentStore = new RecordingStore(m_databaseConnectionString);

            try
            {
                if ((String.Empty != libraryPath) && (!Directory.Exists(libraryPath)))
                {
                    Directory.CreateDirectory(libraryPath);
                }
            }
            catch (IOException ex)
            {
                m_logger.Error(String.Format("Cannot create directory \"{0}\"", libraryPath), ex); 

            }
        }

        #endregion

        #region Properties

        public string DatabaseConnectionString
        {
            get { return (m_databaseConnectionString); }
            set { m_databaseConnectionString = value; }
        }

        public string LibraryPath
        {
            get { return (m_libraryPath); }
            set { m_libraryPath = value; }
        }

        public RecordingStore RecordingStore
        {
            get { return (m_persistentStore); }
            // Read-only property
        }

        #endregion

        #region Methods

        /// <summary>
        /// Save the user information into the library.
        /// </summary>
        /// <remarks>
        /// The user information is saved into the library.  The <code>User</code>
        /// object is updated with the user ID assigned by the library.
        /// </remarks>
        /// <param name="userData">Input data from the user.</param>
        public void CreateUser(User userData)
        {
            m_persistentStore.CreateUser(userData);
        }

        public void UpdateUser(User userData)
        {
            m_persistentStore.UpdateUser(userData);
        }

        /// <summary>
        /// Fetch the user data given the user ID.
        /// </summary>
        /// <param name="userId">User ID of record to retrieve.</param>
        /// <returns><code>User</code> object retrieved from the library.</returns>
        public User RetrieveUser(int userId)
        {
            return (m_persistentStore.RetrieveUser(userId));
        }


        public void ClearUserDatabase()
        {
            m_persistentStore.ClearUsersDatabase();
        }

        /// <summary>
        /// Return all the users in the library.
        /// </summary>
        /// <returns>A <code>List</code> of all the <code>User</code> records.</returns>
        public UserCollection ListUsers()
        {
            return (m_persistentStore.ListUsers());
        }

        public void ExportUserList(string filePath)
        {
            UserCollection userList = m_persistentStore.ListUsers();

            using (TextWriter textWriter = new StreamWriter(filePath))
            {
                UserCsvWriter userWriter = new UserCsvWriter(textWriter);

                userWriter.WriteHeader();

                foreach (User user in userList)
                {
                    userWriter.WriteUser(user);
                }
            }
        }

        public void ExportRecordingList(string filePath)
        {
            RecordingCollection recordingList = m_persistentStore.ListRecordings();

            using (TextWriter textWriter = new StreamWriter(filePath))
            {
                RecordingCsvWriter recordingWriter = new RecordingCsvWriter(textWriter);

                recordingWriter.WriteHeader();

                foreach (Recording recording in recordingList)
                {
                    recordingWriter.WriteRecording(recording);
                }
            }
        }

        /// <summary>
        /// Create a new recording record in the library.
        /// </summary>
        /// <param name="userId">User ID associated with the recording.</param>
        /// <returns>The new <code>Recording</code> application object.</returns>
        public Recording CreateRecording(int userId)
        {
            string fileName = this.CreateTempFileName();

            Recording recording = m_persistentStore.CreateRecording(userId, fileName);

            recording.FilePath = Path.Combine(m_libraryPath, recording.FileName);

            return (recording);
        }

        /// <summary>
        /// Save the recording to the new file name and update the database.
        /// </summary>
        /// <param name="recording">Application object with recording information.</param>
        public void SaveRecording(Recording recording)
        {
            string tempPath = recording.FilePath;
            string fileName = this.CreateFileName();
            string permanentPath = Path.Combine(m_libraryPath, fileName);

            this.SaveContent(tempPath, permanentPath);

            recording.FileName = fileName;
            recording.FilePath = permanentPath;

            m_persistentStore.SaveRecording(recording);
        }

        /// <summary>
        /// Move the content from the temp file to the permanent file.
        /// </summary>
        /// <param name="sourcePath"></param>
        /// <param name="destPath"></param>
        private void SaveContent(string sourcePath, string destPath)
        {
            try
            {
                File.Move(sourcePath, destPath);
            }
            catch (Exception) { }
        }

        /// <summary>
        /// Discard the recording wave file.
        /// </summary>
        /// <param name="recording"></param>
        public void DiscardRecording(Recording recording)
        {
            File.Delete(recording.FilePath);
        }

        /// <summary>
        /// Fetch a <code>Recording</code> given the recording ID.
        /// </summary>
        /// <param name="recordingId">ID of the desired recording.</param>
        /// <returns><code>Recording</code> application object from the library.</returns>
        public Recording RetrieveRecording(int recordingId)
        {
            Recording recording = m_persistentStore.RetrieveRecording(recordingId);

            recording.FilePath = Path.Combine(m_libraryPath, recording.FileName);

            return (recording);
        }

        /// <summary>
        /// Return all the recordings in the library.
        /// </summary>
        /// <returns>A <code>List</code> of all the <code>Recording</code> records.</returns>
        public RecordingCollection ListRecordings()
        {
            return (m_persistentStore.ListRecordings());
        }

        /// <summary>
        /// Return all recordings for the specified user ID.
        /// </summary>
        /// <param name="userId">ID of the user.</param>
        /// <returns>A <code>List</code> of all the <code>Recording</code> records for the user.</returns>
        public RecordingCollection ListUserRecordings(int userId)
        {
            return (m_persistentStore.ListUserRecordings(userId));
        }

        /// <summary>
        /// Allocate a temporary filename.
        /// </summary>
        /// <remarks>
        /// For now, just returns a constant string.
        /// </remarks>
        /// <returns>String containing the new filename.</returns>
        public string CreateTempFileName()
        {
            return (TEMP_FILENAME);
        }

        /// <summary>
        /// Allocate a new filename.
        /// </summary>
        /// <remarks>
        /// While this may seem overly complex, there's no direct way in .NET to use the same
        /// file session to read from a file, seek back to the start, then rewrite the contents.
        /// </remarks>
        /// <returns>String containing the new filename.</returns>
        public string CreateFileName()
        {
            string indexPath = Path.Combine(m_libraryPath, INDEX_FILENAME);
            int recordingIndex = 0;

            EnsureLibraryPath();

            // Fetch the next index number from the index file
            using (FileStream indexStream = new FileStream(indexPath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                // Read the current index value
                try
                {
                    int fileSize = (int) indexStream.Length;
                    if (fileSize == 0)
                    {
                        recordingIndex = 1;
                    }
                    else
                    {
                        byte[] buffer = new byte[fileSize];
                        long readLen = indexStream.Read(buffer, 0, fileSize);

                        string stringIndex = System.Text.Encoding.UTF8.GetString(buffer);

                        recordingIndex = Int32.Parse(stringIndex) + 1;
                    }
                }
                catch (EndOfStreamException)
                {
                    // If the data ain't there, start with 1
                    recordingIndex = 1;
                }

                // Seek back to the start
                indexStream.Seek(0, SeekOrigin.Begin);

                // Truncate the file
                indexStream.SetLength(0);

                // Write the new recording index back to the file
                string outputText = Convert.ToString(recordingIndex);
                byte[] outputBuffer = System.Text.Encoding.UTF8.GetBytes(outputText);

                indexStream.Write(outputBuffer, 0, outputBuffer.Length);
            }

            string answer = String.Format(FILENAME_TEMPLATE, recordingIndex);

            return (answer);
        }

        private void EnsureLibraryPath()
        {
            if (!Directory.Exists(m_libraryPath))
            {
                Directory.CreateDirectory(m_libraryPath);
            }
        }

        #endregion


    }
}
