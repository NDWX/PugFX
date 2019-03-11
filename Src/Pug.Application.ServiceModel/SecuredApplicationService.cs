using Pug.Application.Security;
using Pug.Application.Data;

namespace Pug.Application.ServiceModel
{
	public abstract class SecuredApplicationService<DS> : ApplicationService<DS>
		where DS : class, IApplicationDataSession
	{
		ISecurityManager securityManager;
		IApplicationData<DS> applicationDataProvider;

		protected SecuredApplicationService( ISecurityManager securityManager, IApplicationData<DS> applicationDataProvider )
			: base( applicationDataProvider )
		{
			this.securityManager = securityManager;
		}

		protected ISecurityManager SecurityManager => this.securityManager;
	}
}
