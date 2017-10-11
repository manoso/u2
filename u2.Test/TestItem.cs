using System.Collections.Generic;

namespace u2.Test
{
    public class TestItem : Model
    {
        public int ItemId { get; set; }
        public double Price { get; set; }
        public bool OnSale { get; set; }
        public IEnumerable<TestInfo> Infos { get; set; }
    }

    public class TestInfo : Model
    {
        public int InfoId { get; set; }
        public string Info { get; set; }
    }
}