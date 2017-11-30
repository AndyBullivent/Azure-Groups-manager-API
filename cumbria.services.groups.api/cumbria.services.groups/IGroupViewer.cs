using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cumbria.services.msgraph
{
    public interface IGroupViewer
    {
        Task<IEnumerable<storage.Group>> GetAllowedGroupsAsync();
    }
}
