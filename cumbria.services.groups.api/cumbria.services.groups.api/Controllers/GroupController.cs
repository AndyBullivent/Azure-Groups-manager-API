using cumbria.services.groups.api.Models;
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
using System.Web.Http.Results;

namespace cumbria.services.groups.api.Controllers
{
    [RoutePrefix("v1/group")]
    public class GroupController : BaseApiController
    {
        private IUserGroupManager _groupMgr;

        public GroupController(IUserGroupManager groupManager)
        {
            _groupMgr = groupManager;
        }

        [HttpGet]
        [Route("claims")]
        public IEnumerable<string> ViewClaims()
        {
            return Claims.Select(c=>$"Type:{c.Type}, Value:{c.Value.ToString()}");
        }

        //[AuthorizeTenant]
        // GET api/<controller>
        [HttpGet]
        [Route("")]
        [AuthorizeTenant]
        public async Task<IEnumerable<VmGroup>> Get()
        {
            string subject = Claims.Single(c => c.Type == "sub").Value.ToString();
            var allowedGroups = await _groupMgr.GetAllowedGroupsAsync();
            string[] memberships = await _groupMgr.GetUserMemberships(subject);
            List<VmGroup> groupsVm = BuildMembershipsModel(allowedGroups, memberships);
            return groupsVm;
        }

        private List<VmGroup> BuildMembershipsModel(IEnumerable<Group> allowedGroups, string[] memberships)
        {
            List<VmGroup> vmGroups = new List<VmGroup>();
            foreach(var grp in allowedGroups)
            {
                VmGroup group = new VmGroup
                {
                    Id = grp.Id,
                    StudentFriendlyName = grp.StudentFriendlyName,
                    DisplayName = grp.DisplayName,
                    CategoryId = grp.CategoryId
                };
                group.IsMember = memberships?.Any(memberOf => memberOf == grp.Id)??false;               

                vmGroups.Add(group);                
            }

            return vmGroups;
        }

        [AuthorizeTenant]
        [HttpPost]
        [Route("")]
        // POST api/<controller>
        public async Task<ResponseMessageResult> Post(VmGroup group)
        {
            var subject = Claims.Single(c => c.Type == "sub").Value.ToString();
            try
            {
                if (group.IsMember)
                {
                    await _groupMgr.AddUserToGroup(subject, group.Id);
                }
                else
                {
                    await _groupMgr.RemoveUserFromGroup(subject, group.Id);
                }
            }
            catch (InvalidOperationException e)
            {
                return NotFound(e.Message);
            }
            return new ResponseMessageResult(new HttpResponseMessage(HttpStatusCode.Accepted));
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