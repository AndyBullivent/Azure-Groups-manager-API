using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cumbria.services.msgraph.models;

namespace cumbria.services.storage
{
    public class GroupRepository : IGroupRepository
    {
        public async Task AddGroupsAsync(IEnumerable<Group> groups)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Group>> GetAllAllowedGroupsAsync()
        {
            throw new NotImplementedException();
        }

        public async Task RemoveGroupsAsync(IEnumerable<string> groupIds)
        {
            throw new NotImplementedException();
        }
    }
}
