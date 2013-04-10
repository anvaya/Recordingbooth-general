using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using NLog;

namespace TouchKeyboard
{
    public class TouchKey : Button
    {
        private static Logger m_logger = LogManager.GetCurrentClassLogger();

        #region Properties

        // Dependency property:  Is the key shifted or not?
        public ShiftState ShiftState
        {
            get { return (ShiftState)GetValue(ShiftStateProperty); }
            set { SetValue(ShiftStateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ShiftState.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShiftStateProperty =
            DependencyProperty.Register(
                "ShiftState",
                typeof(ShiftState),
                typeof(TouchKey),
                new UIPropertyMetadata(ShiftState.LOWER_CASE, new PropertyChangedCallback(OnShiftStateChanged)));

        public string ShiftedLabel { get; set; }
        public string UnshiftedLabel { get; set; }

        #endregion

        #region Constructors

        public TouchKey() : base()
        {
            ;
        }

        #endregion

        #region Methods

        private static void OnShiftStateChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            // m_logger.Trace("OnShiftStateChanged() called");

            TouchKey key = dependencyObject as TouchKey;

            if (null != key)
            {
                string keyText = key.Content as string;

                if (null != keyText)
                {
                    switch ((ShiftState)e.NewValue)
                    {
                        case ShiftState.LOWER_CASE:
                            key.Content = key.UnshiftedLabel;
                            break;

                        case ShiftState.UPPER_CASE:
                            key.Content = key.ShiftedLabel;
                            break;
                    }
                }
                else
                {
                    m_logger.Error("Content is not a string - type=" + key.Content.GetType().ToString());
                }
            }
            else
            {
                m_logger.Error("*** Got a dependencyObject other than a TouchKey:  type=" + dependencyObject.GetType().ToString());
            }
        }

        #endregion

    }
}
