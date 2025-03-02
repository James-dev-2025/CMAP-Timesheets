using MediatR;
using Microsoft.EntityFrameworkCore;
using Timesheets.Domain;
using Timesheets.Domain.Entities.Projects;
using Timesheets.Domain.Entities.Users;

namespace Timesheets.Api.Features.Users
{
    public class List
    {
        public class Query : IRequest<Response>
        {
        }

        public class Response
        {
            public List<User> Users { get; set; }
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

                var users = await _context.Users.Where(x => x.IsDeleted == false).ToListAsync(cancellationToken);
                return new Response { Users = users };
            }
        }
    }
}
