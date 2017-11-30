using System;
using System.Threading.Tasks;
using IdentityServer3.AccessTokenValidation;
using System.Web.Http;
using System.Net.Http.Formatting;
using Owin;
using System.IdentityModel.Tokens;
using System.Collections.Generic;
using Microsoft.Owin;
using cumbria.services.groups.api.Config;

[assembly: OwinStartup(typeof(cumbria.services.groups.api.Startup))]

namespace cumbria.services.groups.api
{
    public class Startup
    {
        AppConfiguration _appConfig = new AppConfiguration();

        public void Configuration(IAppBuilder app)
        {
            // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=316888
            JwtSecurityTokenHandler.InboundClaimTypeMap = new Dictionary<string, string>(); //Prevents the re-mapping of JWT claims to Microsoft ones.
            // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=316888
            app.Map("/api", webApi =>
            {
                webApi.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll); //Enable CORS for everything from everywhere. May want to lock this down later.

                webApi.UseIdentityServerBearerTokenAuthentication(new IdentityServerBearerTokenAuthenticationOptions
                {
                    Authority = _appConfig.Authority,
                    EnableValidationResultCache = true,
                    ValidationResultCacheDuration = new TimeSpan(0, 5, 0),
                    RequiredScopes = new[] { "myday-api" },
                    RoleClaimType = "role",
                    NameClaimType = "preferred_username"
                });

                //Setup JSON serialisation formatting
                var config = new HttpConfiguration();

                

                config.Formatters.Clear();
                config.Formatters.Add(new JsonMediaTypeFormatter());

                //Ensures serialisation of json objects use camel case property names and do not include null values in json response.
                config.Formatters.JsonFormatter.SerializerSettings =
                    new Newtonsoft.Json.JsonSerializerSettings
                    {
                        ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver(),
                        NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore
                    };

                //Force all enumerations values in models to be serialised to strings instead of integers
                config.Formatters.JsonFormatter.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());

                // Web API routes using attribute routing only
                WebApiConfig.Register(config);
                IoCConfig.RegisterIoC(config);
                webApi.UseWebApi(config);
            });
        }

    }
}
