using FakeItEasy;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using System.Text;
using Timesheets.Api.Features.TimesheetEntries;
using Timesheets.Api.Services.CsvService;
using Timesheets.Test.Builders.Projects;
using Timesheets.Test.Builders.Timesheets;
using Timesheets.Test.Builders.Users;
using static Timesheets.Api.Features.TimesheetEntries.GetCsv;


namespace Timesheets.Test.Features.TimesheetEntries
{
    public class GetCsvShould : TestBase
    {
        [Fact]
        public async Task Call_csv_service()
        {
            var csvService = A.Fake<ICsvService>();

            var expectedEntries = new List<EntryDto> { };

            var handler = new GetCsv.QueryHandler( _context, csvService); 
            await handler.Handle(new GetCsv.Query { UserId = null, ProjectId = null }, CancellationToken.None);
            A.CallTo(() => csvService.GenerateCsv(A<List<EntryDto>>.Ignored)).MustHaveHappenedOnceExactly();
        }
    }
}
