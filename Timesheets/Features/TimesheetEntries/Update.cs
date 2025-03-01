using MediatR;
using Timesheets.Domain;

namespace Timesheets.Api.Features.TimesheetEntries
{
    public class Update
    {
        public class Command : IRequest<Response>
        {
            public Guid Id { get; set; }
            public DateTime Date { get; set; }
            public string Description { get; set; }
            public TimeSpan HoursWorked { get; set; }
            public Guid UserId { get; set; }
            public Guid ProjectId { get; set; }
        }

        public class Response
        {
            public bool Successful { get; set; }
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
