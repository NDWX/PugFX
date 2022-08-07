using System;
using System.Threading;

namespace Pug.Application.Security
{
	internal class SecurityContext
	{
		public IUser CurrentUser { get; set; }
	}

    public class SecurityManager : ISecurityManager
	{
		private readonly string _application;
		private readonly AsyncLocal<SecurityContext> asyncContext;

		public SecurityManager(string application, ISessionUserIdentityAccessor sessionUserIdentityAccessor, IUserRoleProvider userRoleProvider, IAuthorizationProvider uathorizationProvider)
		{
			_application = application ?? throw new ArgumentNullException( nameof(application) );
			SessionUserIdentityAccessor = sessionUserIdentityAccessor ?? throw new ArgumentNullException( nameof(sessionUserIdentityAccessor) );
			UserRoleProvider = userRoleProvider;
			AuthorizationProvider = uathorizationProvider;

			asyncContext = new AsyncLocal<SecurityContext>()
			{
				Value = new SecurityContext()
			};
		}

		internal SecurityContext SecurityContext
		{
			get
			{
				SecurityContext securityContext = asyncContext.Value;

				if ( securityContext is not null )
					return securityContext;

				securityContext = new SecurityContext();

				SecurityContext = securityContext;

				return securityContext;
			}
			private set
			{
				asyncContext.Value = value;
			}
		}

		public IUser CurrentUser
		{
			get
			{
				SecurityContext securityContext = asyncContext.Value;
				
				if( securityContext.CurrentUser is not null )
					return securityContext.CurrentUser;
				
				IPrincipalIdentity userIdentity = SessionUserIdentityAccessor.GetUserIdentity();

				if( userIdentity is null )
					return null;

				IUser currentUser = new User( userIdentity, UserRoleProvider, AuthorizationProvider );

				securityContext.CurrentUser = currentUser;
				
				return currentUser;
			}
		}

		protected ISessionUserIdentityAccessor SessionUserIdentityAccessor { get; }

		public  IUserRoleProvider UserRoleProvider { get;  }

        public IAuthorizationProvider AuthorizationProvider { get; }
	}
}