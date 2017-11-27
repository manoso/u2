﻿
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using u2.Demo.Data;

namespace u2.Demo.Provider
{
    public class DataProvider : IDataProvider
    {
        public async Task<IEnumerable<Label>> GetLabels()
        {
            return await Task.Run(() =>
                new[]
                {
                    new Label {Text = "label 1", Key = Guid.NewGuid()},
                    new Label {Text = "label 2", Key = Guid.NewGuid()}
                }).ConfigureAwait(false);
        }
    }
}