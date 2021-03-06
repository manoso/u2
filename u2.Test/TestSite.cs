﻿using System;
using System.Collections.Generic;
using u2.Core.Contract;

namespace u2.Test
{
    public class TestSite : ISite
    {
        public Guid Key { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<string> Hosts { get; set; }
        public string SiteName { get; set; }
    }
}
