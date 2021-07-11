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
    /// this window for adding book from Manager Dashbord
    /// </summary>
    public partial class AddBookWindow : Window
    {
        ManagerDashboard md;
        public AddBookWindow(ManagerDashboard managerDashboard)
        {
            md = managerDashboard;
            InitializeComponent();
            txtName.Focus();
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
            if (string.IsNullOrEmpty(txtName.Text) || string.IsNullOrEmpty(txtAuthor.Text) || string.IsNullOrEmpty(txtCategory.Text) || string.IsNullOrEmpty(txtPublishNumber.Text) || string.IsNullOrEmpty(txtBookNumber.Text))
            {
                MessageBox.Show(".ابتدا فیلد هارا به طور کامل پرکنید");
                txtName.Focus();
                return;
            }
            if (int.Parse(txtPublishNumber.Text) < 0)
            {
                MessageBox.Show("شماره چاپ نمی تواند منفی باشد");
                txtPublishNumber.Clear();
                txtPublishNumber.Focus();
                return;
            }
            try
            {
                if (Convert.ToInt32(txtBookNumber.Text) <= 0)
                {
                    MessageBox.Show(".تعداد باید عددی مثبت باشد");
                    txtBookNumber.Clear();
                    txtBookNumber.Focus();
                    return;
                }
            }
            catch
            {
                MessageBox.Show("تعداد نمی تواند شامل حروف باشد");
                txtBookNumber.Text = "";
                txtBookNumber.Focus();
                return;
            }
            try
            {
                int.Parse(txtPublishNumber.Text);
            }
            catch
            {
                MessageBox.Show("شماره چاپ نمی تواند شامل حروف باشد");
                txtPublishNumber.Text = "";
                txtPublishNumber.Focus();
                return;
            }
            Book book = new Book(txtName.Text, txtAuthor.Text, txtCategory.Text, txtPublishNumber.Text, txtBookNumber.Text);

            try
            {
                if (Book.BookExists(book) == 0)
                {
                    int number = int.Parse(txtBookNumber.Text);
                    DatabaseControl.UpdateBookTable(book.Name, number);
                }
                else if (Book.BookExists(book) > 0)
                {
                    MessageBox.Show(".کتابی با همین نام اما اطلاعات متفاوت در کتابخانه موجود است. لطفا در وارد کردن نام کتاب دقت کنید");
                    txtName.Clear();
                    txtName.Focus();
                    return;
                }
                else
                {
                    book.AddBook();
                }
            }
            catch (ArgumentOutOfRangeException)
            {
                MessageBox.Show(".عدد وارد شده باید مثبت باشد");
                txtBookNumber.Focus();
            }
            catch (FormatException)
            {
                MessageBox.Show(".برای بخش تعداد و شماه نشر باید عدد وارد شود");
                txtBookNumber.Focus();
                txtPublishNumber.Focus();
            }
            catch
            {
                throw;
            }

            md.AllBooks = Book.TakeAllBooks().ToList();
            md.allBooksData.ItemsSource = md.AllBooks;
            MessageBox.Show(".کتاب با موفقیت اضافه شد");

            this.Close();
        }
    }
}
