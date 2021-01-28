# cosmos-change-feed-example
Contoso University Example - how to use Cosmos Change Feed with Azure Functions to sync the data of two services
 
Scenario: Two services, one service to manage studient data and one service to manage courses data.
Infrastructure: Each service running on a web app with its own cosmos db. Courses services has a cosmos collection named "Courses" and the Students service has a cosmos collection named "Students"

Problem: When a course info is updated we need to sync the course info on each student enrolled to the course.

Solution: An Azure Function to update the students data when a course data change.

Used Bindigns:
* CosmosDBTrigger to receive the courses changed
* CosmosDB with DocumentClient to read the enrolled students
* CosmosDB with IAsyncCollector to write the updated students data