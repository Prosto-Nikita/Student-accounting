//using ClassLibrary;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Newtonsoft.Json;
//using System.IO;
//using NUnit.Framework;
//using System.Collections.Generic;
//using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;
//namespace Unit_Test
//{
//    [TestClass]
//    public sealed class TestUserManager
//    {
//        private const string TestUsersFile = "Test_Users.json";
//        private UserManager userManager = new UserManager(TestUsersFile);

//        [SetUp]
//        public void Setup()
//        {
//            // Очищаем тестовый файл перед каждым тестом
//            if (File.Exists(TestUsersFile))
//            {
//                File.Delete(TestUsersFile);
//            }
//            userManager = new UserManager(TestUsersFile);
//        }

//        //Проверка на корректное добавление нового пользователя
//        [TestMethod]
//        public void AddUser_NewUser_ReturnsTrue()
//        {
//            // Очищаем тестовый файл перед каждым тестом
//            if (File.Exists(TestUsersFile))
//            {
//                File.Delete(TestUsersFile);
//            }
//            userManager = new UserManager(TestUsersFile);
//            // Act
//            bool result = userManager.AddUser("testUser", "password123");

//            // Assert
//            Assert.IsTrue(result);
//            var json = File.ReadAllText(TestUsersFile);
//            var users = JsonConvert.DeserializeObject<Dictionary<string, User>>(json);
//            Assert.AreEqual(1, users.Count);
//            Assert.AreEqual("password123", users["testUser"].Password);
//        }

//        //Проверка добавления пользователей с одинаковым именем
//        [TestMethod]
//        public void AddUser_ExistingUser_ReturnsFalse()
//        {
//            // Arrange
//            userManager.AddUser("existingUser", "password123");

//            // Act
//            bool result = userManager.AddUser("existingUser", "newPassword");

//            // Assert
//            Assert.IsFalse(result);
//        }

//        //Проверка добавления пользователя и авторизации этого пользователя
//        [TestMethod]
//        public void Authorization_ValidCredentials_ReturnsTrue()
//        {
//            // Arrange
//            userManager.AddUser("validUser", "correctPassword");

//            // Act
//            bool result = userManager.Authorization("validUser", "correctPassword");

//            // Assert
//            Assert.IsTrue(result);
//        }

//        //Попытка авторизации с неправильным паролем
//        [TestMethod]
//        public void Authorization_InvalidPassword_ReturnsFalse()
//        {
//            // Arrange
//            userManager.AddUser("validUser", "correctPassword");

//            // Act
//            bool result = userManager.Authorization("validUser", "wrongPassword");

//            // Assert
//            Assert.IsFalse(result);
//        }
//    }
//}
