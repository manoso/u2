using System;
using System.Threading.Tasks;

namespace u2.Core
{
    public class TaskInfo
    {
        public Task<bool> Task { get; set; }
        public DateTime Timestamp { get; set; }
        public Action<string, object> Save { get; set; }
    }
}