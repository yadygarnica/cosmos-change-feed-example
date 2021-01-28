using System;

namespace CosmosChangeFeed.StudentServiceModels
{
    public class Enrollment
    {
        public Course Course { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}
