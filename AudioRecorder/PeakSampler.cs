using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AudioRecorder
{
    /// <summary>
    /// The <code>SampleAggregator</code> class tracks the min and max
    /// samples over time for the level indicator.
    /// </summary>
    public class PeakSampler
    {
        private int m_sampleSize;
        private int m_currentSample;
        private float m_minValue;
        private float m_maxValue;

        public event EventHandler<PeakSampleEventArgs> SampleCountReached;
        public event EventHandler Restart = delegate { };

        #region Constructors

        public PeakSampler(int sampleCount)
        {
            m_sampleSize = sampleCount;
            m_currentSample = 0;
            m_minValue = 0.0f;
            m_maxValue = 0.0f;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Number of samples between events.
        /// </summary>
        public int SampleSize
        {
            get { return (m_sampleSize); }
            set { m_sampleSize = value; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Signal to anyone listening that the sampler is restarting.
        /// </summary>
        public void RaiseRestart()
        {
            Restart(this, EventArgs.Empty);
        }

        /// <summary>
        /// Clear the counter and peak values.
        /// </summary>
        private void Reset()
        {
            m_currentSample = 0;
            m_minValue = 0.0f;
            m_maxValue = 0.0f;
        }

        /// <summary>
        /// Add a sample.
        /// </summary>
        /// <param name="value">Current sample to add.</param>
        public void Add(float value)
        {
            m_maxValue = Math.Max(m_maxValue, value);
            m_minValue = Math.Min(m_minValue, value);
            m_currentSample++;
            if (m_currentSample >= m_sampleSize && m_sampleSize > 0)
            {
                if (SampleCountReached != null)
                {
                    SampleCountReached(this, new PeakSampleEventArgs(m_minValue, m_maxValue));
                }
                Reset();
            }
        }

        #endregion

    }

}
