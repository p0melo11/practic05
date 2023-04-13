using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace crossroads
{
    class Program
    {
        public static void Main()
        {
            Console.WriteLine("Starting...");

            var crossRoad = new CrossRoad();
            crossRoad.Start();

            Console.ReadKey();
        }
    }
}
