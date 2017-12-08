using System;

namespace u2.Core
{
    public class TaskInfo
    {
        public DateTime Timestamp { get; set; }
        public Action<string, object> Save { get; set; }
    }
}