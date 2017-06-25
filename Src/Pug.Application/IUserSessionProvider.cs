namespace Pug.Application
{
	public interface IApplicationUserSessionProvider
	{
		ApplicationUserSession CurrentSession
		{
			get;
		}
	}
}
