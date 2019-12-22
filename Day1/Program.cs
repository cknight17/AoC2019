using System;
using System.IO;

namespace Day1
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = File.ReadAllLines(Directory.GetCurrentDirectory() + "/Day1/input.txt");
            long sum = 0;
            foreach (var item in input) {
                int mass = Int32.Parse(item);
                sum += (long)allfuel(mass);
                Console.WriteLine(fuel(mass));
            }
            Console.WriteLine("FINAL");
            Console.WriteLine(sum);
        }

        static int allfuel(int mass) {
            int fuelmass = fuel(mass);
            int totalmass = 0;
            while (fuelmass > 0) {
                totalmass += fuelmass;
                fuelmass = fuel(fuelmass);
            }
            return totalmass;
        }

        static int fuel(int mass) {
            return (mass/3)-2;
        }
    }
}
