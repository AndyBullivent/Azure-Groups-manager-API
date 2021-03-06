﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cumbria.services.storage;

namespace cumbria.services.msgraph
{
    public interface IUserGroupManager: IGroupViewer
    {
        Task AddUserToGroup(string userObjectId, string groupObjectId);
        Task RemoveUserFromGroup(string userObjectId, string groupObjectId);
        Task<string[]> GetUserMemberships(string subject);
    }
}
