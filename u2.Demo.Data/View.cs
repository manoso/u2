
using System.Collections.Generic;

namespace u2.Demo.Data
{
    public class View : CmsModel
    {
        public string Title { get; set; }
        public IEnumerable<Block> Blocks { get; set; }
    }

}
