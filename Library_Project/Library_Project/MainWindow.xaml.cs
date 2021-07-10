using System;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Library_Project.Resources.Windows;
using Library_Project.Resources.Classes;
using Library_Project.Resourses.Windows;

namespace Library_Project
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtuserName.Text) && string.IsNullOrWhiteSpace(txtPassword.Password))
            {
                MessageBox.Show(".لطفا تمام فیلد ها را پر کنید");
                return;
            }
            if (string.IsNullOrWhiteSpace(txtPassword.Password))
            {
                MessageBox.Show(".لطفا فیلم رمز عبور را پر کنید");
                return;
            }
            if (string.IsNullOrWhiteSpace(txtuserName.Text))
            {
                MessageBox.Show(".لطفا فیلد نام را پر کنید");
                return;
            }
            if (Properties.Settings.Default.UserName == txtuserName.Text && Properties.Settings.Default.PassWord == txtPassword.Password)
            {
                txtuserName.Clear();
                txtPassword.Clear();
                ManagerDashboard manager = new ManagerDashboard();
                manager.Show();
                this.Close();
                return;
            }
            if (Library_Project.Resources.Classes.Validation.PasswordCorresponds(txtPassword.Password, txtuserName.Text, out string window))
            {
                if (window == "Employee")
                {
                    EmployeeDashboard employee = new EmployeeDashboard(txtuserName.Text.Trim());
                    employee.Show();
                }
                else 
                {
                    MemberDashboard member = new MemberDashboard(txtuserName.Text.Trim());
                    member.Show();
                }
                this.Close();
                return;
            }
            MessageBox.Show("نام کاربری/ پسورد نادرست می باشد");
            txtuserName.Clear();
            txtPassword.Clear();
        }
        private void btnCreat_Click(object sender, RoutedEventArgs e)
        {
            txtuserName.Clear();
            txtPassword.Clear();
            Register CreatAcount = new Register(typeOfUser.Member);
            CreatAcount.Show();
            this.Close();
        }
    }
}