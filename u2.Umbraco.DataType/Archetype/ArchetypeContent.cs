using System.Linq;

namespace u2.Umbraco.DataType.Archetype
{
    /// <summary>
    /// Archetype content, similar to Umbraco content, can be mapped to model types.
    /// </summary>
    public class ArchetypeContent : BaseContent
    {
        public ArchetypeContent(FieldSet fieldSet)
        {
            Fields = fieldSet.Properties.ToDictionary(x => x.Alias.ToLowerInvariant(), x => x.Value.ToString());
        }

        public override string Alias => string.Empty;
    }
}
