using Bogus;
using Timesheets.Domain.Entities.Projects;
using Timesheets.Domain.Entities.TimesheetEntries;

namespace Timesheets.Test.Builders.Timesheets
{
    public class TimesheetEntryBuilder
    {
        private DateTime? _date;
        private string? _description;
        private TimeSpan? _hoursWorked;

        private Guid _userId = Guid.NewGuid();
        private Guid _projectId = Guid.NewGuid();

        private bool _isDeleted = false;

        public TimesheetEntryBuilder WithDate(DateTime date)
        {
            _date = date;
            return this;
        }

        public TimesheetEntryBuilder WithDescription(string description)
        {
            _description = description;
            return this;
        }

        public TimesheetEntryBuilder WithHoursWorked(TimeSpan hoursWorked)
        {
            _hoursWorked = hoursWorked;
            return this;
        }

        public TimesheetEntryBuilder WithUserId(Guid userId)
        {
            _userId = userId;
            return this;
        }

        public TimesheetEntryBuilder WithProjectId(Guid projectId)
        {
            _projectId = projectId;
            return this;
        }

        public TimesheetEntryBuilder WithIsDeleted(bool isdeleted)
        {
            _isDeleted = isdeleted;
            return this;
        }

        public TimesheetEntry Build()
        {
            var faker = new Faker<TimesheetEntry>()
                .RuleFor(x => x.Id, x => Guid.NewGuid())
                .RuleFor(x => x.DateCreated, x => DateTime.Now)
                .RuleFor(x => x.DateModified, x => DateTime.Now)
                .RuleFor(x => x.IsDeleted, x => _isDeleted)
                .RuleFor(x => x.Date, x => _date ?? x.Date.Recent())
                .RuleFor(x => x.Description, x => _description ?? x.Random.Words(20))
                .RuleFor(x => x.HoursWorked, x => _hoursWorked ?? x.Date.Timespan(TimeSpan.FromHours(8)))
                .RuleFor(x => x.UserId, x => _userId)
                .RuleFor(x => x.ProjectId, x => _projectId);

            return faker.Generate(1).First();
        }
    }
}
