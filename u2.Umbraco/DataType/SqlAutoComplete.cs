using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Cinema.Data.Cms.DataType
{
    /*
<SqlAutoComplete>
     <Item Text="Avengers Age Of Ultron" Value="16736" />
     <Item Text="Cinderella" Value="15499" />
     <Item Text="Maleficent" Value="10486" />
</SqlAutoComplete>
    */
    [Serializable]
    [XmlRoot(ElementName = "SqlAutoComplete")]
    public class SqlAutoComplete<T>
    {
        [XmlElement(ElementName = "Item")]
        public List<SqlAutoCompleteItem<T>> Items { get; set; }
    }

    [Serializable]
    public class SqlAutoCompleteItem<T>
    {
        [XmlAttribute(AttributeName = "Text")]
        public string Text { get; set; }
        
        [XmlAttribute(AttributeName = "Value")]
        public T Value { get; set; }
    }
}
