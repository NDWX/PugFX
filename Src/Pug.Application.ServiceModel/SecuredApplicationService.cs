using Pug.Application.Security;
using Pug.Application.Data;

namespace Pug.Application.ServiceModel
{
	public abstract class SecuredApplicationService<DS> : ApplicationService<DS>
		where DS : class, IApplicationDataSession
	{
		private ISecurityManager securityManager;

		protected SecuredApplicationService( ISecurityManager securityManager, IApplicationData<DS> applicationDataProvider, IUserSessionProvider userSessionProvider )
			: base( applicationDataProvider, userSessionProvider )
		{
			this.securityManager = securityManager;
		}

		protected ISecurityManager SecurityManager => securityManager;

        protected IUser User => SecurityManager.CurrentUser;
	}
}
