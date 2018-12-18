using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entities;

namespace DataLayer.Interface
{
    public interface IUsersHelios
    {
        void AddUser(HeliosUser user);
        void UpdateUser(HeliosUser user);
        HeliosUser SearchUserByEmail(string email);
        List<HeliosUser> GetAllUsers();
    }
}
