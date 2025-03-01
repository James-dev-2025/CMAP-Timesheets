using Microsoft.EntityFrameworkCore;
using Shouldly;
using Timesheets.Api.Features.Users;
using Timesheets.Test.Builders.Users;

namespace Timesheets.Test.Features.Users
{
    public class UpdateShould : TestBase
    {
        [Fact]
        public async Task Update_a_User()
        {
            var existingUser = new UserBuilder().WithUserName("Tom").Build();
            _context.Users.Add(existingUser);
            await _context.SaveChangesAsync();

            var handler = new Update.CommandHandler(_context);
            var command = new Update.Command {Id = existingUser.Id, UserName = "Peter" };
            var result = await handler.Handle(command, CancellationToken.None);

            result.Successful.ShouldBeTrue();

            var projectsInDb = await _context.Users.ToListAsync();
            projectsInDb.Count.ShouldBe(1);
            projectsInDb.First().UserName.ShouldBe(command.UserName);
        }

        [Fact]
        public async Task Be_unsuccessful_if_user_name_is_empty()
        {
            var existingUser = new UserBuilder().WithUserName("Tom").Build();
            _context.Users.Add(existingUser);
            await _context.SaveChangesAsync();

            var handler = new Update.CommandHandler(_context);
            var command = new Update.Command { Id = existingUser.Id, UserName = "" };
            var result = await handler.Handle(command, CancellationToken.None);

            result.Successful.ShouldBeFalse();

            var usersInDb = await _context.Users.ToListAsync();
            usersInDb.Count.ShouldBe(1);
            usersInDb.First().UserName.ShouldBe(existingUser.UserName);
        }

        [Fact]
        public async Task Be_unsuccessful_if_user_does_not_exist()
        {
            var handler = new Update.CommandHandler(_context);
            var command = new Update.Command { Id = Guid.Empty, UserName = "Project Beta" };
            var result = await handler.Handle(command, CancellationToken.None);

            result.Successful.ShouldBeFalse();

            var usersInDb = await _context.Users.ToListAsync();
            usersInDb.Count.ShouldBe(0);
        }
    }
}
