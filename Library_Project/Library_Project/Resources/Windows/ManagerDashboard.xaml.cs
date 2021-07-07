using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
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
using Prism.Commands;

namespace Library_Project.Resources.Windows
{
    /// <summary>
    /// Interaction logic for ManagerDashboard.xaml
    /// </summary>
    public partial class ManagerDashboard : Window, INotifyPropertyChanged
    {
        private List<Book> _allBooks;
        public List<Book> AllBooks
        {
            get { return _allBooks; }
            set { _allBooks = value; NotifyPropertyChanged("AllBooks"); }
        }

        private List<Employees> _allEmployees;
        public List<Employees> AllEmployees
        {
            get { return _allEmployees; }
            set { _allEmployees = value; NotifyPropertyChanged("AllEmployees"); }
        }

        private decimal _money;
        public decimal Money { get { return _money; } set { _money = value; NotifyPropertyChanged("Money"); } }

        public ManagerDashboard()
        {
            InitializeComponent();
            UpdateEmployeeGrid();
            UpdateBookGrid();
            BankUpdate();

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

        private void LoginPn_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            LogOutWindow Logout = new LogOutWindow(null, null, this);
            Logout.Show();
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

        /// <summary>
        /// a delegate with a method for deleting employees form datagrid
        /// </summary>
        private DelegateCommand<Employees> _deleteCommand;
        public DelegateCommand<Employees> DeleteCommand => _deleteCommand ?? (_deleteCommand = new DelegateCommand<Employees>(ExecuteDeleteCommand));
        
        private void ExecuteDeleteCommand(Employees parameter)
        {
            if (Managers.RemoveEmployee(parameter.UserName))
            {
                MessageBox.Show("کارمند با موفقیت حذف شد");
                UpdateEmployeeGrid();
            }
        }

        private void SalarayEmployee_Click(object sender, RoutedEventArgs e)
        {
            CheckPassWindow window = new CheckPassWindow("admin", "Manager", this);
            window.Show();
        }

        private void Pay_Click(object sender, RoutedEventArgs e)
        {
            Payment payment = new Payment(typeOfUser.Manager, "admin");
            payment.Show();
            this.Close();
        }

        /// <summary>
        /// Updates Bank of library
        /// </summary>
        public void BankUpdate()
        {
            Money = Properties.Settings.Default.Bank;
            money.Text = Money.ToString("C0", CultureInfo.CreateSpecificCulture("fa-ir"));
        }

        /// <summary>
        /// a method for updating employeegrid
        /// </summary>
        private void UpdateEmployeeGrid()
        {
            if (Managers.TakeAllEmployee() != null)
                AllEmployees = Managers.TakeAllEmployee().ToList();
            else AllEmployees = new List<Employees>();

            if (AllEmployees.Count > 0)
            {
                allEmployeesData.Visibility = Visibility.Visible;
                allEmployeesData.ItemsSource = AllEmployees;
            }
        }

        /// <summary>
        /// a method for upadating bookGrid
        /// </summary>
        private void UpdateBookGrid()
        {
            if (Book.TakeAllBooks() != null)
                AllBooks = Book.TakeAllBooks().ToList();
            else AllBooks = new List<Book>();

            if (AllBooks.Count > 0)
            {
                allBooksData.Visibility = Visibility.Visible;
                allBooksData.ItemsSource = AllBooks;
            }
        }

    }
}
