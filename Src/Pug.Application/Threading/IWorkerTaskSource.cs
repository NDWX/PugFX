namespace Pug.Application.Threading
{
	public interface IWorkerTaskSource<T>// : IWaitable<T>
    {
        bool HasTasks { get; }
        bool GetNextTask(ref T task);
		bool GetNextTask(int waitTimeout, ref T task);
		//void GetNextTask(ResourceWaitContext<T> context);
    }
}
