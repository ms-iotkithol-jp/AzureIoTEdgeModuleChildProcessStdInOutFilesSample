using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace ConsoleAppInOutTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Parent");
            Console.WriteLine($"CurrentDir={Directory.GetCurrentDirectory()}");

            var spInfo = new ProcessStartInfo();
            spInfo.FileName = args[0];
            spInfo.UseShellExecute = false;
            spInfo.RedirectStandardOutput = true;
            spInfo.RedirectStandardError = true;
            spInfo.RedirectStandardInput = true;

            var process = new Process();
            process.StartInfo = spInfo;
            process.OutputDataReceived += Process_OutputDataReceived;
            process.ErrorDataReceived += Process_ErrorDataReceived;
            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            while (true)
            {
                lock (process)
                {
                    if (outputCount > 0) break;
                }
                Console.WriteLine("Waiting for Child process ready");
                Task.Delay(1000).Wait();
            }

            int index = 0;
            while (index < 10)
            {
                string line = $"test{index++}";
                Console.WriteLine(line);
                process.StandardInput.WriteLine(line);
                Console.WriteLine("transfered");
                Task.Delay(1000).Wait();
            }
            process.StandardInput.WriteLine("quit");
            Console.WriteLine("Waiting for Child end");
            process.WaitForExit();

        }

        private static int outputCount = 0;
        private static void Process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            Console.WriteLine($"Child Error: {e.Data}");
        }

        private static void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            Console.WriteLine($"Child Output: {e.Data}");
            outputCount++;
        }
    }
}
