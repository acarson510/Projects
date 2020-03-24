using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

using Amazon.Lambda.Core;
using Amazon.Lambda.SNSEvents;
using Bayada.Common.DataAccess;
using Bayada.Entity;
using Bayada.Repository;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace WriteToVpc
{
    public class Function
    {
        public Function()
        {

        }
        private IGenericRepository _rep;
        private IGenericRepository Rep
        {
            get
            {
                if (_rep == null)
                    _rep = RepositoryFactory.GetRepository<IGenericRepository>("mdRep");
                return _rep;
            }
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

            try
            {
                var skills = Rep.GetAll<Skill>().ToList();



                //var cxString = @"Data Source=dia-dev-mssql-01.ccfwqtcduhbn.us-east-1.rds.amazonaws.com;Initial Catalog=MasterData;integrated security=false;persist security info=false;User Id=acarson;Password=andrew;";
                //var sql = "INSERT INTO [dbo].[Skill] ([Code],[Name],[Description],[Category],[Active],[SortOrder]) VALUES ('ABC','AbcTest','AbcDescription',null,0,77)";
                //SqlParameter[] sqlParms = new SqlParameter[0];
                //SqlHelper.ExecuteNonQuery(cxString, sql, sqlParms);

               
            }
            catch(Exception x)
            {

            }

            await Task.CompletedTask;
        }
    }
}
