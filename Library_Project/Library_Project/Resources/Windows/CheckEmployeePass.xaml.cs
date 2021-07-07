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
            if (WindNam == "Employee" || WindNam == "Remove")
                data = DatabaseControl.Select("SELECT * FROM T_Employees WHERE username='" + UserName.Trim() + "'");
            else
                data = DatabaseControl.Select("SELECT * FROM T_Members WHERE username='" + UserName.Trim() + "'");
        }
        private void Login_Click(object sender, RoutedEventArgs e)
        {
            if (txtPassword.Password == data.Rows[0]["password"].ToString())
            {
                
                if (Window == "Employee")
                {
                    EmployeeInformation changeInfo = new EmployeeInformation(UserName, "Employee");
                    changeInfo.Show();
                }
                if (Window == "Remove")
                {
                    BorrowedBook.RemoveBook(SearchedMemberWindow.UserName); 
                    MessageBox.Show("حذف کاربر با موفقیت انجام شد");
                    EmployeeDashboard employee = new EmployeeDashboard(UserName);
                    employee.Show();
                    this.Close(); 
                }
                if (Window == "Member")
                {
                    EmployeeInformation changeInfo = new EmployeeInformation(UserName, "Member");
                    changeInfo.Show();
                }
                this.Close();
            }
            else
            {
                if (Window == "Remove")
                {
                    MessageBox.Show("رمز نادرست است\nحذف کاربر انجام نشد");
                    SearchedMemberWindow searched = new SearchedMemberWindow(SearchedMemberWindow.UserName);
                    searched.Show();
                    this.Close();
                }                    
                else
                    MessageBox.Show("رمز نادرست است");
                txtPassword.Password = "";
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            txtPassword.Password = "";
            SearchedMemberWindow searched = new SearchedMemberWindow(SearchedMemberWindow.UserName);
            searched.Show();
            this.Close();
        }
    }
}