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
using System.Globalization;
using Library_Project.Resources.Windows;

namespace Library_Project.Resourses.Windows
{
    /// <summary>
    /// Interaction logic for Payment.xaml
    /// </summary>
    public partial class Payment : Window
    {
        private typeOfUser Type { get; set; }
        private string username { get; set; }
        private ManagerDashboard md;
        private decimal Money { get; set; }
        public Payment(typeOfUser type, string username = "", ManagerDashboard managerDashboard = null, string money = null)
        {
            this.md = managerDashboard;
            this.username = username;
            this.Type = type;

            if (money != null)
                this.Money = decimal.Parse(money);

            InitializeComponent();

            InitializeMoneyText();
            txCardNum1.Focus();
        }

        private void InitializeMoneyText()
        {
            if (Type == typeOfUser.Manager || Type == typeOfUser.MemberFromMemberWindow)
            {
                txMoney.Text = Convert.ToDecimal(Money).ToString("#,##0.00");
                txMoney.Select(txMoney.Text.Length, 0);
                txMoney.IsReadOnly = true;
                txMoney.TextAlignment = TextAlignment.Center;
                var bc = new BrushConverter();
                txMoney.Background = (Brush)bc.ConvertFrom("#FF43AD03");
                txMoney.Foreground = Brushes.WhiteSmoke;
                txMoney.Margin = new Thickness(0);
                txMoney.FontSize = 22;
            }
        }

        private void btnMin_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
        private void btnPay_Click(object sender, RoutedEventArgs e)
        {
            string CardNumber = txCardNum1.Text + txCardNum2.Text + txCardNum3.Text + txCardNum4.Text;
            string PassWord = txPass.Password;
            string CVV2 = txCVV2.Text;
            string expiry = txYear.Text + '/' + txMonth.Text;

            if (!Library_Project.Resources.Classes.Validation.IsValidCvv2(CVV2))
            {
                MessageBox.Show("CVV2 نادرست می باشد");
                txCVV2.Focus();
                txCVV2.Text = "";
                return;
            }
            if (!Library_Project.Resources.Classes.Validation.IsValidExpiry(expiry))
            {
                MessageBox.Show("تاریخ انقضا کارت غیر قابل قبول است");
                txMonth.Focus();
                txMoney.Text = "";
                txMonth.Text = "";
                txYear.Text = "";
                return;
            }
            if (!Library_Project.Resources.Classes.Validation.IsValidCardNumber(CardNumber))
            {
                MessageBox.Show("شماره کارت نامعتبر است");
                txCardNum1.Focus();
                txCardNum1.Text = "";
                txCardNum2.Text = "";
                txCardNum3.Text = "";
                txCardNum4.Text = "";
                return;
            }
            if (!Library_Project.Resources.Classes.Validation.IsValidPassCard(PassWord))
            {
                MessageBox.Show("رمز کارت نادرست می باشد");
                txPass.Focus();
                txPass.Password = "";
                return;
            }
            if (decimal.Parse(txMoney.Text) < 100000)
            {
                MessageBox.Show("حداقل باید 100,000ریال پرداخت کنید");
                txMoney.Text = "";
                return;
            }

            if (Type == typeOfUser.Member && username == "")
            {
                if (DatabaseControl.Exe("UPDATE T_Members SET pocket='" + decimal.Parse(txMoney.Text) + "' WHERE username='" + Register.Info[0] + "'"))
                {
                    MessageBox.Show("با موفقیت ثبت نام شد\nموجودی حساب  " + (decimal.Parse(txMoney.Text)).ToString("C0", CultureInfo.CreateSpecificCulture("fa-ir")));
                    MainWindow Login = new MainWindow();
                    Login.Show();
                    this.Close();
                }
            }
            else if (Type == typeOfUser.MemberFromMemberWindow)
            {
                if (Member.UpdateMoneyOfMember(username, Convert.ToDecimal(txMoney.Text)))
                {
                    MessageBox.Show("موجودی حساب افزایش یافت" + (decimal.Parse(txMoney.Text)).ToString("C0", CultureInfo.CreateSpecificCulture("fa-ir")));
                    MemberDashboard md = new MemberDashboard(username);
                    md.Show();
                    this.Close();
                }
            }
            else if (Type == typeOfUser.Employee)
            {
                if (DatabaseControl.Exe("UPDATE T_Employees SET pocket='" + decimal.Parse(txMoney.Text) + "' WHERE username='" + Register.Info[0] + "'"))
                {
                    MessageBox.Show("با موفقیت ثبت نام شد\nموجودی حساب  " + (decimal.Parse(txMoney.Text)).ToString("C0", CultureInfo.CreateSpecificCulture("fa-ir")));
                    ManagerDashboard managerDashboard = new ManagerDashboard();
                    managerDashboard.Show();
                    this.Close();
                }
            }
            else if (Type == typeOfUser.Manager)
            {
                Properties.Settings.Default.Bank += decimal.Parse(txMoney.Text);
                var bank = Properties.Settings.Default.Bank;
                Properties.Settings.Default.Save();
                md.BankUpdate();
                MessageBox.Show("با موفقیت ثبت نام شد\nموجودی حساب  " + bank.ToString("C0", CultureInfo.CreateSpecificCulture("fa-ir")));
                this.Close();
            }
            else
            {
                MessageBox.Show("اطلاعات نادرست می باشد");
                txCardNum1.Focus();
                txCardNum1.Text = "";
                txCardNum2.Text = "";
                txCardNum3.Text = "";
                txCardNum4.Text = "";
                txCVV2.Text = "";
                txMoney.Text = "";
                txMonth.Text = "";
                txPass.Password = "";
                txYear.Text = "";
                return;
            }
        }
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            if (Type == typeOfUser.Employee)
            {
                MessageBox.Show("ثبت نام انجام نشد");
                DatabaseControl.Exe("DELETE FROM T_Employees WHERE username ='" + Register.Info[0] + "'");
                ManagerDashboard md = new ManagerDashboard();
                md.Show();
                this.Close();
            }
            else if (username == "")
            {
                MessageBox.Show("ثبت نام انجام نشد");
                DatabaseControl.Exe("DELETE FROM T_Members WHERE username ='" + Register.Info[0] + "'");
                MainWindow Login = new MainWindow();
                Login.Show();
                this.Close();
            }
            else if (typeOfUser.Manager == Type)
            {
                MessageBox.Show("موجودی افزایش نیافت");
                this.Close();
            }
            else
            {
                MessageBox.Show("موجودی افزایش نیافت");
                this.Close();
            }
        }

        private void txMoney_TextChanged(object sender, TextChangedEventArgs e)
        {
            var text = txMoney.Text;
            if (text.Any(x => char.IsLetter(x)))
            {
                MessageBox.Show(".امکان وارد کردن حرف در این بحش وجود ندارد");
                txMoney.Clear();
                txMoney.Focus();
                return;
            }
            try
            {
                if (txMoney.Text != string.Empty)
                {
                    txMoney.Text = string.Format("{0:N0}", double.Parse(txMoney.Text.Replace(",", "")));
                    txMoney.Select(txMoney.Text.Length, 0);
                }
            }
            catch
            {
                MessageBox.Show("ورودی نامعتبر است");
                txMoney.Clear();
                txMoney.Focus();
                return;
            }
        }

        private void txCardNum1_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txCardNum1.Text.Length == 4)
                txCardNum2.Focus();
        }

        private void txCardNum2_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txCardNum2.Text.Length == 4)
                txCardNum3.Focus();
        }

        private void txCardNum3_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txCardNum3.Text.Length == 4)
                txCardNum4.Focus();
        }

        private void txCardNum4_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txCardNum4.Text.Length == 4)
                txPass.Focus();
        }

        private void txMonth_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txMonth.Text.Length == 2)
                txYear.Focus();
        }

        private void txYear_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txYear.Text.Length == 4)
                txMoney.Focus();
        }
    }
}