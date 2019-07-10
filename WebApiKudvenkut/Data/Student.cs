using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiKudvenkut.Data
{
    public class Student
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; }

        public List<Student> GetStudentList { get; set; }

        public string[] studentList { get; set; }
    }
}