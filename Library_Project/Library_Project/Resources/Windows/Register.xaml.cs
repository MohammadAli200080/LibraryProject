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
using Library_Project.Resources.Windows;
using System.IO;
using System.Data.SqlClient;

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
        typeOfUser type;

        string byteOfImage;
        public Register(typeOfUser type)
        {
            this.type = type;

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

                open.OpenFile();
                FileStream fs = new FileStream(open.FileName, FileMode.Open, FileAccess.Read);
                byteOfImage = ImageToByte(fs);
                DatabaseControl.Exe("INSERT INTO T_Employees (imgSrc) VALUES ('" + byteOfImage + "')");
            }
        }
        public static ImageSource ByteToImage(byte[] imageData)
        {
            BitmapImage biImg = new BitmapImage();
            MemoryStream ms = new MemoryStream(imageData);
            biImg.BeginInit();
            biImg.StreamSource = ms;
            biImg.EndInit();

            ImageSource imgSrc = biImg as ImageSource;

            return imgSrc;
        }
        public static string ImageToByte(FileStream fs)
        {
            byte[] imgBytes = new byte[fs.Length];
            fs.Read(imgBytes, 0, Convert.ToInt32(fs.Length));
            string encodeData = Convert.ToBase64String(imgBytes, Base64FormattingOptions.InsertLineBreaks);

            return encodeData;
        }
        private void btnCreatAcount_Click(object sender, RoutedEventArgs e)
        {
            Info = new List<string>();
            if (IsImage)
            {
                Info.Add(txtuserName.Text);
                Info.Add(txtPassword.Password);
                Info.Add(txtPhone.Text);
                Info.Add(txtEmail.Text);
                Info.Add(byteOfImage);

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
                if (!Library_Project.Resources.Classes.Validation.IsValidPhoneNumber(Info[2]))
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
                if (type == typeOfUser.Member)
                {
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
                        Payment pay = new Payment(type);
                        pay.Show();
                        this.Close();
                        return;
                    }
                }
                else if (type == typeOfUser.Employee)
                {
                    ManagerDashboard md = new ManagerDashboard();
                    if (Managers.AddEmployee(Info))
                    {
                        MessageBox.Show("با موفقیت ثبت شد");
                        //Update the Employees information after adding new employee
                        md.allEmployeesData.ItemsSource = md.AllEmployees;
                        ImageFill.Source = null;
                        User.Visibility = Visibility.Visible;
                        txtEmail.Text = "";
                        txtPassword.Password = "";
                        txtPhone.Text = "";
                        txtuserName.Text = "";
                        IsImage = false;
                        Payment pay = new Payment(type);
                        pay.Show();
                        this.Close();
                        return;
                    }
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
            if (type == typeOfUser.Member)
            {
                MainWindow Login = new MainWindow();
                Login.Show();
            }
            else
            {
                ManagerDashboard md = new ManagerDashboard();
                md.Show();
            }
            this.Close();
        }
    }
}