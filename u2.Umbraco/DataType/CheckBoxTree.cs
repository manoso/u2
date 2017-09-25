using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Cinema.Data.Cms.DataType
{
    /*
<CheckBoxTree>
  <nodeId>1518</nodeId>
  <nodeId>1707</nodeId>
</CheckBoxTree>
     */
    [Serializable]
    [XmlRoot(ElementName = "CheckBoxTree")]
    public class CheckBoxTree<T>
    {
        [XmlElement(ElementName = "nodeId")]
        public List<T> NodeIds { get; set; } 
    }
}