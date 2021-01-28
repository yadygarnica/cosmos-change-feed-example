using Newtonsoft.Json;
using System.Collections.Generic;

namespace CosmosChangeFeed.CourseServiceModels
{
    public class Course
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("partitionKey")]
        public string PartitionKey { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Topic { get; set; }

        public int HoursCount { get; set; }

        public string PrincipalName { get; set; }

        public string PeriodReference { get; set; }

        public IEnumerable<string> ProfessorsNames { get; set; } = new List<string>(0);
        public IEnumerable<Enrollment> Enrollments { get; set; } = new List<Enrollment>(0);
    }
}
