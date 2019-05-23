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
            var connectionPeeker = new ConnectionPeeker();
            connectionPeeker.ToWroclawGlowny(DateTime.Today);
            var result = connectionPeeker.RunQuery();
            foreach (var item in result)
            {
                Console.WriteLine(item.ToString());
            }
            Console.ReadLine();
        }
    }
}
