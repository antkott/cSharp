using System;
using System.Threading;

namespace ThreadDemo
{
    class Program
    {
        public static void Main() {
            Worker worker = new Worker();
            Thread workerThread = new Thread(worker.DoWork);
            workerThread.Start();
            Console.WriteLine("Main thread: starting worker thread...");

            // Loop until the worker thread activates.
            while (!workerThread.IsAlive)
                ;

            Thread.Sleep(500);
            worker.RequestStop();
            workerThread.Join();
            Console.WriteLine("Main thread: worker thread has terminated.");
        }
    }

    public class Worker
    {
        private volatile bool _shouldStop;

        public void DoWork()
        {
            bool work = false;
            while (!_shouldStop)
            {
                work = !work;

            }
            Console.WriteLine("Worker thread: terminating gracefully.");
        }

        public void RequestStop()
        {
            _shouldStop = true;
        }
    }
}
