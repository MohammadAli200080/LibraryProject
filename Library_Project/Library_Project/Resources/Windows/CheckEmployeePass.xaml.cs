using Library_Project.Resources.Classes;
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
using System.Windows.Shapes;

namespace Library_Project.Resources.Windows
{
    /// <summary>
    /// Interaction logic for CheckEmployeePass.xaml
    /// </summary>
    public partial class CheckEmployeePass : Window
    {
        string UserName = "";
        string Window = "";
        DataTable data = new DataTable();
        public CheckEmployeePass(string userName,string WindNam)
        {
            InitializeComponent();
            Window = WindNam;
            UserName = userName;
            data = DatabaseControl.Select("SELECT * FROM T_Employees WHERE username='" + UserName.Trim() + "'");
        }
        private void Login_Click(object sender, RoutedEventArgs e)
        {
            if (txtPassword.Password == data.Rows[0]["password"].ToString())
            {
                if(Window == "Employee")
                {
                    EmployeeInformation changeInfo = new EmployeeInformation(UserName);
                    changeInfo.Show();
                }
                if (Window == "Remove")
                {
                    MessageBox.Show("حذف کاربر با موفقیت انجام شد");
                }
                this.Close();
            }
            else
            {
                MessageBox.Show("رمز نادرست است");
                txtPassword.Password = "";
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            txtPassword.Password = "";
            this.Close();
        }
    }
}