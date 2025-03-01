using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Timesheets.Domain.Entities.Users;

namespace Timesheets.Domain.Entities.Projects
{
    public class Project : BaseEntity
    {
        public string Name { get; private set; }

        public static Project Create(string username) => new()
        {
            Id = Guid.NewGuid(),
            DateCreated = DateTime.UtcNow,
            DateModified = DateTime.UtcNow,
            IsDeleted = false,
            Name = username,
        };
        public void Delete()
        {
            IsDeleted = true;
        }
    }
}
