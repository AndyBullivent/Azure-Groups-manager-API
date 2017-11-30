using cumbria.services.msgraph;
using cumbria.services.msgraph.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cumbria.services.groups
{
    class AdminGroupManager : BaseGraphObject, IAdminGroupManager, IGroupViewer
    {
        private IGroupRepository _repository;

        public AdminGroupManager(GraphCredentials creds, IGroupRepository groupRepository) : base(creds)
        {
            _repository = groupRepository;
        }

        public Task AddToAllowedGroups(IEnumerable<Group> allowedGroups)
        {
            _repository.AddGroups(allowedGroups)
        }

        public Task AddToAllowedGroups(Group allowedGroup)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Group>> GetAADGroups()
        {
            throw new NotImplementedException();
        }

        public Task RemoveFromAllowedGroup(IEnumerable<Group> allowedGroupsToRemove)
        {
            throw new NotImplementedException();
        }

        public Task RemoveFromAllowedGroup(Group allowedGroupToRemove)
        {
            throw new NotImplementedException();
        }

        public Task ShowAllowedGroups()
        {
            throw new NotImplementedException();
        }
    }
}
