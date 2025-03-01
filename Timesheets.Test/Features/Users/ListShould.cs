using Microsoft.EntityFrameworkCore;
using Shouldly;
using Timesheets.Api.Features.Users;
using Timesheets.Test.Builders.Users;

namespace Timesheets.Test.Features.Users
{
    public class ListShould : TestBase
    {
        [Fact]
        public async Task Return_users()
        {
            var existingUser1 = new UserBuilder().WithUserName("Project Alpha").Build();
            var existingUser2 = new UserBuilder().WithUserName("Project Beta").Build();
            var existingUser3 = new UserBuilder().WithUserName("Project Charlie").Build();
            _context.Users.AddRange([existingUser1, existingUser2, existingUser3]);
            await _context.SaveChangesAsync();

            var handler = new List.QueryHandler(_context);
            var query = new List.Query { };
            var result = await handler.Handle(query, CancellationToken.None);

            result.Users.Count.ShouldBe(3);

            var user1 = result.Users.SingleOrDefault(x => x.Id == existingUser1.Id).ShouldNotBeNull();
            var user2 = result.Users.SingleOrDefault(x => x.Id == existingUser2.Id).ShouldNotBeNull();
            var user3 = result.Users.SingleOrDefault(x => x.Id == existingUser3.Id).ShouldNotBeNull();
        }

        [Fact]
        public async Task Not_return_users_that_are_deleted()
        {
            var existingUser1 = new UserBuilder().WithUserName("Project Alpha").Build();
            var existingUser2 = new UserBuilder().WithUserName("Project Beta").Build();
            var existingUser3 = new UserBuilder().WithUserName("Project Charlie").WithIsDeleted(true).Build();
            _context.Users.AddRange([existingUser1, existingUser2, existingUser3]);
            await _context.SaveChangesAsync();

            var handler = new List.QueryHandler(_context);
            var query = new List.Query { };
            var result = await handler.Handle(query, CancellationToken.None);

            result.Users.Count.ShouldBe(3);;

            result.Users.SingleOrDefault(x => x.Id == existingUser1.Id).ShouldNotBeNull();
            result.Users.SingleOrDefault(x => x.Id == existingUser2.Id).ShouldNotBeNull();
            result.Users.SingleOrDefault(x => x.Id == existingUser3.Id).ShouldBeNull();
        }
    }
}
