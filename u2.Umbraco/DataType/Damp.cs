using System;
using System.Xml.Serialization;

namespace Cinema.Data.Cms.DataType
{
    /*
<DAMP fullMedia="">
  <mediaItem>
    <Image id="19703" version="a900b5e5-43ce-4fb3-9642-cae27e7456bb" parentID="17938" level="6" writerID="31" nodeType="1032" template="0" sortOrder="3" createDate="2015-05-14T17:38:13" updateDate="2015-05-14T17:38:13" nodeName="JW Advance Onlinetile 327X137px" urlName="jwadvanceonlinetile327x137px" writerName="jrodriguez" nodeTypeAlias="Image" path="-1,1056,1349,1353,6678,17938,19703">
      <umbracoFile>/media/7421549/JW_Advance_OnlineTile_327x137px.jpg</umbracoFile>
      <umbracoWidth>327</umbracoWidth>
      <umbracoHeight>137</umbracoHeight>
      <umbracoBytes>47437</umbracoBytes>
      <umbracoExtension>jpg</umbracoExtension>
    </Image>
  </mediaItem>
</DAMP>
     */

    [Serializable]
    [XmlRoot(ElementName = "DAMP")]
    public class Damp
    {
        [XmlElement(ElementName = "mediaItem")]
        public MediaItem MediaItem { get; set; }
    }

    [Serializable]
    public class MediaItem
    {
        [XmlElement(ElementName = "Image")]
        public DampImage Image { get; set; }
    }

    public class DampImage
    {
        [XmlElement(ElementName = "umbracoFile")]
        public string UmbracoFile { get; set; }

        [XmlElement(ElementName = "umbracoWidth")]
        public int UmbracoWidth { get; set; }

        [XmlElement(ElementName = "umbracoHeight")]
        public int UmbracoHeight { get; set; }

        [XmlElement(ElementName = "umbracoBytes")]
        public int UmbracoBytes { get; set; }

        [XmlElement(ElementName = "umbracoExtension")]
        public string UmbracoExtension { get; set; }

    }
}
