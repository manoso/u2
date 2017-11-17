using System;
using u2.Core.Contract;

namespace u2.Test
{
    public class TestMedia : IMedia
    {
        public Guid Key { get; set; }
        public int Id { get; set; }
    }
}
