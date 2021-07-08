using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
using Library_Project.Resourses.Windows;

namespace Library_Project.Resources.Windows
{
    /// <summary>
    /// Interaction logic for MassagerEmployee.xaml
    /// </summary>

    public partial class MassagerEmployee : Window, INotifyPropertyChanged
    {
        private MassengeType Type { get; set; }
        private string Username { get; set; }
        public string Receiver { get; set; }
        private List<string> _allRecieverNames;
        public List<string> AllRecieverNames
        {
            get => _allRecieverNames;
            set { _allRecieverNames = value; NotifyPropertyChanged("AllRecieverNames"); }
        }
        private List<string> _allSendersName;
        public List<string> AllSendersName
        {
            get => _allSendersName;
            set { _allSendersName = value; NotifyPropertyChanged("AllSendersName"); }
        }
        private List<Message> _allMessages;
        public List<Message> AllMessages
        {
            get => _allMessages;
            set { _allMessages = value; NotifyPropertyChanged("AllMessages"); }
        }

        public MassagerEmployee(MassengeType type, string username)
        {
            Type = type;
            Username = username;

            AllSendersName = new List<string>();
            if (Type == MassengeType.employee) AllSendersName = Message.Instance.AllSenders(Username, MassengeType.member).ToList();
            else AllSendersName = Message.Instance.AllSenders(Username, MassengeType.employee).ToList();

            AllRecieverNames = new List<string>();
            if (Type == MassengeType.employee) AllRecieverNames = Employees.TakeAllMember().Select(x => x.UserName).ToList();
            else AllRecieverNames = Managers.TakeAllEmployee().Select(x => x.UserName).ToList();

            InitializeComponent();
            DataContext = this;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnMin_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void RecivePn_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            SendMessage.Visibility = Visibility.Collapsed;
            ReciveMessage.Visibility = Visibility.Visible;
        }

        private void SendingPn_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            SendMessage.Visibility = Visibility.Visible;
            ReciveMessage.Visibility = Visibility.Collapsed;
        }

        private void BackPn_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (Type == MassengeType.employee)
            {
                var ew = new EmployeeDashboard(Username);
                ew.Show();
                this.Close();
            }
            else
            {
                var mw = new MemberDashboard(Username);
                mw.Show();
                this.Close();
            }
        }
        private void AllRecieversComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AllRecieversComboBox.SelectedIndex < 0)
                return;

            Receiver = AllRecieversComboBox.SelectedItem.ToString();

            if (Type == MassengeType.employee)
            {
                var SelectedMember = Employees.SearchAllMember(Receiver)[0];
                image.Source = ImageControl.ByteToImage(SelectedMember.Image);
                txtEmail.Text = SelectedMember.Email;
                txtPhoneNumber.Text = SelectedMember.PhoneNumber;
                txtUsername.Text = SelectedMember.UserName;
            }
            else
            {
                var SelectedEmployee = Managers.SearchAllEmployees(Receiver)[0];
                image.Source = ImageControl.ByteToImage(SelectedEmployee.Image);
                txtEmail.Text = SelectedEmployee.Email;
                txtPhoneNumber.Text = SelectedEmployee.PhoneNumber;
                txtUsername.Text = SelectedEmployee.UserName;
            }
        }
        private void Sendbtn_Click(object sender, RoutedEventArgs e)
        {
            if (AllRecieversComboBox.SelectedIndex < 0)
            {
                MessageBox.Show(".ابتدا آیتم مورد نظر را انتخاب کنید");
                return;
            }
            Receiver = AllRecieversComboBox.SelectedItem.ToString();
            TextRange text = new TextRange(MessageTxt.Document.ContentStart, MessageTxt.Document.ContentEnd);
            if (text.Text == "\r\n")
            {
                MessageBox.Show("ابتدا باکس را پرکنید");
                return;
            }

            Message message = new Message(Username, Receiver, text.Text, Type);
            if (message.SendMessage())
            {
                MessageBox.Show("پیام با موفقیت ارسال شد");
                text.Text = "";
            }


        }

        private void AllSendersNameComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AllSendersNameComboBox.SelectedIndex < 0)
                return;

            string senderUsername = AllSendersNameComboBox.SelectedItem.ToString();

            AllMessages = Message.Instance.RecieveAllMessages(Username, senderUsername).ToList();
        }

    }
}
