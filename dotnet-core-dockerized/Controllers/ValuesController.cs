using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using api.Model;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private const string DynamoDBTableName = "Bicycles";

        private readonly IAmazonDynamoDB _amazonDynamoDb;

        public ValuesController(IAmazonDynamoDB amazonDynamoDB) => this._amazonDynamoDb = amazonDynamoDB;

        // GET api/values/init
        [HttpGet]
        [Route("init")]
        public async Task Initialise()
        {
            var request = new ListTablesRequest
            {
                Limit = 10
            };

            var response = await _amazonDynamoDb.ListTablesAsync(request);

            var results = response.TableNames;

            if (!results.Contains(DynamoDBTableName))
            {
                Console.WriteLine("Was not able to find table; creating...");
                var createRequest = new CreateTableRequest
                {
                    TableName = DynamoDBTableName,
                    AttributeDefinitions = new List<AttributeDefinition>
                    {
                        new AttributeDefinition
                        {
                            AttributeName = "SerialNumber",
                            AttributeType = "S"
                        }
                    },
                    KeySchema = new List<KeySchemaElement>
                    {
                        new KeySchemaElement
                        {
                            AttributeName = "SerialNumber",
                            KeyType = "HASH"  //Partition key
                        }
                    },
                    ProvisionedThroughput = new ProvisionedThroughput
                    {
                        ReadCapacityUnits = 2,
                        WriteCapacityUnits = 2
                    }
                };

                await _amazonDynamoDb.CreateTableAsync(createRequest);

                Console.WriteLine("Finished creating table");
            }
        }

        // GET api/values
        [HttpGet]
        public async Task<ActionResult<IEnumerable<string>>> Get()
        {
            Console.WriteLine("Attempting to get table names...");
            try
            {
                var request = new ListTablesRequest
                {
                    Limit = 10
                };
                var response = await _amazonDynamoDb.ListTablesAsync(request);
                var results = response.TableNames;
                return results;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500);
            }
        }

        // GET api/values/5
        [HttpGet("{serialNumber}")]
        public async Task<ActionResult<Bicycle>> Get(string serialNumber)
        {
            var request = new GetItemRequest
            {
                TableName = DynamoDBTableName,
                Key = new Dictionary<string, AttributeValue> { { "SerialNumber", new AttributeValue { S = serialNumber } } }
            };

            var response = await _amazonDynamoDb.GetItemAsync(request);

            if (!response.IsItemSet)
            {
                return NotFound();
            }

            return new Bicycle() { Make = response.Item["Make]"].S, Model = response.Item["Model"].S };
        }

        // POST api/values
        [HttpPost]
        public async Task Post([FromBody] Bicycle input)
        {
            var request = new PutItemRequest
            {
                TableName = DynamoDBTableName,
                Item = new Dictionary<string, AttributeValue>
                {
                    { "SerialNumber", new AttributeValue { S = input.SerialNumber }},
                    { "Make", new AttributeValue { S = input.Make }},
                    { "Model", new AttributeValue { S = input.Model }},
                    { "ModelYear", new AttributeValue { S = input.ModelYear }}
                }
            };

            await _amazonDynamoDb.PutItemAsync(request);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
