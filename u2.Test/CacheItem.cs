using System;

namespace u2.Test
{
    public class CacheItem
    {
        public Guid Id { get; } = Guid.NewGuid();
        public int LookupKey { get; set; }
        public string LookupKeyOther { get; set; }
    }
}