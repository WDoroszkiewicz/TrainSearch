using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;

namespace Presentation
{
    class Program
    {
        static void Main(string[] args)
        {
            var now = DateTime.Now;
            var start = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);

            for (int i = 0; i < 24; i++)
            {
                Console.WriteLine(start);
                start = start.AddHours(1);
            }
            Console.ReadLine();
        }
    }
}
