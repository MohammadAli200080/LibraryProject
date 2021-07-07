using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace Library_Project.Resources.Windows
{
    /// <summary>
    /// Interaction logic for Employee.xaml
    /// </summary>

    public partial class EmployeeDashboard : Window, INotifyPropertyChanged
    {
        private decimal Money { get; set; }

        private List<Book> _allBooks;
        public List<Book> AllBooks
        {
            get { return _allBooks; }
            set { _allBooks = value; NotifyPropertyChanged("AllBooks"); }
        }

        private List<Book> _borrowedBooks;
        public List<Book> BorrowedBooks
        {
            get { return _borrowedBooks; }
            set { _borrowedBooks = value; NotifyPropertyChanged("BorrowedBooks"); }
        }

        private List<Book> _availableBoos;
        public List<Book> AvailableBooks
        {
            get { return _availableBoos; }
            set { _availableBoos = value; NotifyPropertyChanged("AvailableBooks"); }
        }

        private List<Member> _allMembers;
        public List<Member> AllMembers
        {
            get { return _allMembers; }
            set { _allMembers = value; NotifyPropertyChanged("AllMembers"); }
        }

        private List<Member> _delayedMembersInReturning;
        public List<Member> DelayedMembersInReturning
        {
            get { return _delayedMembersInReturning; }
            set { _delayedMembersInReturning = value; NotifyPropertyChanged("DelayedMembersInReturning"); }
        }

        private List<Member> _delayedMembersInPayment;
        public List<Member> DelayedMembersInPayment
        {
            get { return _delayedMembersInPayment; }
            set { _delayedMembersInPayment = value; NotifyPropertyChanged("DelayedMembersInPayment"); }
        }

        public static string Username { get; set; }
        public EmployeeDashboard(string username)
        {
            InitializeComponent();
            DataContext = this;

            InitializeAllBooks();
            InitializeBorrowedBooks();
            InitializeAvailableBooks();
            InitializeAllMembers();
            InitializeDelayedMembersInPayment();
            InitializeDelayedMembersInReturn();
            Username = username;
            InitializeMoney();
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
        private void LoginPn_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            LogOutWindow Logout = new LogOutWindow(null, this);
            Logout.Show();
        }
        private void ShowALlBooks_Click(object sender, RoutedEventArgs e)
        {
            if (AllBooks.Count > 0)
                allBooksData.Visibility = Visibility.Visible;
            borrowedBooksData.Visibility = Visibility.Collapsed;
            availableBooksData.Visibility = Visibility.Collapsed;
        }

        private void BorrowedBooks_Click(object sender, RoutedEventArgs e)
        {
            allBooksData.Visibility = Visibility.Collapsed;
            if (BorrowedBooks.Count > 0)
                borrowedBooksData.Visibility = Visibility.Visible;
            availableBooksData.Visibility = Visibility.Collapsed;
        }

        private void AvailableBooks_Click(object sender, RoutedEventArgs e)
        {
            allBooksData.Visibility = Visibility.Collapsed;
            borrowedBooksData.Visibility = Visibility.Collapsed;
            if (AvailableBooks.Count > 0)
                availableBooksData.Visibility = Visibility.Visible;
        }

        private void ShowALlMembers_Click(object sender, RoutedEventArgs e)
        {
            if (AllMembers.Count > 0)
                allMembersData.Visibility = Visibility.Visible;
            delayedMembersInPaymentData.Visibility = Visibility.Collapsed;
            delayedMembersInReturningData.Visibility = Visibility.Collapsed;
        }

        private void DelayedMembersInReturning_Click(object sender, RoutedEventArgs e)
        {
            if (DelayedMembersInReturning.Count > 0)
                delayedMembersInReturningData.Visibility = Visibility.Visible;
            delayedMembersInPaymentData.Visibility = Visibility.Collapsed;
            allMembersData.Visibility = Visibility.Collapsed;
        }

        private void DelayedMembersInPayment_Click(object sender, RoutedEventArgs e)
        {
            if (DelayedMembersInPayment.Count > 0)
                delayedMembersInPaymentData.Visibility = Visibility.Visible;
            delayedMembersInReturningData.Visibility = Visibility.Collapsed;
            allMembersData.Visibility = Visibility.Collapsed;
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SearchBox.Text))
            {
                MessageBox.Show("ابتدا نام شخص مورد نظر را وارد کنید.");
                SearchBox.Clear();
                return;
            }
            SearchedMemberWindow window = new SearchedMemberWindow(SearchBox.Text.Trim());
            if (Employees.SearchAllMember(SearchBox.Text).Count != 0)
            {
                window.Show();
                SearchBox.Text = "";
                this.Close();
            }
            else
                SearchBox.Text = "";
        }
        private void btnSeting_Click(object sender, RoutedEventArgs e)
        {
            CheckPassWindow check = new CheckPassWindow(Username, "Employee");
            check.Show();           
        }

        private void InitializeMoney()
        {
            Money = Employees.GetMoneyOfEmployee(Username);
            money.Content = Money.ToString("C0", CultureInfo.CreateSpecificCulture("fa-ir"));
        }

        private void InitializeDelayedMembersInReturn()
        {
            if (Employees.TakeDelayedMemebrsInReturn() != null)
                DelayedMembersInReturning = Employees.TakeDelayedMemebrsInReturn().ToList();
            else DelayedMembersInReturning = new List<Member>();

            if (DelayedMembersInReturning.Count > 0)
            {
                delayedMembersInReturningData.ItemsSource = DelayedMembersInReturning;
            }
        }

        private void InitializeDelayedMembersInPayment()
        {
            if (Employees.TakeDelayedMembersInPayment() != null)
                DelayedMembersInPayment = Employees.TakeDelayedMembersInPayment().ToList();
            else DelayedMembersInPayment = new List<Member>();

            if (DelayedMembersInPayment.Count > 0)
            {
                delayedMembersInPaymentData.ItemsSource = DelayedMembersInPayment;
            }
        }

        private void InitializeAllMembers()
        {
            if (Employees.TakeAllMember() != null)
                AllMembers = Employees.TakeAllMember().ToList();
            else AllMembers = new List<Member>();

            if (AllMembers.Count > 0)
            {
                allMembersData.ItemsSource = AllMembers;
            }
        }

        private void InitializeAvailableBooks()
        {
            if (Book.TakeAvailableBooks() != null)
                AvailableBooks = Book.TakeAvailableBooks().ToList();
            else AvailableBooks = new List<Book>();

            if (AvailableBooks.Count > 0)
            {
                availableBooksData.ItemsSource = AvailableBooks;
            }
        }

        private void InitializeBorrowedBooks()
        {
            if (Book.TakeBorrowedBooks() != null)
                BorrowedBooks = Book.TakeBorrowedBooks().ToList();
            else BorrowedBooks = new List<Book>();

            if (BorrowedBooks.Count > 0)
            {
                borrowedBooksData.ItemsSource = BorrowedBooks;
            }
        }

        private void InitializeAllBooks()
        {
            if (Book.TakeAllBooks() != null)
                AllBooks = Book.TakeAllBooks().ToList();
            else AllBooks = new List<Book>();

            if (AllBooks.Count > 0)
            {
                allBooksData.ItemsSource = AllBooks;
            }
        }
    }
}
