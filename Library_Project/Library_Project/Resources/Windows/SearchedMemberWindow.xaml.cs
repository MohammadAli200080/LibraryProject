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
using Library_Project.Resources.Classes;
using Library_Project.Resourses.Windows;

namespace Library_Project.Resources.Windows
{
    /// <summary>
    /// Interaction logic for SearchedMemberWindow.xaml
    /// </summary>
    public partial class SearchedMemberWindow : Window
    {
        public SearchedMemberWindow(string username)
        {
            var member = Employees.SearchAllMember(username)[0];

            InitializeComponent();

            image.Source = ImageControl.ByteToImage(member.Image);
            txtUsername.Text = member.UserName;
            txtEmail.Text = member.Email;
            txtPhoneNumber.Text = member.PhoneNumber;
        }

        private void Return_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
