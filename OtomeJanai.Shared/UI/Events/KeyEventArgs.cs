using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace OtomeJanai.Shared.UI.Events
{
    internal class KeyEventArgs : EventArgs
    {
        internal Keys Key { get; }

        internal KeyEventArgs(Keys key)
        {
            Key = key;
        }
    }
}
