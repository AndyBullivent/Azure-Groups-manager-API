using cumbria.services.groups.api.Models;
using cumbria.services.storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace cumbria.services.groups.api.Controllers
{
    [RoutePrefix("v1/groupadmin")]
    [AuthorizeTenant]
    public class GroupAdminController : BaseApiController
    {
        private IAdminGroupManager _admin;
        private IGroupRepository _groupRepository;

        public GroupAdminController(IAdminGroupManager adminGroupMgr, IGroupRepository groupRepository)
        {
            _admin = adminGroupMgr;
            _groupRepository = groupRepository;
        }

        [Route("")]
        [HttpGet]
        public async Task<IEnumerable<VmWhiteListItem>> Get()
        {
            var query = await _groupRepository.GetAllAllowedGroupsAsync();
            List<string> whitelist = query.Select(item => item.Id).ToList();
            IEnumerable<Group> allGroups = await _admin.GetAADGroupsAsync();
            List<VmWhiteListItem> whitelistGroups = allGroups.Select(group => new VmWhiteListItem
            {
                Id = group.Id,
                DisplayName = group.DisplayName,
                StudentFriendlyName = group.StudentFriendlyName
            }).ToList();

            foreach (VmWhiteListItem item in whitelistGroups)
            {
                item.IsWhitelisted = whitelist.Any(id => id == item.Id);
            }
            
            return whitelistGroups;
        }

        [Route("")]
        [HttpPost]
        public async Task Post([FromBody]IEnumerable<VmWhiteListItem> groupStates)
        {
            var query = await _groupRepository.GetAllAllowedGroupsAsync();
            List<string> currentWhitelist = query.Select(item => item.Id).ToList();
            IEnumerable<Group> groupsToAdd = groupStates.Where(group => group.IsWhitelisted && !currentWhitelist.Any(id=> id == group.Id));
            IEnumerable<string> groupsToRemove = currentWhitelist.Where(item => groupStates.Any(gs => gs.Id == item && !gs.IsWhitelisted));
            await _admin.AddToAllowedGroupsAsync(groupsToAdd);
            await _admin.RemoveFromAllowedGroupAsync(groupsToRemove);
        }

    }
}
