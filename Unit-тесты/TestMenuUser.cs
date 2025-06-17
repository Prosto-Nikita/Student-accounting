//using ClassLibrary;
//using NUnit.Framework;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Reflection.Emit;
//using System.Text;
//using System.Threading.Tasks;
//using System;
//using System.IO;
//using System.Windows;
//using System.Collections.Generic;
//using System.Linq;
//using static System.Net.Mime.MediaTypeNames;
//using StudentAuthorization;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Newtonsoft.Json;
//using System.Collections.Generic;
//using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;
//using System.Windows.Media.Imaging;
//using DataObject = ClassLibrary.DataObject;
//using System.Windows.Controls;
//using Microsoft.Win32;
//using Application = System.Windows.Application;
//using System.Reflection;
//using System.Windows.Controls.Primitives;
//using Label = System.Windows.Controls.Label;
//using System.Drawing;
//using PdfSharp.Pdf;
//using PdfSharp.Drawing;
//using System.Runtime.Serialization;
//using System.Xml.Linq;
//namespace Unit_Test
//{
//    [TestClass]
//    public class TestMenuUser
//    {
//        public const string TestDataFile = "Test_User_Data.json";
//        public const string TestPasswordFile = "Test_User_Password.json";
//        public MenuUser? menuUser;
//        public List<DataObject>? testData;
//        private Application? _app;

//        [SetUp]
//        public void Setup()
//        {
//            // Очистка тестовых файлов перед каждым тестом
//            if (File.Exists(TestDataFile)) File.Delete(TestDataFile);
//            if (File.Exists(TestPasswordFile)) File.Delete(TestPasswordFile);

//            // Создание тестовых данных
//            testData = new List<DataObject>
//            {
//                new DataObject("Иванов Иван", "Группа1", "ivan@mail.com", "45",
//                             "01.01.2023", "12:00", "Да", "1 курс", ""),
//                new DataObject("Петров Петр", "Группа2", "petr@mail.com", "35",
//                             "02.01.2023", "13:00", "Нет", "2 курс", "")
//            };

//            // Инициализация меню пользователя
//            menuUser = new MenuUser("testUser");
//        }
//        //Проверка корректности создания экземпляра класса MenuUser
//        [TestMethod]
//        public void Constructor_WithUserName_InitializesCorrectly()
//        {
//            // Указываем сборку для загрузки ресурсов
//            Application.ResourceAssembly = typeof(MenuUser).Assembly;

//            menuUser = new MenuUser("testUser");
//            Assert.IsNotNull(menuUser);
//        }
//        //Проверка корректности конвертации изображения в строку
//        [TestMethod]
//        public void BitmapImageToBase64_ConvertsImageToBase64()
//        {
//            // Arrange
//            var bitmap = new BitmapImage(new Uri("C:\\Users\\user\\source\\repos\\StudentAuthorizationMain(Отчёты)\\Unit-Test\\Resources\\test.png"));

//            // Act
//            string base64 = MenuUser.BitmapImageToBase64(bitmap);

//            // Assert
//            Assert.IsFalse(string.IsNullOrEmpty(base64));
//            Assert.IsTrue(base64.Length > 100); // Проверка, что строка не пустая
//        }
//        //Проверка корректности перевода строки в изображение
//        [TestMethod]
//        public void Base64ToBitmapImage_ConvertsBase64ToImage()
//        {
//            // Arrange
//            var originalBitmap = new BitmapImage(new Uri("C:\\Users\\user\\source\\repos\\StudentAuthorizationMain(Отчёты)\\Unit-Test\\Resources\\test.png"));
//            string base64 = MenuUser.BitmapImageToBase64(originalBitmap);

//            // Act
//            var convertedBitmap = MenuUser.Base64ToBitmapImage(base64);

//            // Assert
//            Assert.IsNotNull(convertedBitmap);
//            Assert.AreEqual(originalBitmap.PixelWidth, convertedBitmap.PixelWidth);
//            Assert.AreEqual(originalBitmap.PixelHeight, convertedBitmap.PixelHeight);
//        }
//        //Проверка удаления выбранного студента из списка
//        [TestMethod]
//        public void Remove_Click_WithSelectedItem_RemovesItem()
//        {
//            // 1. Инициализация WPF (обязательно)
//            if (Application.Current == null)
//                new Application { ShutdownMode = ShutdownMode.OnExplicitShutdown };

//            // 2. Создаем окно с тестовыми данными
//            var window = new MenuUser("testUser");
//            window.dataUser = new List<DataObject>
//    {
//        new DataObject("Иванов Иван", "Группа1", "test@mail.com", "4.5", "", "", "", "", ""),
//        new DataObject("Петров Петр", "Группа2", "test2@mail.com", "3.5", "", "", "", "", "")
//    };

//            // 3. Эмулируем выбранный элемент
//            var radioButton = new RadioButton { Name = "Label1", IsChecked = true };
//            window.grid.Children.Add(radioButton); // Ключевая строка!

//            // 4. Вызываем метод
//            window.Remove_Click(null, null);

//            // 5. Проверяем
//            Assert.AreEqual(1, window.dataUser.Count);
//            Assert.AreEqual("Петров Петр", window.dataUser[0].FIO);
//        }
//        //Проверка работы фильтра студентов
//        [TestMethod]
//        public void Filter_Click_WithRatingFilter_FiltersCorrectly()
//        {
//            // 1. Инициализация WPF (если ещё не сделано в базовом классе)
//            if (Application.Current == null)
//            {
//                new Application { ShutdownMode = ShutdownMode.OnExplicitShutdown };
//            }

//            // 2. Создаем и инициализируем MenuUser
//            var menuUser = new MenuUser("testUser")
//            {
//                dataUser = new List<DataObject>
//        {
//            new DataObject("Иванов Иван", "Группа1", "test@mail.com", "45",
//                          "01.01.2023", "12:00", "Да", "1 курс", ""),
//            new DataObject("Петров Петр", "Группа2", "petr@mail.com", "35",
//                          "02.01.2023", "13:00", "Нет", "2 курс", "")
//        }
//            };

//            // 3. Инициализируем необходимые UI-элементы
//            menuUser.MinRating = new TextBox { Text = "40" };
//            menuUser.MaxRating = new TextBox { Text = "50" };
//            menuUser.NumberCourse = new TextBox { Text = "" };
//            if (menuUser.ToggleSwitch != null)
//            {
//                menuUser.ToggleSwitch.IsChecked = null;
//            }
//            menuUser.TempDataUser = new List<DataObject>();
//            menuUser.grid = new Grid(); // Или реальный Grid из XAML

//            // 4. Запускаем тестируемый метод
//            menuUser.Filter_Click(null, null);

//            // 5. Проверяем результаты
//            Assert.AreEqual(1, menuUser.TempDataUser.Count);
//            Assert.AreEqual("Иванов Иван", menuUser.TempDataUser[0].FIO);
//        }
//        //Проверка работы создания отчета PDF
//        [TestMethod]
//        public void Report_Generation_Logic_Test()
//        {
//            // Arrange
//            var testStudents = new List<ClassLibrary.DataObject>
//    {
//        new ClassLibrary.DataObject
//        {
//            FIO = "Тест Студент",
//            Group = "Группа 1",
//            Email = "test@test.com",
//            Rating = "100",
//            TransferDate = "01.01.2023",
//            TransferTime = "10:00",
//            ScholarshipAvailability = "Да",
//            Course = "1 курс",
//            Photo = null
//        }
//    };

//            string testFilePath = Path.GetTempFileName();
//            File.Delete(testFilePath);
//            testFilePath = Path.ChangeExtension(testFilePath, ".pdf");

//            // Act - эмулируем основную логику из Report_Click
//            try
//            {
//                // 1. Проверка наличия данных
//                if (testStudents.Count == 0)
//                {
//                    Assert.Fail("Нет данных для отчета");
//                    return;
//                }

//                // 2. Логика создания PDF (адаптированная из исходного кода)
//                using (var document = new PdfDocument())
//                {
//                    var page = document.AddPage();
//                    page.Size = PdfSharp.PageSize.A4;

//                    using (var gfx = XGraphics.FromPdfPage(page))
//                    {
//                        // Шрифты
//                        var titleFont = new XFont("Arial", 16);
//                        var normalFont = new XFont("Arial", 12);

//                        // Заголовок
//                        gfx.DrawString("ОТЧЕТ ПО СТУДЕНТАМ", titleFont, XBrushes.Black,
//                            new XRect(0, 50, page.Width, page.Height),
//                            XStringFormats.TopCenter);

//                        // Данные студентов
//                        double yPos = 100;
//                        foreach (var student in testStudents)
//                        {
//                            gfx.DrawString($"ФИО: {student.FIO}", normalFont, XBrushes.Black,
//                                new XRect(50, yPos, page.Width, page.Height),
//                                XStringFormats.TopLeft);
//                            yPos += 20;

//                            // Добавьте остальные поля по аналогии
//                        }
//                    }

//                    // Сохранение
//                    document.Save(testFilePath);
//                }

//                // Assert
//                Assert.IsTrue(File.Exists(testFilePath));
//                Assert.IsTrue(new FileInfo(testFilePath).Length > 0);
//            }
//            catch (Exception ex)
//            {
//                Assert.Fail($"Ошибка генерации отчета: {ex.Message}");
//            }
//            finally
//            {
//                // Очистка
//                if (File.Exists(testFilePath))
//                    File.Delete(testFilePath);
//            }
//        }
//    }



//}
