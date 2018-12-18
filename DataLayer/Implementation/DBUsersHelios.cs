using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataLayer.Interface;
using Entities;

namespace DataLayer.Implementation
{
    public class DBUsersHelios : IUsersHelios
    {
        private ServiceManager manager;
        public DBUsersHelios(ServiceManager manager)
        {
            this.manager = manager;
        }
        public void UpdateUser(HeliosUser user)
        {
            ResultValue<bool> result = manager.HPService.UpdateUser(user);
            result.GetResult();
        }
        public void AddUser(HeliosUser user)
        {
            ResultValue<bool> result = manager.HPService.AddUser(user);
            result.GetResult();
        }

        public List<HeliosUser> GetAllUsers()
        {
            List<HeliosUser> users = manager.HPService.GetAllUsers().GetResult().ToList();
            return users;
        }

        public HeliosUser SearchUserByEmail(string email)
        {

            ResultValue<HeliosUser> result = manager.HPService.SearchUserByEmail(email);
            return result.GetResult();
        }
    }
}
