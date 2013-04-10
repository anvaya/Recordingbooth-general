using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using NLog;
using WindowsInput;



namespace TouchKeyboard
{  

    /// <summary>
    /// Interaction logic for TouchKeyboard.xaml
    /// </summary>
    public partial class OnScreenKeyboard : UserControl, INotifyPropertyChanged
    {
        private static Logger m_logger = LogManager.GetCurrentClassLogger();

        public static RoutedCommand KeyClickCommand = new RoutedCommand("KeyClick", typeof(OnScreenKeyboard));
        public static RoutedCommand SymbolKeyClickCommand = new RoutedCommand("SymbolKeyClick", typeof(OnScreenKeyboard));
        public static RoutedCommand SpecialKeyClickCommand = new RoutedCommand("SpecialKeyClick", typeof(OnScreenKeyboard));
        public static RoutedCommand CapsLockKeyCommand = new RoutedCommand("CapsLockKey", typeof(OnScreenKeyboard));
        public static RoutedCommand ShiftKeyCommand = new RoutedCommand("ShiftKey", typeof(OnScreenKeyboard));
        public static RoutedCommand CharacterKeyCommand = new RoutedCommand("CharacterKey", typeof(OnScreenKeyboard));

        public event PropertyChangedEventHandler PropertyChanged;

        private ShiftState m_shiftState;
        private bool m_inShift;
        private VirtualKeyCode m_shiftKey;
        private bool m_inCapsLock;

        #region Constructors

        public OnScreenKeyboard()
        {
            InitializeComponent();

            m_inShift = false;
            m_shiftKey = VirtualKeyCode.LSHIFT;     // Default keystroke (used to handle symbol keys)
            m_inCapsLock = false;
            m_shiftState = ShiftState.LOWER_CASE;   // Note that this is initializing the state with no notification
        }

        #endregion

        #region Properties

        public ShiftState ShiftState
        {
            get { return (m_shiftState); }
            set
            {
                if (value != m_shiftState)
                {
                    m_shiftState = value;
                    m_logger.Trace("Changed ShiftState: " + m_shiftState.ToString());
                    NotifyPropertyChanged("ShiftState");
                }
            }
        }

        #endregion

        #region Methods

        private void NotifyPropertyChanged(String propertyName)
        {
            m_logger.Trace("NotifyPropertyChanged() called:  propertyName=" + propertyName);

            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region Key command logic

        // Regular key clicks
        private void KeyClickCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            m_logger.Trace("KeyClickCommandExecuted() called");

            VirtualKeyCode key = (VirtualKeyCode)e.Parameter;

            m_logger.Trace("  Parameter={0}", key.ToString());

            if (m_inShift)
            {
                InputSimulator.SimulateModifiedKeyStroke(m_shiftKey, key);
            }
            else
            {
                InputSimulator.SimulateKeyPress(key);
            }

            ComputeShiftState(false, m_inCapsLock);
        }

        // CanExecuteRoutedEventHandler for all keys.
        private void KeyClickCommandCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        // Symbol key clicks
        // On a real keyboard, you have to hold down the SHIFT key to shift 1 => !, [ => {, etc.
        // On this virtual keyboard, you don't have that option, so we need to simulate it.
        private void SymbolKeyClickCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            m_logger.Trace("SymbolKeyClickCommandExecuted() called");

            VirtualKeyCode key = (VirtualKeyCode)e.Parameter;

            m_logger.Trace("  Parameter={0}", key.ToString());

            if (m_inCapsLock ^ m_inShift)
            {
                InputSimulator.SimulateModifiedKeyStroke(m_shiftKey, key);
            }
            else
            {
                InputSimulator.SimulateKeyPress(key);
            }

            ComputeShiftState(false, m_inCapsLock);
        }

        private void ComputeShiftState(bool newShifted, bool newCapsLock)
        {
            m_inShift = newShifted;
            m_inCapsLock = newCapsLock;

            // Caution:  Clever code - using XOR to select shift state:
            //    inLock | inShift || XOR | ShiftState
            //    -----------------||------------------
            //       0   |    0    ||  0  | LOWER_CASE
            //       0   |    1    ||  1  | UPPER_CASE
            //       1   |    0    ||  1  | UPPER_CASE
            //       1   |    1    ||  0  | LOWER_CASE
            this.ShiftState = (m_inCapsLock ^ m_inShift) ? ShiftState.UPPER_CASE : ShiftState.LOWER_CASE;
        }

        // ExecutedRoutedEventHandler for special keys (backspace, tab, enter, space).
        private void SpecialKeyClickCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            m_logger.Trace("SpecialKeyClickCommandExecuted() called");
            m_logger.Trace("  Parameter={0}", e.Parameter);

            VirtualKeyCode key = (VirtualKeyCode)e.Parameter;

            InputSimulator.SimulateKeyPress(key);

            m_logger.Trace("  Parameter={0}", key.ToString());
        }

        // ExecutedRoutedEventHandler for the CAPS LOCK key.
        private void CapsLockKeyCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            m_logger.Trace("CapsLockKeyCommandExecuted() called");
            m_logger.Trace("  Parameter={0}", e.Parameter);

            VirtualKeyCode key = (VirtualKeyCode)e.Parameter;

            InputSimulator.SimulateKeyPress(key);

            ComputeShiftState(false, !m_inCapsLock);

            m_logger.Trace("  inCapsLock={0}, inShift={1}, new ShiftState={2}",
                m_inCapsLock.ToString(), m_inShift.ToString(), this.ShiftState.ToString());
        }

        // ExecutedRoutedEventHandler for the SHIFT keys.
        private void ShiftKeyCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            m_logger.Trace("ShiftKeyCommandExecuted() called");
            m_logger.Trace("  Parameter={0}", e.Parameter);

            m_shiftKey = (VirtualKeyCode)e.Parameter;

            ComputeShiftState(!m_inShift, m_inCapsLock);

            m_logger.Trace("  inCapsLock={0}, inShift={1}, new ShiftState={2}",
                m_inCapsLock.ToString(), m_inShift.ToString(), this.ShiftState.ToString());
        }

        // ExecutedRoutedEventHandler for the SHIFT keys.
        private void CharacterKeyCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            m_logger.Trace("CharacterKeyCommandExecuted() called");
            m_logger.Trace("  Parameter={0}", e.Parameter);

            string characters = (string)e.Parameter;
            char selected = characters[0];

            if (m_shiftState == ShiftState.UPPER_CASE)
            {
                selected = characters[1];
            }

            
            
            /*             
            InputSimulator.KeyCodeData keyData = InputSimulator.GetKeyCode(selected);

            if (InputSimulator.KeyCodeData.FlagBits.SHIFT == (InputSimulator.KeyCodeData.FlagBits.SHIFT & keyData.flags))
            {
                InputSimulator.SimulateModifiedKeyStroke(m_shiftKey, keyData.keyCode);
            }
            else
            {
                InputSimulator.SimulateKeyPress(keyData.keyCode);
            }*/

            if (selected == '@')
            {
                InputSimulator.SimulateModifiedKeyStroke(VirtualKeyCode.SHIFT, VirtualKeyCode.VK_2);
            }
            else
            {
                if (m_shiftState == TouchKeyboard.ShiftState.UPPER_CASE)
                {
                    InputSimulator.SimulateModifiedKeyStroke(m_shiftKey, (VirtualKeyCode)selected);
                }
                else
                {
                    InputSimulator.SimulateKeyPress((VirtualKeyCode)selected);
                }
            }

            ComputeShiftState(false, m_inCapsLock);
        }

        #endregion

        #region Event handlers

        #endregion

    }
}
