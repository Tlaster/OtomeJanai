using OtomeJanai.Shared.Controls.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace OtomeJanai.Shared.Controls
{
    internal class UIElement
    {
        internal event EventHandler<PointerEventArgs> PointerEntered;
        internal event EventHandler<PointerEventArgs> PointerExited;
        internal event EventHandler<PointerEventArgs> PointerMoved;
        internal event EventHandler<PointerEventArgs> PointerPressed;
        internal event EventHandler<PointerEventArgs> PointerReleased;
        internal event EventHandler<PointerEventArgs> PointerWheelChanged;



    }
    
}
