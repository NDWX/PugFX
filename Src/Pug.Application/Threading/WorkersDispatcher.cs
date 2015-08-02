using System;
using System.Threading;

namespace Pug.Application.Threading
{
    public class WorkersDispatcher<I,T,R>
    {
        I identifier;
		Func<T, R> work;
        bool dispatchFromThreadPool;

        public WorkersDispatcher(I identifier, Func<T,R> work, bool dispatchFromThreadPool)
        {
            this.identifier = identifier;
            this.work = work;
            this.dispatchFromThreadPool = dispatchFromThreadPool;
        }

        public void DispatchWorker(T task)
        {
            if (dispatchFromThreadPool)
                DispatchWorkerFromThreadPool(task);
            else
                DispatchWorkerThread(task);
        }

        void DispatchWorkerThread(T task)
        {
            Thread workerThread = new Thread(new ParameterizedThreadStart(DoWork));

            workerThread.Start(task);
        }

        void DispatchWorkerFromThreadPool(T task)
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(DoWork), task);
        }

        void DoWork(object taskObject)
        {
            T task = (T)taskObject;

            work(task);
        }
    }
}
