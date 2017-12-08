using u2.Core;
using u2.Core.Contract;

namespace u2.Umbraco.DataType.Media
{
    public class Media : CmsModel, IMedia
    {
        public int UmbracoHeight { get; set; }
        public int UmbracoWidth { get; set; }
        public string UmbracoExtension { get; set; }
        public long UmbracoBytes { get; set; }
        public Image UmbracoFile { get; set; }
    }
}