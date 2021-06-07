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

                table = DatabaseControl.TableFiller("select * from T_Members where email = '" + username + "'", connection);

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

        public static bool PasswordCorresponds(string password, string username)
        {
            SqlConnection connection = new SqlConnection("Data Source=.;Initial Catalog=db_Library;Integrated Security=True");

            try
            {
                DataTable table = DatabaseControl.TableFiller("select * from T_Employees where username = '" + username + "' AND password = '" + password + "'", connection);

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

        public static bool IsValidEmail(string email)
        {
            string pattern = @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z";

            Regex re = new Regex(pattern, RegexOptions.IgnoreCase);

            if (re.IsMatch(email)) return true;
            else return false;
        }

        public static bool IsValidPassword(string password)
        {
            string pattern = @"[a-z](?=.*[A-Z]){8,32}";

            Regex re = new Regex(pattern, RegexOptions.IgnoreCase);

            if (re.IsMatch(password)) return true;
            else return false;
        }

        public static bool IsValidUsername(string username)
        {
            string pattern = @"[a-z]{3,32}";

            Regex re = new Regex(pattern, RegexOptions.IgnoreCase);

            if (re.IsMatch(username)) return true;
            else return false;
        }

        public static bool IsValidPhoneNumber(int studentId)
        {
            string stringOfRegex = @"^09+[0-9]{9}$";

            Regex re = new Regex(stringOfRegex, RegexOptions.IgnoreCase);

            if (re.IsMatch(studentId.ToString())) return true;
            else return false;
        }

        public static bool IsValidCvv2(string cvv2)
        {
            string stringOfRegex = @"([0-9]{3} | [0-9]{4})";

            Regex re = new Regex(stringOfRegex, RegexOptions.IgnoreCase);

            if (re.IsMatch(cvv2.ToString())) return true;
            else return false;
        }

        public static bool IsValidExpiry(string expiry)
        {
            string format = "yyMMdd";

            var time = DateTime.ParseExact(expiry, format, CultureInfo.InvariantCulture);

            if (DateTime.Now.Year < time.Year)
                return true;

            if (DateTime.Now.Year == time.Year)
                if (DateTime.Now.Month <= time.Month - 3)
                    return true;

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
                numbers[i] = Convert.ToInt32(card[i++]);

                int number = Convert.ToInt32(card[i]);
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

            int sum = numbers.Sum();
            if (sum % 10 == 0)
                return true;
            return false;
        }

    }
}
