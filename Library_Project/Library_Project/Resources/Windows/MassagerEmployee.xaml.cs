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
        private List<Members> _Member;
        private List<string> _MemberName;
        public List<Members> members
        {
            get { return _Member; }
            set { _Member = value; NotifyPropertyChanged("members"); }
        }
        public List<string> MembersNames
        {
            get => _MemberName;
            set { _MemberName = value; NotifyPropertyChanged("MembersNames"); }
        }
        public MassagerEmployee()
        {
            members = new List<Members>();
            members = Members.Users();
            for(int i = 0; i < Employees.TakeAllMember().Count; i++)
            {
                MembersNames.Add(Employees.TakeAllMember()[i].UserName);
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

        private void KindOfSubsciptionComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MemberComboBox.SelectedIndex < 0)
                return;

           selected = MemberComboBox.SelectedItem.ToString();
        }
    }
}
