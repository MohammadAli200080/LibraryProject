﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library_Project.Resources.Classes
{
    /// <summary>
    /// an enum for using in different windows
    /// Employee -> For registring employee
    /// Member -> For Member Registration
    /// MemberFromMemberWindow -> From Member dashboard for paying money
    /// Manager -> From manager window
    /// </summary>
    public enum typeOfUser
    {
        Employee, Member, MemberFromMemberWindow, Manager
    }
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
            if (EmpInfo == null || EmpInfo.Count < 5) return false;
            if (DatabaseControl.Exe("INSERT INTO T_Employees (username,password,email,phoneNumber,imgSrc,pocket) " +
                "VALUES ('" + EmpInfo[0] + "','" + EmpInfo[1] + "','" + EmpInfo[3] + "','" + EmpInfo[2] + "','" + EmpInfo[4] + "','" + 0 + "')"))
                return true;
            return false;
        }
        //this function for remove employee in library
        public static bool RemoveEmployee(string UserName)
        {
            var data = DatabaseControl.Select("SELECT (username) FROM T_Employees WHERE username ='" + UserName + "' ");
            if (data.Rows.Count == 0)
                return false;

            if (DatabaseControl.Exe("DELETE FROM T_Employees WHERE username ='" + UserName + "'"))
            {
                if (DatabaseControl.Select("SELECT * FROM T_Messages WHERE senderUsername ='" + UserName + "' OR recieverUsername = '" + UserName + "'").Rows.Count == 0)
                    return true;

                if (DatabaseControl.Exe("DELETE FROM T_Messages WHERE senderUsername ='" + UserName + "' OR recieverUsername = '" + UserName + "'"))
                    return true;
            }

            return false;
        }
        //this function for Increase inventory
        public static void PayManager(decimal PayMent)
        {
            Properties.Settings.Default.Bank += PayMent;
            Properties.Settings.Default.Save();
        }

        /// <summary>
        /// a method for calculating salary of employees
        /// </summary>
        /// <param name="payment">amount of money payed to employess</param>
        /// <returns>whole salary of all employees</returns>
        public static decimal CalculatePayment(decimal payment)
        {
            var data = DatabaseControl.Select("SELECT * FROM T_Employees ");
            int count = data.Rows.Count;
            decimal salary = count * payment;
            return salary;
        }
        /// <summary>
        /// a method to check whether manager is able to pay all employees or not
        /// </summary>
        /// <param name="payment"> amount of payment of an employee</param>
        /// <returns>a boolean. if able to pay, returns true else returns fasle</returns>
        public static bool AbleToPay(decimal payment)
        {
            if (Properties.Settings.Default.Bank < CalculatePayment(payment)) return false;
            return true;
        }
        /// <summary>
        /// a method for increasing packet of all employees
        /// </summary>
        /// <param name="Payment">amount of payment of an employee</param>
        /// <returns>a boolean. if able to pay, returns true else returns fasle</returns>
        public static bool PayEmployees(decimal Payment)
        {
            var data = DatabaseControl.Select("SELECT * FROM T_Employees");
            for (int i = 0; i < data.Rows.Count; i++)
            {
                decimal pay = Convert.ToDecimal(data.Rows[i]["pocket"].ToString()) + Payment;
                if (DatabaseControl.Exe("UPDATE T_Employees SET pocket='" + pay + "' WHERE username = '" + data.Rows[i]["username"] + "'"))
                {
                    Properties.Settings.Default.Bank -= Payment;
                    Properties.Settings.Default.Save();
                    if (i == data.Rows.Count - 1)
                        return true;
                }
            }
            return false;
        }
        //this function for show All Employees
        public static List<Employees> TakeAllEmployee()
        {
            List<Employees> employeesTmp = new List<Employees>();
            DataTable data = new DataTable();
            data = DatabaseControl.Select("SELECT * FROM T_Employees");

            string passWord, Username, email, phoneNumber, pocket;
            byte[] image;

            for (int i = 0; i < data.Rows.Count; i++)
            {
                passWord = data.Rows[i]["password"].ToString();
                Username = data.Rows[i]["username"].ToString();
                email = data.Rows[i]["email"].ToString();
                phoneNumber = data.Rows[i]["phoneNumber"].ToString();
                image = Convert.FromBase64String(data.Rows[i]["imgSrc"].ToString());
                pocket = data.Rows[i]["pocket"].ToString();

                employeesTmp.Add(new Employees
                {
                    Row = i+1,
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

        public static List<Employees> SearchAllEmployees(string username)
        {
            List<Employees> EmployeesTmp = new List<Employees>();
            var data = DatabaseControl.Select("SELECT * FROM T_Employees WHERE username='" + username + "'");

            string passWord, Username, email, phoneNumber, pocket;
            byte[] image;

            for (int i = 0; i < data.Rows.Count; i++)
            {
                passWord = data.Rows[i]["password"].ToString();
                Username = data.Rows[i]["username"].ToString();
                email = data.Rows[i]["email"].ToString();
                phoneNumber = data.Rows[i]["phoneNumber"].ToString();
                image = Convert.FromBase64String(data.Rows[i]["imgSrc"].ToString());
                pocket = data.Rows[i]["pocket"].ToString();

                EmployeesTmp.Add(new Employees
                {
                    Row=i+1,
                    UserName = Username,
                    PassWord = passWord,
                    Email = email,
                    PhoneNumber = phoneNumber,
                    Image = image,
                    Pocket = decimal.Parse(pocket)
                });
            }
            return EmployeesTmp;
        }
    }
    public class Employees : Users, IMessanger
    {
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public byte[] Image { get; set; }
        public decimal Pocket { get; set; }
        public int Row { get; set; }

        public static bool date(string d1, string d2)
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

            string passWord, Username, email, phoneNumber, pocket;
            string register, subsriptionDate, subsriptionDateRenewal;
            byte[] image;
            for (int i = 0; i < data.Rows.Count; i++)
            {
                passWord = data.Rows[i]["password"].ToString();
                Username = data.Rows[i]["username"].ToString();
                email = data.Rows[i]["email"].ToString();
                phoneNumber = data.Rows[i]["phoneNumber"].ToString();
                image = Convert.FromBase64String(data.Rows[i]["imgSrc"].ToString());
                pocket = data.Rows[i]["pocket"].ToString();
                register = data.Rows[i]["registeryDate"].ToString();
                subsriptionDate = data.Rows[i]["subscriptionEndingDate"].ToString();
                subsriptionDateRenewal = data.Rows[i]["renewaldate"].ToString();

                MembersTmp.Add(new Member
                {
                    Row = i+1,
                    UserName = Username,
                    PassWord = passWord,
                    Email = email,
                    PhoneNumber = phoneNumber,
                    Image = image,
                    RegisteryDay = register,
                    SubsriptionDate = subsriptionDate,
                    SubsriptionDateRenewal = subsriptionDateRenewal,
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
            data = DatabaseControl.Select("SELECT * FROM T_Members WHERE username='" + MemberName + "' COLLATE SQL_Latin1_General_CP1_CS_AS");

            string passWord, Username, email, phoneNumber, pocket;
            string register, subsriptionDate, subsriptionDateRenewal;
            byte[] image;

            for (int i = 0; i < data.Rows.Count; i++)
            {
                passWord = data.Rows[i]["password"].ToString();
                Username = data.Rows[i]["username"].ToString();
                email = data.Rows[i]["email"].ToString();
                phoneNumber = data.Rows[i]["phoneNumber"].ToString();
                image = Convert.FromBase64String(data.Rows[i]["imgSrc"].ToString());
                pocket = data.Rows[i]["pocket"].ToString();
                register = data.Rows[i]["registeryDate"].ToString();
                subsriptionDate = data.Rows[i]["subscriptionEndingDate"].ToString();
                subsriptionDateRenewal = data.Rows[i]["renewaldate"].ToString();

                MembersTmp.Add(new Member
                {
                    Row = i+1,
                    UserName = Username,
                    PassWord = passWord,
                    Email = email,
                    PhoneNumber = phoneNumber,
                    Image = image,
                    RegisteryDay = register,
                    SubsriptionDate = subsriptionDate,
                    SubsriptionDateRenewal = subsriptionDateRenewal,
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

            string passWord, Username, email, phoneNumber, pocket;
            string register, subsriptionDate;
            byte[] image;
            for (int i = 0; i < data.Rows.Count; i++)
            {
                if (date(DateTime.Now.ToShortDateString().ToString(), data.Rows[i].ItemArray[8].ToString()))
                {
                    passWord = data.Rows[i]["password"].ToString();
                    Username = data.Rows[i]["username"].ToString();
                    email = data.Rows[i]["email"].ToString();
                    phoneNumber = data.Rows[i]["phoneNumber"].ToString();
                    image = Convert.FromBase64String(data.Rows[i]["imgSrc"].ToString());
                    register = data.Rows[i]["registeryDate"].ToString();
                    subsriptionDate = data.Rows[i]["subscriptionEndingDate"].ToString();
                    pocket = data.Rows[i]["pocket"].ToString();

                    MembersTmp.Add(new Member
                    {
                        Row = MembersTmp.Count+ 1,
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
                "JOIN T_Borrowed ON T_Borrowed.username = T_Members.username AND T_Members.username='" + MemberName + "'");

            string passWord, Username, email, phoneNumber, pocket;
            string register, subsriptionDate;
            byte[] image;

            for (int i = 0; i < data.Rows.Count; i++)
            {
                passWord = data.Rows[i]["password"].ToString();
                Username = data.Rows[i]["username"].ToString();
                email = data.Rows[i]["email"].ToString();
                phoneNumber = data.Rows[i]["phoneNumber"].ToString();
                image = Convert.FromBase64String(data.Rows[i]["imgSrc"].ToString());
                pocket = data.Rows[i]["pocket"].ToString();
                register = data.Rows[i]["registeryDate"].ToString();
                subsriptionDate = data.Rows[i]["subscriptionEndingDate"].ToString();

                MembersTmp.Add(new Member
                {
                    Row = i+1,
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
        public static List<Member> TakeDelayedMembersInPayment()
        {
            List<Member> MembersTmp = new List<Member>();
            DataTable data = new DataTable();
            data = DatabaseControl.Select("SELECT * FROM T_Members");

            string passWord, Username, email, phoneNumber, pocket;
            string register, subsriptionDate;
            byte[] image;
            for (int i = 0; i < data.Rows.Count; i++)
            {
                if (date(DateTime.Now.ToShortDateString().ToString(), data.Rows[i].ItemArray[7].ToString()))
                {
                    passWord = data.Rows[i]["password"].ToString();
                    Username = data.Rows[i]["username"].ToString();
                    email = data.Rows[i]["email"].ToString();
                    phoneNumber = data.Rows[i]["phoneNumber"].ToString();
                    image = Convert.FromBase64String(data.Rows[i]["imgSrc"].ToString());
                    register = data.Rows[i]["registeryDate"].ToString();
                    subsriptionDate = data.Rows[i]["subscriptionEndingDate"].ToString();
                    pocket = data.Rows[i]["pocket"].ToString();

                    MembersTmp.Add(new Member
                    {
                        Row = MembersTmp.Count+1,
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

            string passWord, Username, email, phoneNumber, pocket;
            string register, subsriptionDate;
            byte[] image;

            for (int i = 0; i < data.Rows.Count; i++)
            {
                passWord = data.Rows[i]["password"].ToString();
                Username = data.Rows[i]["username"].ToString();
                email = data.Rows[i]["email"].ToString();
                phoneNumber = data.Rows[i]["phoneNumber"].ToString();
                image = Convert.FromBase64String(data.Rows[i]["imgSrc"].ToString());
                pocket = data.Rows[i]["pocket"].ToString();
                register = data.Rows[i]["registeryDate"].ToString();
                subsriptionDate = data.Rows[i]["subscriptionEndingDate"].ToString();

                MembersTmp.Add(new Member
                {
                    Row = i+1,
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
        public static bool ChangeInfo(List<string> ChangedInfo, string UserName)
        {
            if (ChangedInfo.Count == 0) return false;

            else if (DatabaseControl.Exe("UPDATE T_Employees SET password='" + ChangedInfo[0] + "',email='" + ChangedInfo[1] + "'," +
                "phoneNumber='" + ChangedInfo[2] + "',imgSrc='" + ChangedInfo[3] + "' WHERE username='" + UserName + "'"))
                return true;
            return false;
        }
        //for remove member from Library
        public static bool RemoveMember(string UserName)
        {
            var data = DatabaseControl.Select("SELECT (username) FROM T_Members WHERE username ='" + UserName + "' ");
            if (data.Rows.Count == 0)
                return false;
            if (DatabaseControl.Exe("DELETE FROM T_Members WHERE username='" + UserName + "'"))
            {
                if (DatabaseControl.Select("SELECT * FROM T_Messages WHERE senderUsername ='" + UserName + "' OR recieverUsername = '" + UserName + "'").Rows.Count == 0)
                    return true;

                if (DatabaseControl.Exe("DELETE FROM T_Messages WHERE senderUsername ='" + UserName + "' OR recieverUsername = '" + UserName + "'"))
                    return true;
            }

            return false;
        }

        public static decimal GetMoneyOfEmployee(string username)
        {
            string command = "SELECT * FROM T_Employees WHERE username='" + username.Trim() + "'";
            var data = DatabaseControl.Select(command);
            return Convert.ToDecimal(data.Rows[0]["pocket"].ToString());
        }
    }
    public class Member : Users, IMessanger
    {
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public byte[] Image { get; set; }
        public string RegisteryDay { get; set; }
        public string SubsriptionDate { get; set; }
        public string SubsriptionDateRenewal { get; set; }
        public decimal Balance { get; set; }
        public int Row { get; set; }

        public static bool date2(string d1, string d2)
        {
            DateTime a = DateTime.Parse(d2);
            DateTime b = DateTime.Parse(d1);

            TimeSpan result = b - a;
            if (int.Parse(result.TotalDays.ToString()) > 0) return true;
            return false;
        }
        public static bool date(string date1, string date2)
        {
            DateTime a = DateTime.Parse(date2);
            DateTime b = DateTime.Parse(date1);

            TimeSpan result = a - b;
            if (int.Parse(result.TotalDays.ToString()) >= 7) return true;
            return false;
        }
        public static bool CheckAbleToGetBook(string UserName)
        {
            var data = DatabaseControl.Select("SELECT COUNT (*) as count from T_Borrowed where username='" + UserName + "'");
            if (int.Parse(data.Rows[0]["count"].ToString()) > 5) return false;

            data = DatabaseControl.Select("SELECT subscriptionEndingDate FROM T_Members WHERE username='" + UserName + "'");
            if (!date(DateTime.Now.ToShortDateString().ToString(), data.Rows[0]["subscriptionEndingDate"].ToString()))
                return false;

            data = DatabaseControl.Select("SELECT returnDate FROM T_Borrowed WHERE username='" + UserName + "'");
            if (data.Rows.Count != 0)
                if (date2(DateTime.Now.ToShortDateString().ToString(), data.Rows[0]["returnDate"].ToString()))
                    return false;

            return true;
        }
        public static bool AbleToReturnBook(string BookName, out decimal money)
        {
            money = 0;
            decimal cost = 10000;
            var returnDate = DatabaseControl.Select("SELECT returnDate FROM T_Borrowed WHERE bookName='" + BookName + "'").Rows[0]["returnDate"].ToString();
            if (date2(DateTime.Now.ToShortDateString().ToString(), returnDate))
            {
                var username = DatabaseControl.Select("SELECT username FROM T_Borrowed WHERE bookName = '" + BookName + "'").Rows[0]["username"].ToString();
                var pocket = DatabaseControl.Select("SELECT pocket FROM T_Members WHERE username='" + username + "'").Rows[0]["pocket"].ToString();

                DateTime a = DateTime.Parse(DateTime.Now.ToShortDateString().ToString());
                DateTime b = DateTime.Parse(returnDate);

                TimeSpan result = a - b;
                money = Convert.ToInt32(result.Days) * cost - decimal.Parse(pocket);
                if (money > 0) return false;
                else
                {
                    if (Member.UpdateMoneyOfMember(username, Convert.ToInt32(result.Days) * (-1) * cost)) return true;
                }
            }

            return true;
        }
        public static bool CheckExistBook(string Name)
        {
            DataTable data = new DataTable();
            data = DatabaseControl.Select("SELECT quantity FROM T_Books WHERE bookName = '" + Name + "'");
            if (data.Rows.Count == 0) return false;
            if (data.Rows[0]["quantity"].ToString() == "0") return false;

            return true;
        }
        static DataTable Data = new DataTable();
        public static bool GetBook(string BookName, string Username)
        {
            DateTime now = DateTime.Now;
            now = new DateTime(now.Year, now.Month, now.Day, 00, 00, 00);
            if (!CheckExistBook(BookName)) return false;
            if (!CheckAbleToGetBook(Username)) return false;
            Data = DatabaseControl.Select("SELECT * FROM T_Books WHERE LOWER(bookName)='" + BookName.ToLower() + "'");
            var quantity = Convert.ToInt32(Data.Rows[0]["quantity"].ToString()) - 1;
            BookName = Data.Rows[0]["bookName"].ToString();
            if (!DatabaseControl.Exe("INSERT INTO T_Borrowed (bookName,username,gotDate,returnDate) VALUES ('" + BookName.Trim() + "','" + Username.Trim() + "','" + now.ToString("yyyy-MM-dd HH:mm:ss") + "','" + now.AddDays(14).ToString("yyyy-MM-dd HH:mm:ss") + "')"))
                return false;
            if (!DatabaseControl.Exe("UPDATE T_Books SET quantity = '" + quantity + "' WHERE bookName = '" + BookName.Trim() + "'")) return false;

            return true;
        }
        public static bool ReturnBook(string BookName, string UserName)
        {
            Data = DatabaseControl.Select("SELECT * FROM T_Books WHERE bookName='" + BookName + "'");
            var quantity = Convert.ToInt32(Data.Rows[0]["quantity"].ToString()) + 1;
            if (!DatabaseControl.Exe("UPDATE T_Books SET quantity = '" + quantity + "' WHERE bookName = '" + BookName.Trim() + "'")) return false;
            if (!DatabaseControl.Exe("DELETE FROM T_Borrowed WHERE bookName='" + BookName + "'")) return false;

            return true;
        }
        public static bool Addmember(List<string> Information)
        {
            try
            {
                DateTime now = DateTime.Now;
                now = new DateTime(now.Year, now.Month, now.Day, 00, 00, 00);
                if (DatabaseControl.Exe("INSERT INTO T_Members (username,password,email,phoneNumber,imgSrc,pocket,registeryDate,subscriptionEndingDate,renewaldate) " +
                "VALUES ('" + Information[0].Trim() + "','" + Information[1].Trim() + "','" + Information[3].Trim() + "','" + Information[2].Trim() + "','" +
                Information[4].Trim() + "','" + 0 + "','" + now.ToString("yyyy-MM-dd HH:mm:ss") + "','" + now.AddDays(14).ToString("yyyy-MM-dd HH:mm:ss") + "','" + now.ToString("yyyy-MM-dd HH:mm:ss") + "')"))
                    return true;
            }
            catch
            {

            }
            return false;
        }
        public static decimal GetMemberMoney(string username)
        {
            string command = "SELECT * FROM T_Members WHERE username='" + username.Trim() + "'";
            var data = DatabaseControl.Select(command);
            return Convert.ToDecimal(data.Rows[0]["pocket"].ToString());
        }

        /// <summary>
        /// adds money to a member in T_Members after paying to system
        /// </summary>
        /// <param name="username">username of member you want to add money to</param>
        /// <param name="money">amount of money that's gonna be added to the pocket of member</param>
        /// <returns>if operation is successful return true else returns false</returns>

        public static bool UpdateMoneyOfMember(string username, decimal money)
        {
            string command = "SELECT * FROM T_Members WHERE username='" + username + "'";
            var data = DatabaseControl.Select(command);

            if (data.Rows.Count == 0)
                return false;

            var pocket = Convert.ToDecimal(data.Rows[0]["pocket"].ToString()) + money;

            command = "UPDATE T_Members SET pocket='" + pocket + "' WHERE username='" + username + "'";

            return DatabaseControl.Exe(command);
        }

        public static bool UpdateSubscriptionOfmember(string username, string duration)
        {
            string command = "SELECT * FROM T_Members WHERE username='" + username + "'";
            var data = DatabaseControl.Select(command);

            if (data.Rows.Count == 0)
                return false;

            var date = Convert.ToDateTime(data.Rows[0]["subscriptionEndingDate"]);

            var kinds = KindOfSubscription.Instance.GetSubsriptions();
            var newDate = new DateTime();
            for (int i = 0; i < kinds.Length; i++)
            {
                if (kinds[i].Name == duration)
                {
                    double days = kinds[i].Duration;
                    newDate = date.AddDays(days);
                    break;
                }
            }

            DateTime now = DateTime.Now;
            now = new DateTime(now.Year, now.Month, now.Day, 00, 00, 00);
            command = "UPDATE T_Members SET subscriptionEndingDate='" + newDate.ToString("yyyy-MM-dd HH:mm:ss") + "',renewaldate='" + now.ToString("yyyy-MM-dd HH:mm:ss") + "' WHERE username='" + username + "'";
            return DatabaseControl.Exe(command);
        }
    }
}