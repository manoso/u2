using System;
using u2.Core.Contract;

namespace u2.Demo.Data
{
    public class CmsModel : ICmsModel<int>
    {
        public int Id { get; set; }
        public Guid Key { get; set; }
    }
}
