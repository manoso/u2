﻿using System.Collections.Generic;

namespace u2.Core.Contract
{
    public interface ITaskDefer
    {
        IList<IMapItem> Maps { get; }
    }
}