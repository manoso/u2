using System.Collections.Generic;
using u2.Core;
using u2.Umbraco.DataType.Media;

namespace u2.Demo.Data
{
    public class ImageList : CmsModel
    {
        public string Title { get; set; }
        public string BackgroundColor { get; set; }
        public string Caption { get; set; }
        public string Label { get; set; }
        public string Description { get; set; }
        public IEnumerable<Media> Images { get; set; }

    }
}