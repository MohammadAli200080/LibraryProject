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

namespace Library_Project.Resources.Windows
{
    /// <summary>
    /// Interaction logic for MemberDashboard.xaml
    /// </summary>
    public partial class MemberDashboard : Window, INotifyPropertyChanged
    {
        public string SearchBoxText { get; set; }

        private List<Book> _availableBooks;
        private List<BorrowedBook> _borrowedBooks;
        private List<string> _nameCollection;
        private List<string> _subscriptionCollectionNames;
        private string _moneyTxt;
        private MassagerEmployee _message;
        private MassagerEmployee Message
        {
            get => _message;
            set
            {
                if (value == null) _message = value;
                else if (_message == null) _message = value;
            }
        }

        public List<Book> AvailableBooks
        {
            get { return _availableBooks; }
            set { _availableBooks = value; NotifyPropertyChanged("AvailableBooks"); }
        }

        public List<BorrowedBook> BorrowedBooks
        {
            get { return _borrowedBooks; }
            set { _borrowedBooks = value; NotifyPropertyChanged("BorrowedBooks"); }
        }

        public List<string> NameCollection
        {
            get => _nameCollection;
            set { _nameCollection = value; NotifyPropertyChanged("NameCollection"); }
        }
        public List<string> SubscriptionCollectionNames
        {
            get => _subscriptionCollectionNames;
            set { _subscriptionCollectionNames = value; NotifyPropertyChanged("SubscriptionCollectionNames"); }
        }
        public string MoneyTxt
        {
            get => _moneyTxt;
            set { _moneyTxt = value; NotifyPropertyChanged("MoneyTxt"); }
        }

        private string Username { get; set; }
        private decimal Money { get; set; }

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

        public MemberDashboard(string username)
        {
            Money = Member.GetMemberMoney(username);
            Username = username;

            InitializeComponent();
            DataContext = this;

            SubscriptionCollectionNames = KindOfSubscription.Instance.GetNames().ToList();
            InitializeTextOfSubsription();
            UpdateAvailableBoosData();
            UpdateMyBooksData();
            NameCollection = Book.GetAllNames(Username).ToList();

            money.Text = Money.ToString("C0", CultureInfo.CreateSpecificCulture("fa-ir"));
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
            MyBooksPan.Visibility = Visibility.Collapsed;
            WalletPan.Visibility = Visibility.Collapsed;
            SubscriptionPan.Visibility = Visibility.Collapsed;
        }
        private void BooksPn_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            HomePan.Visibility = Visibility.Collapsed;
            BookPan.Visibility = Visibility.Visible;
            MyBooksPan.Visibility = Visibility.Collapsed;
            WalletPan.Visibility = Visibility.Collapsed;
            SubscriptionPan.Visibility = Visibility.Collapsed;
        }
        private void MyBooksPn_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            HomePan.Visibility = Visibility.Collapsed;
            BookPan.Visibility = Visibility.Collapsed;
            MyBooksPan.Visibility = Visibility.Visible;
            WalletPan.Visibility = Visibility.Collapsed;
            SubscriptionPan.Visibility = Visibility.Collapsed;
        }
        private void WalletPn_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            HomePan.Visibility = Visibility.Collapsed;
            BookPan.Visibility = Visibility.Collapsed;
            MyBooksPan.Visibility = Visibility.Collapsed;
            WalletPan.Visibility = Visibility.Visible;
            SubscriptionPan.Visibility = Visibility.Collapsed;
        }

        private void AccountPn_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            HomePan.Visibility = Visibility.Collapsed;
            BookPan.Visibility = Visibility.Collapsed;
            MyBooksPan.Visibility = Visibility.Collapsed;
            WalletPan.Visibility = Visibility.Collapsed;
            SubscriptionPan.Visibility = Visibility.Visible;
        }
        private void LoginPn_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (LogOut == null)
            {
                LogOut = new LogOutWindow(this);
                LogOut.Closed += (s, _) => LogOut = null;
                LogOut.Show();
            }
        }
        private void BookName_Checked(object sender, RoutedEventArgs e)
        {
            //TextOfSearch.Text = "نام کتابی که می خواهید بر اساس آن جستوجو کنید را وارد کنید.";
            SearchBoxText = "نام کتاب";
            AuthorName.IsChecked = false;
        }

        private void Author_Checked(object sender, RoutedEventArgs e)
        {
            //TextOfSearch.Text = "نام نویسنده ای که می خواهید بر اساس آن جستوجو کنید را وارد کنید.";
            SearchBoxText = "نام نویسنده";
            BookName.IsChecked = false;
        }

        private void BookName_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckAllCheckBoxes();
        }

        private void Author_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckAllCheckBoxes();
        }

        /// <summary>
        /// a method for checking if all checkboxes are empty or not
        /// </summary>

        private void CheckAllCheckBoxes()
        {
            if (AuthorName.IsChecked == false && BookName.IsChecked == false)
            {
                //TextOfSearch.Text = "";
                SearchBox.Text = "";
            }
        }

        private void Pay_Click(object sender, RoutedEventArgs e)
        {
            if (PaymentWindow == null)
            {
                PaymentWindow = new Payment(typeOfUser.MemberFromMemberWindow, Username);
                PaymentWindow.Closed += (s, _) => PaymentWindow = null;
                PaymentWindow.Show();
            }
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            string kindOfSearch;

            if (AuthorName.IsChecked == false && BookName.IsChecked == false)
            {
                MessageBox.Show(".لطفا ابتدا یکی از دو مورد داخل چک باکس را انتخاب کنید");
                SearchBox.Clear();
                return;
            }
            else if (AuthorName.IsChecked == true)
                kindOfSearch = "author";
            else kindOfSearch = "name";

            SearchedBookWindow window = new SearchedBookWindow(Username, kindOfSearch, SearchBox.Text);
            if (window.ShowBook()) window.Show();
            else SearchBox.Clear();
        }

        private void Borrow_Click(object sender, RoutedEventArgs e)
        {
            if (!AbleToBorrow())
            {
                BorrowedName.Clear();
                return;
            }

            if (Member.GetBook(BorrowedName.Text, Username))
            {
                UpdateAvailableBoosData();
                UpdateMyBooksData();
                NameCollection = Book.GetAllNames(Username).ToList();
                BorrowedName.Clear();
                MessageBox.Show(".کتاب با موفقیت به امانت گرفته شد");
                return;
            }

            MessageBox.Show(".کتابی با این نام موجود نمی باشد");
            BorrowedName.Clear();
        }

        private bool AbleToBorrow()
        {
            if (BorrowedBook.infoBorrowed(Username).Count >= 5)
            {
                MessageBox.Show(".تعداد کتاب های شما از حد قابل قبول بیشتر است. لطفا ابتدا تعدادی از کتاب های خود را پس دهید و سپس دوباره این عملیات را تکرار کنید");
                return false;
            }

            var member = Employees.SearchAllMember(Username)[0];
            DateTime a = DateTime.Parse(DateTime.Now.ToShortDateString());
            DateTime b = DateTime.Parse(member.SubsriptionDate);
            TimeSpan result = b - a;

            if (int.Parse(result.TotalDays.ToString()) < 7)
            {
                MessageBox.Show(".اشتراک شما در کمتر از یک هفتۀ آتی به اتمام می رسد لطفا ابتدا اقدام به تمدید آن کنید و سپس این عملیات را تکرار کنید");
                return false;
            }

            for (int i = 0; i < BorrowedBook.infoBorrowed(Username).Count; i++)
            {
                if (int.Parse(BorrowedBook.infoBorrowed(Username)[i].remainDate) < 0)
                {
                    MessageBox.Show(".لطفا ابتدا به باز پس دادن کتاب هایی که در اختیار دارید اقدام کنید و سپس این عملیات را تکرار کنید");
                    return false;
                }

                if (BorrowedBooks[i].nameBook.ToLower() == BorrowedName.Text.ToLower())
                {
                    MessageBox.Show(".شما یک کتاب با همین نام دارید، لطفا ابتدا به باز پس دادن آن اقدام کنید و سپس این عملیات را تکرار کنید");
                    return false;
                }
            }
            return true;
        }

        private void GiveBack_Click(object sender, RoutedEventArgs e)
        {
            var book = BorrowedBooks.FirstOrDefault(x => x.nameBook == GiveBackName.Text);

            if (book == null)
            {
                MessageBox.Show(".کتاب مورد نظر یافت نشد");
                return;
            }

            decimal money;
            if (Member.AbleToReturnBook(book.nameBook, out money))
            {
                if (Member.ReturnBook(book.nameBook, Username))
                {
                    UpdateAvailableBoosData();
                    UpdateMyBooksData();
                    NameCollection = Book.GetAllNames(Username).ToList();
                    MessageBox.Show(".کتاب با موفقیت پس داده شد");
                    return;
                }
            }
            else
            {
                MessageBox.Show($".را به حساب خود واریز کنید و سپس کتاب را پس دهید {money} لطفا در ابتدا مبلغ");
                return;
            }
        }

        private void KindOfSubsciptionComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (KindOfSubsciptionComboBox.SelectedIndex < 0)
                return;

            string selected = KindOfSubsciptionComboBox.SelectedItem.ToString();
            var cost = KindOfSubscription.Instance.GetCost(selected);
            MoneyTxt = cost.ToString("C0", CultureInfo.CreateSpecificCulture("fa-ir"));
        }

        private void Extend_Click(object sender, RoutedEventArgs e)
        {
            if (KindOfSubsciptionComboBox.SelectedIndex < 0)
            {
                MessageBox.Show(".ابتدا آیتم مورد نظر را انتخاب کنید");
                return;
            }
            string selected = KindOfSubsciptionComboBox.SelectedItem.ToString();
            var cost = KindOfSubscription.Instance.GetCost(selected);
            if (cost < 0)
            {
                MessageBox.Show(".ابتدا آیتم مورد نظر را انتخاب کنید");
                return;
            }
            if (cost > Member.GetMemberMoney(Username))
            {
                MessageBox.Show(".لطفا ابتدا کیف پول خود را شارژ کنید و سپس این علیات را تکرار کنید");
                return;
            }
            if (Member.UpdateMoneyOfMember(Username, (-1) * cost))
            {
                if (Member.UpdateSubscriptionOfmember(Username, selected))
                {
                    KindOfSubsciptionComboBox.SelectedIndex = -1;
                    MoneyTxt = "";
                    Money = Member.GetMemberMoney(Username);
                    InitializeTextOfSubsription();
                    money.Text = Money.ToString("C0", CultureInfo.CreateSpecificCulture("fa-ir"));
                    MessageBox.Show("    با موفقیت اشتراک شما شارژ شد، موجودی جدید شما     " + Member.GetMemberMoney(Username).ToString("C0", CultureInfo.CreateSpecificCulture("fa-ir")));
                    return;
                }
            }
        }

        /// <summary>
        /// a method for initializing the text of remained subscription. if the number of remained days is less than number of zero then
        /// background will be red. else it will be green.
        /// </summary>

        private void InitializeTextOfSubsription()
        {
            var member = Employees.SearchAllMember(Username)[0];
            DateTime a = DateTime.Parse(DateTime.Now.ToShortDateString());
            DateTime b = DateTime.Parse(member.SubsriptionDate);
            TimeSpan result = b - a;

            if (int.Parse(result.TotalDays.ToString()) > 0)
            {
                txtsubscriptionRemain.Background = new SolidColorBrush(Colors.Green);
                txtsubscriptionRemain.Foreground = new SolidColorBrush(Colors.Black);
                txtsubscriptionRemain.Text = int.Parse(result.TotalDays.ToString()).ToString() + "روز باقی مانده است";
            }
            else
            {
                txtsubscriptionRemain.Background = new SolidColorBrush(Colors.Red);
                txtsubscriptionRemain.Foreground = new SolidColorBrush(Colors.Black);
                txtsubscriptionRemain.Text = Math.Abs(int.Parse(result.TotalDays.ToString())) + "روز گذشته است";
            }
        }

        private void UpdateAvailableBoosData()
        {
            if (Book.TakeAvailableBooks().Length != 0)
                AvailableBooks = Book.TakeAvailableBooks().ToList();
            else AvailableBooks = new List<Book>();

            if (AvailableBooks.Count > 0)
            {
                availableBooksData.ItemsSource = AvailableBooks;
                availableBooksData.Visibility = Visibility.Visible;
            }
        }

        private void UpdateMyBooksData()
        {
            if (BorrowedBook.infoBorrowed(Username) != null)
                BorrowedBooks = BorrowedBook.infoBorrowed(Username);
            else BorrowedBooks = new List<BorrowedBook>();

            var allBooks = Book.TakeAllBooks();

            var listOfBooks = BorrowedBooks.Join(allBooks,
                                              borrowedBook => borrowedBook.nameBook,
                                              searchedBook => searchedBook.Name,
                                              (borrowedBook, searchedBook) => new
                                              {
                                                  Name = borrowedBook.nameBook,
                                                  Author = searchedBook.Author,
                                                  Category = searchedBook.Category,
                                                  PublishNumber = searchedBook.PublishNumber,
                                                  gotDate = borrowedBook.gotDate,
                                                  returnDate = borrowedBook.returnDate,
                                                  remainDate = borrowedBook.remainDate
                                              }).ToList();

            if (listOfBooks.Count > 0)
            {
                myBooksData.ItemsSource = listOfBooks;
                myBooksData.Visibility = Visibility.Visible;
            }
            else myBooksData.Visibility = Visibility.Collapsed;
        }

        private void btnSeting_Click(object sender, RoutedEventArgs e)
        {
            if (CheckPass == null)
            {
                CheckPass = new CheckPassWindow(Username, "Member");
                CheckPass.Closed += (s, _) => CheckPass = null;
                CheckPass.Show();
                this.Close();
            }
        }

        private void BtnMessage_Click(object sender, RoutedEventArgs e)
        {
            if (Message == null)
            {
                Message = new MassagerEmployee(MassengeType.member, Username);
                Message.Closed += (s, _) => Message = null;
                Message.Show();
                this.Close();
            }
        }

    }
}