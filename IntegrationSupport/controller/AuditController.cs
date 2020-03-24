using Bayada.Common.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace IntegrationSupport.controller
{
    [Route("api/[controller]")]
    public class AuditController : Controller
    {
        // GET api/values
        [HttpGet]
        public string Get()//IEnumerable<string> Get()
        {
            var connectionString = @"Data Source=GP_TEST_SQL\GP;Initial Catalog=ODSDB;integrated security=false;persist security info=false;User Id=patrickdevers;Password=bay2010;";
            var a = SqlHelper.GetDataTable(connectionString, "exec auditTest");

            var result = new List<Audit>();

            result = a.AsEnumerable().Select(r =>
            {
                return new Audit()
                {
                    EntityType = r.Field<string>("EntityType"),
                    EntityId = r.Field<string>("EntityId"),
                    RequestType = r.Field<string>("RequestType"),
                    RequestDate = r.Field<string>("RequestDate"),
                    //SourceData = r.Field<string>("SourceData"),
                    //ResponseData = r.Field<string>("ResponseData"),
                    ResponseCode = r.Field<int>("ResponseCode"),
                    ResponseDescription = r.Field<string>("ResponseDescription")
    };
            }).ToList();

            AuditContainer ac = new AuditContainer();
            ac.records = result;
            var json = JsonConvert.SerializeObject(result);
            return json;
            //return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }

    public class Audit
    {
        public string EntityType { get; set; }
		public string EntityId { get; set; }
		public string RequestType { get; set; }
		public string RequestDate { get; set; }
		//public string SourceData { get; set; }
		//public string ResponseData { get; set; }
		public int ResponseCode { get; set; }
		public string ResponseDescription { get; set; }
    }

    public class AuditContainer
    {
        public List<Audit> records { get; set; }
        public AuditContainer()
        {
            records = new List<Audit>();
        }
    }
}
