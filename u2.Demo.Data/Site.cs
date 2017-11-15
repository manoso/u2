﻿using System.Collections.Generic;
using u2.Core.Contract;

namespace u2.Demo.Data
{
    public class Site : CmsModel, IRoot
    {
        public string CountryCode { get; set; }
        public string CurrencyCode { get; set; }

        public IList<string> Hosts { get; set; }
    }
}