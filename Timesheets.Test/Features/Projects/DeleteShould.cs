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
    public class DeleteShould : TestBase
    {
        [Fact]
        public async Task Delete_a_Project()
        {
            var existingProject = new ProjectBuilder().WithName("Project Alpha").Build();
            _context.Projects.Add(existingProject);
            await _context.SaveChangesAsync();

            var handler = new Delete.CommandHandler(_context);
            var command = new Delete.Command { Id = existingProject.Id };
            var result = await handler.Handle(command, CancellationToken.None);

            result.Successful.ShouldBeTrue();

            var projectsInDb = await _context.Projects.ToListAsync();
            projectsInDb.Count.ShouldBe(1);
            projectsInDb.First().IsDeleted.ShouldBeTrue();
        }

        [Fact]
        public async Task Be_unsuccessful_if_project_id_does_not_match()
        {
            var existingProject = new ProjectBuilder().WithName("Project Alpha").Build();
            _context.Projects.Add(existingProject);
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
