using Microsoft.EntityFrameworkCore;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Timesheets.Api.Features.Projects;
using Timesheets.Domain.Entities.Projects;
using Timesheets.Test.Builders.Projects;

namespace Timesheets.Test.Features.Projects
{
    public class UpdateShould : TestBase
    {
        [Fact]
        public async Task Update_a_Project()
        {
            var existingProject = new ProjectBuilder().WithName("Project Alpha").Build();
            _context.Projects.Add(existingProject);
            await _context.SaveChangesAsync();

            var handler = new Update.CommandHandler(_context);
            var command = new Update.Command { Id = existingProject.Id, Name = "Project Beta" };
            var result = await handler.Handle(command, CancellationToken.None);

            result.Successful.ShouldBeTrue();

            var projectsInDb = await _context.Projects.ToListAsync();
            projectsInDb.Count.ShouldBe(1);
            projectsInDb.First().Name.ShouldBe(command.Name);
        }

        [Fact]
        public async Task Be_unsuccessful_if_project_name_is_empty()
        {
            var existingProject = new ProjectBuilder().WithName("Project Alpha").Build();
            _context.Projects.Add(existingProject);
            await _context.SaveChangesAsync();

            var handler = new Update.CommandHandler(_context);
            var command = new Update.Command { Id = existingProject.Id, Name = "" };
            var result = await handler.Handle(command, CancellationToken.None);

            result.Successful.ShouldBeFalse();

            var projectsInDb = await _context.Projects.ToListAsync();
            projectsInDb.Count.ShouldBe(1);
            projectsInDb.First().Name.ShouldBe(existingProject.Name);
        }

        [Fact]
        public async Task Be_unsuccessful_if_project_does_not_exist()
        {
            var handler = new Update.CommandHandler(_context);
            var command = new Update.Command { Id = Guid.Empty, Name = "Project Beta" };
            var result = await handler.Handle(command, CancellationToken.None);

            result.Successful.ShouldBeFalse();

            var projectsInDb = await _context.Projects.ToListAsync();
            projectsInDb.Count.ShouldBe(0);
        }
    }
}
