﻿using MediatR;
using Timesheets.Domain;

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
            public Guid Id { get; set; }
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
