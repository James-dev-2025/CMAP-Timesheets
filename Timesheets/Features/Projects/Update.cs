using MediatR;
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
                
                throw new NotImplementedException();
            }
        }
    }
}
