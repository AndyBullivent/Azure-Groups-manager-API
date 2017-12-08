using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cumbria.services.storage
{
    public interface IGroupRepository:IDisposable
    {
        Task<IEnumerable<Group>> GetAllAllowedGroupsAsync();
        Task AddGroupsAsync(IEnumerable<Group> groups);
        Task RemoveGroupsAsync(IEnumerable<string> groupIds);
    }
}
