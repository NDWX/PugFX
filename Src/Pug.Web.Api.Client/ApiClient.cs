using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Pug.Web.Api.Client
{
   public class ApiClient
   {
      protected Uri BaseAddress { get; }
      protected string BasePath { get; }
      
#if NETSTANDARD_2_0
      private readonly IHttpClientFactory httpClientFactory;
#else
      private static readonly HttpClient customerClient = new HttpClient();
#endif
      
      public Uri BaseUrl { get; }
      
      public IApiClientCredentialsProvider CredentialsProvider { get; }
      
#if NETSTANDARD_2_0
      
      public ApiClient(Uri baseUrl, IHttpClientFactory httpClientFactory, IApiClientCredentialsProvider credentialsProvider)
      {
         BaseUrl = baseUrl ?? throw new ArgumentNullException(nameof(baseUrl));
         this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
         CredentialsProvider = credentialsProvider ?? throw new ArgumentNullException(nameof(credentialsProvider));
         
         BaseAddress = new Uri(baseUrl.ToString().Replace(baseUrl.AbsolutePath, string.Empty));
         BasePath = baseUrl.AbsolutePath;
      }
      
      public ApiClient(string baseUrl, IHttpClientFactory httpClientFactory, IApiClientCredentialsProvider credentialsProvider)
         : this(new Uri(baseUrl), httpClientFactory, credentialsProvider)
      {
      }
      
#else
      
      public ApiClient(Uri baseUrl, IApiClientCredentialsProvider credentialsProvider)
      {
         BaseUrl = baseUrl ?? throw new ArgumentNullException(nameof(baseUrl));
         CredentialsProvider = credentialsProvider ?? throw new ArgumentNullException(nameof(credentialsProvider));
         BaseAddress = new Uri(baseUrl.ToString().Replace(baseUrl.AbsolutePath, string.Empty));
         BasePath = baseUrl.AbsolutePath;
      }
      
      public ApiClient(string baseUrl, IApiClientCredentialsProvider credentialsProvider)
         : this(new Uri(baseUrl), credentialsProvider)
      {
      }
      
#endif
      
      private HttpClient CreateHttpClient()
      {
         HttpClient httpClient;
#if !NETSTANDARD_2_0
         httpClient = customerClient;
#else
         httpClient = httpClientFactory.CreateClient();
#endif
         if (httpClient.BaseAddress == null)
         {
            httpClient.BaseAddress = BaseAddress;
         }
         return httpClient;
      }
      
      protected string ConstructRequestPath(string path, IEnumerable<KeyValuePair<string, string>> queries)
      {
         string query = queries?.Select(x => $"{WebUtility.UrlEncode(x.Key)}={WebUtility.UrlEncode(x.Value)}").Aggregate((x, y) => $"{x}&{y}")?? string.Empty;
         return $"{string.Join("/", BasePath.TrimEnd('/'), path.TrimStart('/'))}?{query}";

      }
      
      protected HttpRequestMessage CreateRequestMessage(HttpMethod httpMethod, string path, IDictionary<string, string> queries, IDictionary<string, string> headers, MediaTypeWithQualityHeaderValue mediaType, HttpContent content)
      {
         HttpRequestMessage requestMessage = new HttpRequestMessage(httpMethod, string.Empty);
         
         if( mediaType != null )
            requestMessage.Headers.Accept.Add(mediaType);
         
         CredentialsProvider.PopulateAuthorizationClaim(requestMessage.Headers, out IDictionary<string, string> authenticationQueries);
         string requestUri = ConstructRequestPath(path, authenticationQueries == null? queries:queries.Union(authenticationQueries));
         requestMessage.RequestUri = new Uri(BaseAddress, requestUri);
         requestMessage.Content = content;
         
         return requestMessage;
      }
      
      protected async Task<HttpResponseMessage> SendAsync(HttpMethod httpMethod, string path, IDictionary<string, string> queries, IDictionary<string, string> headers, MediaTypeWithQualityHeaderValue mediaType, HttpContent content = null)
      {
         HttpClient client = CreateHttpClient();
         HttpRequestMessage requestMessage = CreateRequestMessage(httpMethod, path, queries, headers, mediaType, content);
         
         return await client.SendAsync(requestMessage);
      }
      
      protected HttpResponseMessage Send(HttpMethod httpMethod, string path, IDictionary<string, string> queries, IDictionary<string, string> headers, MediaTypeWithQualityHeaderValue mediaType, HttpContent content = null)
      {
         Task<HttpResponseMessage> task = SendAsync(httpMethod, path, queries, headers, mediaType, content);
         
         task.Wait();
         
         if (task.IsFaulted)
            throw task.Exception;
         
         return task.Result;
      }
      
      public async Task<HttpResponseMessage> GetAsync(string path, IDictionary<string, string> queries, IDictionary<string, string> headers, MediaTypeWithQualityHeaderValue mediaType)
      {
         return await SendAsync(HttpMethod.Get, path, queries, headers, mediaType);
      }
      
      public async Task<HttpResponseMessage> PostAsync(string path, IDictionary<string, string> queries, IDictionary<string, string> headers, MediaTypeWithQualityHeaderValue mediaType, HttpContent content)
      {
         return await SendAsync(HttpMethod.Post, path, queries, headers, mediaType, content);
      }
      
      public async Task<HttpResponseMessage> PutAsync(string path, IDictionary<string, string> queries, IDictionary<string, string> headers, MediaTypeWithQualityHeaderValue mediaType, HttpContent content)
      {
         return await SendAsync(HttpMethod.Put, path, queries, headers, mediaType, content);
      }
      
      public async Task<HttpResponseMessage> DeleteAsync(string path, IDictionary<string, string> queries, IDictionary<string, string> headers = null, MediaTypeWithQualityHeaderValue mediaType = null)
      {
         return await SendAsync(HttpMethod.Delete, path, queries, headers, mediaType);
      }
      
      public async Task<HttpResponseMessage> PatchAsync(string path, IDictionary<string, string> headers, IDictionary<string, string> queries, MediaTypeWithQualityHeaderValue mediaType, HttpContent content)
      {
         return await SendAsync(new HttpMethod("PATCH"), path, queries, headers, mediaType, content);
      }
      
      public HttpResponseMessage Get(string path, IDictionary<string, string> queries, IDictionary<string, string> headers, MediaTypeWithQualityHeaderValue mediaType)
      {
         return Send(HttpMethod.Get, path, queries, headers, mediaType);
      }
      
      public HttpResponseMessage Post(string path, IDictionary<string, string> queries, IDictionary<string, string> headers, MediaTypeWithQualityHeaderValue mediaType, HttpContent content)
      {
         return Send(HttpMethod.Post, path, queries, headers, mediaType, content);
      }
      
      public HttpResponseMessage Put(string path, IDictionary<string, string> queries, IDictionary<string, string> headers, MediaTypeWithQualityHeaderValue mediaType, HttpContent content)
      {
         return Send(HttpMethod.Put, path, queries, headers, mediaType, content);
      }
      
      public HttpResponseMessage Delete(string path, IDictionary<string, string> queries, IDictionary<string, string> headers = null, MediaTypeWithQualityHeaderValue mediaType = null)
      {
         return Send(HttpMethod.Delete, path, queries, headers, mediaType);
      }
      
      public HttpResponseMessage Patch(string path, IDictionary<string, string> headers, IDictionary<string, string> queries, MediaTypeWithQualityHeaderValue mediaType, HttpContent content)
      {
         return Send(new HttpMethod("PATCH"), path, queries, headers, mediaType, content);
      }
   }
}