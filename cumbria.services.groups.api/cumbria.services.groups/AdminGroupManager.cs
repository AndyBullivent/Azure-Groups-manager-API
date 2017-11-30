using cumbria.services.msgraph;
using cumbria.services.msgraph.models;
using cumbria.services.storage;
using Microsoft.Azure.ActiveDirectory.GraphClient;
using Microsoft.Azure.ActiveDirectory.GraphClient.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cumbria.services.groups
{
    public class AdminGroupManager : BaseGraphObject, IAdminGroupManager, IGroupViewer
    {
        private IGroupRepository _repository;

        public AdminGroupManager(GraphCredentials creds, IGroupRepository groupRepository) : base(creds)
        {
            _repository = groupRepository;
        }

        public async Task AddToAllowedGroupsAsync(IEnumerable<storage.Group> allowedGroups)
        {
            await _repository.AddGroupsAsync(allowedGroups);
        }

        public async Task AddToAllowedGroupsAsync(storage.Group allowedGroup)
        {
            await _repository.AddGroupsAsync(new storage.Group[1] { allowedGroup });
        }

        public async Task<IEnumerable<storage.Group>> GetAADGroupsAsync()
        {

            List<storage.Group> aadGroups = new List<storage.Group>();
            var groups = await _activeDirectoryClient.Groups.ExecuteAsync();
            AddToGroupList(aadGroups, groups);

            while (groups.MorePagesAvailable)
            {
                groups = await groups.GetNextPageAsync();
                AddToGroupList(aadGroups, groups);
            }

            return aadGroups;
        }

        public async Task RemoveFromAllowedGroupAsync(IEnumerable<storage.Group> allowedGroupsToRemove)
        {
            await _repository.RemoveGroupsAsync(allowedGroupsToRemove.Select(group=>group.CategoryId));
        }

        public async Task RemoveFromAllowedGroupAsync(storage.Group allowedGroupToRemove)
        {
            await _repository.RemoveGroupsAsync(new string[1] { allowedGroupToRemove.Id });
        }

        public async Task<IEnumerable<storage.Group>> GetAllowedGroupsAsync()
        {
            return await _repository.GetAllAllowedGroupsAsync();
        }


        #region helpers
        private void AddToGroupList(List<storage.Group> aadGroups, IPagedCollection<IGroup> groups)
        {
            aadGroups.AddRange(groups.CurrentPage.Select(group => new storage.Group
            {
                Id = group.ObjectId,
                DisplayName = group.DisplayName,
                StudentFriendlyName = group.DisplayName
            }).ToList());
        }
        #endregion
    }
}
