using System;
using System.Collections.Generic;
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
            if (EmpInfo == null)
                return false;
            //recive the information Employee and add the table related to the Employee in SQL
            return true;
        }
        //this function for remove employee in library
        public static bool RemoveEmployee(List<string> EmpInfo)
        {
            if (EmpInfo == null)
                return false;
            //recive the information Employee and remove the Person from table related to the Employee in SQL
            return true;
        }
        //this function for Increase inventory
        public static decimal CalculatePayment(decimal PayMent)
        {
            //add the PayMent to data base
            return 0;
        }
        //this function for Increase Employee inventory
        public static bool PayEmployee(decimal PayMent)
        {
            //if statment for checking the Balance of Library
            //if PayMent>Balance ===> return fals
            //if PayMent<=Balance ===> Pay the saleray of Employee
            return true;
        }
        //this function for show All Employees
        public static List<Employees> TakeAllEmployee()
        {
            //select from Table related to the Employee Information in SQLServer and return as a list of Object of Employee Class
            return new List<Employees>();
        }
    }
    public class Employees : Users
    {
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public byte[] Image { get; set; }
        public decimal Pocket { get; set; }
        //this function for show all Members
        public static List<Members> TakeAllMember()
        {
            //select from Table related to the Member Information in SQLServer and return as a list of Object of Member Class
            return new List<Members>();
        }
        public static Members SearchNormalMember(string MemberName)
        {
            //select from Table related to the Member Information in SQLServer where name is equal to MemberName 
            //then return as a list of Object of Member Class
            return new Members();
        }
        public static List<Members> TakeDelayedMemebrsInReturn()
        {
            //select from Table related to the Member Information in SQLServer where the Members Delayed in Return,then return 
            //as a list of Object of Member Class
            return new List<Members>();
        }
        public static Members SearchDelayInReturn(string MemberName)
        {
            //select from Table related to the Member Information in SQLServer where name is equal to MemberName 
            //then return as a list of Object of Member Class
            return new Members();
        }
        public static List<Members> TakeDelayedMemebrsInPayment()
        {
            //select from Table related to the Member Information in SQLServer where the Members Delayed in Payment.then return 
            //as a list of Object of Member Class
            return new List<Members>();
        }
        public static Members SearchDelayInPayment(string MemberName)
        {
            //select from Table related to the Member Information in SQLServer where name is equal to MemberName 
            //then return as a list of Object of Member Class
            return new Members();
        }
        public static bool ChangeInfo(List<string> ChangedInfo)
        {
            //if ChangedInfo is null ====> return false
            //if entered PassWord in ChangedInfo List is not equal to Password in SQL server ===> return false
            //select from Table related to the Employee Information in SQLServer where changedInfo is equal to EmployeeWith this Information 
            //then change Of Information
            return true;
        }
    }
    public class Members : Users
    {
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public byte[] Image { get; set; }
        public DateTime RegisteryDay { get; set; }
        public DateTime SubsriptionDate { get; set; }
        public decimal Balance { get; set; }

        public static bool CheckAbleToGetBook()
        {
            //check the condition of Borrow:
            //if Count of Borrowed books > 5 ===> return false
            //if SubsriptionDate is < 7 days ===> return false
            //if select Borrowed books from Information Table where deleyed book return not null ====> return false
            return true;
        }
        public static bool HasDelay()
        {
            //calculate the duration borrow
            return true;
        }
        public static bool AbleToReturnBook()
        {
            //if (!HasDelay())
            //{
            //    if Balance < forfeit ===> return false(Show Message that Increase Balance)
            //    if Balance >= forfeit ===> Balance-=forfeit then return true 
            //}

            //if function return true ===> Borrowed books must return to Table related to Information book
            return true;
        }
        public static void GetBook(string BookName)
        {
            //if user can Borrow book (CheckAbleToGetBook() returns true) ===> BookName add the Borrowed List
            // if can not Borrow Book Message show that You can not borrow bbok
        }
        public static void ReturnBook(string BookName)
        {
            //if function AbleToReturnBook() returns true && HasDelay() return false ===> BookName returns to Book list on SQL
            //if function AbleToReturnBook() returns true && HasDelay() return true ===> Message show that forfeit recived and then 
            //BookName returns to Book list on SQL
        }
    }
}