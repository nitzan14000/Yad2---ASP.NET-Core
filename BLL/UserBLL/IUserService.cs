using System;
using System.Collections.Generic;
using System.Text;
using Entities;

namespace BLL.UserBLL
{
    public interface IUserService
    {
        List<UserModel> GetAll();
        void AddUser(UserModel user);
        bool IsUserLogin(string userName, string password, out UserModel user);
        bool IsUserNameExist(string userName);
        UserModel GetUser(string userName);
        void Update(UserModel user);
    }
}
