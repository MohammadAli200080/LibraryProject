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
    /// this window is to check password of member, employee, manager; in order to perform the operation.
    /// </summary>
    /// <param name="userName">username of user</param>
    /// <param name="WindNam">from which window this window has been called.</param>
    /// <param name="md">if the calling class is ManagerDashboard then it will send itself as one of the params</param>
    public partial class CheckPassWindow : Window
    {
        string UserName = "";
        string Window = "";
        ManagerDashboard managerDashboard = null;
        DataTable data = new DataTable();
        public CheckPassWindow(string userName, string WindNam, ManagerDashboard md = null)
        {
            InitializeComponent();
            Window = WindNam;
            UserName = userName;
            managerDashboard = md;
            if (WindNam == "Employee" || WindNam == "Remove")
                data = DatabaseControl.Select("SELECT * FROM T_Employees WHERE username='" + UserName.Trim() + "'");
            else
                data = DatabaseControl.Select("SELECT * FROM T_Members WHERE username='" + UserName.Trim() + "'");
        }
        private void Login_Click(object sender, RoutedEventArgs e)
        {
            if (Window == "Manager")
            {
                var mainPass = Properties.Settings.Default.PassWord;
                if (!(mainPass == txtPassword.Password))
                {
                    MessageBox.Show(".رمز عبور وارد شده نادرست است");
                    txtPassword.Password = "";
                    return;
                }
                if (!Managers.AbleToPay(600000))
                {
                    MessageBox.Show(".مقدار پول شما کافی نمی باشد");
                    return;
                }
                if (!Managers.PayEmployees(600000))
                {
                    MessageBox.Show("Unknown error.");
                    return;
                }
                managerDashboard.BankUpdate();
                MessageBox.Show($"{Properties.Settings.Default.Bank} : مقدار موجودی جدید بانک پول\n.عملیات با موفقیت به اتمام رسید");
                txtPassword.Password = "";
                this.Close();
            }
            else if (Window == "RemoveEmployee")
            {
                var mainPass = Properties.Settings.Default.PassWord;
                if (!(mainPass == txtPassword.Password))
                {
                    MessageBox.Show(".رمز عبور وارد شده نادرست است");
                    txtPassword.Password = "";
                    return;
                }
                if (Managers.RemoveEmployee(UserName))
                {
                    MessageBox.Show("کارمند با موفقیت حذف شد");
                }
                managerDashboard.UpdateEmployeeGrid();
                managerDashboard.UpdateNumbersList();
                txtPassword.Password = "";
                this.Close();
            }
            else if (txtPassword.Password == data.Rows[0]["password"].ToString())
            {

                txtPassword.Password = "";

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
            if (Window == "Remove")
            {
                SearchedMemberWindow searched = new SearchedMemberWindow(SearchedMemberWindow.UserName);
                searched.Show();
            }
            if(Window== "Member")
            {
                MemberDashboard member = new MemberDashboard(UserName);
                member.Show();
            }
            if (Window == "Employee")
            {
                EmployeeDashboard employee = new EmployeeDashboard(UserName);
                employee.Show();
            }
            this.Close();
        }
    }
}