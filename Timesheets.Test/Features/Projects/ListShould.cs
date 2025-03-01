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
    public class ListShould : TestBase
    {
        [Fact]
        public async Task Return_projects()
        {
            var existingProject1 = new ProjectBuilder().WithName("Project Alpha").Build();
            var existingProject2 = new ProjectBuilder().WithName("Project Beta").Build();
            var existingProject3 = new ProjectBuilder().WithName("Project Charlie").Build();
            _context.Projects.AddRange([existingProject1, existingProject2, existingProject3]);
            await _context.SaveChangesAsync();

            var handler = new List.QueryHandler(_context);
            var query = new List.Query { };
            var result = await handler.Handle(query, CancellationToken.None);

            result.Projects.Count.ShouldBe(3);

            var project1 = result.Projects.SingleOrDefault(x => x.Id == existingProject1.Id).ShouldNotBeNull();
            var project2 = result.Projects.SingleOrDefault(x => x.Id == existingProject2.Id).ShouldNotBeNull();
            var project3 = result.Projects.SingleOrDefault(x => x.Id == existingProject3.Id).ShouldNotBeNull();
        }

        [Fact]
        public async Task Not_return_projects_that_are_deleted()
        {
            var existingProject1 = new ProjectBuilder().WithName("Project Alpha").Build();
            var existingProject2 = new ProjectBuilder().WithName("Project Beta").Build();
            var existingProject3 = new ProjectBuilder().WithName("Project Charlie").WithIsDeleted(true).Build();
            _context.Projects.AddRange([existingProject1, existingProject2, existingProject3]);
            await _context.SaveChangesAsync();

            var handler = new List.QueryHandler(_context);
            var query = new List.Query { };
            var result = await handler.Handle(query, CancellationToken.None);

            result.Projects.Count.ShouldBe(2);


            result.Projects.SingleOrDefault(x => x.Id == existingProject1.Id).ShouldNotBeNull();
            result.Projects.SingleOrDefault(x => x.Id == existingProject2.Id).ShouldNotBeNull();
            result.Projects.SingleOrDefault(x => x.Id == existingProject3.Id).ShouldBeNull();
        }
    }
}
