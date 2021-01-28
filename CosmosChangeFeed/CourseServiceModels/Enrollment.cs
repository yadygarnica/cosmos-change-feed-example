using System;

namespace CosmosChangeFeed.CourseServiceModels
{
    public class Enrollment
    {
        public Student Student { get; set; }

        public int Grade { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}
