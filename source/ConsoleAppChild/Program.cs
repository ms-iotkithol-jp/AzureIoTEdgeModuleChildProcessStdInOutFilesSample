//#define LINUX
using System;
using System.IO;

namespace ConsoleAppChild
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Child!");
            try
            {
                string filename = "";
#if LINUX
                filename = "/data/test.txt";
#else
                filename = "c:\\data\\test.txt";
#endif
                var currentDir = Directory.GetCurrentDirectory();
                Console.WriteLine($"{currentDir}");
                using (var file = File.OpenWrite(filename))
                {
                    using (var writer = new StreamWriter(file))
                    {
                        writer.WriteLine("test!");
                        writer.Flush();
                        Console.WriteLine("Wrote file!");
                    }
                }
                while (true)
                {
                    Console.WriteLine("Waiting input from parent");
                    string line = Console.ReadLine();
                    if (!string.IsNullOrEmpty(line))
                    {
                        Console.WriteLine($"Child Received - '{line}'");
                        if (line.Trim().ToLower() == "quit") break;
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Exception - {ex.Message}");
            }
        }
    }
}
