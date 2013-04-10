using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AudioRecorder
{
    public enum RecordingState
    {
        Stopped,
        Monitoring,
        Recording,
        StopRequested
    }
}
