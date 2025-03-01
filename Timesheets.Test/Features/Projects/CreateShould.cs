using Microsoft.EntityFrameworkCore;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Timesheets.Api.Features.Projects;
using Timesheets.Domain.Entities.Projects;

namespace Timesheets.Test.Features.Projects
{
    public class CreateShould : TestBase
    {
        [Fact]
        public async Task Add_a_Project_to_the_db()
        {
            var handler = new Create.CommandHandler(_context);
            var command = new Create.Command { Name = "Project Zeus" };
            var result = await handler.Handle(command, CancellationToken.None);

            result.Successful.ShouldBeTrue();

            var projectsInDb = await _context.Projects.ToListAsync();
            projectsInDb.Count.ShouldBe(1);
            projectsInDb.First().Name.ShouldBe(command.Name);
        }

        [Fact]
        public async Task Be_unsuccessful_if_project_name_is_empty()
        {
            var handler = new Create.CommandHandler(_context);
            var command = new Create.Command { Name = "" };
            var result = await handler.Handle(command, CancellationToken.None);

            result.Successful.ShouldBeFalse();

            var projectsInDb = await _context.Projects.ToListAsync();
            projectsInDb.Count.ShouldBe(0);
        }
    }
}
