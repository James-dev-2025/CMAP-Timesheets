using MediatR;
using Microsoft.EntityFrameworkCore;
using Timesheets.Domain;

namespace Timesheets.Api.Features.Users
{
    public class Update
    {
        public class Command : IRequest<Response>
        {
            public Guid Id { get; set; }
            public string UserName { get;  set; }
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
                if (string.IsNullOrEmpty(request.UserName))
                {
                    return new Response { Successful = false };
                }

                var users = await _context.Users.SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
                if (users == null)
                {
                    return new Response { Successful = false };
                }

                users.Update(request.UserName);

                await _context.SaveChangesAsync(cancellationToken);

                return new Response { Successful = true };
            }
        }
    }
}
