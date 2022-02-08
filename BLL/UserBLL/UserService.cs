using System;
using System.Collections.Generic;
using System.Text;
using Entities;
using Data;
using System.Linq;

namespace BLL.UserBLL
{
    public class UserService : IUserService
    {
        private MyContext _context;
        public UserService(MyContext context)
        {
            _context = context;
        }
        public void AddUser(UserModel user)
        {
            _context.Users.Add(user);
            if (user.FirstName == null) return;
            _context.SaveChanges();
        }

        public List<UserModel> GetAll()
        {
            var items = _context.Users.ToList();
            return items;
        }

        public UserModel GetUser(string userName)
        {
            var r = _context.Users.FirstOrDefault(x => x.UserName == userName);
            return r;
        }

        public bool IsUserNameExist(string userName)
        {
            var exist = GetUser(userName);
            if (exist == null) return false;
            return true;
        }

        public bool IsUserLogin(string userName, string password, out UserModel user)
        {

            user = _context.Users.FirstOrDefault(x => x.UserName == userName && x.Password == password);
            if (user == null) return false;
            else return true;
            //user = default;
            //var users = GetAll();
            //foreach (var item in users)
            //{
            //    if (item.UserName == userName && item.Password == password)
            //    {
            //        user = GetUser(userName);
            //        return true;
            //    }
            //}
            //return false;
        }

        public void Update(UserModel user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
        }
    }
}
