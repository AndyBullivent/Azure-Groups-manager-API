using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq.AutoMock;
using cumbria.services.groups;
using cumbria.services.msgraph;
using cumbria.services.storage;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;

namespace cumbria.services.groups.tests
{
    [TestClass]
    public class TestGroups
    {
        private GraphCredentials _creds;
        private AutoMocker _mocker;
        private Mock<IGroupRepository> _mockRepository;

        public TestGroups()
        {
            _creds = new GraphCredentials
            {
                ClientId = "5c642f80-ae79-4d3a-a753-5498eeb2e7d0",
                Key = "6WxvoAUri6JXdEDIdTISz/SfCRZa7NUZCL7nAl4lcoM=",
                Tenant = "e58cae89-8f91-4f69-8cce-51abf1d13b44"
            };

            _mocker = new AutoMocker();
            _mockRepository = _mocker.GetMock<IGroupRepository>();
        }
        

        [TestMethod]
        public async Task GetAzureGroups()
        {
            IAdminGroupManager adminMgr = new AdminGroupManager(_creds, _mockRepository.Object);
            var groups = await adminMgr.GetAADGroupsAsync();
        }

        [TestMethod]
        public async Task AddToGroup()
        {

            IUserGroupManager userMgr = new UserGroupManager(_creds, _mockRepository.Object);
            await userMgr.AddUserToGroup("1ca9441f-59c2-4f58-8a90-3ebbbbacef36", "3b22748d-443d-4d28-ada1-6b4f4437c4b1");

        }

        [TestMethod]
        public async Task RemoveFromGroup()
        {
            IUserGroupManager userMgr = new UserGroupManager(_creds, _mockRepository.Object);
            await userMgr.RemoveUserFromGroup("a59140cc-be94-4e70-9e3b-cad4fe857937", "6968242c-a055-4145-8534-39180046d980");

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task BadUserAddToGroup()
        {

            IUserGroupManager userMgr = new UserGroupManager(_creds, _mockRepository.Object);
            await userMgr.AddUserToGroup("1", "3b22748d-443d-4d28-ada1-6b4f4437c4b1");

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task BadUserRemoveFromGroup()
        {
            IUserGroupManager userMgr = new UserGroupManager(_creds, _mockRepository.Object);
            await userMgr.RemoveUserFromGroup("1", "3b22748d-443d-4d28-ada1-6b4f4437c4b1");

        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public async Task BadGroupAddToGroup()
        {

            IUserGroupManager userMgr = new UserGroupManager(_creds, _mockRepository.Object);
            await userMgr.AddUserToGroup("1ca9441f-59c2-4f58-8a90-3ebbbbacef36", "3");

        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public async Task BadGroupRemoveFromGroup()
        {
            IUserGroupManager userMgr = new UserGroupManager(_creds, _mockRepository.Object);
            await userMgr.RemoveUserFromGroup("1ca9441f-59c2-4f58-8a90-3ebbbbacef36", "3");

        }

        [TestMethod]
        public async Task GetUserGroups()
        {
            IUserGroupManager usrGroupMgr = new UserGroupManager(_creds, _mockRepository.Object);
            var groups = usrGroupMgr.GetUserMemberships("1ca9441f-59c2-4f58-8a90-3ebbbbacef36");

        }


    }
}
