using Newtonsoft.Json;
using System.Collections.Generic;

namespace CosmosChangeFeed.StudentServiceModels
{
    public class Student
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("partitionKey")]
        public string PartitionKey { get; set; }

        public string Name { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string TotalGrade { get; set; }
        
        public IList<Enrollment> Enrollments { get; set; } = new List<Enrollment>(0);
    }
}
