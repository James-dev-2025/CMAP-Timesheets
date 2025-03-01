using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Timesheets.Domain.Entities.Projects;
using Timesheets.Domain.Entities.TimesheetEntries;
using Timesheets.Domain.Entities.Users;
using Timesheets.Domain.EntityConfiguration.Projects;
using Timesheets.Domain.EntityConfiguration.TimesheetEntries;
using Timesheets.Domain.EntityConfiguration.Users;

namespace Timesheets.Domain
{
    public class ApplicationDbContext : DbContext
    {

        #region Db Sets

        public DbSet<User> Users { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<TimesheetEntry> TimesheetEntries { get; set; }

        #endregion Db Sets


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            #region Entity Configuration

            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new ProjectConfiguration());
            modelBuilder.ApplyConfiguration(new TimesheetEntryConfiguration());

            #endregion Entity Configuration
        }
    }
}
