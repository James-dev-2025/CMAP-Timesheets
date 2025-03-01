using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Timesheets.Api.Services.CsvService;
using Timesheets.Domain.Entities.TimesheetEntries;
using Timesheets.Test.Builders.Projects;
using Timesheets.Test.Builders.Timesheets;
using Timesheets.Test.Builders.Users;

namespace Timesheets.Test.Services.CsvServiceTests
{


    public class CsvServiceShould : TestBase
    {
        private class TestObject
        {
            public string Prop1 { get; set; }
            public int Prop2 { get; set; }
            public string Prop3 { get; set; }
        }

        [Fact]
        public async Task Generate_a_csv()
        {
            var project = new ProjectBuilder().WithName("Project Alpha").Build();
            _context.Projects.Add(project);

            var user = new UserBuilder().WithUserName("Tom").Build();
            _context.Users.Add(user);

            var date1 = DateTime.Now.AddDays(-2).ToString("d");
            var date2 = DateTime.Now.AddDays(-1).ToString("d");

            var entries = new List<TestObject>
            {
                new TestObject{Prop1 = "test", Prop2 = 100, Prop3 = date1 },
                new TestObject{Prop1 = "test2", Prop2 = 1000, Prop3 = date2},
            };

            var service = new CsvService();

            byte[] csvBytes = service.GenerateCsv(entries);
            string csvString = Encoding.UTF8.GetString(csvBytes);

            string[] lines = csvString.Split("\n", System.StringSplitOptions.RemoveEmptyEntries);
            lines.Length.ShouldBe(3); 

            lines[0].ShouldBe("Prop1,Prop2,Prop3\r"); 
            lines[1].ShouldBe($"test,100,{date1}\r");
            lines[2].ShouldBe($"test2,1000,{date2}\r"); 
        }
    }
}
