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
//    public sealed class TestDataUser
//    {
//        private const string TestDataFile = "Test_Data.json";
//        private DataUser dataUser = new DataUser(TestDataFile);

//        [SetUp]
//        public void Setup()
//        {
//            // Очищаем тестовый файл перед каждым тестом
//            if (File.Exists(TestDataFile))
//            {
//                File.Delete(TestDataFile);
//            }
//            dataUser = new DataUser(TestDataFile);
//        }

//        //Проверка корректности добавления новой записи пользователю
//        [TestMethod]
//        public void AddData_ValidData_AddsToCollection()
//        {
//            // Act
//            dataUser.AddData("testUser", "Иванов Иван", "Группа1", "test@mail.com",
//                           "45", "01.01.2023", "12:00", "Да", "", "1 курс");

//            // Assert
//            var result = dataUser.ProvideData("testUser");
//            Assert.AreEqual(1, result.Count);
//            Assert.AreEqual("Иванов Иван", result[0].FIO);
//            Assert.AreEqual("Группа1", result[0].Group);
//        }

//        //Проверка корректности удаления записи пользователя
//        [TestMethod]
//        public void RemoveData_ExistingIndex_RemovesItem()
//        {
//            // Arrange
//            dataUser.AddData("testUser", "Иванов Иван", "Группа1", "test@mail.com",
//                           "45", "01.01.2023", "12:00", "Да", "", "1 курс");
//            dataUser.AddData("testUser", "Петров Петр", "Группа2", "petrov@mail.com",
//                           "37", "02.01.2023", "13:00", "Нет", "", "2 курс");

//            // Act
//            dataUser.RemoveData(1, "testUser");

//            // Assert
//            var result = dataUser.ProvideData("testUser");
//            Assert.AreEqual(1, result.Count);
//            Assert.AreEqual("Петров Петр", result[0].FIO);
//        }

//        //Проверка корректности получения записей пользователя
//        [TestMethod]
//        public void ProvideData_NonExistentUser_ReturnsNull()
//        {
//            // Act
//            var result = dataUser.ProvideData("nonExistentUser");

//            // Assert
//            Assert.IsNull(result);
//        }
//    }
//}
