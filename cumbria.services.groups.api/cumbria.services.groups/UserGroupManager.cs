using cumbria.services.msgraph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cumbria.services.storage;
using Microsoft.Azure.ActiveDirectory.GraphClient;
using Microsoft.Azure.ActiveDirectory.GraphClient.Extensions;

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
                var allowedGroups = await _repository.GetAllAllowedGroupsAsync();
                if (allowedGroups.Any(aGroup => aGroup.Id == groupObjectId))
                {

                    Microsoft.Azure.ActiveDirectory.GraphClient.Group group = await
                        _activeDirectoryClient.Groups.GetByObjectId(groupObjectId).ExecuteAsync() as Microsoft.Azure.ActiveDirectory.GraphClient.Group;
                    DirectoryObject user = await _activeDirectoryClient.Users.GetByObjectId(userObjectId).ExecuteAsync() as DirectoryObject;

                    group.Members.Add(user);
                    await group.UpdateAsync();
                }
                else
                {
                    throw new InvalidOperationException("You do not have rights to join this group");
                }
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
                Microsoft.Azure.ActiveDirectory.GraphClient.Group group = await
                    _activeDirectoryClient.Groups.Where(g => g.ObjectId == groupObjectId).Expand(g => g.Members).ExecuteSingleAsync() as Microsoft.Azure.ActiveDirectory.GraphClient.Group;
                DirectoryObject user = await _activeDirectoryClient.Users.GetByObjectId(userObjectId).ExecuteAsync() as DirectoryObject;

                group.Members.Remove(user);
                await group.UpdateAsync();
            }
            catch(AggregateException e)
            {
                CheckAggregateException(userObjectId, groupObjectId, e);
            }
         }

        public async Task<string[]> GetUserMemberships(string objectId)
        {
            try
            {
                IUserFetcher retrievedUserFetcher;
                IUser user = await _activeDirectoryClient.Users.GetByObjectId(objectId).ExecuteAsync();
                retrievedUserFetcher = (IUserFetcher)user;

                IPagedCollection<IDirectoryObject> memberOf = await retrievedUserFetcher.MemberOf.ExecuteAsync();

                List<IDirectoryObject> groups = memberOf.CurrentPage.ToList();

                while(memberOf.MorePagesAvailable)
                {
                    memberOf = await memberOf.GetNextPageAsync();
                    groups.AddRange(memberOf.CurrentPage.ToList());
                }

                string[] objIds = groups.Select(g => g.ObjectId).ToArray();

                return objIds;
            }
            catch(Exception)
            {
                return new string[0];
            }
        }
    }
}
