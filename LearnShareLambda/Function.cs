using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Amazon;
using Amazon.Lambda.Core;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Amazon.S3.Util;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace LearnShareLambda
{
    class UrlRequestMessage
    {
        public string[] Urls { get; set; }
    }
    public class Function
    {
        public string FunctionHandler(string input, ILambdaContext context)
        {
            
            return input.ToUpper();
        }

        public void SendJobCompleteEmail(ILambdaContext context)
        {
            
            // Replace USWest2 with the AWS Region you're using for Amazon SES.
            // Acceptable values are EUWest1, USEast1, and USWest2.
            using (var client = new AmazonSimpleEmailServiceClient(RegionEndpoint.USEast1))
            {
                var sendRequest = new SendEmailRequest
                {
                    Source = "acarson510@gmail.com",
                    Destination = new Destination
                    {
                        ToAddresses =
                        new List<string> { "acarson510@gmail.com" }
                    },
                    Message = new Message
                    {
                        Subject = new Content("Subject"),
                        Body = new Body
                        {
                            Html = new Content
                            {
                                Charset = "UTF-8",
                                Data = htmlBody
                            },
                            Text = new Content
                            {
                                Charset = "UTF-8",
                                Data = textBody
                            }
                        }
                    },
                    // If you are not using a configuration set, comment
                    // or remove the following line 
                    //ConfigurationSetName = configSet
                };
                try
                {                    
                    var response = client.SendEmail(sendRequest);                    
                }
                catch (Exception ex)
                {
                    context.Logger.LogLine(ex.Message);
                    //Console.WriteLine("The email was not sent.");
                    //Console.WriteLine("Error message: " + ex.Message);

                }
            }            
        }

        static async Task SendMessage(IAmazonSimpleNotificationService snsClient, string topicArn)
        {
            //WorkerEo w = new WorkerEo();
            //w.FirstName = "Andrew";
            //w.LastName = "Carson";
            //w.Email = "test@test.com";

            var request = new PublishRequest
            {
                TopicArn = topicArn,
                Message = "test"//JsonConvert.SerializeObject(w)
            };

            await snsClient.PublishAsync(request);
        }




        public static async Task<bool> PutS3Object(string bucket, string key, string content)
        {
            try
            {
                using (var client = new AmazonS3Client(Amazon.RegionEndpoint.USEast1))
                {
                    var request = new PutObjectRequest
                    {
                        BucketName = bucket,
                        Key = key,
                        ContentBody = content
                    };
                    var response = await client.PutObjectAsync(request);
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception in PutS3Object:" + ex.Message);
                return false;
            }
        }
        //static async Task CreateBucketAsync()
        //{
        //    try
        //    {
        //        if (!(await AmazonS3Util.DoesS3BucketExistAsync(s3Client, bucketName)))
        //        {
        //            var putBucketRequest = new PutBucketRequest
        //            {
        //                BucketName = bucketName,
        //                UseClientRegion = true
        //            };

        //            PutBucketResponse putBucketResponse = await s3Client.PutBucketAsync(putBucketRequest);
        //        }
        //        // Retrieve the bucket location.
        //        string bucketLocation = await FindBucketLocationAsync(s3Client);
        //    }
        //    catch (AmazonS3Exception e)
        //    {
        //        Console.WriteLine("Error encountered on server. Message:'{0}' when writing an object", e.Message);
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine("Unknown encountered on server. Message:'{0}' when writing an object", e.Message);
        //    }
        //}
        //static async Task<string> FindBucketLocationAsync(IAmazonS3 client)
        //{
        //    string bucketLocation;
        //    var request = new GetBucketLocationRequest()
        //    {
        //        BucketName = bucketName
        //    };
        //    GetBucketLocationResponse response = await client.GetBucketLocationAsync(request);
        //    bucketLocation = response.Location.ToString();
        //    return bucketLocation;
        //}

        public static void SendNotificationEmail(string messageBody)
        {
            try
            {
                StringBuilder messages = new StringBuilder();

                MailMessage msg = new MailMessage();
                msg.From = new MailAddress("dataintegration@bayada.com");


                List<string> recipients = new List<string> { "acarson@bayada.com" };
                foreach (var recipient in recipients)
                {
                    msg.To.Add(new MailAddress(recipient));
                }

                msg.Subject = "Test";
                msg.Priority = MailPriority.Normal;
                msg.Body = messages.AppendLine(messageBody).ToString();
                msg.IsBodyHtml = true;

                SmtpClient client = new SmtpClient();
                client.Host = "smtp.devtest.bayada.com";
                client.Port = 25;
                client.UseDefaultCredentials = true;

                client.Send(msg);
            }
            catch (Exception x)
            {
                //LogHelper.LogError("LearnShareEmailController.SendNotificationEmail", x);
            }
        }
    }
}
