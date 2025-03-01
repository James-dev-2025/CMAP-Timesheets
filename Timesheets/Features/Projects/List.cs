using MediatR;
using Microsoft.EntityFrameworkCore;
using Timesheets.Domain;
using Timesheets.Domain.Entities.Projects;

namespace Timesheets.Api.Features.Projects
{
    public class List
    {
        public class Query : IRequest<Response>
        {
        }

        public class Response
        {
            public List<Project> Projects { get; set; }
        }

        public class QueryHandler : IRequestHandler<Query, Response>
        {
            private readonly ApplicationDbContext _context;
            public QueryHandler(ApplicationDbContext context)
            {
                _context = context;   
            }

            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                var projects = await _context.Projects.Where(x => x.IsDeleted == false).ToListAsync(cancellationToken);
                return new Response { Projects = projects };
            }
        }
    }
}
