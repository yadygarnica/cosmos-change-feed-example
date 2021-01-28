using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CosmosChangeFeed
{
    public static class UpdateStudentsEnrollment
    {        
        [FunctionName("update-students-enrollment")]
        public static async Task SyncAsync(
            [CosmosDBTrigger(databaseName: "ContosoCourses", collectionName: "Courses", ConnectionStringSetting = "CosmosDBConnection", LeaseCollectionName = "leases", CreateLeaseCollectionIfNotExists = true)]IReadOnlyList<Document> documents
            , [CosmosDB(databaseName: "ContosoStudents", collectionName: "Students", ConnectionStringSetting = "CosmosDBConnection")] DocumentClient studentsClient
            , [CosmosDB(databaseName: "ContosoStudents", collectionName: "Students", ConnectionStringSetting = "CosmosDBConnection")] IAsyncCollector<StudentServiceModels.Student> studentsCollector
            , ILogger log)
        {
            if (documents == null || documents.Count == 0) return;
            foreach (var document in documents)
            {
                var course = JsonConvert.DeserializeObject<CourseServiceModels.Course>(document.ToString());
                foreach (var enrollment in course.Enrollments)
                {
                    var docUri = UriFactory.CreateDocumentUri("ContosoStudents", "Students", enrollment.Student.Id);
                    var student = await studentsClient.ReadDocumentAsync<StudentServiceModels.Student>(docUri,
                        new RequestOptions { PartitionKey = new PartitionKey(enrollment.Student.Id) });
                    var updatedStudent = UpdateStudentEnrollment(course, enrollment, student);
                    await studentsCollector.AddAsync(updatedStudent);
                }
            }

            log.LogInformation("Students updated.");
        }

        private static StudentServiceModels.Student UpdateStudentEnrollment(CourseServiceModels.Course course, CourseServiceModels.Enrollment enrollment, StudentServiceModels.Student student)
        {
            var studentCurrentEnrollment = student.Enrollments.FirstOrDefault(x => x.Course.Id.Equals(course.Id, StringComparison.InvariantCultureIgnoreCase));
            var studentUpdatedEnrollment = new StudentServiceModels.Enrollment
            {
                StartDate = enrollment.StartDate,
                EndDate = enrollment.EndDate,
                Course = new StudentServiceModels.Course
                {
                    Id = course.Id,
                    Title = course.Title,
                    Description = course.Description
                }
            };
            if (studentCurrentEnrollment == default)
            {
                student.Enrollments.Add(studentUpdatedEnrollment);
            }
            else
            {
                studentCurrentEnrollment = studentUpdatedEnrollment;
            }

            return student;
        }
    }
}
