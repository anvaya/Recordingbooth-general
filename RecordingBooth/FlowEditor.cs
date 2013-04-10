using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Documents;

namespace RecordingBooth
{
    public class FlowEditor
    {

        #region Public static methods

        public static void ReplaceRuns(FlowDocument document, FlowEditSubstitutionCollection substitutions)
        {
            TextPointer curPos = document.ContentStart;

            while (curPos != null)
            {
                // Scan for a span
                if (curPos.GetPointerContext(LogicalDirection.Backward) == TextPointerContext.ElementStart)
                {
                    if (curPos.Parent is Run)
                    {
                        Run curRun = (Run)curPos.Parent;

                        // See if the span has a tag that matches and element tag from the substitution list
                        foreach (FlowEditSubstitution curSub in substitutions)
                        {
                            if ((curRun.Tag is String) && (curSub.ElementTag == ((String)curRun.Tag)))
                            {
                                curRun.Text = curSub.Replacement;
                            }
                        }
                    }
                }

                curPos = curPos.GetNextContextPosition(LogicalDirection.Forward);
            }
        }

        #endregion

    }
}
