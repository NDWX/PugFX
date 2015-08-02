using System;

namespace Pug.Application.Threading
{
    public interface IWorkerTaskWait<T>
    {
        bool HasTasks { get; }
        T GetNextTask();
        T GetNextTask(int waitTimeout);
    }
}
