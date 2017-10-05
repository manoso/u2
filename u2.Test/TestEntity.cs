using System.Collections.Generic;
using u2.Core;

namespace u2.Test
{
    public class TestEntity : CmsKey
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IList<string> Infos { get; set; }
        public IEnumerable<TestItem> Items { get; set; }
    }
}