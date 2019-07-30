using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GCCapstone.Data
{
    public class Enrollment
    {
        public int EnrollmentID { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; }
        public int Grade { get; set; }

    }
}
