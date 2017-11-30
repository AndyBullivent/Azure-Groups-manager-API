using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cumbria.services.msgraph
{
    public interface IUserGroupManager
    {
        Task AddSelfToGroup(string groupId);
        Task RemoveSelfFromGroup(string groupId);
    }
}
