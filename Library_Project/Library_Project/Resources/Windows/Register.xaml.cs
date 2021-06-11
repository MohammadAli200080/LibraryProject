using Microsoft.Win32;
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

namespace Library_Project.Resourses.Windows
{
    /// <summary>
    /// Interaction logic for Register.xaml
    /// </summary>
    public partial class Register : Window
    {
        OpenFileDialog open = new OpenFileDialog();
        public static List<string> Info = new List<string>();
        bool IsImage = false;
        public Register()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btnMin_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void ImageButton_Click(object sender, RoutedEventArgs e)
        {
            open = new OpenFileDialog();
            open.Title = "Select a Picture";
            open.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
                "Portable Network Graphic (*.png)|*.png";
            open.Multiselect = false;
            if (open.ShowDialog() == true)
            {
                ImageFill.Source = new BitmapImage(new Uri(open.FileName));
                User.Visibility = System.Windows.Visibility.Hidden;
                IsImage = true;
            }
        }
        private void btnCreatAcount_Click(object sender, RoutedEventArgs e)
        {
            string ImagePath;
            Info = new List<string>();
            if (IsImage)
            {
                ImagePath = open.FileName;
                Info.Add(txtuserName.Text);
                Info.Add(txtPassword.Password);
                Info.Add(txtPhone.Text);
                Info.Add(txtEmail.Text);
                Info.Add(ImagePath);

                if (!Library_Project.Resources.Classes.Validation.IsValidUsername(Info[0]))
                {
                    MessageBox.Show("نام کاربری نادرست می باشد");
                    txtuserName.Text = "";
                    return;
                }
                if (Library_Project.Resources.Classes.Validation.UserNameExists(txtuserName.Text))
                {
                    MessageBox.Show("نام کاربری تکراری است");
                    txtuserName.Text = "";
                    return;
                }
                if (!Library_Project.Resources.Classes.Validation.IsValidPassword(Info[1]))
                {
                    MessageBox.Show("پسورد نادرست می باشد");
                    txtPassword.Password = "";
                    return;
                }
                if (Library_Project.Resources.Classes.Validation.PasswordCorresponds(txtPassword.Password))
                {
                    MessageBox.Show("پسورد تکراری می باشد");
                    txtPassword.Password = "";
                    return;
                }
                if(!Library_Project.Resources.Classes.Validation.IsValidPhoneNumber(Info[2]))
                {
                    MessageBox.Show("تلفن همراه نادرست می باشد");
                    txtPhone.Text = "";
                    return;
                }
                if (Library_Project.Resources.Classes.Validation.PhoneNumberExists(txtPhone.Text))
                {
                    MessageBox.Show("تلفن همراه تکراری می باشد");
                    txtPhone.Text = "";
                    return;
                }
                if (!Library_Project.Resources.Classes.Validation.IsValidEmail(Info[3]))
                {
                    MessageBox.Show("ایمیل نادرست می باشد");
                    txtEmail.Text = "";
                    return;
                }
                if (Library_Project.Resources.Classes.Validation.EmailExists(txtEmail.Text))
                {
                    MessageBox.Show("ایمیل تکراری می باشد");
                    txtEmail.Text = "";
                    return;
                }
                if (Member.Addmember(Info))
                {
                    MessageBox.Show("با موفقیت ثبت شد");
                    ImageFill.Source = null;
                    User.Visibility = Visibility.Visible;
                    txtEmail.Text = "";
                    txtPassword.Password = "";
                    txtPhone.Text = "";
                    txtuserName.Text = "";
                    IsImage = false;
                    Payment pay = new Payment();
                    pay.Show();
                    this.Close();
                }                         
                else
                {
                    MessageBox.Show("اطلاعات نادرست می باشند");
                    IsImage = false;
                    ImageFill.Source = null;
                    User.Visibility = Visibility.Visible;
                    txtEmail.Text = "";
                    txtPassword.Password = "";
                    txtPhone.Text = "";
                    txtuserName.Text = "";
                    txtuserName.Text = "";
                    return;
                }                                 
            }
            else
                MessageBox.Show("عکس خود را انتخاب نمایید");
        }
        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow Login = new MainWindow();
            Login.Show();
            this.Close();
        }
    }
}