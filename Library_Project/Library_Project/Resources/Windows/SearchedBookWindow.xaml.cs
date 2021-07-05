using Library_Project.Resources.Classes;
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

namespace Library_Project.Resources.Windows
{
    /// <summary>
    /// a window for showing result of search in Member dashboard
    /// </summary>
    public partial class SearchedBookWindow : Window
    {
        private List<Book> SearchedAvailableBooks { get; set; }
        //private List<BorrowedBook> BorrowedBooks { get; set; }
        private string Username { get; set; }

        public SearchedBookWindow(string username, string kindOfSearch, string search)
        {
            Username = username;

            try
            {
                if (kindOfSearch == "author")
                    SearchedAvailableBooks = Book.SearchAvailableBooksByAuthor(search).ToList();
                else SearchedAvailableBooks = Book.SearchAvailableBooksByName(search).ToList();
            }
            catch
            {
                MessageBox.Show(".کتاب مورد نظر یافت نشد");
                this.Close();
                return;
            }

            BooksSearchData.ItemsSource = SearchedAvailableBooks;

            InitializeComponent();
        }

        private void Return_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
