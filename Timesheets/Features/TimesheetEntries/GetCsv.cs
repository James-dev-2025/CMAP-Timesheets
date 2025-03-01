using MediatR;
using Timesheets.Domain;

namespace Timesheets.Api.Features.TimesheetEntries
{
    public class GetCsv
    {
        public class Query : IRequest<Response>
        {
            public Guid UserId { get; set; }
            public Guid ProjectId { get; set; }
        }

        public class Response
        {
            public Guid Id { get; set; }
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
