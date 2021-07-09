using Library_Project.Resources.Classes;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
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
    /// Interaction logic for EmployeeInformation.xaml
    /// </summary>
    public partial class EmployeeInformation : Window
    {
        DataTable data = new DataTable();
        string userName = "", Window = "";
        OpenFileDialog open = new OpenFileDialog();
        public static List<string> Info = new List<string>();
        bool IsImage = false;

        string byteOfImage;
        public EmployeeInformation(string UserName,string Window)
        {           
            InitializeComponent();
            userName = UserName;
            this.Window = Window;
            if (Window == "Employee")
                data = DatabaseControl.Select("SELECT * FROM T_Employees WHERE username='" + userName.Trim() + "'");
            else
                data = DatabaseControl.Select("SELECT * FROM T_Members WHERE username='" + userName.Trim() + "'");

            ImageFill.Source = ImageControl.ByteToImage(Convert.FromBase64String(data.Rows[0]["imgSrc"].ToString()));
            txtuserName.Text = data.Rows[0]["username"].ToString();
            txtPhone.Text = data.Rows[0]["phoneNumber"].ToString();
            txtEmail.Text = data.Rows[0]["email"].ToString();
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
                IsImage = true;

                open.OpenFile();
                FileStream fs = new FileStream(open.FileName, FileMode.Open, FileAccess.Read);
                byteOfImage = ImageControl.ImageToByte(fs);

                if (Window == "Employee")
                    DatabaseControl.Exe("UPDATE T_Employees SET imgSrc ='" + byteOfImage + "' WHERE username='" + userName.Trim() + "' ");
                else
                    DatabaseControl.Exe("UPDATE T_Members SET imgSrc ='" + byteOfImage + "' WHERE username='" + userName.Trim() + "' ");

            }
        }

        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            if (Window == "Employee")
            {
                EmployeeDashboard employee = new EmployeeDashboard(userName);
                employee.Show();
            }
            else
            {
                MemberDashboard member = new MemberDashboard(userName);
                member.Show();
            }

            ImageFill.Source = null;
            txtuserName.Text = "";
            txtPassword.Password = "";
            txtEmail.Text = "";
            txtPhone.Text = "";
            this.Close();
            
        }

        private void btnCreatAcount_Click(object sender, RoutedEventArgs e)
        {
            Info = new List<string>();

            Info.Add(txtPassword.Password);
            Info.Add(txtPhone.Text);
            Info.Add(txtEmail.Text);

            if (!Library_Project.Resources.Classes.Validation.IsValidPassword(Info[0]))
            {
                MessageBox.Show("پسورد نادرست می باشد");
                txtPassword.Password = "";
                return;
            }
            if (!Library_Project.Resources.Classes.Validation.IsValidPhoneNumber(Info[1]))
            {
                MessageBox.Show("تلفن همراه نادرست می باشد");
                txtPhone.Text = "";
                return;
            }
            if (Library_Project.Resources.Classes.Validation.PhoneNumberExists(txtPhone.Text) && txtPhone.Text != data.Rows[0]["phoneNumber"].ToString())
            {
                MessageBox.Show("تلفن همراه تکراری می باشد");
                txtPhone.Text = "";
                return;
            }
            if (!Library_Project.Resources.Classes.Validation.IsValidEmail(Info[2]))
            {
                MessageBox.Show("ایمیل نادرست می باشد");
                txtEmail.Text = "";
                return;
            }
            if (Library_Project.Resources.Classes.Validation.EmailExists(txtEmail.Text) && txtEmail.Text != data.Rows[0]["email"].ToString())
            {
                MessageBox.Show("ایمیل تکراری می باشد");
                txtEmail.Text = "";
                return;
            }

            if (Window == "Employee")
                DatabaseControl.Exe("UPDATE T_Employees SET password='" + Info[0] + "',email='" + Info[2] + "',phoneNumber='" + Info[1] + "' WHERE username='" + userName.Trim() + "' ");
            else
                DatabaseControl.Exe("UPDATE T_Members SET password='" + Info[0] + "',email='" + Info[2] + "',phoneNumber='" + Info[1] + "' WHERE username='" + userName.Trim() + "' ");

            MessageBox.Show("تغییرات اعمال شد");
            ImageFill.Source = null;
            txtEmail.Text = "";
            txtPassword.Password = "";
            txtPhone.Text = "";
            txtuserName.Text = "";
            IsImage = false;

            if (Window == "Employee")
            {
                EmployeeDashboard employee = new EmployeeDashboard(userName);
                employee.Show();
            }
            else
            {
                MemberDashboard member = new MemberDashboard(userName);
                member.Show();
            }

            this.Close();
        }

        private void btnMin_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
    }
}
