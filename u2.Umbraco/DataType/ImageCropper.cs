using System.Collections.Generic;


namespace u2.Umbraco.DataType
{
    /// <summary>
    /// Umbraco ImageCropper type.
    /// </summary>
    public class ImageCropper
    {
        /// <summary>
        /// Get or set crop url keys and values.
        /// </summary>
        public IDictionary<string, string> CropUrls { get; set; }
    }
}
