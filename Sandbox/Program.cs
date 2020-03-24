using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using Amazon;
using Amazon.SQS;
using Amazon.SQS.Model;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using System.Threading.Tasks;

namespace Sandbox
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new AmazonSimpleNotificationServiceClient("AKIATTCTLFKP4XWNQLQK", "/ZKFCAvdBxkFPD0t9zovaXB1qhwQ1mSfd46+kmXG", Amazon.RegionEndpoint.USEast1);

            for(var i = 1000; i < 1050; i++)
            {
                var request = new PublishRequest
                {
                    TopicArn = "arn:aws:sns:us-east-1:247135414943:MasterDataWorkerChanged",
                    Message = i.ToString()
                };

                client.Publish(request);
            }
           
        }
    }
}
