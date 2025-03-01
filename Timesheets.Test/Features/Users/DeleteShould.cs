using Microsoft.EntityFrameworkCore;
using Shouldly;
using Timesheets.Api.Features.Users;
using Timesheets.Test.Builders.Users;

namespace Timesheets.Test.Features.Users
{
    public class DeleteShould : TestBase
    {
        [Fact]
        public async Task Delete_a_User()
        {
            var existingUser = new UserBuilder().WithUserName("Tom").Build();
            _context.Users.Add(existingUser);
            await _context.SaveChangesAsync();

            var handler = new Delete.CommandHandler(_context);
            var command = new Delete.Command { Id = existingUser.Id };
            var result = await handler.Handle(command, CancellationToken.None);

            result.Successful.ShouldBeTrue();

            var usersInDb = await _context.Users.ToListAsync();
            usersInDb.Count.ShouldBe(1);
            usersInDb.First().IsDeleted.ShouldBeTrue();
        }

        [Fact]
        public async Task Be_unsuccessful_if_user_id_does_not_match()
        {
            var existingUser = new UserBuilder().WithUserName("Tom").Build();
            _context.Users.Add(existingUser);
            await _context.SaveChangesAsync();

            var handler = new Delete.CommandHandler(_context);
            var result = await handler.Handle(new Delete.Command { Id = Guid.NewGuid() }, CancellationToken.None);

            result.Successful.ShouldBeFalse();

            var usersInDb = await _context.Users.ToListAsync();
            usersInDb.Count.ShouldBe(1);
            usersInDb.First().IsDeleted.ShouldBeFalse();
        }
    }
}
