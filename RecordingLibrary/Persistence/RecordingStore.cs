using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RecordingLibrary.Persistence
{
    public class RecordingStore
    {
        private RecordingsData m_dataContext;

        #region Constructors

        public RecordingStore(string connectionString)
        {
            m_dataContext = new RecordingsData(connectionString);
        }

        #endregion

        #region Properties

        #endregion

        #region Operations

        /// <summary>
        /// Utility function to convert application object to database record.
        /// </summary>
        /// <param name="userData">Application object.</param>
        /// <returns>Database record.</returns>
        private void SaveUserToDatabaseRecord(User userData, USERS userRecord)
        {
            userRecord.NAME = userData.UserName;
            userRecord.ADDRESS_1 = userData.AddressLine1;
            userRecord.ADDRESS_2 = userData.AddressLine2;
            userRecord.CITY = userData.AdministrativeRegion;
            userRecord.POST_REGION = userData.PostalRegion;
            userRecord.POST_CODE = userData.PostalCode;
            userRecord.AGE = userData.Age;
            userRecord.EMAIL = userData.Email;
            userRecord.VISIT_TIMESTAMP = userData.CreationDateTime;
            userRecord.ACCEPTS_AGREEMENT = userData.AcceptsTerms ? "Y" : "N";
        }

        /// <summary>
        /// Utility function to convert application object to database record.
        /// </summary>
        /// <param name="userData">Application object.</param>
        /// <returns>Database record.</returns>
        private USERS SaveUserToDatabaseRecord(User userData)
        {
            USERS userRecord = new USERS();

            SaveUserToDatabaseRecord(userData, userRecord);

            return (userRecord);
        }

        /// <summary>
        /// Utility function to convert database record to application object.
        /// </summary>
        /// <param name="userRecord">Record from the database.</param>
        /// <returns>Application <code>User</code> object.</returns>
        private User LoadUserFromDatabaseRecord(USERS userRecord)
        {
            User newUser = new User();

            newUser.UserId = userRecord.USER_ID;
            newUser.UserName = userRecord.NAME;
            newUser.AddressLine1 = userRecord.ADDRESS_1;
            newUser.AddressLine2 = userRecord.ADDRESS_2;
            newUser.AdministrativeRegion = userRecord.CITY;
            newUser.PostalRegion = userRecord.POST_REGION;
            newUser.PostalCode = userRecord.POST_CODE;
            newUser.Age = userRecord.AGE;
            newUser.Email = userRecord.EMAIL;
            newUser.CreationDateTime = userRecord.VISIT_TIMESTAMP;
            newUser.AcceptsTerms = (userRecord.ACCEPTS_AGREEMENT == "Y") ? true : false;

            return (newUser);
        }

        /// <summary>
        /// Create a new user from the provided data.
        /// </summary>
        /// <remarks>
        /// The database record is created from the input provided by the user
        /// (in contrast to creation of a Recording).  The <code>User</code>
        /// object is updated with the user ID assigned by the database.
        /// </remarks>
        /// <param name="userData">Input data from the user.</param>
        public void CreateUser(User userData)
        {
            USERS userRecord = SaveUserToDatabaseRecord(userData);

            m_dataContext.USERS.InsertOnSubmit(userRecord);

            m_dataContext.SubmitChanges();

            // Copy the new user ID back to the application's object
            userData.UserId = userRecord.USER_ID;
        }

        /// <summary>
        /// Update the contents of an existing user record.
        /// </summary>
        /// <param name="userData">New data to store.</param>
        public void UpdateUser(User userData)
        {
            USERS record = m_dataContext.USERS.Single(u => u.USER_ID == userData.UserId);

            SaveUserToDatabaseRecord(userData, record);

            m_dataContext.SubmitChanges();
        }

        /// <summary>
        /// Read a user record given the user ID.
        /// </summary>
        /// <param name="userId">User ID of record to retrieve.</param>
        /// <returns>User object built from database record.</returns>
        public User RetrieveUser(int userId)
        {
            User newUser = new User();

            USERS record = m_dataContext.USERS.Single(u => u.USER_ID == userId);

            newUser = LoadUserFromDatabaseRecord(record);

            return (newUser);
        }

        /// <summary>
        /// Return all the users in the database.
        /// </summary>
        /// <returns>A <code>List</code> of all the user records.</returns>
        public UserCollection ListUsers()
        {
            UserCollection userList = new UserCollection();

            // Build the query to select all users from the database
            var userQuery = from u in m_dataContext.USERS          
                            orderby u.VISIT_TIMESTAMP descending                  
                            select u;

            // For each user record in the database, create a User object
            
            foreach (USERS record in userQuery)
            {
                User newUser = LoadUserFromDatabaseRecord(record);
                if(newUser.AcceptsTerms)
                    userList.Add(newUser);
            }

            return (userList);
        }

        public void ClearUsersDatabase()
        {          
            m_dataContext.ExecuteCommand("Delete From USERS");
            m_dataContext.ExecuteCommand("Delete From RECORDING");
        }


        /// <summary>
        /// Utility function to convert application <code>Recording</code> object to a database record.
        /// </summary>
        /// <param name="recordingData">Application <code>Recording</code> object.</param>
        /// <returns>Database record.</returns>
        private RECORDING SaveRecordingToDatabaseRecord(Recording recordingData)
        {
            RECORDING record = new RECORDING();

            record.USER_ID = recordingData.UserId;
            record.TIMESTAMP = recordingData.RecordingTimestamp;
            record.FILENAME = recordingData.FileName;

            return (record);
        }

        /// <summary>
        /// Utility function to create an application <code>Recording</code> object from a database record.
        /// </summary>
        /// <param name="record">Database recording record.</param>
        /// <returns>Application <code>Recording</code> object.</returns>
        private Recording LoadRecordingFromDatabaseRecord(RECORDING record)
        {
            Recording recordingData = new Recording();

            recordingData.RecordingId = record.RECORDING_ID;
            recordingData.UserId = record.USER_ID;
            recordingData.RecordingTimestamp = record.TIMESTAMP;
            recordingData.FileName = record.FILENAME;

            return (recordingData);
        }

        /// <summary>
        /// Create a new recording record and return a <code>Recording</code> application object.
        /// </summary>
        /// <remarks>
        /// In the case of a <code>Recording</code>, all that's needed is the user ID.  The <code>Recording</code>
        /// application object is created and returned.
        /// </remarks>
        /// <param name="userId">User ID associated with the recording.</param>
        /// <param name="fileName">Name of the recording's file.</param>
        /// <returns>The new <code>Recording</code> application object.</returns>
        public Recording CreateRecording(int userId, string fileName)
        {
            // Pass a dummy library path;  must be set by caller.
            Recording newRecording = new Recording(userId, String.Empty, fileName);

            return (newRecording);
        }

        /// <summary>
        /// Store the data from the <code>Recording</code> application object into the database.
        /// The generated record ID is updated in the application object.
        /// </summary>
        /// <param name="recording">Application object to save.</param>
        public void SaveRecording(Recording recording)
        {
            RECORDING record = SaveRecordingToDatabaseRecord(recording);

            m_dataContext.RECORDING.InsertOnSubmit(record);

            m_dataContext.SubmitChanges();

            // Copy the new recording ID back to the application's object
            recording.UserId = record.RECORDING_ID;
        }

        /// <summary>
        /// Read a recording record given the recording ID.
        /// </summary>
        /// <param name="recordingId">ID of the recording record.</param>
        /// <returns><code>Recording</code> application object built from the database record.</returns>
        public Recording RetrieveRecording(int recordingId)
        {
            Recording newRecording = new Recording();

            RECORDING record = m_dataContext.RECORDING.Single(rec => rec.RECORDING_ID == recordingId);

            newRecording = LoadRecordingFromDatabaseRecord(record);

            return (newRecording);
        }

        /// <summary>
        /// Return all the recordings in the database.
        /// </summary>
        /// <returns>A <code>List</code> of all the <code>Recording</code> records.</returns>
        public RecordingCollection ListRecordings()
        {
            RecordingCollection recordingList = new RecordingCollection();

            // Build the query to select all recordings from the database
            var recordingQuery = from rec in m_dataContext.RECORDING
                                 select rec;

            // For each recording record in the database, create a Recording object
            foreach (RECORDING record in recordingQuery)
            {
                Recording newRecording = LoadRecordingFromDatabaseRecord(record);

                recordingList.Add(newRecording);
            }

            return (recordingList);
        }

        /// <summary>
        /// Return all recordings for the specified user ID.
        /// </summary>
        /// <param name="userId">ID of the user.</param>
        /// <returns>A <code>List</code> of all the <code>Recording</code> records for the user.</returns>
        public RecordingCollection ListUserRecordings(int userId)
        {
            RecordingCollection recordingList = new RecordingCollection();

            // Build the query to select those recordings having the specified user ID
            var recordingQuery = from rec in m_dataContext.RECORDING
                                 where rec.USER_ID == userId
                                 select rec;

            // For each recording record found, create a Recording object
            foreach (RECORDING record in recordingQuery)
            {
                Recording newRecording = LoadRecordingFromDatabaseRecord(record);

                recordingList.Add(newRecording);
            }

            return (recordingList);
        }

        #endregion


    }
}
