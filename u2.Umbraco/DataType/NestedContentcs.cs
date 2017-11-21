namespace u2.Umbraco.DataType
{
    internal class NestedContent : JsonContent
    {
        public NestedContent(string json) : base(json)
        {
        }

        public override string Alias => Get<string>("nccontenttypealias");
    }
}