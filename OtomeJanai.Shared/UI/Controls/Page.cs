using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace OtomeJanai.Shared.UI.Controls
{
    public class Page : UIElement
    {
        public override Point Position { get; set; } = new Point(0, 0);
        public Color BackGround { get; set; }
        public override Type UIType => typeof(Page);
    }
}
