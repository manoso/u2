using System.Linq;

namespace u2.Umbraco.DataType.Archetype
{
    public class ArchetypeContent : JsonContent
    {
        public ArchetypeContent(FieldSet fieldSet)
        {
            Fields = fieldSet.Properties.ToDictionary(x => x.Alias.ToLowerInvariant(), x => x.Value.ToString());
        }

        public override string Alias => string.Empty;
    }
}
