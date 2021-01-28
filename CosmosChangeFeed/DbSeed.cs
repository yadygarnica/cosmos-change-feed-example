using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CosmosChangeFeed
{
    public static class DbSeed
    {
        [FunctionName("create-students")]
        public static async Task CreateStudentsAsync([HttpTrigger("get")] HttpRequest req
            , [CosmosDB(databaseName: "ContosoStudents", collectionName: "Students", ConnectionStringSetting = "CosmosDBConnection")] IAsyncCollector<StudentServiceModels.Student> studentsCollector
            , ILogger log)
        {
            for (int i = 1; i <= 10; i++)
            {
                var student = new StudentServiceModels.Student
                {
                    Id = i.ToString(),
                    PartitionKey = i.ToString(),
                    Email = $"email{i}@mail.com",
                    LastName = $"Lastname{i}",
                    Name = $"Name{i}",
                    Phone = $"213455{i}",
                    Enrollments = new List<StudentServiceModels.Enrollment>()
                };
                await studentsCollector.AddAsync(student);
            }

            log.LogInformation("Students created.");
        }

        [FunctionName("create-courses")]
        public static async Task CreateCoursesAsync([HttpTrigger("get")] HttpRequest req
            , [CosmosDB(databaseName: "ContosoCourses", collectionName: "Courses", ConnectionStringSetting = "CosmosDBConnection")] IAsyncCollector<CourseServiceModels.Course> cooursesCollector
            , ILogger log)
        {
            for (int i = 1; i <= 5; i++)
            {
                var course = new CourseServiceModels.Course 
                {
                    Title = $"Course Title {i}",
                    Description = $"Description {i}",
                    HoursCount = 40,
                    PeriodReference = "2021.1",
                    Topic = $"Topic {i}",
                    ProfessorsNames = new List<string> { "Professor1", "Professor2", "Professor3" },
                    PrincipalName = "Professor4"
                };
                var enrollments = new List<CourseServiceModels.Enrollment>(2);
                for (int j = (i-1)*2+1; j <= i*2; j++)
                {
                    enrollments.Add(new CourseServiceModels.Enrollment
                    {
                        StartDate = DateTime.UtcNow,
                        EndDate = DateTime.UtcNow.AddMonths(6),
                        Student = new CourseServiceModels.Student
                        {
                            Id = j.ToString(),
                            LastName = $"Lastname{j}",
                            Name = $"Name{j}"
                        }
                    });
                }

                course.Enrollments = enrollments;
                await cooursesCollector.AddAsync(course);
            }
            log.LogInformation("Courses created.");
        }

    }
}
