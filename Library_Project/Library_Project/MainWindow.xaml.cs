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
            if(Properties.Settings.Default.UserName==txtuserName.Text && Properties.Settings.Default.PassWord == txtPassword.Password)
            {
                ManagerDashboard Manegar = new ManagerDashboard();
                Manegar.Show();
                this.Close();
                return;
            }
            DataTable data = new DataTable();
            data = DatabaseControl.Select("SELECT username,password FROM T_Employees");
            for(int i = 0; i < data.Rows.Count; i++)
            {
                if (data.Rows[i]["username"].ToString() == txtuserName.Text && data.Rows[i]["password"].ToString() == txtPassword.Password)
                {
                    Employee employee = new Employee();
                    employee.Show();
                    this.Close();
                    return;
                }
            }
            DataTable data2 = new DataTable();
            data2 = DatabaseControl.Select("SELECT username,password FROM T_Members");
            for (int i = 0; i < data2.Rows.Count; i++)
            {
                if (data2.Rows[i]["username"].ToString() == txtuserName.Text && data2.Rows[i]["password"].ToString() == txtPassword.Password)
                {
                    Members Member = new Members();
                    Member.Show();
                    this.Close();
                    return;
                }
            }
            MessageBox.Show("نام کاربری/ پسورد نادرست می باشد");
        }
    }
}
