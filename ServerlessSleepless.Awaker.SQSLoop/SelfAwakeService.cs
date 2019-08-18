using Amazon.SQS;
using Amazon.SQS.Model;
using Microsoft.Extensions.Configuration;
using ServerlessSleepless.Awaker.Abstraction;
using ServerlessSleepless.Awaker.Common;
using ServerlessSleepless.Awaker.SQSLoop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerlessSleepless.Awaker.SQSLoop
{
    public class SelfAwakeService : SelfAwakerServiceBase, IPleaseAwakeMyselfService
    {
        public SelfAwakeService(IConfiguration configuration, ISerializer serializer)
            : base(configuration, serializer)
        {

        }


        public override async Task TryAwake(params object[] arg)
        {
            await ConsumeMessage();

            await PublishMessage();
        }

        public override void InitializeIfEnabled()
        {
            if (!Enabled) return;

            var option = new AwsSqsOption();
            Configuration.Bind($"{this.GetType().FullName}", option);

            Option = option ?? throw new ArgumentNullException(nameof(Configuration) + $":{this.GetType().FullName}");

            var amazonSQSConfig = new AmazonSQSConfig();
            amazonSQSConfig.ServiceURL = Option.ServiceUrl;
            SQSClient = new AmazonSQSClient(amazonSQSConfig);
        }

        async Task<ReceiveMessageResponse> ConsumeMessage()
        {

            var message = await SQSClient.ReceiveMessageAsync(new ReceiveMessageRequest()
            {
                QueueUrl = Option.QueueUrl,
                MaxNumberOfMessages = Option.MaxNumberOfMessages
            });

            if (message?.Messages?.Count() > 0)
            {
                // Keep consuming next message on next OnTimer
                LastConsumeMessage = message.Messages.Last();
                var batch = message.Messages.ConvertAll(o => new DeleteMessageBatchRequestEntry()
                {
                    Id = o.MessageId,
                    ReceiptHandle = o.ReceiptHandle
                });

                var deleteResult = await SQSClient.DeleteMessageBatchAsync(Option.QueueUrl, batch);
                if (deleteResult.Failed?.Count() > 0)
                {
                    throw new Exception("Delete from SQS failed. ");
                }
            }

            return message;
        }

        async Task<SendMessageResponse> PublishMessage()
        {
            var message = ExecutedCount.ToString();
            var result = await SQSClient.SendMessageAsync(new SendMessageRequest()
            {
                QueueUrl = Option.QueueUrl,
                MessageBody = message,
                MessageAttributes = new Dictionary<string, MessageAttributeValue>()
                {
                    { "generatedTs", new MessageAttributeValue(){DataType = "String", StringValue = GeneratedTs.ToString("s")} },
                    { "generatedId", new MessageAttributeValue(){DataType = "String", StringValue = GeneratedId.ToString()} }
                }
            });

            return result;
        }

        public AwsSqsOption Option { get; private set; }
        public IAmazonSQS SQSClient { get; private set; }
        public Message LastConsumeMessage { get; private set; }

    }
}
