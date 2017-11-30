using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Azure;

namespace cumbria.services.groups.api.Config
{
    public class AppConfiguration
    {
        private static string _clientId = CloudConfigurationManager.GetSetting("clientId");
        private static string _key = CloudConfigurationManager.GetSetting("key");
        private static string _tenantId = CloudConfigurationManager.GetSetting("tenantId");
        private static string _authority = CloudConfigurationManager.GetSetting("authority");
        private static string _allowedTenants = CloudConfigurationManager.GetSetting("allowedTenants");

        public string ClientId { get { return _clientId; } }

        public string Key { get { return _key; } }

        public string TenantId { get { return _tenantId; } }

        public string Authority { get { return _authority; } }

        public string AllowedTenants { get { return _allowedTenants; } }
    }
}