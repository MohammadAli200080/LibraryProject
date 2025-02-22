﻿using System;
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


        private CheckPassWindow _checkPassWindow;
        private LogOutWindow _logOutWindow;
        private MassagerEmployee _message;

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
        private MassagerEmployee Message
        {
            get => _message;
            set
            {
                if (value == null) _message = value;
                else if (_message == null) _message = value;
            }
        }
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

            allBooksData.Visibility = Visibility.Collapsed;
            if (BorrowedBooks.Count > 0)
                borrowedBooksData.Visibility = Visibility.Visible;
            availableBooksData.Visibility = Visibility.Collapsed;
        }

        private void MembersPn_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            HomePan.Visibility = Visibility.Collapsed;
            BookPan.Visibility = Visibility.Collapsed;
            MembersPan.Visibility = Visibility.Visible;
            WalletPan.Visibility = Visibility.Collapsed;

            if (DelayedMembersInReturning.Count > 0)
                delayedMembersInReturningData.Visibility = Visibility.Visible;
            delayedMembersInPaymentData.Visibility = Visibility.Collapsed;
            allMembersData.Visibility = Visibility.Collapsed;
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
            if (LogOut == null)
            {
                LogOut = new LogOutWindow(null, this);
                LogOut.Closed += (s, _) => LogOut = null;
                LogOut.Show();
            }
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
        private void BtnSeting_Click(object sender, RoutedEventArgs e)
        {
            if (CheckPass == null)
            {
                CheckPass = new CheckPassWindow(Username, "Employee");
                CheckPass.Closed += (s, _) => CheckPass = null;
                CheckPass.Show();
                this.Close();
            }
        }

        private void BtnMessage_Click(object sender, RoutedEventArgs e)
        {
            if (Message == null)
            {
                Message = new MassagerEmployee(MassengeType.employee, Username);
                Message.Closed += (s, _) => Message = null;
                Message.Show();
                this.Close();
            }
        }
        private void InitializeMoney()
        {
            Money = Employees.GetMoneyOfEmployee(Username);
            money.Content = Money.ToString("C0", CultureInfo.CreateSpecificCulture("fa-ir"));
        }

        private void InitializeDelayedMembersInReturn()
        {
            var list = Employees.TakeDelayedMemebrsInReturn();
            if (list.Count != 0)
                DelayedMembersInReturning = list;
            else DelayedMembersInReturning = new List<Member>();

            if (DelayedMembersInReturning.Count > 0)
            {
                delayedMembersInReturningData.ItemsSource = DelayedMembersInReturning;
                delayedMembersInReturningData.Visibility = Visibility.Visible;
            }
        }

        private void InitializeDelayedMembersInPayment()
        {
            var list = Employees.TakeDelayedMembersInPayment();
            if (list.Count != 0)
                DelayedMembersInPayment = list;
            else DelayedMembersInPayment = new List<Member>();

            if (DelayedMembersInPayment.Count > 0)
            {
                delayedMembersInPaymentData.ItemsSource = DelayedMembersInPayment;
            }
        }

        private void InitializeAllMembers()
        {
            var list = Employees.TakeAllMember();
            if (list.Count != 0)
                AllMembers = list;
            else AllMembers = new List<Member>();

            if (AllMembers.Count > 0)
            {
                allMembersData.ItemsSource = AllMembers;
            }
        }

        private void InitializeAvailableBooks()
        {
            var list = Book.TakeAvailableBooks();
            if (list.Length != 0)
                AvailableBooks = list.ToList();
            else AvailableBooks = new List<Book>();

            if (AvailableBooks.Count > 0)
            {
                availableBooksData.ItemsSource = AvailableBooks;
            }
        }

        private void InitializeBorrowedBooks()
        {
            var list = Book.TakeBorrowedBooks();
            if (list.Length != 0)
                BorrowedBooks = list.ToList();
            else BorrowedBooks = new List<Book>();

            if (BorrowedBooks.Count > 0)
            {
                borrowedBooksData.ItemsSource = BorrowedBooks;
                borrowedBooksData.Visibility = Visibility.Visible;
            }
        }

        private void InitializeAllBooks()
        {
            var list = Book.TakeAllBooks();
            if (list.Length != 0)
                AllBooks = list.ToList();
            else AllBooks = new List<Book>();

            if (AllBooks.Count > 0)
            {
                allBooksData.ItemsSource = AllBooks;
            }
        }
    }
}
