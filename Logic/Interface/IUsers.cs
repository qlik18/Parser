using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataLayer.Interface;
using Entities;

namespace LogicLayer.Interface
{
    public interface IUsers
    {
        List<Entities.HeliosUser> GetAllUsers();       
        void SaveUserData(Entities.HeliosUser user, bool isNew);
        Entities.HeliosUser SearchUserByEmail(string email);
        List<Entities.HeliosUser> FilterUsers(string surname, string email, List<Entities.HeliosUser> list);
        void setUserDao(IUsersHelios daoImpl);
        void updateBillingInfoInHeliosIssues(List<BillingIssueDtoHelios> list, List<HeliosUser> PolsatUsers);
        void updateInfoInHeliosIssues(List<IssueDtoHelios> list, List<HeliosUser> PolsatUsers);
        void updateInfoInHeliosIssue(IssueHelios iHelios, List<HeliosUser> PolsatUsers);
    }
}
