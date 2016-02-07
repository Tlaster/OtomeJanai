using OtomeJanai.Shared.Extension;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace OtomeJanai.Shared.Common.BmFont.FontModel
{
    public class FontPage
    {
        [XmlAttribute("id")]
        public int ID { get; set; }

        [XmlAttribute("file")]
        public string File { get; set; }
    }
}
