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

            var usersInDb = await _context.Users.ToListAsync();
            usersInDb.Count.ShouldBe(1);
            usersInDb.First().UserName.ShouldBe(command.UserName);
        }

        [Fact]
        public async Task Be_unsuccessful_if_User_name_is_empty()
        {
            var handler = new Create.CommandHandler(_context);
            var command = new Create.Command { UserName = "" };
            var result = await handler.Handle(command, CancellationToken.None);

            result.Successful.ShouldBeFalse();

            var usersInDb = await _context.Users.ToListAsync();
            usersInDb.Count.ShouldBe(0);
        }
    }
}
