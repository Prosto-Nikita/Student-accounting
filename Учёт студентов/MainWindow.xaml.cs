using System;
using System.Windows;
using ClassLibrary;
namespace StudentAuthorization
{
    public partial class MainWindow : Window
    {
        public static string pathFileUserPassword = "User_Password.json";
        public UserManager userManager = new UserManager(pathFileUserPassword);
        public MainWindow()
        {
            InitializeComponent();
        }
        public void ClickExit(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public void Click_Entrance(object sender, RoutedEventArgs e)
        {
            userManager = new UserManager(pathFileUserPassword);
            ErrorAuthorization.Visibility = Visibility.Hidden;
            InputName.Text = InputName.Text.Trim();
            InputPassword.Text = InputPassword.Text.Trim();
            if (userManager.Authorization(InputName.Text, InputPassword.Text))
            {
                MenuUser menuUser = new MenuUser(InputName.Text);
                this.Close();
                menuUser.Show();
            }
            else
            {
                ErrorAuthorization.Visibility = Visibility.Visible;
            }
        }

        public void Click_Authorization(object sender, RoutedEventArgs e)
        {
            ErrorAuthorization.Visibility = Visibility.Hidden;
            MainFrame.Navigate(new Uri("AuthorizationPage.xaml", UriKind.Relative));
        }
    }

}
