using System.Collections.Generic;

namespace u2.Demo.Data
{
    public class Block : CmsModel
    {
        public BlockSize Size { get; set; }
        public string Description { get; set; }

        public IEnumerable<Label> Labels { get; set; }
    }
}