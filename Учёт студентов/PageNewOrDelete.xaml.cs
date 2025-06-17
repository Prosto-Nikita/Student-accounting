using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using ClassLibrary;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;
using static StudentAuthorization.MenuUser;
namespace StudentAuthorization
{
    /// <summary>
    /// Логика взаимодействия для PageNewOrDelete.xaml
    /// </summary>
    public partial class PageNewOrDelete : Page
    {
        public List<ClassLibrary.DataObject> dataObjects;
        public event Action<List<ClassLibrary.DataObject>, int> DataReturned;
        int number;
        string Photo0;
        public PageNewOrDelete(List<ClassLibrary.DataObject> datas, int number, string Photo)
        {
            Photo0 = Photo;
            dataObjects = new List<ClassLibrary.DataObject>();

            if (datas != null)
            {
                dataObjects.AddRange(datas);
            }

            InitializeComponent();
            this.number = number;
            if (number != 0)
            {
                FIO.Text = datas[number - 1].FIO;
                Group.Text = datas[number - 1].Group;
                Email.Text = datas[number - 1].Email;
                Rating.Text = datas[number - 1].Rating;
                Date.Text = datas[number - 1].TransferDate;
                Time.Text = datas[number - 1].TransferTime;
                if (datas[number - 1].ScholarshipAvailability == "Да")
                {
                    ToggleSwitch.IsChecked = true;
                    Yes_No.Content = "Да";
                }
                Cours.Text = datas[number - 1].Course;
            }
        }

        public void SaveEdit_Click(object sender, RoutedEventArgs e)
        {
            // Получаем главное окно через визуальное дерево
            Window mainWindow = Window.GetWindow(this);

            if (mainWindow != null)
            {
                // Обращаемся к кнопке по имени
                if (mainWindow.FindName("Exit") is Button button)
                {
                    button.Visibility = Visibility.Visible;
                }
                if (mainWindow.FindName("NewText") is Button button2)
                {
                    button2.Visibility = Visibility.Visible;
                }
                if (mainWindow.FindName("Edit") is Button button3)
                {
                    button3.Visibility = Visibility.Visible;
                }
                if (mainWindow.FindName("PhotoImage") is Image image)
                {
                    image.Visibility = Visibility.Visible;
                }
                if (mainWindow.FindName("NameUser") is Label label)
                {
                    label.Visibility = Visibility.Visible;
                }
                if (mainWindow.FindName("Panel") is Label panel)
                {
                    panel.Visibility = Visibility.Visible;
                }
                if (mainWindow.FindName("Remove") is Button button5)
                {
                    button5.Visibility = Visibility.Visible;
                }
                if (mainWindow.FindName("Menu") is Menu menu)
                {
                    menu.Visibility = Visibility.Visible;
                }
            }

            var newDataObject = new ClassLibrary.DataObject
            {
                FIO = Convert.ToString(FIO.Text),
                Group = Convert.ToString(Group.Text),
                Email = Convert.ToString(Email.Text),
                Rating = Convert.ToString(Rating.Text),
                TransferDate = Convert.ToString(Date.Text),
                TransferTime = Convert.ToString(Time.Text),
                Course = Convert.ToString(Cours.Text),
                ScholarshipAvailability = Convert.ToString(Yes_No.Content)
            };

            if (number == 0)
            {
                // Добавляем новый объект в список
                dataObjects.Add(newDataObject);
            }
            else
            {
                newDataObject.Photo= Photo0;
                dataObjects[number - 1] = newDataObject;
            }
            DataReturned?.Invoke(dataObjects, number);
            NavigationService.Navigate(null);
        }

        public void ToggleSwitch_Checked(object sender, RoutedEventArgs e)
        {
            Yes_No.Content = "Да";
        }

        public void ToggleSwitch_Unchecked(object sender, RoutedEventArgs e)
        {
            Yes_No.Content = "Нет";
        }

    }

}
