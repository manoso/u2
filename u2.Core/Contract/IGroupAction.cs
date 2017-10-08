using System;
using System.Collections.Generic;

namespace u2.Core.Contract
{
    public interface IGroupAction
    {
        Action<object, IList<object>> Action { get; set; }
        IList<string> Aliases { get; set; }
    }
}