using System;
using System.Collections.Generic;
using System.Globalization;
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

namespace Library_Project.Resources.Windows
{
    /// <summary>
    /// Interaction logic for PayEmployeeWindow.xaml
    /// </summary>
    public partial class PayEmployeeWindow : Window
    {
        public PayEmployeeWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void Pay_CLick(object sender, RoutedEventArgs e)
        {
            //payment.Text = Managers.CalculatePayment(600).ToString() + " تومان";
            ManagerDashboard md = new ManagerDashboard();
            Managers.CalculatePayment(decimal.Parse(payment.Text));
            if (!(Properties.Settings.Default.PassWord == password.Password))
            {
                MessageBox.Show(".رمز عبور وارد شده نادرست است");
                return;
            }    
            if (!Managers.AbleToPay(600))
            {
                MessageBox.Show(".مقدار پول شما کافی نمی باشد");
                return;
            }
            if (!Managers.PayEmployees(600))
            {
                MessageBox.Show("Unknown error.");
                return;
            }
            MessageBox.Show(".عملیات با موفقیت به اتمام رسید");
            payment.Text = "";
            password.Password = "";
            this.Close();
            md.Show();
        }

        private void Cancle_Click(object sender, RoutedEventArgs e)
        {
            ManagerDashboard md = new ManagerDashboard();
            md.Show();
            this.Close();
        }
        private void payment_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (payment.Text != string.Empty)
            {
                payment.Text = string.Format("{0:N0}", double.Parse(payment.Text.Replace(",", "")));
                payment.Select(payment.Text.Length, 0);
            }
        }
    }
}
