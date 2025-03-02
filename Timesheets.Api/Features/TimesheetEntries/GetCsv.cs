using CsvHelper.Configuration.Attributes;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Timesheets.Api.Services.CsvService;
using Timesheets.Domain;

namespace Timesheets.Api.Features.TimesheetEntries
{
    public class GetCsv
    {
        public class Query : IRequest<Response>
        {
            public Guid? UserId { get; set; }
            public Guid? ProjectId { get; set; }
        }

        public class Response
        {
            public byte[] Csv { get; set; }
            public string FileName { get; set; }
        }

        public class EntryDto
        {
            [Name("User Name")]
            public string UserName { get; set; }
            public string Date { get; set; }
            public string Project { get; set; }

            [Name("Description of Tasks")]
            public string Description { get; set; }

            [Name("Hours Worked")]
            public decimal HoursWorked { get; set; }

            [Name("Total Hours for the Day")]
            public decimal HoursWorkedForTheDay { get; set; } 
        }
            

        public class QueryHandler : IRequestHandler<Query, Response>
        {
            private readonly ApplicationDbContext _context;
            private readonly ICsvService _csvService;
            public QueryHandler(ApplicationDbContext context, ICsvService csvService)
            {
                _context = context;
                _csvService = csvService;
            }

            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {

                var entries = await _context
                    .TimesheetEntries
                    .Where(x => request.UserId == null || x.UserId == request.UserId)
                    .Where(x => request.ProjectId == null || x.ProjectId == request.ProjectId)
                    .Include(x => x.User)
                    .Include(x => x.Project)
                    .ToListAsync(cancellationToken);

                var entryDtos = entries
                    .GroupBy(x => new { x.UserId, x.Date.Date })
                    .SelectMany(g => g.Select(x => new EntryDto
                    {
                        UserName = x.User.UserName,
                        Date = x.Date.ToString("d"),
                        Project = x.Project.Name,
                        Description = x.Description,
                        HoursWorked = x.HoursWorked,
                        HoursWorkedForTheDay = g.Sum(e => e.HoursWorked)
                    }))
                    .OrderBy(x => x.UserName)
                    .ThenBy(x => x.Project)
                    .ToList();

                var csv =  _csvService.GenerateCsv(entryDtos);

                return new Response { Csv = csv, FileName = $"Timesheet_{DateTime.Now}" };
            }
        }
    }
}
