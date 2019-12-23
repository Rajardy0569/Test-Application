using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Serilog;

namespace Testing_Application
{
    public class LongRunningJobs
    {
        public void CopyDataToAnotherFile(Queue qe)
        {
            string clearDestinationFileData = @"C:\Test App\destination\file2.txt";
            File.WriteAllText(clearDestinationFileData, String.Empty);
            Log.Information("Copy Started    {Now}", DateTime.Now);
            using (var reader = new System.IO.StreamReader(@"C:\Test App\src\file1.txt"))
            using (var writer = new System.IO.StreamWriter(clearDestinationFileData))
            {
                while (reader.Peek() >= 0)
                {
                    string line = reader.ReadLine();
                    char[] stripped = line.Where(c => char.IsLetter(c)).ToArray();
                    writer.WriteLine(stripped);
                }
            }
            Log.Information("Copy Completed    {Now} ---{ThreadID}", DateTime.Now, Thread.CurrentThread.ManagedThreadId);
            lock (qe)
            {
                Log.Information("(Dequeue)\t{0}", qe.Dequeue());
            }
            Console.WriteLine("Copy Job Completed");
            Thread.Sleep(4000);
        }

        public void RandomNumbers(int frm, int to, Queue qe)
        {
            string filePath = @"C:\Test App\destination\RandomNumbers.txt";
            File.WriteAllText(filePath, String.Empty);
            Log.Information("RandomNumbers Printing Started  {Now}", DateTime.Now);
            Random r = new Random();
            int number = 0;
            using (StreamWriter writer = File.AppendText(filePath))
            {
                writer.Write(DateTime.Now + "\n");
                for (int i = 1; i < 10; i++)
                {
                    number = r.Next(frm, to);
                    writer.Write(number + "\n");
                }
                writer.Write(DateTime.Now + "\n");
            }
            Log.Information("RandomNumbers Printing Completed  {Now} ---{Threadid}", DateTime.Now, Thread.CurrentThread.ManagedThreadId);
            lock (qe)
            {
                Log.Information("(Dequeue)\t{0}", qe.Dequeue());
            }
            Console.WriteLine("RandomNumber Job Completed");
            Thread.Sleep(4000);
        }


        public void Add(int a, int b, Queue qe)
        {
            Log.Information("Add job is started  {Now}", DateTime.Now);
            Log.Information("Add result  {Sum}", a + b);
            Log.Information("Add job is completed  {Now}----{Threadid}", DateTime.Now, Thread.CurrentThread.ManagedThreadId);
            lock (qe)
            {
                Log.Information("(Dequeue)\t{0}", qe.Dequeue());
            }
            Thread.Sleep(7000);
            Console.WriteLine("Addition Job Completed");
        }
        public void Subtract(int a, int b, Queue qe)
        {
            Log.Information("subtract job is started  {Now}", DateTime.Now);
            Log.Information("subtract result  {Sum}", a - b);
            Log.Information("subtract job is completed  {Now}------{Threadid}", DateTime.Now, Thread.CurrentThread.ManagedThreadId);
            lock (qe)
            {
                Log.Information("(Dequeue)\t{0}", qe.Dequeue());
            }
            Console.WriteLine("Subtract Job Completed");
            Thread.Sleep(3000);
        }
    }
}
