using ServerlessSleepless.Awaker.Common.Models;

namespace ServerlessSleepless.Awaker.SQSLoop.Models
{
    public class AwsSqsOption : ConfigBase
    {
        public int MaxNumberOfMessages { get; set; }

        public string Region { get; set; }

        public string ServiceUrl { get; set; }

        public string QueueUrl { get; set; }
    }
}
