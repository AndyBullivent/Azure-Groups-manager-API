using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cumbria.services.msgraph
{
    public interface IAdminGroupManager
    {
        Task<IEnumerable<Group>> GetAADGroups();
        Task AddToAllowedGroups(Group allowedGroup);
        Task AddToAllowedGroups(IEnumerable<Group> allowedGroup);
        Task RemoveFromAllowedGroup(Group allowedGroupToRemove);
        Task RemoveFromAllowedGroup(IEnumerable<Group> allowedGroupsToRemove);
    }
}
