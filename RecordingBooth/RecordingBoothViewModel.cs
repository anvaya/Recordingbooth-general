using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RecordingBooth
{
    class RecordingBoothViewModel : IBoothViewModel
    {
        private HostWindow m_hostWindow;

        public RecordingBoothViewModel(HostWindow hostWindow)
        {
            m_hostWindow = hostWindow;
        }

        public void ViewStartScreen()
        {
            m_hostWindow.ViewStartScreen();
        }

        public void ViewAdminScreen()
        {
            m_hostWindow.ViewAdminScreen();
        }

        public void ViewRegisterScreen()
        {
            m_hostWindow.ViewRegisterScreen();
        }

        public void ViewAgreementScreen()
        {
            m_hostWindow.ViewAgreementScreen();
        }

        public void ViewRecordingScreen()
        {
            m_hostWindow.ViewRecordingScreen();
        }

        public void ViewRecordingCompletedScreen()
        {
            m_hostWindow.ViewRecordingCompletedScreen();
        }

        public void ViewUserListScreen()
        {
            m_hostWindow.ViewUserListScreen();
        }

        public void ViewUserRecordingsScreen()
        {
            m_hostWindow.ViewUserRecordingsScreen();
        }

    }
}
