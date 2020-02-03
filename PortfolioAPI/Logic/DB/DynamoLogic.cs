using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Amazon.Extensions.NETCore.Setup;
using Microsoft.Extensions.Configuration;
using PortfolioAPI.Models.POCO.Scoreboard;

namespace PortfolioAPI.Logic.DB
{
    public class DynamoLogic : IDisposable, DI.Interfaces.INoSql
    {
        private static String DyanmoTable = "portfolioScore";

        private IAmazonDynamoDB DynamoClient;
       
        private bool disposed { get; set; }

        public DynamoLogic() 
        {

            var options = new AWSOptions
            {
                Profile = "PortfolioDBUser",
                Region = RegionEndpoint.USEast1
            };
            DynamoClient = options.CreateServiceClient<IAmazonDynamoDB>();

        }

        public async Task<List<ScoreBoard>> GetLeaderBoard()
        {
            List<ScoreBoard> returnData = new List<ScoreBoard>();
            var request = new ScanRequest
            {
                TableName = DyanmoTable
            };

            var results = await DynamoClient.ScanAsync(request);

            foreach (Dictionary<string, AttributeValue> item in results.Items)
            {
                returnData.Add(new ScoreBoard
                {
                    Id = item["id"].N,
                    Initials = item["initials"].S,
                    DateSubmitted = item["dateSubmitted"].S
                });
            }
            return returnData;
        }

        public async Task AddLeaderBoardEntry(String initials)
        {
            var count = GetLeaderBoard().Result.Count;
            var request = new PutItemRequest
            {
                TableName = DyanmoTable,
                Item = new Dictionary<string, AttributeValue>()
                    {
                        { "id", new AttributeValue {
                              N =  (count + 1).ToString()
                          }},
                        { "initials", new AttributeValue {
                              S = initials
                          }},
                        { "dateSubmitted", new AttributeValue {
                              S = DateTime.UtcNow.ToString("MM-dd-yyyy")
                          }}
                    }
            };

            var result = await DynamoClient.PutItemAsync(request);
        }

        public async Task DeleteLeaderBoardEntry(String id)
        {
            Dictionary<String, AttributeValue> keyVal = new Dictionary<string, AttributeValue>();
            keyVal["id"] = new AttributeValue
            {
                N = id
            };

            var request = new DeleteItemRequest
            {
                Key = keyVal,
                TableName = DyanmoTable
            };
            var result = await DynamoClient.DeleteItemAsync(request);
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }

            if (disposing)
            {
                DynamoClient.Dispose();
            }

            disposed = true;
        }
    }
}