using Amazon.DynamoDBv2.Model;

namespace api.Model
{
    public class Bicycle
    {
        public string SerialNumber { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string ModelYear { get; set; }
    }
}