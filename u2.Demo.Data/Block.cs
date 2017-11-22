using System.Collections.Generic;
using u2.Umbraco.DataType;

namespace u2.Demo.Data
{
    public class Block : CmsModel
    {
        public BlockSize Size { get; set; }
        public string Description { get; set; }

        public IEnumerable<Label> Labels { get; set; }
        public IEnumerable<Media> Images { get; set; }

        public IEnumerable<ImageList> ImageList { get; set; }
    }
}