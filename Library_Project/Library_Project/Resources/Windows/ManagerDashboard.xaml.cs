using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for ManagerDashboard.xaml
    /// </summary>
    public partial class ManagerDashboard : Window, INotifyPropertyChanged
    {
        private List<Book> _allBooks;
        public List<Book> AllBooks { get { return _allBooks; } set { _allBooks = value; NotifyPropertyChanged("AllBooks"); } }

        private List<Employees> _allEmployees;
        public List<Employees> AllEmployees { get { return _allEmployees; } set { _allEmployees = value; NotifyPropertyChanged("AllEmployees"); } }

        public ManagerDashboard()
        {
            if (Book.TakeAllBooks() != null)
                AllBooks = Book.TakeAllBooks().ToList();
            else AllBooks = null;

            InitializeComponent();
            DataContext = this;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
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
            EmployeesPan.Visibility = Visibility.Collapsed;
            BankPan.Visibility = Visibility.Collapsed;
        }

        private void BooksPn_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            HomePan.Visibility = Visibility.Collapsed;
            BookPan.Visibility = Visibility.Visible;
            EmployeesPan.Visibility = Visibility.Collapsed;
            BankPan.Visibility = Visibility.Collapsed;
        }
        private void WalletPn_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            HomePan.Visibility = Visibility.Collapsed;
            BookPan.Visibility = Visibility.Collapsed;
            EmployeesPan.Visibility = Visibility.Collapsed;
            BankPan.Visibility = Visibility.Visible;
        }
        private void EmployeesPn_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            HomePan.Visibility = Visibility.Collapsed;
            BookPan.Visibility = Visibility.Collapsed;
            EmployeesPan.Visibility = Visibility.Visible;
            BankPan.Visibility = Visibility.Collapsed;
        }

        private void AddBook_Click(object sender, RoutedEventArgs e)
        {
            AddBookWindow window = new AddBookWindow(this);
            window.Show();
        }

        private void AddEmployee_Click(object sender, RoutedEventArgs e)
        {
            Register register = new Register(typeOfUser.Employee);
            register.Show();
            this.Close();
        }

        private void RemoveEmployee_Click(object sender, RoutedEventArgs e)
        {
            RemoveEmployeeWindow window = new RemoveEmployeeWindow();
            window.Show();
            this.Close();
        }
    }
}
