using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Timesheets.Domain.Entities.Users
{
    public class User : BaseEntity
    {
        public string UserName { get; private set; }

        public static User Create(string username) => new()
        {
            Id = Guid.NewGuid(),
            DateCreated = DateTime.UtcNow,
            DateModified = DateTime.UtcNow,
            IsDeleted = false,
            UserName = username,
        };
        public void Delete()
        {
            IsDeleted = true;
        }
    }
}
