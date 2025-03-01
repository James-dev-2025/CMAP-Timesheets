using MediatR;
using Timesheets.Domain;
using Timesheets.Domain.Entities.Projects;

namespace Timesheets.Api.Features.Projects
{
    public class Create
    {
        public class Command : IRequest<Response>
        {
            public string Name { get; set; }
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

                var project = Project.Create(request.Name);
                _context.Projects.Add(project);
                await _context.SaveChangesAsync(cancellationToken);

                return new Response { Successful = true };
            }
        }
    }
}
