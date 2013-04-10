using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RecordingBooth
{
    public interface IBoothViewModel
    {
        // Display selection functions
        void ViewStartScreen();
        void ViewAdminScreen();
        void ViewRegisterScreen();
        void ViewAgreementScreen();
        void ViewRecordingScreen();
        void ViewRecordingCompletedScreen();
        void ViewUserListScreen();
        void ViewUserRecordingsScreen();
    }
}
