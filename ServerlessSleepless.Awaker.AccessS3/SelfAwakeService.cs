using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Configuration;
using ServerlessSleepless.Awaker.Abstraction;
using ServerlessSleepless.Awaker.Common;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ServerlessSleepless.Awaker.AccessS3
{
    public class SelfAwakeService : SelfAwakerServiceBase, IPleaseAwakeMyselfService
    {
        public SelfAwakeService(IConfiguration configuration, ISerializer serializer)
            : base(configuration, serializer)
        {
            
        }

        public override async Task TryAwake(params object[] arg)
        {
            var listResponse = await S3Client.ListObjectsV2Async(new ListObjectsV2Request
            {
                BucketName = BucketName
            });
        }

        public override void InitializeIfEnabled()
        {
            if (!Enabled) return;

            BucketName = Configuration.GetValue<string>($"{this.GetType().FullName}:BucketName");

            if (string.IsNullOrWhiteSpace(BucketName))
            {
                throw new ArgumentNullException(nameof(Configuration) + $":{this.GetType().FullName}:BucketName");
            }

            S3Client = new AmazonS3Client();
        }


        public IAmazonS3 S3Client { get; private set; }
        public string BucketName { get; private set; }
    }
}
