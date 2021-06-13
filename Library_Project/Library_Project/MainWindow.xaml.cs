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
            if (Properties.Settings.Default.UserName == txtuserName.Text && Properties.Settings.Default.PassWord == txtPassword.Password)
            {
                ManagerDashboard Manegar = new ManagerDashboard();
                Manegar.Show();
                this.Close();
                return;
            }
            if (Library_Project.Resources.Classes.Validation.PasswordCorresponds(txtPassword.Password, txtuserName.Text, out string window))
            {
                if (window == "Employee")
                {
                    EmployeeDashboard employee = new EmployeeDashboard();
                    employee.Show();
                }
                else 
                {
                    MemberDashboard member = new MemberDashboard();
                    member.Show();
                }
                this.Close();
                return;
            }
            MessageBox.Show("نام کاربری/ پسورد نادرست می باشد");
        }
        private void btnCreat_Click(object sender, RoutedEventArgs e)
        {
            Register CreatAcount = new Register();
            CreatAcount.Show();
            this.Close();
        }
    }
}