using System;
using System.Collections.Generic;
using System.Text;
using Amazon;
using Amazon.SQS;
using Amazon.SQS.Model;
using ApplicantTrackingSystem.dto;
using Newtonsoft.Json;

namespace ApplicantTrackingSystem.controller
{
    public class ApplicantTrackingSystemController 
    {
        public void PlaceApplicantOnQueue(Person p)
        {
            IAmazonSQS sqs = new AmazonSQSClient("AKIA3TBFCIWD4POXP5H5", "+AG5qg+Pxp5cp+b9L/nbNpQkbOxVZbLKCfJnW6W0", RegionEndpoint.USEast2);

            var sqsRequest = new CreateQueueRequest
            {
                QueueName = "ApplicantTrackingSystemQueue"
            };

            var createQueueResponse = sqs.CreateQueueAsync(sqsRequest).Result;

            var myQueueUrl = createQueueResponse.QueueUrl;

            var listQueueRequest = new ListQueuesRequest();
            var listQueueResponse = sqs.ListQueuesAsync(listQueueRequest);

            var sqsMessageRequest = new SendMessageRequest
            {
                QueueUrl = myQueueUrl,
                MessageBody = JsonConvert.SerializeObject(p)
            };

            sqs.SendMessageAsync(sqsMessageRequest);

            Console.WriteLine("Finished sending message to our SQS queue. \n");

            Console.ReadLine();
        }
    }
}
