using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace AudioRecorder
{
    public class PeakSampleEventArgs : EventArgs
    {
        private float m_maxSample;
        private float m_minSample;

        #region Constructors

        [DebuggerStepThrough]
        public PeakSampleEventArgs(float minValue, float maxValue)
        {
            m_maxSample = maxValue;
            m_minSample = minValue;
        }

        #endregion

        #region Properties

        public float MaxSample
        {
            get { return (m_maxSample); }
            // Read-only property
        }

        public float MinSample
        {
            get { return (m_minSample); }
            // Read-only property
        }

        #endregion

        #region Methods

        #endregion

    }
}
