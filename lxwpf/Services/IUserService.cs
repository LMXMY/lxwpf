using lxwpf.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace lxwpf.Services
{
    public interface IUserService
    {
        public int AddUser(UserModel user);

        public int DeleteUser(int userId);

        public int EditUser(UserModel user);

        public List<UserModel> FindAllUser();

        public UserModel Login(string userName, string password, bool IsChecked);
    }
}
