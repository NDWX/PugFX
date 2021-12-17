﻿using System;
using System.Threading;

namespace Pug.Application.Threading
{
    public class WorkersDispatcher<I,T,R>
    {
        private I _identifier;
        private Func<T, R> _work;
        private bool _dispatchFromThreadPool;

        public WorkersDispatcher(I identifier, Func<T,R> work, bool dispatchFromThreadPool)
        {
            this._identifier = identifier;
            this._work = work;
            this._dispatchFromThreadPool = dispatchFromThreadPool;
        }

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
