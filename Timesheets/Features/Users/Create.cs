using MediatR;
using Timesheets.Domain;
using Timesheets.Domain.Entities.Projects;
using Timesheets.Domain.Entities.Users;

namespace Timesheets.Api.Features.Users
{
    public class Create
    {
        public class Command : IRequest<Response>
        {
            public string UserName { get; set; }
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

                var user = User.Create(request.UserName);
                _context.Users.Add(user);
                await _context.SaveChangesAsync(cancellationToken);

                return new Response { Successful = true };
            }
        }
    }
}
