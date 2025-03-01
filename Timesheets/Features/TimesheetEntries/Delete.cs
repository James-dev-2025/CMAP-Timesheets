using MediatR;
using Microsoft.EntityFrameworkCore;
using Timesheets.Domain;

namespace Timesheets.Api.Features.TimesheetEntries
{
    public class Delete
    {
        public class Command : IRequest<Response>
        {
            public Guid Id { get; set; }
        }

        public class Response
        {
            public bool Successful { get; set; }
        }

        public class CommandHandler : IRequestHandler<Command, Response>
        {
            private readonly ApplicationDbContext _context;
            public CommandHandler(ApplicationDbContext context)
            {
                _context = context;   
            }

            public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
            {

                var entry = await _context.TimesheetEntries.SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
                if (entry == null)
                {
                    return new Response { Successful = false };
                }

                entry.Delete();
                await _context.SaveChangesAsync(cancellationToken);

                return new Response { Successful = true };
            }
        }
    }
}
