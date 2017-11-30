using cumbria.services.msgraph;
using cumbria.services.storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;

namespace cumbria.services.groups.api.Controllers
{
    
    public class GroupController : BaseApiController
    {
        private IUserGroupManager _groupMgr;

        public GroupController(IUserGroupManager groupManager)
        {
            _groupMgr = groupManager;
        }

        [HttpGet]
        public IEnumerable<Claim> ViewClaims()
        {
            return this.Claims;
        }

        //[AuthorizeTenant]
        // GET api/<controller>
        public async Task<IEnumerable<Group>> Get()
        {
            return await _groupMgr.GetAllowedGroupsAsync();
        }

        [AuthorizeTenant]
        // POST api/<controller>
        public async Task Post([FromBody]string groupObjectId, [FromBody] bool addUser)
        {
            if (addUser)
            {

            }
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}