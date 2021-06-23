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
    /// Interaction logic for Employee.xaml
    /// </summary>
    public partial class EmployeeDashboard : Window
    {
        public string Username { get; set; }
        public EmployeeDashboard(string username)
        {
            InitializeComponent();
            DataContext = this;

            Username = username;
            money.Text = Employees.GetMoneyOfEmployee(Username) + " تومان";
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
            MembersPan.Visibility = Visibility.Collapsed;
            WalletPan.Visibility = Visibility.Collapsed;
        }

        private void BooksPn_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            HomePan.Visibility = Visibility.Collapsed;
            BookPan.Visibility = Visibility.Visible;
            MembersPan.Visibility = Visibility.Collapsed;
            WalletPan.Visibility = Visibility.Collapsed;
        }

        private void MembersPn_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            HomePan.Visibility = Visibility.Collapsed;
            BookPan.Visibility = Visibility.Collapsed;
            MembersPan.Visibility = Visibility.Visible;
            WalletPan.Visibility = Visibility.Collapsed;
        }
        private void WalletPn_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            HomePan.Visibility = Visibility.Collapsed;
            BookPan.Visibility = Visibility.Collapsed;
            MembersPan.Visibility = Visibility.Collapsed;
            WalletPan.Visibility = Visibility.Visible;
        }

        private void ShowALlBooks_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BorrowedBooks_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AvailableBooks_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ShowALlMembers_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DelayedMembersInReturning_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DelayedMembersInPayment_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
