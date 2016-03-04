using OtomeJanai.Shared.UI.Common.Exception;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using static System.Convert;

namespace OtomeJanai.Shared.UI.Entity
{
    public struct Thickness
    {
        public Thickness(string value)
        {
            var values = value.Split(',');
            switch (values.Count())
            {
                case 1:
                    Up = Down = Left = Right = ToInt32(values[0]);
                    break;
                case 2:
                    Up = Down = ToInt32(values[0]);
                    Left = Right = ToInt32(values[1]);
                    break;
                case 3:
                    Up = ToInt32(values[0]);
                    Right = Left = ToInt32(values[1]);
                    Down = ToInt32(values[2]);
                    break;
                case 4:
                    Up = ToInt32(values[0]);
                    Right = ToInt32(values[1]);
                    Down = ToInt32(values[2]);
                    Left = ToInt32(values[3]);
                    break;
                default:
                    throw new InvalidPropertyValueException($"Invalid Property Value at {value}");
            }
            _value = value;
        }
        public int Up { get; set; }
        public int Down { get; set; }
        public int Left { get; set; }
        public int Right { get; set; }
        private string _value;

        public static bool operator ==(Thickness a, Thickness b)
            =>a.Left == b.Left && a.Right == b.Right && a.Up == b.Up && a.Down == b.Down;

        public static bool operator !=(Thickness a, Thickness b)
            => a.Left != b.Left || a.Right != b.Right || a.Up != b.Up || a.Down != b.Down;

        public override bool Equals(object obj)
            => obj is Thickness && ((Thickness)obj) == this;

        public override int GetHashCode() => base.GetHashCode();

        public override string ToString() => _value;

    }
}
