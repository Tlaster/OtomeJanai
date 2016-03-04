using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Xml.Linq;
using OtomeJanai.Shared.UI.Common.Exception;
using OtomeJanai.Shared.UI.Controls;
using OtomeJanai.Shared.UI.Entity;
using Microsoft.Xna.Framework;
using System.Reflection;

using static System.Convert;

namespace OtomeJanai.Shared.UI.Common
{
    internal class Generator
    {

        private UIElement Creator(string typeName)
        {
            var type = Type.GetType($"OtomeJanai.Shared.UI.Controls.{typeName}, {typeof(Generator).GetTypeInfo().Assembly.FullName}");
            if (type != null)
                return Expression.Lambda<Func<UIElement>>(Expression.New(type.GetConstructor(Type.EmptyTypes))).Compile()();
            else
                throw new InvalidElementException($"Invalid element {typeName}");
        }

        internal static Generator Instance { get; } = new Generator();

        private UIElement _treeHolder;

        internal void GenerateTree(string data)
        {
            var doc = XDocument.Parse(data);
            _treeHolder = GenerateElement(doc.Root);
        }

        private UIElement GenerateElement(XElement item)
        {
            UIElement element = Creator(item.Name.LocalName);

            foreach (var attribute in item.Attributes())
            {
                var prop = element.GetType().GetProperty(attribute.Name.LocalName);
                if (prop == null)
                    throw new InvalidPropertyException($"Invalid property {attribute.Name.LocalName} at {item.Name.LocalName}");
                object value;
                switch (prop.PropertyType.Name)
                {
                    case nameof(String):
                        value = Convert.ToString(attribute.Value);
                        break;
                    case nameof(Double):
                        value = ToDouble(attribute.Value);
                        break;
                    case nameof(Int32):
                        value = ToInt32(attribute.Value);
                        break;
                    case nameof(Boolean):
                        value = ToBoolean(attribute.Value);
                        break;
                    case nameof(Thickness):
                        value = new Thickness(attribute.Value);
                        break;
                    case nameof(Color):
                        try
                        {
                            value = Colors.FromName(attribute.Value);
                        }
                        catch (InvalidColorNameException)
                        {
                            value = Colors.FromHex(attribute.Value);
                        }
                        break;
                    default:
                        throw new UnsupportedPropertyTypeException($"Dose not support {prop.PropertyType.Name}");
                }
                prop.SetValue(element, value);
            }

            if (!item.HasElements)
                return element;

            element.ChildElement = new List<UIElement>();
            var childElement = item.Elements() as IList<XElement> ?? item.Elements().ToList();
            for (int i = 0; i < childElement.Count(); i++)
                ((List<UIElement>)element.ChildElement).Add(GenerateElement(childElement.ElementAt(i)));
            return element;
        }
        

    }
}
