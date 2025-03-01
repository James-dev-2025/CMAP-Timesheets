using Microsoft.EntityFrameworkCore;
using Shouldly;
using Timesheets.Api.Features.TimesheetEntries;
using Timesheets.Test.Builders.Projects;
using Timesheets.Test.Builders.Timesheets;
using Timesheets.Test.Builders.Users;

namespace Timesheets.Test.Features.TimesheetEntries
{
    public class UpdateShould : TestBase
    {
        [Fact]
        public async Task Update_an_entry()
        {
            var project = new ProjectBuilder().WithName("Project Alpha").Build();
            _context.Projects.Add(project);

            var user = new UserBuilder().WithUserName("Tom").Build();
            _context.Users.Add(user);

            var existingEntry = new TimesheetEntryBuilder().WithUserId(user.Id).WithProjectId(project.Id).Build();
            _context.TimesheetEntries.Add(existingEntry);
            await _context.SaveChangesAsync();

            var handler = new Update.CommandHandler(_context);
            var command = new Update.Command
            {
                Id = existingEntry.Id,
                Date = DateTime.Now.AddDays(-1),
                HoursWorked = 6.0m,
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
            var project = new ProjectBuilder().WithName("Project Alpha").Build();
            _context.Projects.Add(project);

            var user = new UserBuilder().WithUserName("Tom").Build();
            _context.Users.Add(user);

            var existingEntry = new TimesheetEntryBuilder().WithUserId(user.Id).WithProjectId(project.Id).Build();
            _context.TimesheetEntries.Add(existingEntry);
            await _context.SaveChangesAsync();

            var handler = new Update.CommandHandler(_context);
            var command = new Update.Command
            {
                Id = existingEntry.Id,
                Date = DateTime.Now.AddDays(-1),
                HoursWorked = 6.0m,
                Description = "Developed new feature",
                ProjectId = Guid.Empty,
                UserId = user.Id
            };
            var result = await handler.Handle(command, CancellationToken.None);

            result.Successful.ShouldBeFalse();

            var TimesheetEntryInDb = await _context.TimesheetEntries.ToListAsync();
            TimesheetEntryInDb.Count.ShouldBe(1);
            TimesheetEntryInDb.First().Date.ShouldBe(existingEntry.Date);
            TimesheetEntryInDb.First().HoursWorked.ShouldBe(existingEntry.HoursWorked);
            TimesheetEntryInDb.First().Description.ShouldBe(existingEntry.Description);
            TimesheetEntryInDb.First().ProjectId.ShouldBe(existingEntry.ProjectId);
            TimesheetEntryInDb.First().UserId.ShouldBe(existingEntry.UserId);
        }

        [Fact]
        public async Task Be_unsuccessful_if_User_does_notExist()
        {
            var project = new ProjectBuilder().WithName("Project Alpha").Build();
            _context.Projects.Add(project);

            var user = new UserBuilder().WithUserName("Tom").Build();
            _context.Users.Add(user);

            var existingEntry = new TimesheetEntryBuilder().WithUserId(user.Id).WithProjectId(project.Id).Build();
            _context.TimesheetEntries.Add(existingEntry);
            await _context.SaveChangesAsync();

            var handler = new Update.CommandHandler(_context);
            var command = new Update.Command
            {
                Id = existingEntry.Id,
                Date = DateTime.Now.AddDays(-1),
                HoursWorked = 6.0m,
                Description = "Developed new feature",
                ProjectId =project.Id,
                UserId = Guid.Empty
            };
            var result = await handler.Handle(command, CancellationToken.None);

            result.Successful.ShouldBeFalse();

            var TimesheetEntryInDb = await _context.TimesheetEntries.ToListAsync();
            TimesheetEntryInDb.Count.ShouldBe(1);
            TimesheetEntryInDb.First().Date.ShouldBe(existingEntry.Date);
            TimesheetEntryInDb.First().HoursWorked.ShouldBe(existingEntry.HoursWorked);
            TimesheetEntryInDb.First().Description.ShouldBe(existingEntry.Description);
            TimesheetEntryInDb.First().ProjectId.ShouldBe(existingEntry.ProjectId);
            TimesheetEntryInDb.First().UserId.ShouldBe(existingEntry.UserId);
        }
    }
}
