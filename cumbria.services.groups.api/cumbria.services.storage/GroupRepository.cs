using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;


namespace cumbria.services.storage
{
    public class GroupRepository : IGroupRepository
    {
        private ManagedGroupsDB _ctx;

        public GroupRepository ()
        {
            _ctx = new ManagedGroupsDB();
        }

        /// <summary>
        /// Adds groups to the allowed groups whitelist
        /// </summary>
        /// <param name="groups">An IEnumerable of Group</param>
        /// <returns></returns>
        public async Task AddGroupsAsync(IEnumerable<Group> groups)
        {
            var newAllowedGroups = groups.Select(newGroup => new AllowedGroup
            {
                id = newGroup.Id,
                Name = newGroup.DisplayName,
                FriendlyName = newGroup.StudentFriendlyName,
                CategoryId = Guid.Parse("{5BC4F7BE-17CC-4C22-BD3C-4133FBCBB213}")
            });

            _ctx.AllowedGroups.AddRange(newAllowedGroups);
            try
            {
                await _ctx.SaveChangesAsync();
            }
            catch(Exception e)
            {
                var x = e;
            }
        }

        /// <summary>
        /// Returns all the allowed groups in the whitelist
        /// </summary>
        /// <returns>An IEnumerable of Group</returns>
        public async Task<IEnumerable<Group>> GetAllAllowedGroupsAsync()
        {
            return await _ctx.AllowedGroups.Select(allowedGroup => new Group
            {
                Id = allowedGroup.id,
                DisplayName = allowedGroup.Name,
                StudentFriendlyName = allowedGroup.FriendlyName
            }).ToArrayAsync();
        }

        /// <summary>
        /// Removes groups from the allowed groups whitelist
        /// </summary>
        /// <param name="groupIds">an IEnumerable of id's representing the groups</param>
        /// <returns></returns>
        public async Task RemoveGroupsAsync(IEnumerable<string> groupIds)
        {
            var groups = _ctx.AllowedGroups.Where(allowedGroup => groupIds.Any(gid => gid == allowedGroup.id));
            _ctx.AllowedGroups.RemoveRange(groups);

            await _ctx.SaveChangesAsync();
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _ctx?.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            
            Dispose(true);

        }
        #endregion
    }
}
