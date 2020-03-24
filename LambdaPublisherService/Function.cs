using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using System.Threading.Tasks;
using Newtonsoft.Json;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace LambdaPublisherService
{
    public class LearnShareLambdaController
    {
        //underlying sql tables created/updated
        //cache table populated
        //populate entity object class for created/updated and publish message (Basically just make sure I'm only getting the changed people and cache correct by time populated) to create/update topic 
        //lambda by system subscribes to topics and can filter -> maybe filter against in-memory entity mapping tables 

        //Subscribe to create (CreateEntity) topic
        //Subscribe to update (UpdateEntity) topic  

        //business logic and transmission layers applied in lambda

        //LMS 
        //Alayacare 
        //Tellus
        //Greenhouse
        //Bears
        //ODS
        //MasterData
        public void ReadWorkerMessageFromTopic()
        {

        }
        public void ProcessAlayaCareAssistiveCareEntity<SourceType, TargetType>(EntityObject eo, ILambdaContext context)
        {
            //Download visit (How are we going to poll AlayaCare? - Lambda with dto based on timed service - this can be handled in lambda filtering or timing 
            //should there be a "Read" topic which is feed through a polling object?
            var alayaCareVisits = GetEntityList<VisitDto>();

            foreach (var alayaCareVisit in alayaCareVisits)
            {
                var entityObject = EntityObjectGetEntityObject(alayaCareVisit);
                
                //publish entityObject to create/created/update/updated topic
                    //Bears/ODS will subscribe to this queue -> will have to be careful, should ODS subscribe to bears created only? or if comes from created, send back to created queue etc etc 
            }


            //upload clients (do we have office configuration) - maybe configuration object with propertyName and propertyValue 
            var alayaCareClientDto = ClientDtoGetClient(eo.Entity);
            //confirm/apply/process data dependencies
            //send to learnshare 
            //if success - handle response (created message), another topic, persist to any relevant mapping structure 
            //if fail - apply retry logic 

        }

        public void ProcessAlayaCareEvvEntity<SourceType, TargetType>(EntityObject eo, ILambdaContext context)
        {
            //UploadVisit, but only once confirmed the service has been created
            var acVisitDto = VisitDtoGetVisit(eo.Entity);
            //confirm/apply/process data dependencies ----make sure service has been uploaded
            //send to AC
            //if success - handle response (created message), another topic, persist to any relevant mapping structure 
            //if fail - apply retry logic 

            //DownloadVisit

        }

        public void ProcessLmsEntity(EntityObject eo, ILambdaContext context)
        {        
            //From entity object: EntityTypeName, EntityId, EntityInstance, Action, ProcessDate            
            if (eo.EntityTypeName == "Office")
            {
                //deserialize worker dto into LS dto - theoretically all attributes should be available in message, so removed step "Get remaining attributes            
                var lmsOfficeDto = OfficeDtoGetOffice(eo.Entity);
                //confirm/apply/process data dependencies
                //send to learnshare 
                //if success - handle response (created message), another topic, persist to any relevant mapping structure 
                //if fail - apply retry logic 
            }

            if (eo.EntityTypeName == "Worker")
            {
                //deserialize worker dto into LS dto - theoretically all attributes should be available in message, so removed step "Get remaining attributes            
                var lmsPersonDto = PersonDtoGetPerson(eo.Entity);
                //confirm/apply/process data dependencies   
                //send to learnshare 
                //if success - handle response (created message), another topic 
            }

            //CreateEntity
            //CreateEntityList
            //GetEntityById
            //GetEntityList
            //UpdateEntityById
            //UpdateEntityByList
            //DeleteEntityById
            //DeleteEntityByList

            var client = new AmazonSimpleNotificationServiceClient("AKIA3TBFCIWD4POXP5H5", "+AG5qg+Pxp5cp+b9L/nbNpQkbOxVZbLKCfJnW6W0");
            client.GetTopicAttributes("arn:aws:sns:us-east-2:796794176903:Create");
            CreateTopicRequest createTopicRequest = new CreateTopicRequest("Create");
            CreateTopicResponse createTopicResponse = client.CreateTopic(createTopicRequest);

            SendMessage(client, createTopicResponse.TopicArn).Wait();

            client.Subscribe(new SubscribeRequest
            {
                Endpoint = "johndoe@example.com", //This will be Lambda ARN once published to AWS
                Protocol = "email", //This will be "AWS Lambda"
                TopicArn = "arn:aws:sns:us-east-2:796794176903:Create"//createTopicResponse.TopicArn
            });

            DateTime latest = DateTime.Now + TimeSpan.FromMinutes(.5);

            while (DateTime.Now < latest)
            {
                var subsRequest = new ListSubscriptionsByTopicRequest
                {
                    TopicArn = createTopicResponse.TopicArn
                };

                var subs = client.ListSubscriptionsByTopic(subsRequest).Subscriptions;

                var sub = subs[0];

                if (!string.Equals(sub.SubscriptionArn,
                  "PendingConfirmation", StringComparison.Ordinal))
                {
                    break;
                }

                // Wait 15 seconds before trying again.
                System.Threading.Thread.Sleep(TimeSpan.FromSeconds(15));
            }
            //return input?.ToUpper();
        }
        public PersonDto PersonDtoGetPerson(string entityObject)
        {
            PersonDto person = new PersonDto();

            person = JsonConvert.DeserializeObject<PersonDto>(entityObject);

            return person;

        }
        public VisitDto VisitDtoGetVisit(string entityObject)
        {
            VisitDto visit = new VisitDto();

            visit = JsonConvert.DeserializeObject<VisitDto>(entityObject);

            return visit;

        }

        public ClientDto ClientDtoGetClient(string entityObject)
        {
            ClientDto client = new ClientDto();

            client = JsonConvert.DeserializeObject<ClientDto>(entityObject);

            return client;

        }

        public OfficeDto OfficeDtoGetOffice(string entityObject)
        {
            OfficeDto office = new OfficeDto();           

            office = JsonConvert.DeserializeObject<OfficeDto>(entityObject);

            return office;

        }
        public EntityObject EntityObjectGetEntityObject(VisitDto visit)
        {
            EntityObject eo = new EntityObject();

            eo.EntityTypeName = "Visit";
            eo.Entity = JsonConvert.SerializeObject(visit);
            eo.ProcessDate = DateTime.Now;

            return eo;
        }

        public List<T> GetEntityList<T>()
        {
            List<T> entityList = new List<T>();

            return entityList;
        }

        static async Task SendMessage(IAmazonSimpleNotificationService snsClient, string topicArn)
        {
            WorkerEo w = new WorkerEo();
            w.FirstName = "Andrew";
            w.LastName = "Carson";
            w.Email = "test@test.com";

            var request = new PublishRequest
            {
                TopicArn = topicArn,
                Message = JsonConvert.SerializeObject(w)
            };

            await snsClient.PublishAsync(request);
        }
    }
    public class EntityObject
    {
        public string EntityTypeName { get; set; }
        public int? EntityId { get; set; }
        public string Entity { get; set; } 
        public DateTime ProcessDate { get; set; }
    }
    public class OfficeEo
    {
        public int OfficeId { get; set; }
        public string OfficeName { get; set; }        
    }
    public class OfficeDto
    {
        public int OfficeId { get; set; }
        public string OfficeName { get; set; }
    }
    public class VisitDto
    {
        public int VisitId { get; set; }
        public string VisitName { get; set; }
    }
    public class VisitEo
    {
        public int VisitId { get; set; }
        public string VisitName { get; set; }
    }
    public class WorkerEo 
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }

    public class DomainDto
    {
        public int DomainId { get; set; }
        public string DomainName { get; set; }
        public string DomainDescription { get; set; }
    }
    public class PersonDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }
    public class ClientDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }
}
