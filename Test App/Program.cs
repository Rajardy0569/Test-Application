using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;
using System.Collections;
using Testing_Application;
using System.Threading;

namespace Test_App
{
    class Program
    {
        static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
               .MinimumLevel.Debug()
               .WriteTo.File(@"C:\Logs\myapp.txt")
               .CreateLogger();
            Log.Information("Application starting");
            Queue qe = new Queue();
            Console.WriteLine("The below are the list of jobs \n 1. CopyDataToAnotherFile \n 2.RandomNumbers \n 3.Add \n 4.Subtract \n");
            Console.WriteLine("Currently  1,2 number jobs are added to the queue, if you want you can add with respective number");
            lock (qe)
            {
                qe.Enqueue(1);
                qe.Enqueue(2);
            }
            // qe.Enqueue(3);
            // qe.Enqueue(4);
            // Thread t = new Thread(()=>MyMethodAsync(qe));
            // Task task = Task.Run((Action)MyMethodAsync(qe));


            string str = "";
            while (str != null)
            {
                Thread t = new Thread(() => MyMethodAsync(qe));
                t.IsBackground = true;
                t.Start();
                str = Console.ReadLine();
                Console.WriteLine("Received input is " + str);
                Log.Information("added to queue {Value}", str);
                lock (qe)
                {
                    qe.Enqueue(Convert.ToInt32(str));
                }
            }
            Console.ReadKey();
            Log.CloseAndFlush();
        }
        static async Task MyMethodAsync(Queue qe)
        {
            LongRunningJobs longRunningJobs = new LongRunningJobs();
            var t = Task.Factory.StartNew(delegate { });
            int a = qe.Count;
            for (int i = qe.Count; i >= qe.Count; i--)
            {
                string s = string.Empty;
                lock (qe)
                {
                    s = qe.Peek().ToString();
                }
                
                switch (s)
                {

                    case "1":
                        // Log.Information("The number of elements in the Queue {Count}", qe.Count);
                        await t.ContinueWith((prevTask) => longRunningJobs.CopyDataToAnotherFile(qe));
                        break;

                    case "2":
                        // Log.Information("The number of elements in the Queue {Count}", qe.Count);
                        await t.ContinueWith((prevTask) => longRunningJobs.RandomNumbers(1, 10, qe));
                        break;

                    case "3":
                        // Log.Information("The number of elements in the Queue {Count}", qe.Count);
                        await t.ContinueWith((prevTask) => longRunningJobs.Add(10, 100, qe));
                        break;

                    case "4":
                        // Log.Information("The number of elements in the Queue {Count}", qe.Count);
                        await t.ContinueWith((prevTask) => longRunningJobs.Subtract(100, 10, qe));
                        break;
                }
            }
        }
    }
}
