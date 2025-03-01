using MediatR;
using Timesheets.Domain;

namespace Timesheets.Api.Features.Projects
{
    public class Delete
    {
        public class Command : IRequest<Response>
        {
            public Guid Id { get; set; }
        }

        public class Response
        {
        }

        public class QueryHandler : IRequestHandler<Command, Response>
        {
            private readonly ApplicationDbContext _context;
            public QueryHandler(ApplicationDbContext context)
            {
                _context = context;   
            }

            public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
            {
                
                throw new NotImplementedException();
            }
        }
    }
}
