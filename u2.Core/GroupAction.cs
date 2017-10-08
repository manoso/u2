using System;
using System.Collections.Generic;
using u2.Core.Contract;

namespace u2.Core
{
    public class GroupAction : IGroupAction
    {
        public IList<string> Aliases { get; set; }
        public Action<object, IList<object>> Action { get; set; }
    }
}