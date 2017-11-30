using cumbria.services.groups.api.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace cumbria.services.groups.api.Controllers
{
    public class AuthorizeTenant : AuthorizeAttribute
    {

        private string TenantId(HttpActionContext actionContext)
        {
            var principal = actionContext.RequestContext.Principal as ClaimsPrincipal;
            var claim = principal.FindFirst("tenant");
            return claim?.Value;
        }

        static readonly IEnumerable<string> allowedTeanants = new AppConfiguration().AllowedTenants?.Split(',');

        /// <summary>
        /// Is valid tenant request
        /// </summary>
        protected bool IsValidTenant(string tenantId)
        {
            return allowedTeanants?.Any(at => at == "*" || at == tenantId) ?? false;
        }


        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            return base.IsAuthorized(actionContext) && this.IsValidTenant(TenantId(actionContext));
        }
    }
}