using MediatR;
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
                
                throw new NotImplementedException();
            }
        }
    }
}
