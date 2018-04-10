using u2.Core;
using u2.Core.Contract;

namespace u2.Umbraco.DataType.Media
{
    /// <summary>
    /// Umbraco media type
    /// </summary>
    public class Media : CmsModel, IMedia
    {
        /// <summary>
        /// Get or set height.
        /// </summary>
        public int UmbracoHeight { get; set; }

        /// <summary>
        /// Get or set width.
        /// </summary>
        public int UmbracoWidth { get; set; }

        /// <summary>
        /// Get or set media file extension.
        /// </summary>
        public string UmbracoExtension { get; set; }

        /// <summary>
        /// Get or set media size in bytes.
        /// </summary>
        public long UmbracoBytes { get; set; }

        /// <summary>
        /// Get and set media image.
        /// </summary>
        public Image UmbracoFile { get; set; }
    }
}