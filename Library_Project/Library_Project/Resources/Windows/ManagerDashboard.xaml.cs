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

        private List<int> _rowNumberAllEmployee;
        public List<int> RowNumberAllEmployee
        {
            get => _rowNumberAllEmployee;
            set { _rowNumberAllEmployee = value; NotifyPropertyChanged("Numbers"); }
        }

        private decimal _money;
        public decimal Money { get { return _money; } set { _money = value; NotifyPropertyChanged("Money"); } }

        private CheckPassWindow _checkPassWindow;
        private LogOutWindow _logOutWindow;
        private Payment _paymentWindow;
        private CheckPassWindow CheckPass
        {
            get => _checkPassWindow;
            set
            {
                if (value == null) _checkPassWindow = value;
                else if (_checkPassWindow == null) _checkPassWindow = value;
            }
        }
        private LogOutWindow LogOut
        {
            get => _logOutWindow;
            set
            {
                if (value == null) _logOutWindow = value;
                else if (_logOutWindow == null) _logOutWindow = value;
            }
        }
        private Payment PaymentWindow
        {
            get => _paymentWindow;
            set
            {
                if (value == null) _paymentWindow = value;
                else if (_paymentWindow == null) _paymentWindow = value;
            }
        }
        public ManagerDashboard()
        {
            InitializeComponent();
            UpdateEmployeeGrid();
            UpdateBookGrid();
            BankUpdate();
            UpdateNumbersList();

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
            if (LogOut == null)
            {
                LogOut = new LogOutWindow(null, null, this);
                LogOut.Closed += (s, _) => LogOut = null;
                LogOut.Show();
            }
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
            if (CheckPass == null)
            {
                CheckPass = new CheckPassWindow(parameter.UserName, "RemoveEmployee", this);
                CheckPass.Closed += (s, _) => CheckPass = null;
                CheckPass.Show();
            }
        }

        private void SalarayEmployee_Click(object sender, RoutedEventArgs e)
        {
            if (CheckPass == null)
            {
                CheckPass = new CheckPassWindow("admin", "Manager", this);
                CheckPass.Closed += (s, _) => CheckPass = null;
                CheckPass.Show();
            }
        }

        private void Pay_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMoney.Text))
            {
                MessageBox.Show(".لطفا ابتدا فیلد مبلغ را پر کنید");
                return;
            }
            if (decimal.Parse(txtMoney.Text) < 100000)
            {
                MessageBox.Show(".حداقل باید 100,000ریال پرداخت کنید");
                txtMoney.Clear();
                return;
            }
            if (PaymentWindow == null)
            {
                PaymentWindow = new Payment(typeOfUser.Manager, "admin", this, txtMoney.Text);
                PaymentWindow.Closed += (s, _) => PaymentWindow = null;
                PaymentWindow.Show();
                txtMoney.Clear();
            }
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
        public void UpdateEmployeeGrid()
        {
            if (Managers.TakeAllEmployee().Count != 0)
                AllEmployees = Managers.TakeAllEmployee().ToList();
            else AllEmployees = new List<Employees>();

            if (AllEmployees.Count > 0)
            {
                allEmployeesData.Visibility = Visibility.Visible;
                allEmployeesData.ItemsSource = AllEmployees;
            }
            else
            {
                allEmployeesData.Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// a method for upadating bookGrid
        /// </summary>
        private void UpdateBookGrid()
        {
            if (Book.TakeAllBooks().Length != 0)
                AllBooks = Book.TakeAllBooks().ToList();
            else AllBooks = new List<Book>();

            if (AllBooks.Count > 0)
            {
                allBooksData.Visibility = Visibility.Visible;
                allBooksData.ItemsSource = AllBooks;
            }
            else
            {
                allBooksData.Visibility = Visibility.Collapsed;
            }
        }

        public void UpdateNumbersList()
        {
            RowNumberAllEmployee = new List<int>();
            for (int i = 0; i< AllEmployees.Count; i++)
            {
                RowNumberAllEmployee.Add(i + 1);
            }
        }

        private void allEmployeesData_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }

        private void txtMoney_TextChanged(object sender, TextChangedEventArgs e)
        {
            var text = txtMoney.Text;
            if (text.Any(x => char.IsLetter(x)))
            {
                MessageBox.Show(".امکان وارد کردن حرف در این بحش وجود ندارد");
                txtMoney.Clear();
                txtMoney.Focus();
                return;
            }
            if (txtMoney.Text != string.Empty)
            {
                txtMoney.Text = string.Format("{0:N0}", double.Parse(txtMoney.Text.Replace(",", "")));
                txtMoney.Select(txtMoney.Text.Length, 0);
            }
        }
    }
}
