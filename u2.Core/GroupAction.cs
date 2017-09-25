using System;
using System.Collections.Generic;

namespace u2.Core
{
    public class GroupAction
    {
        public IList<string> Aliases { get; set; }
        public Action<object, IList<object>> Action { get; set; }
    }
}