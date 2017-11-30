using cumbria.services.msgraph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cumbria.services.storage;
using Microsoft.Azure.ActiveDirectory.GraphClient;


namespace cumbria.services.groups
{
    public class UserGroupManager : BaseGraphObject, IUserGroupManager
    {
        private IGroupRepository _repository;

        public UserGroupManager(GraphCredentials creds, IGroupRepository groupRepository) : base(creds)
        {
            _repository = groupRepository;
        }

        public async Task AddUserToGroup(string userObjectId,string groupObjectId)
        {
            try
            {
                Microsoft.Azure.ActiveDirectory.GraphClient.Group group =
                    _activeDirectoryClient.Groups.GetByObjectId(groupObjectId).ExecuteAsync().Result as Microsoft.Azure.ActiveDirectory.GraphClient.Group;
                DirectoryObject user = _activeDirectoryClient.Users.GetByObjectId(userObjectId).ExecuteAsync().Result as DirectoryObject;

                group.Members.Add(user);
                await group.UpdateAsync();
            }
            catch(AggregateException e)
            {
                CheckAggregateException(userObjectId, groupObjectId, e);
            }
        }

        private void CheckAggregateException(string userObjectId, string groupObjectId, AggregateException e)
        {
            foreach (var ex in e.InnerExceptions)
            {
                if(ex.InnerException!= null && ex.InnerException.Message.Contains("Invalid object identifier")&& ex.InnerException.Message.Contains(groupObjectId))
                {
                    throw new ArgumentException("An error occured retrieving the selected group.", ex);
                }

                if (ex.Message.Contains(userObjectId))
                {
                    throw new ArgumentException("An error occured retrieving the selected user.", ex);
                }
            }
        }

        public async Task<IEnumerable<storage.Group>> GetAllowedGroupsAsync()
        {
            return await _repository.GetAllAllowedGroupsAsync();
        }

        public async Task RemoveUserFromGroup(string userObjectId, string groupObjectId)
        {
            try
            {
                Microsoft.Azure.ActiveDirectory.GraphClient.Group group =
                    _activeDirectoryClient.Groups.Where(g => g.ObjectId == groupObjectId).Expand(g => g.Members).ExecuteSingleAsync().Result as Microsoft.Azure.ActiveDirectory.GraphClient.Group;
                DirectoryObject user = _activeDirectoryClient.Users.GetByObjectId(userObjectId).ExecuteAsync().Result as DirectoryObject;

                group.Members.Remove(user);
                await group.UpdateAsync();
            }
            catch(AggregateException e)
            {
                CheckAggregateException(userObjectId, groupObjectId, e);
            }
        }
    }
}
