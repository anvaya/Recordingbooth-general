using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.IO;

namespace RecordingLibrary
{
    public class User : INotifyPropertyChanged
    {
        public const int UNASSIGNED_ID = -1;

        private int m_userId;
        private DateTime m_dateTime;
        private string m_userName;
        private string m_address1;
        private string m_address2;
        private string m_city;
        private string m_postalRegion;
        private string m_postalCode;
        private string m_email;
        private int? m_age;
        private bool m_acceptsTerms;

        public event PropertyChangedEventHandler PropertyChanged;

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public User()
        {
            m_userId = UNASSIGNED_ID;
            m_dateTime = DateTime.Now;
            m_userName = "";
            m_address1 = "";
            m_address2 = "";
            m_city = "";
            m_postalRegion = "";
            m_postalCode = "";
            m_email = "";
            m_age = null;
            m_acceptsTerms = false;
        }

        /// <summary>
        /// Restore a Recording from data
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="dateTime"></param>
        internal User(int userId, DateTime dateTime)
        {
            m_userId = userId;
            m_dateTime = dateTime;
        }

        #endregion

        #region Properties

        public int UserId
        {
            get { return (m_userId); }
            set
            {
                m_userId = value;
                OnPropertyChanged("UserId");
            }
        }

        public DateTime CreationDateTime
        {
            get { return (m_dateTime); }
            set
            {
                m_dateTime = value;
                OnPropertyChanged("CreationDateTime");
            }
        }

        public string UserName
        {
            get { return (m_userName); }
            set
            {
                m_userName = value;
                OnPropertyChanged("UserName");
            }
        }

        public string AddressLine1
        {
            get { return (m_address1); }
            set
            {
                m_address1 = value;
                OnPropertyChanged("AddressLine1");
            }
        }

        public string AddressLine2
        {
            get { return (m_address2); }
            set
            {
                m_address2 = value;
                OnPropertyChanged("AddressLine2");
            }
        }

        public string AdministrativeRegion
        {
            get { return (m_city); }
            set
            {
                m_city = value;
                OnPropertyChanged("City");
            }
        }

        public string PostalRegion
        {
            get { return (m_postalRegion); }
            set
            {
                m_postalRegion = value;
                OnPropertyChanged("PostalRegion");
            }
        }

        public string PostalCode
        {
            get { return (m_postalCode); }
            set
            {
                m_postalCode = value;
                OnPropertyChanged("PostalCode");
            }
        }

        public string Email
        {
            get { return (m_email); }
            set
            {
                m_email = value;
                OnPropertyChanged("Email");
            }
        }

        public int? Age
        {
            get { return (m_age); }
            set
            {
                m_age = value;
                OnPropertyChanged("Age");
            }
        }

        public bool AcceptsTerms
        {
            get { return (m_acceptsTerms); }
            set
            {
                m_acceptsTerms = value;
                OnPropertyChanged("AcceptsTerms");
            }
        }


        #endregion

        #region Operations

        protected void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        public void DebugPrint(TextWriter output, string indent)
        {
            string childIndent = indent + "  ";
            string userIdString = "UNASSIGNED_ID";
            if (m_userId != UNASSIGNED_ID)
            {
                userIdString = m_userId.ToString();
            }

            output.WriteLine("{0}User:", indent);
            output.WriteLine("{0}UserId={1}", childIndent, userIdString);
            output.WriteLine("{0}CreationDateTime={1}", childIndent, m_dateTime);
            output.WriteLine("{0}UserName={1}", childIndent, m_userName);
            output.WriteLine("{0}AddressLine1={1}", childIndent, m_address1);
            output.WriteLine("{0}AddressLine2={1}", childIndent, m_address2);
            output.WriteLine("{0}City={1}", childIndent, m_city);
            output.WriteLine("{0}PostalRegion={1}", childIndent, m_postalRegion);
            output.WriteLine("{0}PostalCode={1}", childIndent, m_postalCode);
            output.WriteLine("{0}Email={1}", childIndent, m_email);
            output.WriteLine("{0}Age={1}", childIndent, m_age);
            output.WriteLine("{0}AcceptsTerms={1}", childIndent, m_acceptsTerms.ToString());
        }

        #endregion

    }
}
