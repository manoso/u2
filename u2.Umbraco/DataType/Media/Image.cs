namespace u2.Umbraco.DataType.Media
{
    /// <summary>
    /// Umbraco media image.
    /// </summary>
    public class Image
    { 
        /// <summary>
        /// Get or set image focal point.
        /// </summary>
        public FocalPoint FocalPoint { get; set; }

        /// <summary>
        /// Get or set image url.
        /// </summary>
        public string Src { get; set; }
    }
}