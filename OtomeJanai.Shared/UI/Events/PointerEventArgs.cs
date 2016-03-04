using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace OtomeJanai.Shared.UI.Events
{
    internal class PointerEventArgs : EventArgs
    {

        internal Point Position { get; }
        internal PointerType ButtonType { get; }
        internal int ScrollWheelValue { get; }

        internal PointerEventArgs(Point position, PointerType buttonType,int scrollWheelValue = 0) : base()
        {
            Position = position;
            ButtonType = buttonType;
            ScrollWheelValue = scrollWheelValue;
        }
    }

    internal enum PointerType
    {
        MouseLeftButton,
        MouseMiddleButton,
        MouseRightButton,
        MouseXButton1,
        MouseXButton2,
        MouseScrollWheel,
        Mouse,
        Touch,
    }

}
