# CMAP Timesheet System
To run this system clone or download the repository, then open up Timesheets.sln. Feel free to run the tests at this point.

## Project Description
This solution contains three projects: Timesheets.Api, Timesheets.Domain, Timesheets.test.

### Timesheets.api
This project contains the api used to read and write data, as well as any services needed to process that data. I have opted to use the Mediatr Handler to make the codebase more modular and maintainable. You can see this by looking in the features folder. Here there is a folder per database entity which contain a file for every action that can be done to that entity and a controller to house the endpoints.

### Timesheets.domain
This project utilises EF core to design the database. You can find a file called ApplicationDbContext.cs in this project, inside you can see some DbSets and their configurations. Looking at these showcases the relationships between these entities. For example A TimesheetEntry has a foreign key to both User and Project

### Timesheets.test
This project houses all the tests for the features in the Timesheets.api project. These tests make use of a few packages: Shouldy, an assertion framework that makes test code and responses more human readable and FakeItEasy which allows you to fake out objects and there functionality This was used to test the CSVService.

## How to use
Run the Timesheets.Api Project with the https launch profile. You should see a swagger UI open in your browser (If it does not open go to this Url while running the project: https://localhost:7286/swagger/index.html).

The First thing to do is to add a User and A Project To the Database. You can do this with the "/api/user/create" and the "/api/project/create" endpoints. Feel free to create as many any as you want.

Once all Projects and Users have been created. Call "api/users/list" and note down the data you get back. Do the same for Projects on the "api/projects/list" endpoint.

With a projects and users in the database you can now record Timesheet entries. Use the "api/timesheetEntries/create" endpoint to try this out. For the "userId" field use the Id from a previously created User. Do the same for the ProjectId field.

Next, try the "/api/timesheetEntries/getCsv" endpoint. You will notice that there are two extra fields. One allows you to specify getting all timesheet entries of a single user and one allows you to get all timesheet entries for a project. To get all entries regardless of user or project just leave them as null.

After making the request you will see a Download file button in the Response body.

After this you can update a project or user. After doing so, request another csv and you will see your changes have persisted to the time sheet data