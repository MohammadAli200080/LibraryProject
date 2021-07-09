using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private ObservableCollection<string> _allRecieverNames;
        public ObservableCollection<string> AllRecieverNames
        {
            get => _allRecieverNames;
            set { _allRecieverNames = value; }
        }
        private ObservableCollection<string> _allSendersName;
        public ObservableCollection<string> AllSendersName
        {
            get => _allSendersName;
            set { _allSendersName = value; }
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

            AllSendersName = new ObservableCollection<string>();
            if (Type == MassengeType.employee) AllSendersName = Message.Instance.AllSenders(Username, MassengeType.employee);
            else AllSendersName = Message.Instance.AllSenders(Username, MassengeType.member);

            AllRecieverNames = new ObservableCollection<string>();

            if (Type == MassengeType.employee)
            {
                DataTable data = new DataTable();
                data = DatabaseControl.Select("SELECT username FROM T_Members");
                for (int i = 0; i < data.Rows.Count; i++)
                {
                    AllRecieverNames.Add(data.Rows[i]["username"].ToString());
                }
            }

            else
            {
                DataTable data = new DataTable();
                data = DatabaseControl.Select("SELECT DISTINCT senderUsername FROM T_Messages WHERE recieverUsername='" + Username + "'");
                for(int i = 0; i < data.Rows.Count; i++)
                {
                    AllRecieverNames.Add(data.Rows[i]["senderUsername"].ToString());
                }
            }
            
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
            AllSendersNameComboBox.ItemsSource = AllSendersName;
            SendMessage.Visibility = Visibility.Collapsed;
            ReciveMessage.Visibility = Visibility.Visible;
        }

        private void SendingPn_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            AllRecieversComboBox.ItemsSource = AllRecieverNames;
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
            allMessagesData.ItemsSource = AllMessages;
        }

    }
}
