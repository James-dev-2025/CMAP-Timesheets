using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Timesheets.Domain.Entities.Projects;
using Timesheets.Domain.Entities.Users;

namespace Timesheets.Domain.Entities.TimesheetEntries
{
    public class TimesheetEntry : BaseEntity
    {
        public DateTime Date { get; private set; }
        public string Description { get; private set; }
        public decimal HoursWorked { get; private set; }

        public Guid UserId { get; private set; }
        public User User { get; private set; }

        public Guid ProjectId { get; private set; }
        public Project Project { get; private set; }

        public static TimesheetEntry Create(DateTime date, string description, decimal hoursWorked, Guid userId, Guid projectId) => new()
        {
            Id = Guid.NewGuid(),
            DateCreated = DateTime.UtcNow,
            DateModified = DateTime.UtcNow,
            IsDeleted = false,
            Date = date,
            Description = description,
            HoursWorked = hoursWorked,
            UserId = userId,
            ProjectId = projectId
        };

        public void Update(DateTime date, string description, decimal hoursWorked, Guid userId, Guid projectId)
        {
            DateModified = DateTime.UtcNow;
            Date = date;
            Description = description;
            HoursWorked = hoursWorked;
            UserId = userId;
            ProjectId = projectId;
        }

        public void Delete()
        {
            IsDeleted = true;
        }
    }
}
