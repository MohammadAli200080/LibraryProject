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
using System.Windows.Shapes;
using Library_Project.Resources.Classes;
using Library_Project.Resourses.Windows;

namespace Library_Project.Resources.Windows
{
    /// <summary>
    /// Interaction logic for MemberDashboard.xaml
    /// </summary>
    public partial class MemberDashboard : Window
    {
        public string Username { get; set; }
        public MemberDashboard(string username)
        {
            InitializeComponent();
            DataContext = this;

            Username = username;
            money.Text = Member.GetMemberMoney(Username) + " تومان";
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btnMin_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
        private void HomePn_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            HomePan.Visibility = Visibility.Visible;
            BookPan.Visibility = Visibility.Collapsed;
            MyBooksPan.Visibility = Visibility.Collapsed;
            WalletPan.Visibility = Visibility.Collapsed;
        }

        private void BookName_Checked(object sender, RoutedEventArgs e)
        {
            TextOfSearch.Text = "نام کتابی که می خواهید بر اساس آن جستوجو کنید را وارد کنید.";
            SearchBox.Text = "نام کتاب";
        }

        private void Author_Checked(object sender, RoutedEventArgs e)
        {
            TextOfSearch.Text = "نام نویسنده ای که می خواهید بر اساس آن جستوجو کنید را وارد کنید.";
            SearchBox.Text = "نام نویسنده";
        }

        private void BookName_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckAllCheckBoxes();
        }

        private void Author_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckAllCheckBoxes();
        }

        private void CheckAllCheckBoxes()
        {
            if (AuthorName.IsChecked == false && BookName.IsChecked == false)
            {
                TextOfSearch.Text = "";
                SearchBox.Text = "";
            }
        }

        private void Pay_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
