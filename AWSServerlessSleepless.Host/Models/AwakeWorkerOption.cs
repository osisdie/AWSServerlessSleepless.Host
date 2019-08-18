using ServerlessSleepless.Awaker.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerlessSleepless.Host.Models
{
    public class AwakeWorkerOption : ConfigBase
    {
        public int DelayStartInSecs { get; set; }

        public int PeriodInSecs { get; set; }
    }
}
