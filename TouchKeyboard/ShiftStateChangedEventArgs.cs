using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TouchKeyboard
{
    public class ShiftStateChangedEventArgs : EventArgs
    {
        public ShiftState ShiftState { get; private set; }

        public ShiftStateChangedEventArgs(ShiftState shiftState)
        {
            ShiftState = shiftState;
        }

    }
}
