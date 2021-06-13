using Library_Project.Resources.Classes;
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
    /// Interaction logic for RemoveEmployeeWindow.xaml
    /// </summary>
    public partial class RemoveEmployeeWindow : Window
    {
        public RemoveEmployeeWindow()
        {
            InitializeComponent();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (Managers.RemoveEmployee(name.Text.Trim()))
                MessageBox.Show("کارمند با موفقیت حذف شد");
            else MessageBox.Show("همچین کاربری موجود نیست");
            ManagerDashboard md = new ManagerDashboard();
            md.Show();
            this.Close();
        }

        private void Cancle_Click(object sender, RoutedEventArgs e)
        {
            ManagerDashboard md = new ManagerDashboard();
            md.Show();
            this.Close();
        }
    }
}
