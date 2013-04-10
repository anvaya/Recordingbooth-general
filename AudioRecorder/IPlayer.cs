using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AudioRecorder
{
    public interface IPlayer : IDisposable
    {
        #region Properties

        TimeSpan TotalTime { get; }
        TimeSpan CurrentTime { get; set; }
        float Volume { get; set; }

        #endregion

        #region Methods

        void LoadFile(string filePath);
        void Play();
        void Stop();

        #endregion

        #region Events

        event EventHandler PlaybackStopped;

        #endregion
    }
}
