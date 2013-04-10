using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NAudio.Wave;

namespace AudioRecorder
{
    /// <summary>
    /// Interface for recording audio.
    /// </summary>
    public interface IRecorder
    {

        #region Properties

        /// <summary>
        /// Current state of the recorder.
        /// </summary>
        RecordingState RecordingState { get; }

        /// <summary>
        /// Volume level for recording.
        /// </summary>
        double RecordingLevel { get; set; }

        /// <summary>
        /// Length of the recording (time).
        /// </summary>
        TimeSpan RecordingLength { get; }

        /// <summary>
        /// Recording parameters - sample rate, bit depth, channels, etc.
        /// </summary>
        WaveFormat RecordingFormat { get; set; }

        /// <summary>
        /// Track the samples over time for the level indicator.
        /// </summary>
        PeakSampler PeakMonitor { get; }

        /// <summary>
        /// Fired when recording has stopped.
        /// </summary>
        /// <remarks>
        /// The audio driver takes time to deliver the samples to the application.
        /// When <code>Stop()</code> is called, the remaining samples are
        /// saved, then the <code>RecordingStopped</code> event is
        /// fired.
        /// </remarks>
        event EventHandler RecordingStopped;


        #endregion

        #region Methods

        /// <summary>
        /// Start by monitoring the audio input.  Samples are collected and
        /// fed to the <code>SampleAggregator</code>, but the samples are
        /// not saved until <code>StartRecording()</code> is called.
        /// </summary>
        /// <param name="deviceIndex">Index of audio device to use.</param>
        void Monitor(int deviceIndex);

        /// <summary>
        /// Start recording the samples.
        /// </summary>
        /// <param name="fileName">File to which the audio should be written.</param>
        void StartRecording(string fileName);

        /// <summary>
        /// Stop recording the samples.
        /// </summary>
        /// <remarks>
        /// As described for <code>RecordingStopped</code>, when <code>Stop()</code> is
        /// called the application cannot assume all the input has been saved until
        /// the <code>RecordingStopped</code> event is fired.
        /// </remarks>
        void StopRecording();

        /// <summary>
        /// End monitoring of the audio input.
        /// </summary>
        void StopMonitoring();

        #endregion

    }
}
