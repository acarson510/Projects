using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

using Amazon.Lambda.Core;
using Amazon.Lambda.SNSEvents;
using LearnShareProcessor.dto;
using Newtonsoft.Json;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace LearnShareProcessor
{
    public class Function
    {
        public static string _accessToken;
        public static object _locker = new object();

        const string _httpPost = "POST";
        const string _httpPut = "PUT";
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
            context.Logger.LogLine($"Processed record {record.Sns.Message}");

            GetAccessToken(context);

            UploadEntity(GetPerson(), context);            

            DestroyAccessToken(context);

            await Task.CompletedTask;
        }
        public static void UploadEntity(Person p, ILambdaContext context)
        {
            context.Logger.LogLine("Uploading person");

            PerformHttpRequestResponse(context, GetHttpWebRequest("PUT", "https://sandbox2api.learnshare.com/People(4244087)"), JsonConvert.SerializeObject(p));

            context.Logger.LogLine("Completed uploading person");
        }

        public static void GetAccessToken(ILambdaContext context)
        {            
            context.Logger.LogLine("Getting access token.");

            try
            {
                if (string.IsNullOrEmpty(_accessToken))
                {
                    lock (_locker)
                    {
                        if (string.IsNullOrEmpty(_accessToken))
                        {
                            var data = string.Format("grant_type={0}&username={1}&password={2}&client_id={3}", "password"
                                , "LSAPIUser" /*ConfigHelper.GetAppSetting("LearnShareUsername")*/
                                , "Ay6h5aKsOK"/*ConfigHelper.GetAppSetting("LearnSharePassword")*/
                                , "2858" /*ConfigHelper.GetAppSetting("LearnShareClientId")*/
                                );

                            var response = PerformHttpRequestResponse(context, GetHttpWebLoginRequest(), data);
                            _accessToken = response.AccessToken;

                            if (string.IsNullOrEmpty(_accessToken))
                                throw new ApplicationException("LearnShare access token not acquired.");
                        }
                    }
                }
                
                context.Logger.LogLine("LearnShare access token acquired.");
                
            }
            catch (Exception x)
            {
                context.Logger.LogLine(x.Message);                
                throw;
            }
        }


        public static void DestroyAccessToken(ILambdaContext context)
        {
            context.Logger.LogLine("Destroying LearnShare access token.");            

            try
            {
                if (!string.IsNullOrEmpty(_accessToken))
                {
                    lock (_locker)
                    {
                        if (!string.IsNullOrEmpty(_accessToken))
                        {
                            _accessToken = "";
                        }
                    }
                }

                context.Logger.LogLine("LearnShare access token destroyed.");                
            }
            catch (Exception x)
            {
                context.Logger.LogLine(x.Message);                
                throw;
            }
        }

       

        private static HttpWebRequest GetHttpWebLoginRequest()
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("https://sandbox2sso.learnshare.com/OAuth2/token"/*ConfigHelper.GetAppSetting("LearnShareAuthUri")*/);
            request.Method = _httpPost;
            request.ContentType = "application/x-www-form-urlencoded";

            return request;
        }
        private static HttpWebRequest GetHttpWebRequest(string verb, string uri)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(uri);
            request.Method = verb;
            request.ContentType = "application/json";
            request.Headers.Set("Authorization", string.Format("Bearer {0}", _accessToken));

            return request;
        }


        public static LearnShareApiResponse PerformHttpRequestResponse(ILambdaContext context, HttpWebRequest request, string sourceData = null)
        {
            HttpWebResponse response = null;
            LearnShareApiResponse result = new LearnShareApiResponse();

            try
            {
                if ((request.Method == _httpPost) || (request.Method == _httpPut))
                    using (var sw = new StreamWriter(request.GetRequestStream()))
                    {
                        sw.Write(sourceData);
                        sw.Close();
                    }

                response = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException wx)
            {
                context.Logger.LogLine(wx.Message);
                
                if (wx.Status == WebExceptionStatus.ProtocolError)
                    response = (HttpWebResponse)wx.Response;
                else if (wx.Status == WebExceptionStatus.Timeout)
                    result.StatusCode = 408;
            }
            catch (Exception x)
            {
                context.Logger.LogLine(x.Message);                
            }

            if (response != null)
            {
                dynamic jsonResponse;

                result.StatusCode = (int)response.StatusCode;
                result.StatusDescription = response.StatusDescription;
                result.RequestMethod = response.Method;
                using (var sr = new StreamReader(response.GetResponseStream()))
                    jsonResponse = JsonConvert.DeserializeObject(sr.ReadToEnd());

                if (jsonResponse != null)
                {
                    result.ResponsePayload = jsonResponse.ToString();
                    result.RequestPayload = sourceData;

                    if (jsonResponse.access_token != null)
                        result.AccessToken = jsonResponse.access_token.Value;
                    if (jsonResponse.PersonID != null)
                        result.PersonID = Int32.Parse(jsonResponse.PersonID.Value.ToString());
                    if (jsonResponse.Level != null)
                        result.Level = Int32.Parse(jsonResponse.Level.Value.ToString());
                    if (jsonResponse.EmployeeID != null)
                        result.EmployeeID = jsonResponse.EmployeeID.Value;
                    if (jsonResponse.error != null)
                        result.ErrorMessage = jsonResponse.error.message.Value;
                }
            }

            return result;
        }

        public static Person GetPerson()
        {
            Person p = new Person();

            Properties prop = new Properties();
            Address a = new Address();

            prop.hiredate = DateTime.Now;
            prop.termdate = null;
            prop.costcategory = "";

            a.Street1 = "";
            a.Street2 = "";
            a.City = "";
            a.State = "";
            a.Postal = "";
            a.Country = "";

            p.EmployeeID = "105012870";
            p.PersonID = 4244087;
            p.UserName = "105012870";
            p.Password = "105012870";
            p.FirstName = "FirstName" + DateTime.Now.ToString();
            p.LastName = "LastName";
            p.Phonenum = "9998887777";
            p.EMail = "email@email.com";
            p.Active = "True";
            p.SupervisorID = "2300228";
            p.Properties = prop;
            p.Address = a;
            p.Domains = new List<Domain>()
            {
                new Domain
                {
                    DomainID = 517,
                    Description = "Secondary Job Title",
                    Active = false,
                    Levels = new List<Level>()
                    {
                        new Level
                        {
                            Active = false,
                            Description = null,
                            Code = "UNKNOWN",
                            MemberOf = 0,
                            DomainID = 0
                        }
                    }
                },
                 new Domain
                {
                    DomainID = 516,
                    Description = "Employee Location",
                    Active = false,
                    Levels = new List<Level>()
                    {
                        new Level
                        {
                            Active = false,
                            Description = null,
                            Code = "UNKNOWN",
                            MemberOf = 0,
                            DomainID = 0
                        }
                    }
                },
                 new Domain
                {
                    DomainID = 515,
                    Description = "Employee Status",
                    Active = false,
                    Levels = new List<Level>()
                    {
                        new Level
                        {
                            Active = false,
                            Description = null,
                            Code = "UNKNOWN",
                            MemberOf = 0,
                            DomainID = 0
                        }
                    }
                },
                 new Domain
                {
                    DomainID = 514,
                    Description = "Employee Category",
                    Active = false,
                    Levels = new List<Level>()
                    {
                        new Level
                        {
                            Active = false,
                            Description = null,
                            Code = "UNKNOWN",
                            MemberOf = 0,
                            DomainID = 0
                        }
                    }
                },
                 new Domain
                {
                    DomainID = 513,
                    Description = "Language Spoken",
                    Active = false,
                    Levels = new List<Level>()
                    {
                        new Level
                        {
                            Active = false,
                            Description = null,
                            Code = "UNKNOWN",
                            MemberOf = 0,
                            DomainID = 0
                        }
                    }
                },
                 new Domain
                {
                    DomainID = 512,
                    Description = "Certifications",
                    Active = false,
                    Levels = new List<Level>()
                    {
                        new Level
                        {
                            Active = false,
                            Description = null,
                            Code = "UNKNOWN",
                            MemberOf = 0,
                            DomainID = 0
                        }
                    }
                },
                 new Domain
                {
                    DomainID = 511,
                    Description = "Office Sub-Specialty",
                    Active = false,
                    Levels = new List<Level>()
                    {
                        new Level
                        {
                            Active = false,
                            Description = null,
                            Code = "UNKNOWN",
                            MemberOf = 0,
                            DomainID = 0
                        }
                    }
                },
                 new Domain
                {
                    DomainID = 510,
                    Description = "Company",
                    Active = false,
                    Levels = new List<Level>()
                    {
                        new Level
                        {
                            Active = false,
                            Description = null,
                            Code = "UNKNOWN",
                            MemberOf = 0,
                            DomainID = 0
                        }
                    }
                },
                 new Domain
                {
                    DomainID = 509,
                    Description = "Country",
                    Active = false,
                    Levels = new List<Level>()
                    {
                        new Level
                        {
                            Active = false,
                            Description = null,
                            Code = "UNKNOWN",
                            MemberOf = 0,
                            DomainID = 0
                        }
                    }
                },
                 new Domain
                {
                    DomainID = 508,
                    Description = "Coach",
                    Active = false,
                    Levels = new List<Level>()
                    {
                        new Level
                        {
                            Active = false,
                            Description = null,
                            Code = "UNKNOWN",
                            MemberOf = 0,
                            DomainID = 0
                        }
                    }
                },
                 new Domain
                {
                    DomainID = 507,
                    Description = "Skills",
                    Active = false,
                    Levels = new List<Level>()
                    {
                        new Level
                        {
                            Active = false,
                            Description = null,
                            Code = "UNKNOWN",
                            MemberOf = 0,
                            DomainID = 0
                        }
                    }
                },
                 new Domain
                {
                    DomainID = 506,
                    Description = "Job Role",
                    Active = false,
                    Levels = new List<Level>()
                    {
                        new Level
                        {
                            Active = false,
                            Description = null,
                            Code = "UNKNOWN",
                            MemberOf = 0,
                            DomainID = 0
                        }
                    }
                },
                 new Domain
                {
                    DomainID = 505,
                    Description = "Job Title",
                    Active = false,
                    Levels = new List<Level>()
                    {
                        new Level
                        {
                            Active = false,
                            Description = null,
                            Code = "UNKNOWN",
                            MemberOf = 0,
                            DomainID = 0
                        }
                    }
                },
                 new Domain
                {
                    DomainID = 504,
                    Description = "Employee Type",
                    Active = false,
                    Levels = new List<Level>()
                    {
                        new Level
                        {
                            Active = false,
                            Description = null,
                            Code = "UNKNOWN",
                            MemberOf = 0,
                            DomainID = 0
                        }
                    }
                },
                 new Domain
                {
                    DomainID = 503,
                    Description = "Office Type",
                    Active = false,
                    Levels = new List<Level>()
                    {
                        new Level
                        {
                            Active = false,
                            Description = null,
                            Code = "UNKNOWN",
                            MemberOf = 0,
                            DomainID = 0
                        }
                    }
                },
                 new Domain
                {
                    DomainID = 502,
                    Description = "Division",
                    Active = false,
                    Levels = new List<Level>()
                    {
                        new Level
                        {
                            Active = false,
                            Description = null,
                            Code = "UNKNOWN",
                            MemberOf = 0,
                            DomainID = 0
                        }
                    }
                },
                 new Domain
                {
                    DomainID = 501,
                    Description = "Office",
                    Active = false,
                    Levels = new List<Level>()
                    {
                        new Level
                        {
                            Active = false,
                            Description = null,
                            Code = "UNKNOWN",
                            MemberOf = 0,
                            DomainID = 0
                        }
                    }
                },
                 new Domain
                {
                    DomainID = 500,
                    Description = "State",
                    Active = false,
                    Levels = new List<Level>()
                    {
                        new Level
                        {
                            Active = false,
                            Description = null,
                            Code = "UNKNOWN",
                            MemberOf = 0,
                            DomainID = 0
                        }
                    }
                },
                 new Domain
                {
                    DomainID = 499,
                    Description = "Office Specialty",
                    Active = false,
                    Levels = new List<Level>()
                    {
                        new Level
                        {
                            Active = false,
                            Description = null,
                            Code = "UNKNOWN",
                            MemberOf = 0,
                            DomainID = 0
                        }
                    }
                },
                 new Domain
                {
                    DomainID = 498,
                    Description = "All Employees",
                    Active = false,
                    Levels = new List<Level>()
                    {
                        new Level
                        {
                            Active = false,
                            Description = null,
                            Code = "UNKNOWN",
                            MemberOf = 0,
                            DomainID = 0
                        }
                    }
                },
                 new Domain
                {
                    DomainID = 518,
                    Description = "Manager",
                    Active = false,
                    Levels = new List<Level>()
                    {
                        new Level
                        {
                            Active = false,
                            Description = null,
                            Code = "N",
                            MemberOf = 0,
                            DomainID = 0
                        }
                    }
                }
            };
            return p;
        }
    }
}
