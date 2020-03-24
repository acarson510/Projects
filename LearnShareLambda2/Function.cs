using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon;
using Amazon.Lambda.Core;
using Amazon.Lambda.SNSEvents;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace LearnShareLambda2
{
    public class Function
    {
        static readonly string htmlBody = @"<html>
            <head></head>
            <body>
              <h1>Test</h1>
            </body>
            </html>";
        // The email body for recipients with non-HTML email clients.
        static readonly string textBody = "Amazon SES Test (.NET)\r\n"
                                        + "This email was sent through Amazon SES "
                                        + "using the AWS SDK for .NET.";
        public Function()
        {

        }

        public async Task FunctionHandler(SNSEvent evnt, ILambdaContext context)
        {
            
            foreach(var record in evnt.Records)
            {
                await ProcessRecordAsync(record, context);
            }            
        }

        private async Task ProcessRecordAsync(SNSEvent.SNSRecord record, ILambdaContext context)
        {
            context.Logger.LogLine($"Processed record {record.Sns.Message}    test test test");

            await Task.CompletedTask;
        }

        
    }
}
