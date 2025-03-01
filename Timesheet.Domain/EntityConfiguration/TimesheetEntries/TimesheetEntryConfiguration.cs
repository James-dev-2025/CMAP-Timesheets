using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Timesheets.Domain.Entities.Projects;
using Timesheets.Domain.Entities.Users;
using Timesheets.Domain.Entities.TimesheetEntries;

namespace Timesheets.Domain.EntityConfiguration.TimesheetEntries
{
    public class TimesheetEntryConfiguration : IEntityTypeConfiguration<TimesheetEntry>
    {
        public void Configure(EntityTypeBuilder<TimesheetEntry> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Project)
                .WithMany()
                .HasForeignKey(x => x.ProjectId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => x.ProjectId);

            builder.HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => x.UserId);
        }
    }
}
