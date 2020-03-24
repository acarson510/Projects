using Amazon;
using Amazon.SQS;
using Amazon.SQS.Model;
using System;
using System.Linq;

namespace Email_Consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            var sqs = new AmazonSQSClient("AKIA3TBFCIWD4POXP5H5", "+AG5qg+Pxp5cp+b9L/nbNpQkbOxVZbLKCfJnW6W0", RegionEndpoint.USEast2);

            var queueUrl = sqs.GetQueueUrlAsync("EmailQueue").Result.QueueUrl;

            var receiveMessageRequest = new ReceiveMessageRequest
            {
                QueueUrl = queueUrl
            };

            var receiveMessageResponse = sqs.ReceiveMessageAsync(receiveMessageRequest).Result;

            foreach (var message in receiveMessageResponse.Messages)
            {
                Console.WriteLine("Message \n");
                Console.WriteLine($"    MessageId: {message.MessageId} \n");
                Console.WriteLine($"    ReceiptHandle: {message.ReceiptHandle} \n");
                Console.WriteLine($"    MSD5Body: {message.MD5OfBody} \n");
                Console.WriteLine($"    Body: {message.Body} \n");
                
                foreach (var attribute in message.Attributes)
                {
                    Console.WriteLine("Attribute \n");
                    Console.WriteLine($"    Name: {attribute.Key} \n");
                    Console.WriteLine($"    Body: {attribute.Value} \n");
                }

                var messageReceptHandle = receiveMessageResponse.Messages.FirstOrDefault()?.ReceiptHandle;

                var deleteRequest = new DeleteMessageRequest
                {
                    QueueUrl = queueUrl,
                    ReceiptHandle = messageReceptHandle
                };

                sqs.DeleteMessageAsync(deleteRequest);

                Console.ReadLine();
            }
        }
    }
}
