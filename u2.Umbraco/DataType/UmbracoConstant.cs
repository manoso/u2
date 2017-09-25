namespace Cinema.Data.Cms.DataType
{
    public static class UmbracoConstant
    {
        public const string XPathDescendantFormatId = @"descendant::* [@id='{0}']/";
        public const string XPathDescendantFormatName = @"descendant::* [@nodeName='{0}']/";
        public const string XPathDescendantFormatIdNoSlash = @"descendant::* [@id='{0}']";
        public const string XPathDescendantFormatNameNoSlash = @"descendant::* [@nodeName='{0}']";
        public const string XmlFormat = "<?xml version=\"1.0\" ?>{0}";

        public static char[] CsvSplitter = {','};
    }
}
