using Microsoft.Xna.Framework;
using OtomeJanai.Shared.UI.Common.Exception;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

using static System.Convert;

namespace OtomeJanai.Shared.UI.Entity
{
    public static class Colors
    {
        private static readonly Dictionary<string, Color> dictionary =
            typeof(Color).GetProperties(BindingFlags.Public | BindingFlags.Static)
                         .Where(prop => prop.PropertyType == typeof(Color))
                         .ToDictionary(prop => prop.Name, prop => (Color)prop.GetValue(null, null));

        public static Color FromName(string name)
        {
            if (dictionary.Keys.Contains(name))
                return dictionary[name];
            else
                throw new InvalidColorNameException($"Color does not contain {name}");
        }

        public static Color FromHex(string hexValue)
        {
            if (hexValue.Length != 7)
                return Color.Gray ;
            int a = ToInt32(hexValue.Substring(1, 2), 16);
            int r = ToInt32(hexValue.Substring(3, 2), 16);
            int g = ToInt32(hexValue.Substring(5, 2), 16);
            int b = ToInt32(hexValue.Substring(7, 2), 16);
            return new Color(r, g, b, a);
        }
    }
}
