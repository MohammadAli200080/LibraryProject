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

namespace Library_Project.Resources.Windows
{
    /// <summary>
    /// Interaction logic for LogOutWindow.xaml
    /// </summary>
    public partial class LogOutWindow : Window
    {
        MemberDashboard memberDashboard;
        EmployeeDashboard employeeDashboard;
        ManagerDashboard managerDashboard;

        public LogOutWindow(MemberDashboard md = null, EmployeeDashboard ed = null, ManagerDashboard mnd = null)
        {
            if (md != null)
            {
                memberDashboard = md;
                employeeDashboard = null;
                managerDashboard = null;
            }
            else if (ed != null)
            {
                memberDashboard = null;
                employeeDashboard = ed;
                managerDashboard = null;
            }
            else if (mnd != null)
            {
                memberDashboard = null;
                employeeDashboard = null;
                managerDashboard = mnd;
            }

            InitializeComponent();
        }

        private void BtnLogOut_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow();
            main.Show();

            if (memberDashboard != null) memberDashboard.Close();
            else if (managerDashboard != null) managerDashboard.Close();
            else employeeDashboard.Close();

            this.Close();
        }

        private void BtnCancle_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
