using ClassLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace StudentAuthorization
{
    public partial class AuthorizationPage : Page
    {
        public static string pathFileUserPassword = "User_Password.json";
        public static string pathFileUserData = "User_Data.json";
        public static UserManager userManager = new UserManager(pathFileUserPassword);
        public static DataUser dataUser = new DataUser(pathFileUserData);
        public AuthorizationPage()
        {
            InitializeComponent();
        }

        public void Click_Entrance(object sender, RoutedEventArgs e)
        {
            NameRepetition.Visibility = Visibility.Hidden;
            EmptyField.Visibility = Visibility.Hidden;
            PasswordMismatch.Visibility = Visibility.Hidden;

            InputNewName.Text = InputNewName.Text.Trim();
            InputNewPassword.Text = InputNewPassword.Text.Trim();
            InputNewPassword2.Text = InputNewPassword2.Text.Trim();

            if (InputNewName.Text == null || InputNewPassword.Text == null || InputNewName.Text == "" || InputNewPassword.Text == "")
            {
                EmptyField.Visibility = Visibility.Visible;
                return;
            }
            if (InputNewPassword.Text == InputNewPassword2.Text)
            {
                if (userManager.AddUser(InputNewName.Text, InputNewPassword.Text))
                {
                    dataUser.AddUser(InputNewName.Text);
                    userManager.SaveAll(pathFileUserPassword);
                    userManager = new UserManager(pathFileUserPassword);
                    var mainWindow = Application.Current.MainWindow;
                    MenuUser menuUser = new MenuUser(InputNewName.Text);
                    NavigationService.Navigate(null);
                    mainWindow.Close();
                    menuUser.Show();
                }
                else { NameRepetition.Visibility = Visibility.Visible; }
            }
            else { PasswordMismatch.Visibility = Visibility.Visible; }

        }

        public void Exit_Authorization(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(null);
        }
    }
}
