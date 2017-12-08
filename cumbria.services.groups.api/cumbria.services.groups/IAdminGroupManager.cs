using cumbria.services.msgraph.models;
using Microsoft.Azure.ActiveDirectory.GraphClient;
using Microsoft.Azure.ActiveDirectory.GraphClient.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cumbria.services.groups
{
    public interface IAdminGroupManager
    {
        Task<IEnumerable<storage.Group>> GetAADGroupsAsync();
        Task AddToAllowedGroupsAsync(storage.Group allowedGroup);
        Task AddToAllowedGroupsAsync(IEnumerable<storage.Group> allowedGroup);
        Task RemoveFromAllowedGroupAsync(storage.Group allowedGroupToRemove);
        Task RemoveFromAllowedGroupAsync(IEnumerable<string> allowedGroupsToRemove);
    }
}
