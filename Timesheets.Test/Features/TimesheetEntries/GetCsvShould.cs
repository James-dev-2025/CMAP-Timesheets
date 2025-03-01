using FakeItEasy;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using System.Text;
using Timesheets.Api.Features.TimesheetEntries;
using Timesheets.Api.Services.CsvService;
using Timesheets.Test.Builders.Projects;
using Timesheets.Test.Builders.Timesheets;
using Timesheets.Test.Builders.Users;
using static Timesheets.Api.Features.TimesheetEntries.GetCsv;


namespace Timesheets.Test.Features.TimesheetEntries
{
    public class GetCsvShould : TestBase
    {
        [Fact]
        public async Task Call_csv_service()
        {
            var csvService = A.Fake<ICsvService>();

            var expectedEntries = new List<EntryDto> { };

            var handler = new GetCsv.QueryHandler( _context, csvService); 
            await handler.Handle(new GetCsv.Query { UserId = null, ProjectId = null }, CancellationToken.None);
            A.CallTo(() => csvService.GenerateCsv(A<List<EntryDto>>.Ignored)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task Get_all_entries_when_userId_and_projectId_are_null()
        {
            var project1 = new ProjectBuilder().WithName("Project Alpha").Build();
            var project2 = new ProjectBuilder().WithName("Project Beta").Build();
            _context.Projects.AddRange(project1, project2);

            var user1 = new UserBuilder().WithUserName("Bob").Build();
            var user2 = new UserBuilder().WithUserName("Tom").Build();
            _context.Users.AddRange(user1, user2);

            var entry1 = new TimesheetEntryBuilder().WithUserId(user1.Id).WithProjectId(project1.Id).Build();
            var entry2 = new TimesheetEntryBuilder().WithUserId(user2.Id).WithProjectId(project2.Id).Build();
            _context.TimesheetEntries.AddRange(entry1, entry2);
            await _context.SaveChangesAsync();

            var csvService = new CsvService();

            var handler = new GetCsv.QueryHandler(_context, csvService);
            var result = await handler.Handle(new GetCsv.Query { UserId = null, ProjectId = null }, CancellationToken.None);

            string csvString = Encoding.UTF8.GetString(result.Csv);
            string[] lines = csvString.Split("\n", System.StringSplitOptions.RemoveEmptyEntries);
            lines.Length.ShouldBe(3);

            lines[0].ShouldBe("User Name,Date,Project,Description of Tasks,Hours Worked,Total Hours for the Day\r");

            lines[1].ShouldBe($"{entry1.User.UserName},{entry1.Date.ToString("d")},{entry1.Project.Name},{entry1.Description},{entry1.HoursWorked},{entry1.HoursWorked}\r");

            lines[2].ShouldBe($"{entry2.User.UserName},{entry2.Date.ToString("d")},{entry2.Project.Name},{entry2.Description},{entry2.HoursWorked},{entry2.HoursWorked}\r");
        }

        [Fact]
        public async Task Get_only_entries_with_specific_userId()
        {
            var project1 = new ProjectBuilder().WithName("Project Alpha").Build();
            var project2 = new ProjectBuilder().WithName("Project Beta").Build();
            _context.Projects.AddRange(project1, project2);

            var user1 = new UserBuilder().WithUserName("Bob").Build();
            var user2 = new UserBuilder().WithUserName("Tom").Build();
            _context.Users.AddRange(user1, user2);

            var entry1 = new TimesheetEntryBuilder().WithUserId(user1.Id).WithProjectId(project1.Id).Build();
            var entry2 = new TimesheetEntryBuilder().WithUserId(user2.Id).WithProjectId(project2.Id).Build();
            _context.TimesheetEntries.AddRange(entry1, entry2);
            await _context.SaveChangesAsync();

            var csvService = new CsvService();

            var handler = new GetCsv.QueryHandler(_context, csvService);
            var result = await handler.Handle(new GetCsv.Query { UserId = user1.Id, ProjectId = null }, CancellationToken.None);

            string csvString = Encoding.UTF8.GetString(result.Csv);
            string[] lines = csvString.Split("\n", System.StringSplitOptions.RemoveEmptyEntries);
            lines.Length.ShouldBe(2);

            lines[0].ShouldBe("User Name,Date,Project,Description of Tasks,Hours Worked,Total Hours for the Day\r");

            lines[1].ShouldBe($"{entry1.User.UserName},{entry1.Date.ToString("d")},{entry1.Project.Name},{entry1.Description},{entry1.HoursWorked},{entry1.HoursWorked}\r");
        }

        [Fact]
        public async Task Get_only_entries_with_specific_projectId()
        {
            var project1 = new ProjectBuilder().WithName("Project Alpha").Build();
            var project2 = new ProjectBuilder().WithName("Project Beta").Build();
            _context.Projects.AddRange(project1, project2);

            var user1 = new UserBuilder().WithUserName("Bob").Build();
            var user2 = new UserBuilder().WithUserName("Tom").Build();
            _context.Users.AddRange(user1, user2);

            var entry1 = new TimesheetEntryBuilder().WithUserId(user1.Id).WithProjectId(project1.Id).Build();
            var entry2 = new TimesheetEntryBuilder().WithUserId(user2.Id).WithProjectId(project2.Id).Build();
            _context.TimesheetEntries.AddRange(entry1, entry2);
            await _context.SaveChangesAsync();

            var csvService = new CsvService();

            var handler = new GetCsv.QueryHandler(_context, csvService);
            var result = await handler.Handle(new GetCsv.Query { UserId = null, ProjectId = project1.Id }, CancellationToken.None);

            string csvString = Encoding.UTF8.GetString(result.Csv);
            string[] lines = csvString.Split("\n", System.StringSplitOptions.RemoveEmptyEntries);
            lines.Length.ShouldBe(2);

            lines[0].ShouldBe("User Name,Date,Project,Description of Tasks,Hours Worked,Total Hours for the Day\r");

            lines[1].ShouldBe($"{entry1.User.UserName},{entry1.Date.ToString("d")},{entry1.Project.Name},{entry1.Description},{entry1.HoursWorked},{entry1.HoursWorked}\r");
        }

        [Fact]
        public async Task Include_summed_hours_for_that_day()
        {
            var project1 = new ProjectBuilder().WithName("Project Alpha").Build();
            _context.Projects.AddRange(project1);

            var user1 = new UserBuilder().WithUserName("Bob").Build();
            _context.Users.AddRange(user1);

            var entry1 = new TimesheetEntryBuilder().WithUserId(user1.Id).WithProjectId(project1.Id).WithDate(DateTime.Now).WithHoursWorked(1).Build();
            var entry2 = new TimesheetEntryBuilder().WithUserId(user1.Id).WithProjectId(project1.Id).WithDate(DateTime.Now).WithHoursWorked(2).Build();
            var entry3 = new TimesheetEntryBuilder().WithUserId(user1.Id).WithProjectId(project1.Id).WithDate(DateTime.Now).WithHoursWorked(3).Build();
            _context.TimesheetEntries.AddRange(entry1, entry2, entry3);
            await _context.SaveChangesAsync();

            var csvService = new CsvService();

            var handler = new GetCsv.QueryHandler(_context, csvService);
            var result = await handler.Handle(new GetCsv.Query { UserId = null, ProjectId = null }, CancellationToken.None);

            string csvString = Encoding.UTF8.GetString(result.Csv);
            string[] lines = csvString.Split("\n", System.StringSplitOptions.RemoveEmptyEntries);
            lines.Length.ShouldBe(4);

            lines[0].ShouldBe("User Name,Date,Project,Description of Tasks,Hours Worked,Total Hours for the Day\r");

            lines[1].ShouldBe($"{entry1.User.UserName},{entry1.Date.ToString("d")},{entry1.Project.Name},{entry1.Description},{entry1.HoursWorked},{entry1.HoursWorked + entry2.HoursWorked + entry3.HoursWorked}\r");
        }
    }
}
