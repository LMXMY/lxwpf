using lxwpf.Entities;
using lxwpf.Repository;
using lxwpf.Share;
using System;
using System.Collections.Generic;
using System.Text;

namespace lxwpf.Services
{
    public class UserService : IUserService
    {
        public int AddUser(UserModel user)
        {
            int nRet = 0;
            using (AppDbContext hc = new AppDbContext())
            {
                try
                {
                    hc.Users!.Add(user);
                    nRet = hc.SaveChanges();
                }
                catch (Exception ex)
                {

                }
            }
            return nRet;
        }

        

        public List<UserModel> FindAllUser()
        {
            List<UserModel> data = new List<UserModel>();
            using (AppDbContext hc = new AppDbContext())
            {
                try
                {
                    data = hc.Users!.Select(u => u).ToList();

                }
                catch (Exception ex)
                {

                }
            }
            return data;
        }

        /*
        public UserModel Login(string userName, string password, bool IsChecked)
        {
            UserModel user = new UserModel();
            using (AppDbContext hc = new AppDbContext())
            {
                try
                {
                    var list = hc.Users!.Where(u => u.UserName == userName && u.State == 1).ToList();

                    foreach (var item in list)
                    {
                        //if (Cryptography.Decrypt(item.Password!) != password)
                        //    list.Remove(item);
                        //break;
                        if (item.Password != password)
                        {
                            list.Remove(item);
                        }
                        break;
                    }

                    if (list.Count > 0)
                    {
                        var _user = list.FirstOrDefault();
                        if (_user != null)
                        {
                            user.Id = _user.Id;
                            user.UserName = _user.UserName;
                            user.Password = _user.Password;
                        }

                        //int roleId = hc.UserRoles!.Where(u => u.UserId == resultModel.Data!.UserId).FirstOrDefault()!.RoleId;
                        //if (roleId > 0)
                        //{
                        //    resultModel.Data!.RealName = hc.Roles!.Where(r => r.RoleId == roleId).FirstOrDefault()!.RoleName;
                        //    resultModel.Data!.Level = hc.Roles!.Where(r => r.RoleId == roleId).FirstOrDefault()!.Level;
                        //}
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (Exception ex)
                {

                }
            }
            return user;
        }
        */

        public UserModel? Login(string userName, string password, bool isChecked)
        {
            using (AppDbContext hc = new AppDbContext())
            {
                try
                {
                    var user = hc.Users!.FirstOrDefault(u =>
                        u.UserName == userName &&
                        u.Password == password &&
                        u.State == 1);

                    if (user == null) return null;

                    return new UserModel
                    {
                        Id = user.Id,
                        UserName = user.UserName,
                        Password = user.Password
                    };


                }
                catch (Exception ex)
                {
                    return null;
                }
            }

        }



        public int DeleteUser(int userId)
        {
            int nRet = 0;
            using (AppDbContext hc = new AppDbContext())
            {
                try
                {
                    hc.Users!.Remove(hc.Users.Where(u => u.Id == userId).FirstOrDefault()!);
                    hc.SaveChanges();
                }
                catch (Exception ex)
                {

                }
            }
            return nRet;
        }

        public int EditUser(UserModel user)
        {
            int nRet = 0;
            using (AppDbContext hc = new AppDbContext())
            {
                try
                {
                    hc.Users!.Update(user);
                    nRet = hc.SaveChanges();
                }
                catch (Exception ex)
                {

                }
            }
            return nRet;
        }



    }
}
