﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace u2.Core.Contract
{
    public interface ICmsFetcher
    {
        IEnumerable<IContent> Fetch(ICmsQuery cmsQuery);
    }
}
