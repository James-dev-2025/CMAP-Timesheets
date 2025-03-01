using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Timesheets.Domain.Entities
{
    public class BaseEntity
    {
        public Guid Id { get; protected set; }
        public DateTime DateCreated { get; protected set; }
        public DateTime DateModified { get; protected set; }
        public bool IsDeleted { get; protected set; }
    }
}
