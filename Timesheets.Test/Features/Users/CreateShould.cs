using Microsoft.EntityFrameworkCore;
using Shouldly;
using Timesheets.Api.Features.Users;

namespace Timesheets.Test.Features.Users
{
    public class CreateShould : TestBase
    {
        [Fact]
        public async Task Add_a_User_to_the_db()
        {
            var handler = new Create.CommandHandler(_context);
            var command = new Create.Command { UserName = "Tom" };
            var result = await handler.Handle(command, CancellationToken.None);

            result.Successful.ShouldBeTrue();

            var projectsInDb = await _context.Projects.ToListAsync();
            projectsInDb.Count.ShouldBe(1);
            projectsInDb.First().Name.ShouldBe(command.UserName);
        }

        [Fact]
        public async Task Be_unsuccessful_if_User_name_is_empty()
        {
            var handler = new Create.CommandHandler(_context);
            var command = new Create.Command { UserName = "" };
            var result = await handler.Handle(command, CancellationToken.None);

            result.Successful.ShouldBeFalse();

            var projectsInDb = await _context.Projects.ToListAsync();
            projectsInDb.Count.ShouldBe(0);
        }
    }
}
