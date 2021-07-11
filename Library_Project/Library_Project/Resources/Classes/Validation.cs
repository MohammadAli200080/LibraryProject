using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace Library_Project.Resources.Classes
{
    public static class Validation
    {
        // <summary> Checks wether an email address exists or not </summary>

        public static bool EmailExists(string email)
        {
            SqlConnection connection = new SqlConnection("Data Source=.;Initial Catalog=db_Library;Integrated Security=True");
            try
            {
                DataTable table = DatabaseControl.TableFiller("select * from T_Employees where email = '" + email + "'", connection);

                if (table.Rows.Count == 1)
                    return true;

                table.Clear();
                table = DatabaseControl.TableFiller("select * from T_Members where email = '" + email + "'", connection);

                if (table.Rows.Count == 1)
                    return true;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                connection.Close();
            }

            return false;
        }
        // <summary> Checks wether a username address exists or not </summary>

        public static bool UserNameExists(string username)
        {
            SqlConnection connection = new SqlConnection("Data Source=.;Initial Catalog=db_Library;Integrated Security=True");

            try
            {
                DataTable table = DatabaseControl.TableFiller("select * from T_Employees where username = '" + username + "'", connection);

                if (table.Rows.Count == 1)
                    return true;

                table.Clear();
                table = DatabaseControl.TableFiller("select * from T_Members where username = '" + username + "'", connection);

                if (table.Rows.Count == 1)
                    return true;        
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                connection.Close();
            }

            return false;
        }
        // <summary> Checks wether an email and password corresponds or not </summary>

        public static bool PhoneNumberExists(string phoneNumber)
        {
            SqlConnection connection = new SqlConnection("Data Source=.;Initial Catalog=db_Library;Integrated Security=True");

            try
            {

                DataTable table = DatabaseControl.TableFiller("select * from T_Employees where phoneNumber = '" + phoneNumber + "'", connection);

                    if (table.Rows.Count == 1)
                        return true;

                table.Clear();
                table = DatabaseControl.TableFiller("select * from T_Members where phoneNumber = '" + phoneNumber + "'", connection);

                    if (table.Rows.Count == 1)
                        return true;              
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                connection.Close();
            }

            return false;
        }
        // <summary> Checks wether a username and password corresponds or not </summary>

        public static bool PasswordCorresponds(string password, string username, out string window)
        {
            SqlConnection connection = new SqlConnection("Data Source=.;Initial Catalog=db_Library;Integrated Security=True");

            try
            {
                DataTable table = DatabaseControl.TableFiller("select * from T_Employees where username = '" + username.Trim() + "' AND password = '" + password + "'", connection);

                if (table.Rows.Count == 1)
                {
                    window = "Employee";
                    return true;
                }

                table.Clear();

                table = DatabaseControl.TableFiller("select * from T_Members where username = '" + username.Trim() + "' AND password = '" + password + "'", connection);

                if (table.Rows.Count == 1)
                {
                    window = "Member";
                    return true;
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                connection.Close();
            }

            window = null;
            return false;
        }


        public static bool IsValidEmail(string email)
        {
            string pattern = @"(^[a-zA-Z0-9_-]{1,32})@([a-zA-Z0-9]{1,8}\.)([a-zA-Z]{1,3})$";

            Regex re = new Regex(pattern);

            if (re.IsMatch(email)) return true;
            else return false;
        }

        public static bool IsValidPassword(string password)
        {
            string pattern = @"^(?=.*[A-Z])[a-zA-Z0-9_]{8,32}";
            
            Regex re = new Regex(pattern);

            if (re.IsMatch(password)) return true;
            else return false;
        }
        public static bool IsValidPassCard(string PassCard)
        {
            string pattern = @"^[0-9]*$";

            Regex re = new Regex(pattern);

            if (re.IsMatch(PassCard)) return true;
            else return false;
        }
        public static bool IsValidUsername(string username)
        {
            string pattern = @"^(?=.*[a-zA-Z])[a-zA-Z ]{3,32}$";

            Regex re = new Regex(pattern);

            if (re.IsMatch(username)) return true;
            else return false;
        }

        public static bool IsValidPhoneNumber(string PhoneNumber)
        {
            string stringOfRegex = @"^09+[0-9]{9}$";

            Regex re = new Regex(stringOfRegex);

            if (re.IsMatch(PhoneNumber.ToString())) return true;
            else return false;
        }

        public static bool IsValidCvv2(string cvv2)
        {
            string stringOfRegex = @"^[0-9]{3,4}$";

            Regex re = new Regex(stringOfRegex);

            if (re.IsMatch(cvv2.ToString())) return true;
            else return false;
        }
        public static bool IsValidExpiry(string expiry)
        {
            try
            {
                string[] temp=expiry.Split('/');
                PersianCalendar pc = new PersianCalendar();
                DateTime dt1 = pc.ToDateTime(int.Parse(temp[0]), int.Parse(temp[1]), 01, 0, 0, 0, 0);

                if (DateTime.Now.Year < dt1.Year)
                    return true;

                if (DateTime.Now.Year == dt1.Year)
                    if (DateTime.Now.Month <= dt1.Month - 3)
                        return true;
            }
            catch
            {
                return false;
            }
            return false;
        }
        public static bool IsValidCardNumber(string card)
        {
            string strRegex = @"^[0-9]{16}";
            Regex regex = new Regex(strRegex);
            if (!regex.IsMatch(card)) return false;

            int[] numbers = new int[16];

            for (int i = 0; i < 16; i++)
            {
                numbers[i] = int.Parse(card.Substring(i, 1));

                if (i % 2 == 0)
                {
                    int number = int.Parse(card.Substring(i, 1));
                    number *= 2;
                    if (number < 10)
                        numbers[i] = number;
                    else
                    {
                        int a = number % 10;
                        number /= 10;
                        numbers[i] = number + a;
                    }
                }               
            }

            int sum = numbers.Sum();
            if (sum % 10 == 0)
                return true;
            return false;
        }

    }
}
