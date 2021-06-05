using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Library_Project.Resources.Classes
{
    static class Validation
    {

        // <summary> Checks wether an email address exists or not </summary>

        public static bool EmailExists(string email)
        {
            // take data from database_Employee. it may have data besides string
            // take data1 from database_Member. it may have data besides string

            string[] data = null;

            string[] data1 = null;

            for (int i = 0; i < data.Length; i++)
            {
                if (data[i] == email)
                    return true;
            }

            for (int i = 0; i < data1.Length; i++)
            {
                if (data1[i] == email)
                    return true;
            }

            return false;
        }

        // <summary> Checks wether a username address exists or not </summary>

        public static bool UserNameExists(string username)
        {
            // take data from database_Employee. it may have data besides string
            // take data1 from database_Member. it may have data besides string

            string[] data = null;

            string[] data1 = null;

            for (int i = 0; i < data.Length; i++)
            {
                if (data[i] == username)
                    return true;
            }

            for (int i = 0; i < data1.Length; i++)
            {
                if (data1[i] == username)
                    return true;
            }

            return false;
        }

        // <summary> Checks wether an email and password corresponds or not </summary>

        public static bool PhoneNumberExists(string phoneNumber)
        {
            // take data from database_Employee. it may have data besides string
            // take data1 from database_Member. it may have data besides string

            string[] data = null;

            string[] data1 = null;

            for (int i = 0; i < data.Length; i++)
            {
                if (data[i] == phoneNumber)
                    return true;
            }

            for (int i = 0; i < data1.Length; i++)
            {
                if (data1[i] == phoneNumber)
                    return true;
            }

            return false;
        }

        // <summary> Checks wether an email and password corresponds or not </summary>

        public static bool PasswordCorresponds(string password, string email)
        {
            // take data from database_Employee. it may have data besides string
            // take data1 from database_Member. it may have data besides string

            bool flag = false;

            Dictionary<string, string> dict1 = null;
            Dictionary<string, string> dict2 = null;

            string actualValue;
            flag = dict1.TryGetValue(email, out actualValue) && actualValue == password;

            if (flag == true)
                return flag;

            return dict2.TryGetValue(email, out actualValue) && actualValue == password;
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
