using MediatR;
using Microsoft.EntityFrameworkCore;
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
            public decimal HoursWorked { get; set; }
            public Guid UserId { get; set; }
            public Guid ProjectId { get; set; }
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
                if (request.UserId == Guid.Empty || request.ProjectId == Guid.Empty)
                {
                    return new Response { Successful = false };
                }


                var entry = await _context.TimesheetEntries.SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
                if (entry == null)
                {
                    return new Response { Successful = false };
                }

                entry.Update(request.Date,request.Description,request.HoursWorked,request.UserId,request.ProjectId);

                await _context.SaveChangesAsync(cancellationToken);

                return new Response { Successful = true };
            }
        }
    }
}
