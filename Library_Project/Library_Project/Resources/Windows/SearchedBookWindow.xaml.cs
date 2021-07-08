using Library_Project.Resources.Classes;
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

namespace Library_Project.Resources.Windows
{
    /// <summary>
    /// a window for showing result of search in Member dashboard
    /// </summary>
    public partial class SearchedBookWindow : Window, INotifyPropertyChanged
    {
        private List<Book> _searchedAvailableBooks { get; set; }
        public List<Book> SearchedAvailableBooks {
            get { return _searchedAvailableBooks; }
            set { _searchedAvailableBooks = value; NotifyPropertyChanged("SearchedAvailableBooks"); }
        }
        //private List<BorrowedBook> BorrowedBooks { get; set; }
        private string Username { get; set; }
        private string KindOfSearch { get; set; }
        private string Search { get; set; }

        public SearchedBookWindow(string username, string kindOfSearch, string search)
        {
            Username = username;
            this.KindOfSearch = kindOfSearch;
            this.Search = search;
            
            InitializeComponent();
        }
        public bool ShowBook()
        {
            try
            {
                if (KindOfSearch == "author")
                {
                    SearchedAvailableBooks = Book.SearchAvailableBooksByAuthor(Search).ToList();
                }
                else
                {
                    SearchedAvailableBooks = Book.SearchAvailableBooksByName(Search).ToList();
                }
            }
            catch
            {
                MessageBox.Show(".کتاب مورد نظر یافت نشد");
                return false;
            }

            if (SearchedAvailableBooks.Count == 0)
                return false;

            BooksSearchData.ItemsSource = SearchedAvailableBooks;
            BooksSearchData.Visibility = Visibility.Visible;
            return true;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        private void Return_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
