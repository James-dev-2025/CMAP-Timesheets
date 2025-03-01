using Microsoft.EntityFrameworkCore;
using Shouldly;
using Timesheets.Api.Features.TimesheetEntries;
using Timesheets.Test.Builders.Projects;
using Timesheets.Test.Builders.Users;


namespace Timesheets.Test.Features.TimesheetEntries
{
    public class CreateShould : TestBase
    {
        [Fact]
        public async Task Add_a_TimesheetEntry_to_the_db()
        {
            var project = new ProjectBuilder().WithName("Project Alpha").Build();
            _context.Projects.Add(project);

            var user = new UserBuilder().WithUserName("Tom").Build();
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var handler = new Create.CommandHandler(_context);
            var command = new Create.Command
            {
                Date = DateTime.Now.AddDays(-1),
                HoursWorked = TimeSpan.FromHours(6),
                Description = "Developed new feature",
                ProjectId = project.Id,
                UserId = user.Id
            };
            var result = await handler.Handle(command, CancellationToken.None);

            result.Successful.ShouldBeTrue();

            var TimesheetEntryInDb = await _context.TimesheetEntries.ToListAsync();
            TimesheetEntryInDb.Count.ShouldBe(1);
            TimesheetEntryInDb.First().Date.ShouldBe(command.Date);
            TimesheetEntryInDb.First().HoursWorked.ShouldBe(command.HoursWorked);
            TimesheetEntryInDb.First().Description.ShouldBe(command.Description);
            TimesheetEntryInDb.First().ProjectId.ShouldBe(command.ProjectId);
            TimesheetEntryInDb.First().UserId.ShouldBe(command.UserId);
        }

        [Fact]
        public async Task Be_unsuccessful_if_Project_does_notExist()
        {
            var user = new UserBuilder().WithUserName("Tom").Build();
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var handler = new Create.CommandHandler(_context);
            var command = new Create.Command
            {
                Date = DateTime.Now.AddDays(-1),
                HoursWorked = TimeSpan.FromHours(6),
                Description = "Developed new feature",
                ProjectId = Guid.Empty,
                UserId = user.Id
            };
            var result = await handler.Handle(command, CancellationToken.None);

            result.Successful.ShouldBeFalse();
        }

        [Fact]
        public async Task Be_unsuccessful_if_User_does_notExist()
        {
            var project = new ProjectBuilder().WithName("Project Alpha").Build();
            _context.Projects.Add(project);
            await _context.SaveChangesAsync();

            var handler = new Create.CommandHandler(_context);
            var command = new Create.Command
            {
                Date = DateTime.Now.AddDays(-1),
                HoursWorked = TimeSpan.FromHours(6),
                Description = "Developed new feature",
                ProjectId = project.Id,
                UserId = Guid.Empty
            };
            var result = await handler.Handle(command, CancellationToken.None);

            result.Successful.ShouldBeFalse();
        }
    }
}
