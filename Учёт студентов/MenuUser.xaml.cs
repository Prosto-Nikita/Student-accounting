using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using PdfSharp.Pdf;
using PdfSharp.Drawing;
//using System.Web.UI.WebControls;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ClassLibrary;
using Microsoft.Win32;
using Image = System.Windows.Controls.Image;
using RadioButton = System.Windows.Controls.RadioButton;
using Style = System.Windows.Style;
using System.Windows.Controls.Primitives;
using Path = System.IO.Path;
using System.Data;
using PdfSharpCore.Drawing;
using XGraphics = PdfSharp.Drawing.XGraphics;
using XFont = PdfSharp.Drawing.XFont;
using XRect = PdfSharp.Drawing.XRect;
using XBrushes = PdfSharp.Drawing.XBrushes;
using XStringFormats = PdfSharp.Drawing.XStringFormats;
using XImage = PdfSharp.Drawing.XImage;
using XPens = PdfSharp.Drawing.XPens;
using System.Diagnostics;
using System.Xml.Linq;
namespace StudentAuthorization
{
    /// <summary>
    /// Логика взаимодействия для MenuUser.xaml
    /// </summary>
    public partial class MenuUser : Window
    {
        public List<ClassLibrary.DataObject> dataUser;
        public List<ClassLibrary.DataObject> TempDataUser;
        public string AllText = "";
        public static string pathFileUserData = "User_Data.json";
        public DataUser dataManager = new DataUser(pathFileUserData);
        public static string pathFileUserPassword = "User_Password.json";
        public UserManager userManager = new UserManager(pathFileUserPassword);
        int TopMargin = 50;
        static int Count = 0;
        public string user;
        public int action = 0;
        public MainWindow mainWindow;

        public MenuUser(string userName)
        {
            TempDataUser = new List<ClassLibrary.DataObject>();
            user = userName;
            Count = 0;
            InitializeComponent();
            NameUser.Content = userName;
            dataUser = dataManager.ProvideData(userName);
            if (dataUser == null) { return; }
            for (int i = 0; i < dataUser.Count; i++)
            {
                AllText = "";
                int number = i + 1;
                AllText += (number + ". ФИО: " + dataUser[i].FIO + "\n");
                AllText += ("  Группа: " + dataUser[i].Group + "\n");
                AllText += ("  Email: " + dataUser[i].Email + "\n");
                AllText += ("  Рейтинг: " + dataUser[i].Rating + "\n");
                AllText += ("  Дата зачисления: " + dataUser[i].TransferDate + "\n");
                AllText += ("  Время зачисления: " + dataUser[i].TransferTime + "\n");
                AllText += ("  Наличие стипендии: " + dataUser[i].ScholarshipAvailability + "\n");
                AllText += ("  Курс: " + dataUser[i].Course + "\n");
                Count++;
                //Создаем новое изображение
                // Создаем новый Image
                Image newImage = new Image
                {
                    Name = "Image" + Count,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Height = 210,
                    Width = 180,
                    Margin = new Thickness(10, TopMargin, 10, 5),
                    ToolTip = new TextBlock { Text = "Кликните для выбора изображения" },
                    Source = new BitmapImage(new Uri("free-icon-user-149071.png", UriKind.Relative)),
                    Cursor = Cursors.Hand
                };
                if (dataUser[i].Photo != null)
                {
                    if (!string.IsNullOrEmpty(dataUser[i].Photo))
                    {
                        newImage.Source = Base64ToBitmapImage(dataUser[i].Photo);
                    }
                }
                // Добавляем обработчик события
                newImage.MouseDown += Image_MouseDown;

                // Добавляем элемент в Grid
                grid.Children.Add(newImage);
                // Создаем новый RadioButton
                RadioButton newRadioButton = new RadioButton
                {
                    Name = "Label" + Count,
                    Content = AllText,
                    FontFamily = new FontFamily("Arial"),
                    FontSize = 24,
                    Margin = new Thickness(200, TopMargin, 10, 5),
                    GroupName = "DynamicGroup", // Обязательно устанавливаем GroupName для группировки
                    Style = (Style)FindResource("SelectablePanelStyle"),
                    Height = 250,
                    Width = 500,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                };
                TopMargin += 260;
                // Добавляем в Grid
                grid.Children.Add(newRadioButton);
            }
        }
        //public static string ImageToBase64(string imagePath)
        //{
        //    byte[] imageBytes = File.ReadAllBytes(imagePath);
        //    return Convert.ToBase64String(imageBytes);
        //}
        public static string BitmapImageToBase64(BitmapImage bitmapImage)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                PngBitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bitmapImage));
                encoder.Save(ms);
                return Convert.ToBase64String(ms.ToArray());
            }
        }
        public static BitmapImage Base64ToBitmapImage(string base64String)
        {
            byte[] imageBytes = Convert.FromBase64String(base64String);
            using (MemoryStream ms = new MemoryStream(imageBytes))
            {
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.StreamSource = ms;
                bitmapImage.EndInit();
                return bitmapImage;
            }
        }
        public void OnDataReturned(List<ClassLibrary.DataObject> returnedList, int number)
        {
            if (dataUser == null) { dataUser = new List<ClassLibrary.DataObject>(); }
            dataUser.Clear();
            dataUser.AddRange(returnedList);
            dataManager.SaveAll(user, returnedList);
            if (number == 0)
            {
                AddElement();
            }
            else
            {
                //Перезапись текста в ячейку
                AllText = "";
                AllText += (number + ". ФИО: " + dataUser[number - 1].FIO + "\n");
                AllText += ("  Группа: " + dataUser[number - 1].Group + "\n");
                AllText += ("  Email: " + dataUser[number - 1].Email + "\n");
                AllText += ("  Рейтинг: " + dataUser[number - 1].Rating + "\n");
                AllText += ("  Дата зачисления: " + dataUser[number - 1].TransferDate + "\n");
                AllText += ("  Время зачисления: " + dataUser[number - 1].TransferTime + "\n");
                AllText += ("  Наличие стипендии: " + dataUser[number - 1].ScholarshipAvailability + "\n");
                AllText += ("  Курс: " + dataUser[number - 1].Course + "\n");

                foreach (RadioButton button in grid.Children.OfType<RadioButton>())
                {
                    if (button.Name == "Label" + number)
                    {
                        button.Content = AllText;
                        break;
                    }
                }
            }
        }

        public void Click_NewEntry(object sender, RoutedEventArgs e)
        {
            var listPage = new PageNewOrDelete(dataUser, 0, null);
            listPage.DataReturned += OnDataReturned; // Подписываемся на событие
            TwoMainFrame.Navigate(listPage);

            Exit.Visibility = Visibility.Hidden;
            NewText.Visibility = Visibility.Hidden;
            Edit.Visibility = Visibility.Hidden;
            PhotoImage.Visibility = Visibility.Hidden;
            NameUser.Visibility = Visibility.Hidden;
            Panel.Visibility = Visibility.Hidden;
            Remove.Visibility = Visibility.Hidden;
            Menu.Visibility = Visibility.Hidden;
        }
        public void Edit_Click(object sender, RoutedEventArgs e)
        {
            int number = 0;
            foreach (RadioButton button in grid.Children.OfType<RadioButton>())
            {
                if (button.IsChecked == true)
                {
                    number = Convert.ToInt32(button.Name.Replace("Label", ""));
                    break;
                }
            }
            if (number == 0) { return; }

            var listPage = new PageNewOrDelete(dataUser, number, dataUser[number-1].Photo);
            listPage.DataReturned += OnDataReturned; // Подписываемся на событие
            TwoMainFrame.Navigate(listPage);

            Exit.Visibility = Visibility.Hidden;
            NewText.Visibility = Visibility.Hidden;
            Edit.Visibility = Visibility.Hidden;
            PhotoImage.Visibility = Visibility.Hidden;
            NameUser.Visibility = Visibility.Hidden;
            Panel.Visibility = Visibility.Hidden;
            Remove.Visibility = Visibility.Hidden;
            Menu.Visibility = Visibility.Hidden;

        }
        public void AddElement()
        {
            AllText = "";
            int number = dataUser.Count;
            AllText += (number + ". ФИО: " + dataUser[dataUser.Count - 1].FIO + "\n");
            AllText += ("  Группа: " + dataUser[dataUser.Count - 1].Group + "\n");
            AllText += ("  Email: " + dataUser[dataUser.Count - 1].Email + "\n");
            AllText += ("  Рейтинг: " + dataUser[dataUser.Count - 1].Rating + "\n");
            AllText += ("  Дата зачисления: " + dataUser[dataUser.Count - 1].TransferDate + "\n");
            AllText += ("  Время зачисления: " + dataUser[dataUser.Count - 1].TransferTime + "\n");
            AllText += ("  Наличие стипендии: " + dataUser[dataUser.Count - 1].ScholarshipAvailability + "\n");
            AllText += ("  Курс: " + dataUser[dataUser.Count - 1].Course + "\n");


            Count++;
            //Создаем новое изображение
            // Создаем новый Image
            Image newImage = new Image
            {
                Name = "Image" + Count,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Height = 210,
                Width = 180,
                Margin = new Thickness(10, TopMargin, 10, 5),
                ToolTip = new TextBlock { Text = "Кликните для выбора изображения" },
                Source = new BitmapImage(new Uri("free-icon-user-149071.png", UriKind.Relative)),
                Cursor = Cursors.Hand
            };

            // Добавляем обработчик события
            newImage.MouseDown += Image_MouseDown;

            // Добавляем элемент в Grid
            grid.Children.Add(newImage);
            // Создаем новый RadioButton
            RadioButton newRadioButton = new RadioButton
            {
                Name = "Label" + Count,
                Content = AllText,
                Margin = new Thickness(200, TopMargin, 10, 5),
                GroupName = "DynamicGroup", // Обязательно устанавливаем GroupName для группировки
                Style = (System.Windows.Style)FindResource("SelectablePanelStyle"),
                FontFamily = new FontFamily("Arial"),
                FontSize = 24,
                Height = 250,
                Width = 500,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
            };
            TopMargin += 260;
            // Добавляем в Grid
            grid.Children.Add(newRadioButton);


            //TwoMainFrame.Navigate(new PageNewOrDelete(dataUser, 0));

        }

        public void Exit_to_Authorization(object sender, RoutedEventArgs e)
        {
            userManager.SaveAll(pathFileUserPassword);
            dataManager.SaveAll(Name, dataUser);
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
        public void Image_MouseDown(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Image clickedImage = sender as System.Windows.Controls.Image;

            // Создаем диалоговое окно выбора файла
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "Выберите изображение",
                Filter = "Изображения (*.jpg;*.jpeg;*.png;*.bmp)|*.jpg;*.jpeg;*.png;*.bmp|Все файлы (*.*)|*.*",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)
            };

            // Если пользователь выбрал файл
            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    // Загружаем изображение
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(openFileDialog.FileName);
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.EndInit();

                    // Отображаем в элементе Image
                    clickedImage.Source = bitmap;

                    //Сохраняем в файле
                    string base64Image = BitmapImageToBase64(bitmap);

                    // Сохраняем в объект данных
                    dataUser[Convert.ToInt32(clickedImage.Name.Replace("Image", "")) - 1].Photo = base64Image;
                    dataManager.SaveAll(Name, dataUser);
                }
                catch
                {
                    MessageBox.Show($"Ошибка загрузки изображения. Неверный формат.", "Ошибка",
                                  MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        public void Remove_Click(object sender, RoutedEventArgs e)
        {
            int number = 0;
            foreach (RadioButton button in grid.Children.OfType<RadioButton>())
            {
                if (button.IsChecked == true)
                {
                    number = Convert.ToInt32(button.Name.Replace("Label", ""));
                    break;
                }
            }
            if (number == 0) { return; }

            MessageBoxResult result = MessageBox.Show(
                 $"Вы действительно хотите удалить запись №{number}?", // Текст сообщения
                 "Подтверждение",                                   // Заголовок окна
                 MessageBoxButton.YesNo,                            // Кнопки Да/Нет
                 MessageBoxImage.Question);                         // Иконка вопроса

            if (result == MessageBoxResult.No)
            {
                return;
            }

            //Удаляем из списка
            dataUser.RemoveAt(number - 1);
            grid.Children.Clear();
            dataManager.SaveAll(user, dataUser);
            //Перерисовываем всё
            TopMargin = 50;
            Count = 0;
            for (int i = 0; i < dataUser.Count; i++)
            {
                AllText = "";
                number = i + 1;
                AllText += (number + ". ФИО: " + dataUser[i].FIO + "\n");
                AllText += ("  Группа: " + dataUser[i].Group + "\n");
                AllText += ("  Email: " + dataUser[i].Email + "\n");
                AllText += ("  Рейтинг: " + dataUser[i].Rating + "\n");
                AllText += ("  Дата зачисления: " + dataUser[i].TransferDate + "\n");
                AllText += ("  Время зачисления: " + dataUser[i].TransferTime + "\n");
                AllText += ("  Наличие стипендии: " + dataUser[i].ScholarshipAvailability + "\n");
                AllText += ("  Курс: " + dataUser[i].Course + "\n");
                Count++;
                //Создаем новое изображение
                // Создаем новый Image
                Image newImage = new Image
                {
                    Name = "Image" + Count,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Height = 210,
                    Width = 180,
                    Margin = new Thickness(10, TopMargin, 10, 5),
                    ToolTip = new TextBlock { Text = "Кликните для выбора изображения" },
                    Source = new BitmapImage(new Uri("free-icon-user-149071.png", UriKind.Relative)),
                    Cursor = Cursors.Hand
                };

                if (dataUser[i].Photo != null)
                {
                    if (!string.IsNullOrEmpty(dataUser[i].Photo))
                    {
                        newImage.Source = Base64ToBitmapImage(dataUser[i].Photo);
                    }
                }

                // Добавляем обработчик события
                newImage.MouseDown += Image_MouseDown;

                // Добавляем элемент в Grid
                grid.Children.Add(newImage);
                // Создаем новый RadioButton
                RadioButton newRadioButton = new RadioButton
                {
                    Name = "Label" + Count,
                    Content = AllText,
                    FontFamily = new FontFamily("Arial"),
                    FontSize = 24,
                    Margin = new Thickness(200, TopMargin, 10, 5),
                    GroupName = "DynamicGroup", // Обязательно устанавливаем GroupName для группировки
                    Style = (Style)FindResource("SelectablePanelStyle"),
                    Height = 250,
                    Width = 500,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                };
                TopMargin += 260;
                // Добавляем в Grid
                grid.Children.Add(newRadioButton);

            }
        }
        public void OneStudent_Click(object sender, RoutedEventArgs e)
        {
            if (TempDataUser != null)
            {
                TempDataUser.Clear();
            }
            action = 1;
            Exit.Visibility = Visibility.Hidden;
            NewText.Visibility = Visibility.Hidden;
            Edit.Visibility = Visibility.Hidden;
            PhotoImage.Visibility = Visibility.Hidden;
            NameUser.Visibility = Visibility.Hidden;
            Remove.Visibility = Visibility.Hidden;
            Menu.Visibility = Visibility.Hidden;

            Ex.Visibility = Visibility.Visible;
            Report.Visibility = Visibility.Visible;
            Help.Visibility = Visibility.Visible;
            Help.Content = "Выберите одного студента.";

            int number;
            grid.Children.Clear();
            //Перерисовываем всё
            TopMargin = 50;
            Count = 0;
            for (int i = 0; i < dataUser.Count; i++)
            {
                AllText = "";
                number = i + 1;
                AllText += (number + ". ФИО: " + dataUser[i].FIO + "\n");
                AllText += ("  Группа: " + dataUser[i].Group + "\n");
                AllText += ("  Email: " + dataUser[i].Email + "\n");
                AllText += ("  Рейтинг: " + dataUser[i].Rating + "\n");
                AllText += ("  Дата зачисления: " + dataUser[i].TransferDate + "\n");
                AllText += ("  Время зачисления: " + dataUser[i].TransferTime + "\n");
                AllText += ("  Наличие стипендии: " + dataUser[i].ScholarshipAvailability + "\n");
                AllText += ("  Курс: " + dataUser[i].Course + "\n");
                Count++;

                // Создаем новый RadioButton
                RadioButton newRadioButton = new RadioButton
                {
                    Name = "Label" + Count,
                    Content = AllText,
                    FontFamily = new FontFamily("Arial"),
                    FontSize = 24,
                    Margin = new Thickness(10, TopMargin, 10, 5),
                    GroupName = "DynamicGroup", // Обязательно устанавливаем GroupName для группировки
                    Style = (Style)FindResource("SelectablePanelStyle"),
                    Height = 250,
                    Width = 670,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                };
                TopMargin += 260;
                // Добавляем в Grid
                grid.Children.Add(newRadioButton);
            }
        }
        public void Report_Click(object sender, RoutedEventArgs e)
        {
            // Сбор выбранных студентов
            TempDataUser = new List<ClassLibrary.DataObject>();
            TempDataUser.Clear();
            if (action == 1)
            {
                foreach (var button in grid.Children.OfType<RadioButton>().Where(b => b.IsChecked == true))
                {
                    int number = Convert.ToInt32(button.Name.Replace("Label", ""));
                    if (number > 0 && number <= dataUser.Count)
                    {
                        TempDataUser.Add(dataUser[number - 1]);
                    }
                }
            }
            else if (action == 2)
            {
                foreach (var button in grid.Children.OfType<ToggleButton>().Where(b => b.IsChecked == true))
                {
                    int number = Convert.ToInt32(button.Name.Replace("Label", ""));
                    if (number > 0 && number <= dataUser.Count)
                    {
                        TempDataUser.Add(dataUser[number - 1]);
                    }
                }
            }
            else if (action == 3)
            {
                foreach (System.Windows.Controls.Label button in grid.Children.OfType<System.Windows.Controls.Label>())
                {
                    int number = Convert.ToInt32(button.Name.Replace("Label", ""));
                    if (number > 0 && number <= dataUser.Count)
                    {
                        TempDataUser.Add(dataUser[number - 1]);
                    }
                }
            }

            if (TempDataUser.Count == 0)
            {
                MessageBox.Show("Нет данных для создания отчета. Сначала выберите студентов.",
                              "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var saveFileDialog = new SaveFileDialog
            {
                Filter = "PDF files (*.pdf)|*.pdf",
                DefaultExt = ".pdf",
                FileName = $"Отчет_студентов_{DateTime.Now:yyyyMMdd_HHmmss}.pdf"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                try

                {
                    // Создаем PDF документ
                    PdfDocument document = new PdfDocument();
                    PdfPage page = document.AddPage();
                    page.Size = PdfSharp.PageSize.A4;
                    XGraphics gfx = XGraphics.FromPdfPage(page);

                    // Шрифты
                    XFont titleFont = new XFont("Arial", 16);
                    XFont headerFont = new XFont("Arial", 12);
                    XFont normalFont = new XFont("Arial", 10);

                    // Параметры размещения
                    double leftMargin = 50;
                    double textBlockWidth = 250;
                    double photoX = leftMargin + textBlockWidth + 20;
                    double yPos = 50;
                    double lineHeight = 15;
                    double pageBottomMargin = 50;
                    double studentBlockHeight = 180; // Примерная высота блока студента

                    // Функция для создания новой страницы
                    void NewPage()
                    {
                        page = document.AddPage();
                        page.Size = PdfSharp.PageSize.A4;
                        gfx = XGraphics.FromPdfPage(page);
                        yPos = 50;
                    }

                    // Заголовок отчета
                    if (action == 1)
                    {
                        gfx.DrawString("ОТЧЕТ ПО СТУДЕНТУ", titleFont, XBrushes.Black,
                                 new XRect(0, yPos, page.Width, page.Height),
                                 XStringFormats.TopCenter);
                    }
                    else if (action == 2)
                    {
                        gfx.DrawString("ОТЧЕТ ПО СТУДЕНТАМ", titleFont, XBrushes.Black,
                                 new XRect(0, yPos, page.Width, page.Height),
                                 XStringFormats.TopCenter);
                    }
                    else
                    {
                        gfx.DrawString("ОТЧЕТ ПО СТУДЕНТАМ", titleFont, XBrushes.Black,
                                 new XRect(0, yPos, page.Width, page.Height),
                                 XStringFormats.TopCenter);
                    }

                    yPos += 40;

                    // Дата создания
                    gfx.DrawString($"Дата создания: {DateTime.Now:dd.MM.yyyy HH:mm}", normalFont, XBrushes.Black,
                                  new XRect(0, yPos, page.Width - 20, page.Height),
                                  XStringFormats.TopRight);
                    yPos += 40;

                    // Для каждого студента
                    foreach (var student in TempDataUser)
                    {
                        // Проверяем, поместится ли следующий студент на странице
                        if (yPos + studentBlockHeight > page.Height - pageBottomMargin)
                        {
                            NewPage();
                        }

                        double studentBlockStart = yPos;

                        // Выводим данные студента
                        double textY = yPos;
                        gfx.DrawString($"ФИО: {student.FIO}", headerFont, XBrushes.Black,
                                      new XRect(leftMargin, textY, textBlockWidth, lineHeight),
                                      XStringFormats.TopLeft);
                        textY += lineHeight;

                        gfx.DrawString($"Группа: {student.Group}", normalFont, XBrushes.Black,
                                      new XRect(leftMargin, textY, textBlockWidth, lineHeight),
                                      XStringFormats.TopLeft);
                        textY += lineHeight;

                        gfx.DrawString($"Email: {student.Email}", normalFont, XBrushes.Black,
                                      new XRect(leftMargin, textY, textBlockWidth, lineHeight),
                                      XStringFormats.TopLeft);
                        textY += lineHeight;

                        gfx.DrawString($"Рейтинг: {student.Rating}", normalFont, XBrushes.Black,
                                      new XRect(leftMargin, textY, textBlockWidth, lineHeight),
                                      XStringFormats.TopLeft);
                        textY += lineHeight;

                        gfx.DrawString($"Дата зачисления: {student.TransferDate}", normalFont, XBrushes.Black,
                                      new XRect(leftMargin, textY, textBlockWidth, lineHeight),
                                      XStringFormats.TopLeft);
                        textY += lineHeight;

                        gfx.DrawString($"Время зачисления: {student.TransferTime}", normalFont, XBrushes.Black,
                                      new XRect(leftMargin, textY, textBlockWidth, lineHeight),
                                      XStringFormats.TopLeft);
                        textY += lineHeight;

                        gfx.DrawString($"Стипендия: {student.ScholarshipAvailability}", normalFont, XBrushes.Black,
                                      new XRect(leftMargin, textY, textBlockWidth, lineHeight),
                                      XStringFormats.TopLeft);
                        textY += lineHeight;

                        gfx.DrawString($"Курс: {student.Course}", normalFont, XBrushes.Black,
                                      new XRect(leftMargin, textY, textBlockWidth, lineHeight),
                                      XStringFormats.TopLeft);
                        textY += lineHeight;

                        // Вычисляем высоту текстового блока
                        double textBlockHeight = textY - studentBlockStart;

                        // Выводим фото
                        if (!string.IsNullOrEmpty(student.Photo))
                        {
                            try
                            {
                                string base64Data = student.Photo.Contains(",")
                                    ? student.Photo.Split(',')[1]
                                    : student.Photo;

                                byte[] imageBytes = Convert.FromBase64String(base64Data);

                                string tempFile = Path.GetTempFileName();
                                File.WriteAllBytes(tempFile, imageBytes);

                                using (XImage image = XImage.FromFile(tempFile))
                                {
                                    double photoHeight = textBlockHeight;
                                    double photoWidth = image.PixelWidth * (photoHeight / image.PixelHeight);

                                    double maxPhotoWidth = page.Width - photoX - 50;
                                    if (photoWidth > maxPhotoWidth)
                                    {
                                        photoHeight = photoHeight * (maxPhotoWidth / photoWidth);
                                        photoWidth = maxPhotoWidth;
                                    }

                                    gfx.DrawRectangle(XPens.LightGray, photoX, studentBlockStart, photoWidth, photoHeight);
                                    gfx.DrawImage(image, photoX, studentBlockStart, photoWidth, photoHeight);
                                }

                                File.Delete(tempFile);
                            }
                            catch (Exception ex)
                            {
                                gfx.DrawString("[Ошибка фото]",
                                              new XFont("Arial", 8),
                                              XBrushes.Red,
                                              new XRect(photoX, studentBlockStart, 100, 20),
                                              XStringFormats.TopLeft);
                            }
                        }
                        else
                        {
                            gfx.DrawString("[Нет фото]",
                                          new XFont("Arial", 8),
                                          XBrushes.Gray,
                                          new XRect(photoX, studentBlockStart, 100, 20),
                                          XStringFormats.TopLeft);
                        }

                        yPos = textY + 20;
                        gfx.DrawLine(XPens.LightGray, leftMargin, yPos, page.Width - 50, yPos);
                        yPos += 30;
                    }

                    document.Save(saveFileDialog.FileName);
                    MessageBox.Show("Отчет успешно сохранен!", "Успех",
                                  MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при создании отчета: {ex.Message}",
                                  "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            Ex_Click(sender, e);
        }
        public void ManyStudent_Click(object sender, RoutedEventArgs e)
        {
            action = 2;
            if (TempDataUser != null)
            {
                TempDataUser.Clear();
            }

            Exit.Visibility = Visibility.Hidden;
            NewText.Visibility = Visibility.Hidden;
            Edit.Visibility = Visibility.Hidden;
            PhotoImage.Visibility = Visibility.Hidden;
            NameUser.Visibility = Visibility.Hidden;
            Remove.Visibility = Visibility.Hidden;
            Menu.Visibility = Visibility.Hidden;

            Ex.Visibility = Visibility.Visible;
            Report.Visibility = Visibility.Visible;
            Help.Visibility = Visibility.Visible;
            Help.Content = "Выберите группу студентов.";

            int number;
            grid.Children.Clear();
            //Перерисовываем всё
            TopMargin = 50;
            Count = 0;
            for (int i = 0; i < dataUser.Count; i++)
            {
                AllText = "";
                number = i + 1;
                AllText += (number + ". ФИО: " + dataUser[i].FIO + "\n");
                AllText += ("  Группа: " + dataUser[i].Group + "\n");
                AllText += ("  Email: " + dataUser[i].Email + "\n");
                AllText += ("  Рейтинг: " + dataUser[i].Rating + "\n");
                AllText += ("  Дата зачисления: " + dataUser[i].TransferDate + "\n");
                AllText += ("  Время зачисления: " + dataUser[i].TransferTime + "\n");
                AllText += ("  Наличие стипендии: " + dataUser[i].ScholarshipAvailability + "\n");
                AllText += ("  Курс: " + dataUser[i].Course + "\n");
                Count++;

                // Создаем новый RadioButton
                ToggleButton newToggleButton = new ToggleButton
                {
                    Name = "Label" + Count,
                    Content = AllText,
                    FontFamily = new FontFamily("Arial"),
                    FontSize = 24,
                    Margin = new Thickness(10, TopMargin, 10, 5),
                    Style = (Style)FindResource("SelectablePanelStyle"),
                    Height = 250,
                    Width = 670,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                };
                TopMargin += 260;
                // Добавляем в Grid
                grid.Children.Add(newToggleButton);
            }
        }
        public void ProgressStudent_Click(object sender, RoutedEventArgs e)
        {
            action = 3;
            Exit.Visibility = Visibility.Hidden;
            NewText.Visibility = Visibility.Hidden;
            Edit.Visibility = Visibility.Hidden;
            PhotoImage.Visibility = Visibility.Hidden;
            NameUser.Visibility = Visibility.Hidden;
            Remove.Visibility = Visibility.Hidden;
            Menu.Visibility = Visibility.Hidden;

            MaxRating.Visibility = Visibility.Visible;
            MinRating.Visibility = Visibility.Visible;
            ToggleSwitch.Visibility = Visibility.Visible;
            NumberCourse.Visibility = Visibility.Visible;
            Filter_Button.Visibility = Visibility.Visible;

            Ex.Visibility = Visibility.Visible;
            Report.Visibility = Visibility.Visible;
            Help.Visibility = Visibility.Visible;
            Filter.Visibility = Visibility.Visible;
            Filter.Content = @"     Фильтр
по интервалу 
успеваемости:
от                
до

по номеру 
курса:


по наличию 
стипендии:";
            int number;
            grid.Children.Clear();
            //Перерисовываем всё
            TopMargin = 50;
            Count = 0;
            for (int i = 0; i < dataUser.Count; i++)
            {
                AllText = "";
                number = i + 1;
                AllText += (number + ". ФИО: " + dataUser[i].FIO + "\n");
                AllText += ("  Группа: " + dataUser[i].Group + "\n");
                AllText += ("  Email: " + dataUser[i].Email + "\n");
                AllText += ("  Рейтинг: " + dataUser[i].Rating + "\n");
                AllText += ("  Дата зачисления: " + dataUser[i].TransferDate + "\n");
                AllText += ("  Время зачисления: " + dataUser[i].TransferTime + "\n");
                AllText += ("  Наличие стипендии: " + dataUser[i].ScholarshipAvailability + "\n");
                AllText += ("  Курс: " + dataUser[i].Course + "\n");
                Count++;

                // Создаем новый Label
                System.Windows.Controls.Label newLabel = new System.Windows.Controls.Label();
                newLabel.Name = "Label" + Count;
                newLabel.Content = AllText;
                newLabel.FontFamily = new FontFamily("Arial");
                newLabel.FontSize = 24;
                newLabel.Margin = new Thickness(200, TopMargin, 10, 5);
                newLabel.Height = 250;
                newLabel.Width = 500;
                newLabel.HorizontalAlignment = HorizontalAlignment.Left;
                newLabel.VerticalAlignment = VerticalAlignment.Top;
                newLabel.Style = (Style)FindResource("SelectableLabelStyle");
                TopMargin += 260;
                // Добавляем в Grid
                grid.Children.Add(newLabel);
            }
        }
        public void Ex_Click(object sender, RoutedEventArgs e)
        {
            Exit.Visibility = Visibility.Visible;
            NewText.Visibility = Visibility.Visible;
            Edit.Visibility = Visibility.Visible;
            PhotoImage.Visibility = Visibility.Visible;
            NameUser.Visibility = Visibility.Visible;
            Remove.Visibility = Visibility.Visible;
            Menu.Visibility = Visibility.Visible;

            Ex.Visibility = Visibility.Hidden;
            Report.Visibility = Visibility.Hidden;
            Help.Visibility = Visibility.Hidden;
            Filter.Visibility = Visibility.Hidden;
            MaxRating.Visibility = Visibility.Hidden;
            MinRating.Visibility = Visibility.Hidden;
            ToggleSwitch.Visibility = Visibility.Hidden;
            NumberCourse.Visibility = Visibility.Hidden;
            Filter_Button.Visibility = Visibility.Hidden;

            int number;
            grid.Children.Clear();
            //Перерисовываем всё
            TopMargin = 50;
            Count = 0;
            for (int i = 0; i < dataUser.Count; i++)
            {
                AllText = "";
                number = i + 1;
                AllText += (number + ". ФИО: " + dataUser[i].FIO + "\n");
                AllText += ("  Группа: " + dataUser[i].Group + "\n");
                AllText += ("  Email: " + dataUser[i].Email + "\n");
                AllText += ("  Рейтинг: " + dataUser[i].Rating + "\n");
                AllText += ("  Дата зачисления: " + dataUser[i].TransferDate + "\n");
                AllText += ("  Время зачисления: " + dataUser[i].TransferTime + "\n");
                AllText += ("  Наличие стипендии: " + dataUser[i].ScholarshipAvailability + "\n");
                AllText += ("  Курс: " + dataUser[i].Course + "\n");
                Count++;
                //Создаем новое изображение
                // Создаем новый Image
                Image newImage = new Image
                {
                    Name = "Image" + Count,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Height = 210,
                    Width = 180,
                    Margin = new Thickness(10, TopMargin, 10, 5),
                    ToolTip = new TextBlock { Text = "Кликните для выбора изображения" },
                    Source = new BitmapImage(new Uri("free-icon-user-149071.png", UriKind.Relative)),
                    Cursor = Cursors.Hand
                };

                if (dataUser[i].Photo != null)
                {
                    if (!string.IsNullOrEmpty(dataUser[i].Photo))
                    {
                        newImage.Source = Base64ToBitmapImage(dataUser[i].Photo);
                    }
                }

                // Добавляем обработчик события
                newImage.MouseDown += Image_MouseDown;

                // Добавляем элемент в Grid
                grid.Children.Add(newImage);
                // Создаем новый RadioButton
                RadioButton newRadioButton = new RadioButton
                {
                    Name = "Label" + Count,
                    Content = AllText,
                    FontFamily = new FontFamily("Arial"),
                    FontSize = 24,
                    Margin = new Thickness(200, TopMargin, 10, 5),
                    GroupName = "DynamicGroup", // Обязательно устанавливаем GroupName для группировки
                    Style = (Style)FindResource("SelectablePanelStyle"),
                    Height = 250,
                    Width = 500,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                };
                TopMargin += 260;
                // Добавляем в Grid
                grid.Children.Add(newRadioButton);

            }
        }

        public void Filter_Click(object sender, RoutedEventArgs e)
        {
            int number;
            grid.Children.Clear();
            //Перерисовываем всё
            TopMargin = 50;
            Count = 0;
            if (TempDataUser != null)
            {
                TempDataUser.Clear();
            }
            MinRating.Text = MinRating.Text.Replace(" ", "");
            MaxRating.Text = MaxRating.Text.Replace(" ", "");
            NumberCourse.Text = NumberCourse.Text.Replace(" ", "");
            for (int i = 0; i < dataUser.Count; i++)
            {
                if (double.TryParse(MinRating.Text, out _) && double.TryParse(MaxRating.Text, out _) && dataUser[i].Rating != "")
                {
                    if (Convert.ToDouble(dataUser[i].Rating) < Convert.ToDouble(Convert.ToString(MinRating.Text)) || Convert.ToDouble(dataUser[i].Rating) > Convert.ToDouble(MaxRating.Text))
                    {
                        Count++;
                        continue;
                    }
                }
                if (int.TryParse(NumberCourse.Text, out _) && dataUser[i].Course != "")
                {
                    if (Convert.ToInt32(NumberCourse.Text) != Convert.ToInt32(dataUser[i].Course.Replace(" курс", "")))
                    {
                        Count++;
                        continue;
                    }
                }

                if (ToggleSwitch.IsChecked == true && dataUser[i].ScholarshipAvailability != "Да")
                {
                    Count++;
                    continue;
                }
                if (ToggleSwitch.IsChecked == false && dataUser[i].ScholarshipAvailability != "Нет")
                {
                    Count++;
                    continue;
                }
                AllText = "";
                number = i + 1;
                AllText += (number + ". ФИО: " + dataUser[i].FIO + "\n");
                AllText += ("  Группа: " + dataUser[i].Group + "\n");
                AllText += ("  Email: " + dataUser[i].Email + "\n");
                AllText += ("  Рейтинг: " + dataUser[i].Rating + "\n");
                AllText += ("  Дата зачисления: " + dataUser[i].TransferDate + "\n");
                AllText += ("  Время зачисления: " + dataUser[i].TransferTime + "\n");
                AllText += ("  Наличие стипендии: " + dataUser[i].ScholarshipAvailability + "\n");
                AllText += ("  Курс: " + dataUser[i].Course + "\n");
                Count++;

                // Создаем новый Label
                System.Windows.Controls.Label newLabel = new System.Windows.Controls.Label();
                newLabel.Name = "Label" + Count;
                newLabel.Content = AllText;
                newLabel.FontFamily = new FontFamily("Arial");
                newLabel.FontSize = 24;
                newLabel.Margin = new Thickness(200, TopMargin, 10, 5);
                newLabel.Height = 250;
                newLabel.Width = 500;
                newLabel.HorizontalAlignment = HorizontalAlignment.Left;
                newLabel.VerticalAlignment = VerticalAlignment.Top;
                newLabel.Style = (Style)FindResource("SelectableLabelStyle");
                TopMargin += 260;
                // Добавляем в Grid
                grid.Children.Add(newLabel);

                //Добавляем во временный список
                TempDataUser.Add(dataUser[i]);
            }
        }

    }

}
