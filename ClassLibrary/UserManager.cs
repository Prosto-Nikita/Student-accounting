using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;
using static System.Net.WebRequestMethods;

namespace ClassLibrary
{
    public class UserManager
    {
        string pathFile;
        private Dictionary<string, User> AllUsers;
        public UserManager(string pathFile)
        {
            this.pathFile = pathFile;
            if (System.IO.File.Exists(pathFile))
            {
                var json = System.IO.File.ReadAllText(pathFile);
                AllUsers = JsonConvert.DeserializeObject<Dictionary<string, User>>(json) ?? new Dictionary<string, User>();
            }
            else
            {
                AllUsers = new Dictionary<string, User>();
            }
        }
        public bool AddUser(string username, string password)
        {
            try
            {
                if (AllUsers.ContainsKey(username))
                {
                    return false;
                }
                User temp = new User(password);

                AllUsers.Add(username, temp);
                SaveAll(pathFile);
                return true;
            }
            catch (Exception ex) { return false; }
        }
        public bool Authorization(string username, string password)
        {
            try
            {
                if (username == null || password == null)
                {
                    return false;
                }
                if (!AllUsers.ContainsKey(username))
                {
                    throw new Exception("Нет такого пользователя.");
                }
                User value = AllUsers[username];
                if (value.Password != password)
                {
                    throw new Exception("Введен не верный пароль.");
                }
                return true;
            }
            catch (Exception ex) { return false; }
        }
        public void SaveAll(string pathFile)
        {
            try
            {
                // Сериализуем словарь в строку JSON
                string json = JsonConvert.SerializeObject(AllUsers, Newtonsoft.Json.Formatting.Indented);
                // Записываем строку JSON в файл
                System.IO.File.WriteAllText(pathFile, json);
            }
            catch {; }
        }
    }
    public class User
    {
        public string Password;
        public User(string Password)
        {
            this.Password = Password;
        }

    }
}
