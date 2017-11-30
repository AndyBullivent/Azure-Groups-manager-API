using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.Azure.ActiveDirectory.GraphClient;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace cumbria.services.msgraph
{
    public abstract class BaseGraphObject
    {
        protected AuthenticationContext _authContext;
        protected ClientCredential _clientCreds;
        protected HttpClient _httpClient;
        protected AuthenticationResult _authenticationResult;
        protected ActiveDirectoryClient _activeDirectoryClient;
        protected string _resource;

        public BaseGraphObject(GraphCredentials creds)
        {
            _httpClient = new HttpClient();
            _authContext = new AuthenticationContext(creds.Authority);
            _clientCreds = new ClientCredential(creds.ClientId, creds.Key);
            Uri servicePointUri = new Uri("https://graph.windows.net");
            Uri serviceRoot = new Uri(servicePointUri, creds.Tenant);

            _activeDirectoryClient = new ActiveDirectoryClient(serviceRoot, async () => await GetAppTokenAsync());
            _resource = servicePointUri.ToString();
        }

        private async Task<string> GetAppTokenAsync()
        {
            var token = await _authContext.AcquireTokenAsync(_resource, _clientCreds);
            return token.AccessToken;
        }

        public string AuthHeader
        {
            get
            {
                return _authenticationResult.CreateAuthorizationHeader();
            }
        }

        
        

    }
}
