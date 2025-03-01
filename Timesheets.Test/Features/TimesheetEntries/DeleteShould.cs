using Microsoft.EntityFrameworkCore;
using Shouldly;
using Timesheets.Api.Features.TimesheetEntries;
using Timesheets.Test.Builders.Projects;
using Timesheets.Test.Builders.Timesheets;
using Timesheets.Test.Builders.Users;

namespace Timesheets.Test.Features.TimesheetEntries
{
    public class DeleteShould : TestBase
    {
        [Fact]
        public async Task Delete_a_TimesheetEntry()
        {
            var project = new ProjectBuilder().WithName("Project Alpha").Build();
            _context.Projects.Add(project);

            var user = new UserBuilder().WithUserName("Tom").Build();
            _context.Users.Add(user);

            var existingEntry = new TimesheetEntryBuilder().WithUserId(user.Id).WithProjectId(project.Id).Build();
            _context.TimesheetEntries.Add(existingEntry);
            await _context.SaveChangesAsync();

            var handler = new Delete.CommandHandler(_context);
            var command = new Delete.Command { Id = existingEntry.Id };
            var result = await handler.Handle(command, CancellationToken.None);

            result.Successful.ShouldBeTrue();

            var projectsInDb = await _context.Projects.ToListAsync();
            projectsInDb.Count.ShouldBe(1);
            projectsInDb.First().IsDeleted.ShouldBeTrue();
        }

        [Fact]
        public async Task Be_unsuccessful_if_TimesheetEntry_id_does_not_match()
        {
            var project = new ProjectBuilder().WithName("Project Alpha").Build();
            _context.Projects.Add(project);

            var user = new UserBuilder().WithUserName("Tom").Build();
            _context.Users.Add(user);

            var existingEntry = new TimesheetEntryBuilder().WithUserId(user.Id).WithProjectId(project.Id).Build();
            _context.TimesheetEntries.Add(existingEntry);
            await _context.SaveChangesAsync();

            var handler = new Delete.CommandHandler(_context);
            var result = await handler.Handle(new Delete.Command { Id = Guid.NewGuid() }, CancellationToken.None);

            result.Successful.ShouldBeFalse();

            var projectsInDb = await _context.Projects.ToListAsync();
            projectsInDb.Count.ShouldBe(1);
            projectsInDb.First().IsDeleted.ShouldBeFalse();
        }
    }
}
