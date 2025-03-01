using MediatR;
using Microsoft.EntityFrameworkCore;
using Timesheets.Domain;

namespace Timesheets.Api.Features.Projects
{
    public class Update
    {
        public class Command : IRequest<Response>
        {
            public Guid Id { get; set; }
            public string Name { get;  set; }
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
                if (string.IsNullOrEmpty(request.Name))
                {
                    return new Response { Successful = false };
                }

                var project = await _context.Projects.SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
                if (project == null)
                {
                    return new Response { Successful = false };
                }

                project.Update(request.Name);

                await _context.SaveChangesAsync(cancellationToken);

                return new Response { Successful = true };
            }
        }
    }
}
