using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{

    public delegate bool CheckStudentNameRepeatEventHandler(Student obj, List<Student> StudentList);
    public class Student
    {
        public string UserName { get; set; }
        public string Sex { get; set; }
    }

    public class CLassRoom
    {
        public event CheckStudentNameRepeatEventHandler DoCheck;
        private static void EnglishGreeting(string name)
        {
            Console.WriteLine("Morning, " + name);
        }
    }
}
