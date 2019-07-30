using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GCCapstone.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        { }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<Enrollment> Enrollments { get; set; }

    }
}
