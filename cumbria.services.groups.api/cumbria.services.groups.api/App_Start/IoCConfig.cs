using cumbria.services.msgraph;
using cumbria.services.storage;
using DryIoc;
using Owin;
using DryIoc.WebApi.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using DryIoc.WebApi;
using cumbria.services.groups.api.Config;

namespace cumbria.services.groups.api
{
    public class IoCConfig
    {
        public static void RegisterIoC(HttpConfiguration config)
        {
            var appCfg = new AppConfiguration();
            var di = new DryIoc.Container();
            var creds =
                new GraphCredentials
                {
                    ClientId = appCfg.ClientId, // "5c642f80-ae79-4d3a-a753-5498eeb2e7d0",
                    Key = appCfg.Key, //"6WxvoAUri6JXdEDIdTISz/SfCRZa7NUZCL7nAl4lcoM=",
                    Tenant = appCfg.TenantId //"e58cae89-8f91-4f69-8cce-51abf1d13b44"
                };

            di.Register<IGroupRepository, GroupRepository>();
            di.Register<IUserGroupManager, UserGroupManager>(Made.Of(() =>
                new UserGroupManager(Arg.Of<GraphCredentials>(creds), Arg.Of<IGroupRepository>())));             

            di.WithWebApi(config);
        }
    }
}