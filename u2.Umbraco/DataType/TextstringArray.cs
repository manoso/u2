using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace Cinema.Data.Cms.DataType
{
    /*
     * <TextstringArray>
     *   <values>
     *     <value>1</value>
     *     <value>0</value>
     *     <value>-1</value>
     *   </values>
     *   <values>
     *     <value>2</value>
     *     <value>0</value>
     *     <value>4</value>
     *   </values>
     * </TextstringArray>
     */

    [Serializable]
    [XmlRoot(ElementName = "TextstringArray")]
    public class TextstringArray
    {
        [XmlElement(ElementName = "values")]
        public List<TextstringArrayItem> Items { get; set; }

        [XmlIgnore]
        public IList<string[]> List
        {
            get { return Items.Select(x => x.Values).ToList(); }
        }

        [XmlIgnore]
        public bool IsEmpty
        {
            get { return Items == null || Items.Count <= 0; }
        }
    }

    [Serializable]
    public class TextstringArrayItem
    {
        [XmlElement(ElementName = "value")]
        public string[] Values { get; set; }
    }
}
