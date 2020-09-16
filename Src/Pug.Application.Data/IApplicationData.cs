
namespace Pug.Application.Data
{
	public interface IApplicationData<out T> where T : class, IApplicationDataSession
	{
		T GetSession();
	}
}