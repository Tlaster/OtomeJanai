using Microsoft.Xna.Framework;
using OtomeJanai.Shared.UI.Entity;
using OtomeJanai.Shared.UI.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OtomeJanai.Shared.UI.Controls
{
    /// <summary>
    /// Base class for every Element
    /// </summary>
    public abstract class UIElement
    {
        internal event EventHandler<PointerEventArgs> PointerEntered;
        internal event EventHandler<PointerEventArgs> PointerExited;
        internal event EventHandler<PointerEventArgs> PointerMoved;
        internal event EventHandler<PointerEventArgs> PointerPressed;
        internal event EventHandler<PointerEventArgs> PointerReleased;
        internal event EventHandler<PointerEventArgs> PointerWheelChanged;

        public abstract Type UIType { get; }
        public abstract Point Position { get; set; }

        public Thickness Padding { get; set; }
        public Thickness Margin { get; set; }
        public string Name { get; set; }
        public string ID { get; set; }
        public string Tag { get; set; }

        public IEnumerable<UIElement> ChildElement { get; internal set; }
        public bool HasChildElement => ChildElement != null && ChildElement.Any();

    }
    
}
