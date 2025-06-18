using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using Formatting = Newtonsoft.Json.Formatting;

namespace ClassLibrary
{
    public class DataUser
    {

        private Dictionary<string, List<DataObject>> AllDataUsers;
        public DataUser(string pathFile)
        {
            if (File.Exists(pathFile))
            {
                var json = File.ReadAllText(pathFile);
                AllDataUsers = JsonConvert.DeserializeObject<Dictionary<string, List<DataObject>>>(json) ?? new Dictionary<string, List<DataObject>>();
            }
            else
            {
                AllDataUsers = new Dictionary<string, List<DataObject>>();
            }
        }
        public void AddData(string userName, string FIO, string Group, string Email, string Rating, string TransferDate, string TransferTime, string ScholarshipAvailability, string Photo, string Course)
        {
            try
            {
                if (!AllDataUsers.ContainsKey(userName))
                {
                    AllDataUsers.Add(userName, new List<DataObject>());
                }
                if (string.IsNullOrWhiteSpace(FIO))
                {
                    throw new Exception("Имя не может быть пустым.");
                }
                else if (string.IsNullOrWhiteSpace(Group))
                {
                    throw new Exception("Группа не может быть пустой.");
                }
                else if (string.IsNullOrWhiteSpace(Email))
                {
                    throw new Exception("Email не может быть пустым.");
                }
                else if (string.IsNullOrWhiteSpace(Convert.ToString(Rating)))
                {
                    throw new Exception("Рейтинг не может быть пустым.");
                }
                double NullD = 0;
                if (!double.TryParse(Convert.ToString(Rating), out NullD))
                {
                    throw new Exception("Поле рейтинг должно содержать только число.");
                }
                if (!Email.Contains("@")) { throw new Exception("В поле Email отсутствует символ \"@\"."); }

                if (AllDataUsers[userName].Count == 0)
                {
                    AllDataUsers[userName] = new List<DataObject>();
                    AllDataUsers[userName].Add(new DataObject(FIO, Group, Email, Rating, TransferDate, TransferTime, ScholarshipAvailability, Course, Photo));
                }
                else { AllDataUsers[userName].Add(new DataObject(FIO, Group, Email, Rating, TransferDate, TransferTime, ScholarshipAvailability, Course, Photo)); }
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }
        public void RemoveData(int indexToRemove, string userName)
        {
            try
            {
                if (!AllDataUsers.ContainsKey(userName))
                {
                    AllDataUsers.Add(userName, new List<DataObject>());
                }
                if (indexToRemove > 0 && indexToRemove <= AllDataUsers[userName].Count)
                {
                    AllDataUsers[userName].RemoveAt(indexToRemove - 1);
                    Console.WriteLine($"Запись с номером {indexToRemove} удалена.");
                }
                else
                {
                    throw new Exception("Введен несуществующий номер записи.");
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }

        public void AddUser(string name)
        {
            List<DataObject> dataObjects = new List<DataObject>();
            AllDataUsers.Add(name, dataObjects);
            SaveAll(name, dataObjects);
        }
        public List<DataObject> ProvideData(string userName)
        {
            try
            {
                return AllDataUsers[userName];
            }
            catch (Exception ex) { return null; }
        }
        public void SaveAll(string nameUser, List<DataObject> newDatas)
        {
            string pathFile = "User_Data.json";
            try
            {
                AllDataUsers[nameUser] = newDatas;
                // Сериализуем словарь в строку JSON
                string json = JsonConvert.SerializeObject(AllDataUsers, Formatting.Indented);
                // Записываем строку JSON в файл
                File.WriteAllText(pathFile, json);
            }
            catch { return; }
        }
    }

    public class DataObject
    {
        public string FIO;
        public string Group;
        public string Email;
        public string Rating;
        public string TransferDate;
        public string TransferTime;
        public string ScholarshipAvailability;
        public string Photo;
        public string Course;
        public DataObject(string FIO, string Group, string Email, string Rating, string TransferDate, string TransferTime, string ScholarshipAvailability, string Course, string Photo)
        {
            this.FIO = FIO;
            this.Group = Group;
            this.Email = Email;
            this.Rating = Rating;
            this.TransferDate = TransferDate;
            this.TransferTime = TransferTime;
            this.ScholarshipAvailability = ScholarshipAvailability;
            this.Photo = Photo;
            this.Course = Course;
        }
        public DataObject() {; }
    }
}
