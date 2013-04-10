using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RecordingBooth
{
    public class FlowEditSubstitution
    {
        private string m_elementTag;
        private string m_replacement;

        #region Constructors

        public FlowEditSubstitution()
        {
            m_elementTag = "";
            m_replacement = "";
        }

        public FlowEditSubstitution(string elementTag, string replacement)
        {
            m_elementTag = elementTag;
            m_replacement = replacement;
        }

        #endregion

        #region Properties

        public string ElementTag
        {
            get { return (m_elementTag); }
            set { m_elementTag = value; }
        }

        public string Replacement
        {
            get { return (m_replacement); }
            set { m_replacement = value; }
        }

        #endregion

        #region Methods

        #endregion

    }
}
