using System;
using System.Threading;

namespace Pug.Application.Threading
{
    public class WorkersDispatcher<I,T,R>
    {
        private readonly Func<T, R> _work;
        private readonly bool _dispatchFromThreadPool;

        public WorkersDispatcher(I identifier, Func<T,R> work, bool dispatchFromThreadPool)
        {
            this.Identifier = identifier;
            this._work = work;
            this._dispatchFromThreadPool = dispatchFromThreadPool;
        }
        
        public I Identifier { get; }

        public void DispatchWorker(T task)
        {
            if (_dispatchFromThreadPool)
                DispatchWorkerFromThreadPool(task);
            else
                DispatchWorkerThread(task);
        }

        private void DispatchWorkerThread(T task)
        {
            Thread workerThread = new Thread(new ParameterizedThreadStart(DoWork));

            workerThread.Start(task);
        }

        private void DispatchWorkerFromThreadPool(T task)
        {
            ThreadPool.QueueUserWorkItem(DoWork, task);
        }

        private void DoWork(object taskObject)
        {
            T task = (T)taskObject;

            _work(task);
        }
    }
}
