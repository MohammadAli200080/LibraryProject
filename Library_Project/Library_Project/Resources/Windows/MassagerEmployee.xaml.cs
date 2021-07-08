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
    
    public class Members
    {
        public string UserName { get; set; }
        public byte[] Image { get; set; }
        public static List<Members> Users()
        {
            DataTable data = new DataTable();
            List<Members> members = new List<Members>();
            string userName;
            byte[] image;
            data = DatabaseControl.Select("SELECT username,imgSrc FROM T_Members");

            for (int i = 0; i < data.Rows.Count; i++)
            {
                userName = data.Rows[i]["username"].ToString();
                image = Convert.FromBase64String(data.Rows[i]["imgSrc"].ToString());

                members.Add(new Members { UserName = userName, Image = image });
            }
            return members;
        }
    }
    public partial class MassagerEmployee : Window, INotifyPropertyChanged
    {
        public string selected = "";
        private List<string> _name;
        public List<string> Names
        {
            get => _name;
            set { _name = value; NotifyPropertyChanged("MembersNames"); }
        }
        public MassagerEmployee()
        {
            List<Member> members2 = new List<Member>();
            members2 = Employees.TakeAllMember();
            Names = new List<string>();

            for(int i = 0; i < members2.Count; i++)
            {
                Names.Add(members2[i].UserName.ToString());
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
            SendMessage.Visibility = Visibility.Collapsed;
            ReciveMessage.Visibility = Visibility.Visible;
        }

        private void SendingPn_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            SendMessage.Visibility = Visibility.Visible;
            ReciveMessage.Visibility = Visibility.Collapsed;
        }
        private void MemberOrEmployeeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MemberOrEmployeeComboBox.SelectedIndex < 0)
                return;

            selected = MemberOrEmployeeComboBox.SelectedItem.ToString();
            var SelectedMember = Employees.SearchAllMember(selected)[0];
            image.Source = ImageControl.ByteToImage(SelectedMember.Image);
            txtEmail.Text = SelectedMember.Email;
            txtPhoneNumber.Text = SelectedMember.PhoneNumber;
            txtUsername.Text = SelectedMember.UserName;
        }
        private void Sendbtn_Click(object sender, RoutedEventArgs e)
        {
            if (MemberOrEmployeeComboBox.SelectedIndex < 0)
            {
                MessageBox.Show(".ابتدا آیتم مورد نظر را انتخاب کنید");
                return;
            }
            selected = MemberOrEmployeeComboBox.SelectedItem.ToString();
            TextRange text = new TextRange(MessageSend.Document.ContentStart, MessageSend.Document.ContentEnd);
            if (text.Text == "\r\n")
            {
                MessageBox.Show("ابتدا باکس را پرکنید");
                return;
            }

            MessageBox.Show("پیام با موفقیت ارسال شد");
            this.Close();


        }        
    }
}
