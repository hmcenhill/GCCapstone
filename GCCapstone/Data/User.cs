﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GCCapstone.Data
{
    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Role { get; set; }
        public bool IsAdmin { get; set; }



        public ICollection<Enrollment> Enrollment { get; set; }

        public void Play()
        {
            Console.WriteLine(Enrollment.Count);
            foreach(var enrollment in Enrollment)
            {
                Console.WriteLine(enrollment.Course);
            }

        }

    }
}
