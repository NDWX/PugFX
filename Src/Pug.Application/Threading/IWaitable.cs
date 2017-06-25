namespace Pug.Application.Threading
{
	public interface IWaitable
	{
		bool Wait();
		bool Wait(int timeout);
	}

	public interface IWaitable<R>
	{
		bool Wait(ref R result);
		bool Wait(int timeout, ref R result);
	}
}
