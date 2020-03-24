using Amazon;
using Amazon.SQS;
using Amazon.SQS.Model;
using Bayada.Common.DataAccess;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Email_Producer
{
    class Program
    {
        static void Main(string[] args)
        {
            //Bayada.Entity.Master.Worker -> call save method 

            //DIA similar - I'm out of office 

            //Console.WriteLine("*********************");
            //Console.WriteLine("Amazon SQS");
            //Console.WriteLine("*********************");

            //IAmazonSQS sqs = new AmazonSQSClient("AKIA3TBFCIWD4POXP5H5", "+AG5qg+Pxp5cp+b9L/nbNpQkbOxVZbLKCfJnW6W0", RegionEndpoint.USEast2);

            //Console.WriteLine("Create a queue called EmailQueue.\n");

            //var sqsRequest = new CreateQueueRequest
            //{
            //    QueueName = "EmailQueue"
            //};

            //var createQueueResponse = sqs.CreateQueueAsync(sqsRequest).Result;

            //var myQueueUrl = createQueueResponse.QueueUrl;

            //var listQueueRequest = new ListQueuesRequest();
            //var listQueueResponse = sqs.ListQueuesAsync(listQueueRequest);

            //Console.WriteLine("List of Amazon SQS queues.\n");
            //foreach(var queueUrl in listQueueResponse.Result.QueueUrls)
            //{
            //    Console.WriteLine($"QueueUrl: {queueUrl}");
            //}

            //ShoppingList a = new ShoppingList();
            //a.Name = "Bread";
            //a.Quantity = 3;

            //Console.WriteLine("Sending a message to our EmailQueue.\n");
            //var sqsMessageRequest = new SendMessageRequest
            //{
            //    QueueUrl = myQueueUrl,
            //    MessageBody = JsonConvert.SerializeObject(a)
            //};

            //sqs.SendMessageAsync(sqsMessageRequest);

            //Console.WriteLine("Finished sending message to our SQS queue. \n");

            //Console.ReadLine();
            var a = MapEmployee(GetEmployee());

        }
        public static DataTable GetEmployee()
        {
            var connectionString = "Data Source=dia-dev-mssql-01.ccfwqtcduhbn.us-east-1.rds.amazonaws.com;Initial Catalog=MasterData;integrated security=false;persist security info=false;User Id=acarson;Password=andrew;";

            return SqlHelper.GetDataTable(connectionString, "exec awsGetMockEmployee");
        }

        public static IEnumerable<Employee> MapEmployee(DataTable data)
        {
            List<Employee> employees = new List<Employee>();

            for (var i = 0; i < data.Rows.Count; i++)
            {
                var row = data.Rows[i];

                var e = new Employee()
                {
                    FirstName = row row["FirstName"],
                    LastName = row["LastName"],
                    Email = row["Email"]
                };

                employees.Add(e);
            }

            //return employees.FirstOrDefault();

            List<Employee> domains = data.AsEnumerable().Select(r =>
            {
                return new Employee()
                {
                    FirstName = r.Field<string>("DomainID"),
                    LastName = r.Field<string>("DomainDescription"),
                    Email = r.Field<string>("Active")

                };
            }).ToList();

            return domains;
        }

    }

    public class Employee
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }
   

    public class ShoppingList
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
    }
}
