namespace u2.Umbraco.DataType
{
    internal class Archetype : JsonContent
    {
        public Archetype(string json) : base(json)
        {
            //_fields = model.Properties.ToDictionary(x => x.Alias.ToLowerInvariant(), x => x.Value.ToString());
        }

        public override string Alias => string.Empty;
    }
}
