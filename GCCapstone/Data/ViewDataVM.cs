using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GCCapstone.Data
{
    public class ViewDataVM
    {
        public User CurrentUser { get; set; }
        public List<User> Users { get; set; }
        public List<Course> Courses { get; set; }
        public List<Enrollment> Enrollments { get; set; }
    }
}
