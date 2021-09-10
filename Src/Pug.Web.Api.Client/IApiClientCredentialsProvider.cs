using System.Collections.Generic;
using System.Net.Http.Headers;

namespace Pug.Web.Api.Client
{
	public interface IApiClientCredentialsProvider
	{
		void PopulateAuthorizationClaim(HttpRequestHeaders headers, out IDictionary<string, string> queries);
	}
}