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

namespace Library_Project.Resources.Windows
{
    /// <summary>
    /// Interaction logic for AddBookWindow.xaml
    /// </summary>
    public partial class AddBookWindow : Window
    {
        ManagerDashboard md;
        public AddBookWindow(ManagerDashboard managerDashboard)
        {
            md = managerDashboard;
            InitializeComponent();
        }

        private void btnMin_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnAddBook_Click(object sender, RoutedEventArgs e)
        {
            if (txtName.Text == "" || txtAuthor.Text == "" || txtCategory.Text == "" || txtPublishNumber.Text == "")
            {
                MessageBox.Show("ابتدا فیلد هارا به طور کامل پرکنید");
                return;
            }

            Book book = new Book(txtName.Text, txtAuthor.Text, txtCategory.Text, txtPublishNumber.Text, txtBookNumber.Text);
           
            try
            {
                if (Book.BookExists(book.Name))
                {
                    DatabaseControl.UpdateBookTable(book.Name);
                }
                else
                {
                    book.AddBook();
                }
            }
            catch (ArgumentOutOfRangeException err)
            {
                MessageBox.Show(err.Message);
            }
            catch(FormatException err)
            {
                MessageBox.Show(err.Message);
            }
            catch
            {
                throw;
            }

            md.AllBooks = Book.TakeAllBooks().ToList();
            md.allBooksData.ItemsSource = md.AllBooks;
            MessageBox.Show("کتاب با موفقیت اضافه شد");

            this.Close();
        }
    }
}
