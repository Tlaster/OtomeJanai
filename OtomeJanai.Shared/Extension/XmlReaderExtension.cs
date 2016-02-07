using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace OtomeJanai.Shared.Extension
{
    public static class XmlReaderExtension
    {
        public static int GetAttributeValueAsInt(this XmlReader reader, string name) => Convert.ToInt32(reader.GetAttribute(name));
    }
}
