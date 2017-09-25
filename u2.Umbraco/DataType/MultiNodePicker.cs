using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Cinema.Data.Cms.DataType
{
    /*
<MultiNodePicker type="content">
  <nodeId>16779</nodeId>
  <nodeId>16780</nodeId>
</MultiNodePicker>
    */

    [Serializable]
    [XmlRoot(ElementName = "MultiNodePicker")]
    public class MultiNodePicker<T>
    {
        [XmlAttribute(AttributeName = "type")]
        public string Type { get; set; }

        [XmlElement(ElementName = "nodeId")]
        public List<T> NodeIds { get; set; } 
    }
}
