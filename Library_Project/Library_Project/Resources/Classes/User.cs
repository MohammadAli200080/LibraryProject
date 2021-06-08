using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library_Project.Resources.Classes
{
    //the Main classes for User in project
    public class Users
    {
        public string UserName { get; set; }
        public string PassWord { get; set; }
    }
    public class Managers : Users
    {
        private decimal _balance;
        public decimal Balance
        {
            get { return _balance; }
            set { _balance = value; }
        }

        //this function for add new employee in library
        public static bool AddEmployee(List<string> EmpInfo)
        {
            if (EmpInfo == null || EmpInfo.Count < 6) return false;
            if (DatabaseControl.Exe("INSERT INTO T_Employees (username,password,email,phoneNumber,imgSrc,pocket) " +
                "VALUES ('" + EmpInfo[0] + "','" + EmpInfo[1] + "','" + EmpInfo[2] + "','" + EmpInfo[3] + "','" + EmpInfo[4] + "','" + EmpInfo[5] + "')"))
                return true;
            return false;
        }
        //this function for remove employee in library
        public static bool RemoveEmployee(string UserName)
        {
            if (DatabaseControl.Exe("DELETE FROM T_Employees WHERE username =" + UserName))
                return true;
            return false;
        }
        //this function for Increase inventory
        public static void CalculatePayment(decimal PayMent)
        {
            Properties.Settings.Default.Bank += PayMent;
            Properties.Settings.Default.Save();
        }
        //this function for Increase Employee inventory
        public static bool PayEmployee(decimal PayMent,string Username)
        {
            if (Properties.Settings.Default.Bank < PayMent) return false;
            if (DatabaseControl.Exe("UPDATE T_Employees SET pocket='" + PayMent + "' WHERE username='" + Username + "'"))
            {
                Properties.Settings.Default.Bank -= PayMent;
                Properties.Settings.Default.Save();
                return true;
            }
            return false;
        }
        //this function for show All Employees
        public static List<Employees> TakeAllEmployee()
        {
            List<Employees> employeesTmp = new List<Employees>();
            DataTable data = new DataTable();
            data = DatabaseControl.Select("SELECT * FROM T_Employees");

            string passWord, Username, email, phoneNumber, image, pocket;

            for(int i = 0; i < data.Rows.Count; i++)
            {
                passWord = data.Rows[i]["password"].ToString();
                Username = data.Rows[i]["username"].ToString();
                email = data.Rows[i]["email"].ToString();
                phoneNumber = data.Rows[i]["phoneNumber"].ToString();
                image = data.Rows[i]["imgSrc"].ToString();
                pocket = data.Rows[i]["pocket"].ToString();

                employeesTmp.Add(new Employees
                {
                    UserName = Username,
                    PassWord = passWord,
                    Email = email,
                    PhoneNumber = phoneNumber,
                    Image = image,
                    Pocket = decimal.Parse(pocket)
                });
            }
            return employeesTmp;
        }
    }
    public class Employees : Users
    {
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Image { get; set; }
        public decimal Pocket { get; set; }

        public static bool date(string d1,string d2)
        {
            DateTime a = DateTime.Parse(d2);
            DateTime b = DateTime.Parse(d1);

            TimeSpan result = b - a;
            if (int.Parse(result.TotalDays.ToString()) > 0) return true;
            return false;
        }
        //this function for show all Members
        public static List<Member> TakeAllMember()
        {
            List<Member> MembersTmp = new List<Member>();
            DataTable data = new DataTable();
            data = DatabaseControl.Select("SELECT * FROM T_Members");

            string passWord, Username, email, phoneNumber, image, pocket;
            string register, subsriptionDate;
            for (int i = 0; i < data.Rows.Count; i++)
            {
                passWord = data.Rows[i]["password"].ToString();
                Username = data.Rows[i]["username"].ToString();
                email = data.Rows[i]["email"].ToString();
                phoneNumber = data.Rows[i]["phoneNumber"].ToString();
                image = data.Rows[i]["imgSrc"].ToString();
                pocket = data.Rows[i]["pocket"].ToString();
                register = data.Rows[i]["registeryDate"].ToString();
                subsriptionDate = data.Rows[i]["subscriptionEndingDate"].ToString();

                MembersTmp.Add(new Member
                {
                    UserName = Username,
                    PassWord = passWord,
                    Email = email,
                    PhoneNumber = phoneNumber,
                    Image = image,
                    RegisteryDay=register,
                    SubsriptionDate=subsriptionDate,
                    Balance = decimal.Parse(pocket)
                });
            }
            return MembersTmp;
        }
        //this function for show information of selected Member in between All Members
        public static List<Member> SearchAllMember(string MemberName)
        {
            List<Member> MembersTmp = new List<Member>();
            DataTable data = new DataTable();
            data = DatabaseControl.Select("SELECT * FROM T_Members WHERE username='" + MemberName + "'");

            string passWord, Username, email, phoneNumber, image, pocket;
            string register, subsriptionDate;

            for(int i = 0; i < data.Rows.Count; i++)
            {
                passWord = data.Rows[0]["password"].ToString();
                Username = data.Rows[0]["username"].ToString();
                email = data.Rows[0]["email"].ToString();
                phoneNumber = data.Rows[0]["phoneNumber"].ToString();
                image = data.Rows[0]["imgSrc"].ToString();
                pocket = data.Rows[0]["pocket"].ToString();
                register = data.Rows[0]["registeryDate"].ToString();
                subsriptionDate = data.Rows[0]["subscriptionEndingDate"].ToString();

                MembersTmp.Add(new Member
                {
                    UserName = Username,
                    PassWord = passWord,
                    Email = email,
                    PhoneNumber = phoneNumber,
                    Image = image,
                    RegisteryDay = register,
                    SubsriptionDate = subsriptionDate,
                    Balance = decimal.Parse(pocket)
                });
            }           
            return MembersTmp;
        }
        public static List<Member> TakeDelayedMemebrsInReturn()
        {
            List<Member> MembersTmp = new List<Member>();
            DataTable data = new DataTable();
            data = DatabaseControl.Select("SELECT T_Members.username,T_Members.password,T_Members.email,T_Members.phoneNumber,T_Members.imgSrc,T_Members.pocket,T_Members.registeryDate,T_Members.subscriptionEndingDate,T_Borrowed.returnDate FROM T_Members INNER " +
                "JOIN T_Borrowed ON T_Borrowed.username = T_Members.username");

            string passWord, Username, email, phoneNumber, image, pocket;
            string register, subsriptionDate;
            for (int i = 0; i < data.Rows.Count; i++)
            {
                if (date(DateTime.Now.ToShortDateString().ToString(), data.Rows[i].ItemArray[8].ToString()))
                {
                    passWord = data.Rows[i]["password"].ToString();
                    Username = data.Rows[i]["username"].ToString();
                    email = data.Rows[i]["email"].ToString();
                    phoneNumber = data.Rows[i]["phoneNumber"].ToString();
                    image = data.Rows[i]["imgSrc"].ToString();
                    register = data.Rows[i]["registeryDate"].ToString();
                    subsriptionDate = data.Rows[i]["subscriptionEndingDate"].ToString();
                    pocket = data.Rows[i]["pocket"].ToString();

                    MembersTmp.Add(new Member
                    {
                        UserName = Username,
                        PassWord = passWord,
                        Email = email,
                        PhoneNumber = phoneNumber,
                        Image = image,
                        RegisteryDay = register,
                        SubsriptionDate = subsriptionDate,
                        Balance = decimal.Parse(pocket)
                    });
                }               
            }
            return MembersTmp;
        }
        //this function for show information of selected Member in between All DelayedMemebrsInReturn BOOKS
        public static List<Member> SearchDelayInReturn(string MemberName)
        {
            List<Member> MembersTmp = new List<Member>();
            DataTable data = new DataTable();
            data = DatabaseControl.Select("SELECT T_Members.username,T_Members.password,T_Members.email,T_Members.phoneNumber,T_Members.imgSrc,T_Members.pocket,T_Members.registeryDate,T_Members.subscriptionEndingDate,T_Borrowed.returnDate FROM T_Members INNER " +
                "JOIN T_Borrowed ON T_Borrowed.username = T_Members.username AND T_Members.username='"+MemberName+"'");

            string passWord, Username, email, phoneNumber, image, pocket;
            string register, subsriptionDate;

            for(int i = 0; i < data.Rows.Count; i++)
            {
                passWord = data.Rows[0]["password"].ToString();
                Username = data.Rows[0]["username"].ToString();
                email = data.Rows[0]["email"].ToString();
                phoneNumber = data.Rows[0]["phoneNumber"].ToString();
                image = data.Rows[0]["imgSrc"].ToString();
                pocket = data.Rows[0]["pocket"].ToString();
                register = data.Rows[0]["registeryDate"].ToString();
                subsriptionDate = data.Rows[0]["subscriptionEndingDate"].ToString();

                MembersTmp.Add(new Member
                {
                    UserName = Username,
                    PassWord = passWord,
                    Email = email,
                    PhoneNumber = phoneNumber,
                    Image = image,
                    RegisteryDay = register,
                    SubsriptionDate = subsriptionDate,
                    Balance = decimal.Parse(pocket)
                });
            }           
            return MembersTmp;
        }
        public static List<Member> TakeDelayedMemebrsInPayment()
        {
            List<Member> MembersTmp = new List<Member>();
            DataTable data = new DataTable();
            data = DatabaseControl.Select("SELECT * FROM T_Members");

            string passWord, Username, email, phoneNumber, image, pocket;
            string register, subsriptionDate;
            for (int i = 0; i < data.Rows.Count; i++)
            {
                if (date(DateTime.Now.ToShortDateString().ToString(), data.Rows[i].ItemArray[7].ToString()))
                {
                    passWord = data.Rows[i]["password"].ToString();
                    Username = data.Rows[i]["username"].ToString();
                    email = data.Rows[i]["email"].ToString();
                    phoneNumber = data.Rows[i]["phoneNumber"].ToString();
                    image = data.Rows[i]["imgSrc"].ToString();
                    register = data.Rows[i]["registeryDate"].ToString();
                    subsriptionDate = data.Rows[i]["subscriptionEndingDate"].ToString();
                    pocket = data.Rows[i]["pocket"].ToString();

                    MembersTmp.Add(new Member
                    {
                        UserName = Username,
                        PassWord = passWord,
                        Email = email,
                        PhoneNumber = phoneNumber,
                        Image = image,
                        RegisteryDay = register,
                        SubsriptionDate = subsriptionDate,
                        Balance = decimal.Parse(pocket)
                    });
                }
            }
            return MembersTmp;
        }
        public static List<Member> SearchDelayInPayment(string MemberName)
        {
            List<Member> MembersTmp = new List<Member>();
            DataTable data = new DataTable();
            data = DatabaseControl.Select("SELECT * FROM T_Members WHERE username='" + MemberName + "'");

            string passWord, Username, email, phoneNumber, image, pocket;
            string register, subsriptionDate;

            for(int i=0;i<data.Rows.Count; i++)
            {
                passWord = data.Rows[0]["password"].ToString();
                Username = data.Rows[0]["username"].ToString();
                email = data.Rows[0]["email"].ToString();
                phoneNumber = data.Rows[0]["phoneNumber"].ToString();
                image = data.Rows[0]["imgSrc"].ToString();
                pocket = data.Rows[0]["pocket"].ToString();
                register = data.Rows[0]["registeryDate"].ToString();
                subsriptionDate = data.Rows[0]["subscriptionEndingDate"].ToString();

                MembersTmp.Add(new Member
                {
                    UserName = Username,
                    PassWord = passWord,
                    Email = email,
                    PhoneNumber = phoneNumber,
                    Image = image,
                    RegisteryDay = register,
                    SubsriptionDate = subsriptionDate,
                    Balance = decimal.Parse(pocket)
                });
            }
            return MembersTmp;
        }
        public static bool ChangeInfo(List<string> ChangedInfo,string UserName)
        {
            if (ChangedInfo.Count == 0)  return false;

            else if (DatabaseControl.Exe("UPDATE T_Employees SET password='" + ChangedInfo[0] + "',email='" + ChangedInfo[1] + "'," +
                "phoneNumber='" + ChangedInfo[2] + "',imgSrc='" + ChangedInfo[3] + "' WHERE username='" + UserName + "'"))
                return true;
            return false;
        }
        //for remove member from Library
        public static bool RemoveMember(string UserName)
        {
            if (DatabaseControl.Exe("DELETE FROM T_Members WHERE username = " + UserName))
                return true;
            return false;
        }
    }
    public class Member : Users
    {
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Image { get; set; }
        public string RegisteryDay { get; set; }
        public string SubsriptionDate { get; set; }
        public decimal Balance { get; set; }
  
        public static bool date2(string d1, string d2)
        {
            DateTime a = DateTime.Parse(d2);
            DateTime b = DateTime.Parse(d1);

            TimeSpan result = b - a;
            if (int.Parse(result.TotalDays.ToString()) > 0) return true;
            return false;
        }
        public static bool date(string date1,string date2)
        {
            DateTime a = DateTime.Parse(date2);
            DateTime b = DateTime.Parse(date1);

            TimeSpan result = a - b;
            if (int.Parse(result.TotalDays.ToString()) >= 7) return true;
            return false;
        }
        public static bool CheckAbleToGetBook(string UserName)
        {
            DataTable data = new DataTable();
            data = DatabaseControl.Select("SELECT COUNT (*) as count from T_Borrowed where username=" + UserName);
            if (int.Parse(data.Rows[0]["count"].ToString()) > 5) return false;

            data = new DataTable();
            data = DatabaseControl.Select("SELECT subscriptionEndingDate FROM T_Members WHERE username=" + UserName);
            if (!date(DateTime.Now.ToShortDateString().ToString(), data.Rows[0]["subscriptionEndingDate"].ToString()))
                return false;
            data = new DataTable();
            data = DatabaseControl.Select("SELECT returnDate FROM T_Borrowed WHERE username=" + UserName);
            if (date2(DateTime.Now.ToShortDateString().ToString(), data.Rows[0]["returnDate"].ToString()))
                return false;

            return true;
        }
        public static bool AbleToReturnBook(string BookName)
        {
            DataTable data = new DataTable();
            DataTable data2 = new DataTable();
            data = DatabaseControl.Select("SELECT returnDate FROM T_Borrowed WHERE bookName=" + BookName);
            if (date2(DateTime.Now.ToShortDateString().ToString(), data.Rows[0]["returnDate"].ToString()))
            {
                data = new DataTable();
                data = DatabaseControl.Select("SELECT username FROM T_Borrowed WHERE bookName = " + BookName);
                data2 = DatabaseControl.Select("SELECT pocket FROM T_Members WHERE username=" + data.Rows[0]["username"].ToString());

                if (decimal.Parse(data2.Rows[0]["pocket"].ToString()) < 10000) return false;
            }

            return true;
        }
        static DataTable Data = new DataTable();
        public static bool GetBook(string BookName,string UserName)
        {
            if (!CheckAbleToGetBook(UserName)) return false;
            Data = DatabaseControl.Select("SELECT * FROM T_Books WHERE bookName=" + BookName);
            if (!DatabaseControl.Exe("DELETE FROM T_Books WHERE bookName=" + BookName)) return false;
            if (!DatabaseControl.Exe("INSERT INTO T_Borrowed VALUES ('" + BookName.Trim() + "','" + UserName.Trim() + "','" + DateTime.Now.ToShortDateString() + "','" + DateTime.Now.AddDays(14).ToShortDateString() + "')"))
                return false;

            return true;
        }
        public static bool ReturnBook(string BookName, string UserName)
        {
            if (!AbleToReturnBook(BookName)) return false;
            if (!DatabaseControl.Exe("DELETE FROM T_Borrowed WHERE bookName=" + BookName)) return false;
            if (!DatabaseControl.Exe("INSERT INTO T_Books VALUES ('" + Data.Rows[0]["bookName"].ToString() + "','" + int.Parse(Data.Rows[0]["publishNumber"].ToString()) + "'," +
                "'" + int.Parse(Data.Rows[0]["quantity"].ToString()) + "','" + Data.Rows[0]["author"].ToString() + "','" + Data.Rows[0]["category"].ToString() + "')"))
                return false;

            return true;
        }
    }
}