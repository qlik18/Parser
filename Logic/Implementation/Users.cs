using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LogicLayer.Interface;
using DataLayer.Interface;
using Utility;
using Logging;
using Entities;
namespace LogicLayer.Implementation
{
    class Users : IUsers
    {
        IUsersHelios usersManager = null;
        List<HeliosUser> users = new List<Entities.HeliosUser>();

        public void setUserDao(IUsersHelios daoImpl)
        {
            this.usersManager = daoImpl;
        }


        public List<Entities.HeliosUser> GetAllUsers()
        {
            users= usersManager.GetAllUsers();
            return users;

        }
        public void SaveUserData(Entities.HeliosUser user, bool isNew)
        {
            if (isNew)
                usersManager.AddUser(user);
            else
                usersManager.UpdateUser(user);
        }        

        public HeliosUser SearchUserByEmail(string email)
        {
            try
            {
                return usersManager.SearchUserByEmail(email);
            }
            catch (Exception ex)
            {
                ExceptionManager.LogError(ex, Logger.Instance, true);
                return new HeliosUser();
            }
        }

        public List<HeliosUser> FilterUsers(string surname, string email, List<Entities.HeliosUser> list)
        {
            try
            {
                return (from u in list
                        where u.nazwisko.ToUpper().StartsWith(surname.ToUpper()) &&
                              u.email.ToUpper().StartsWith(email.ToUpper())
                        select u).ToList();
            }
            catch (Exception ex)
            {
                ExceptionManager.LogError(ex, Logger.Instance, true);
                return new List<HeliosUser>();
                
            }
           
        }

        public void updateBillingInfoInHeliosIssues(List<BillingIssueDtoHelios> list, List<HeliosUser> PolsatUsers)
        {
            for (int i = 0; i < list.Count; i++)
            {
                updateInfoInHeliosIssue(list[i].issueHelios, PolsatUsers);
            }

        }
        public void updateInfoInHeliosIssues(List<IssueDtoHelios> list, List<HeliosUser> PolsatUsers)
        {
            for (int i = 0; i < list.Count; i++)
            {
                updateInfoInHeliosIssue(list[i].issueHelios, PolsatUsers);
            }

        }
        public void updateInfoInHeliosIssue(IssueHelios iHelios, List<HeliosUser> PolsatUsers)
        {
            HeliosUser hu = PolsatUsers.Find(x => x.email == iHelios.email);
            if (hu != null)
            {
                iHelios.firstName = hu.imie;
                iHelios.lastName = hu.nazwisko;
            }
        }

    }
}
