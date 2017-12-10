using System;
using u2.Core.Contract;

namespace u2.Core
{
    public class TaskInfo : ITaskInfo
    {
        public DateTime Timestamp { get; set; }
        public Action<string, object> Save { get; set; }
    }
}