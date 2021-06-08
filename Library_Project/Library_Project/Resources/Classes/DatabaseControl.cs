using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.IO;


namespace Library_Project.Resources.Classes
{
    public class DatabaseControl
    {
        public static DataTable Select(string inputCmd)
        {
            DataTable data = new DataTable();
            SqlConnection sqlcon = new SqlConnection();
            SqlCommand sqlcmd = new SqlCommand();
            SqlDataAdapter sqldata = new SqlDataAdapter();

            sqlcon.ConnectionString = @"Data Source=.;Initial Catalog=db_Library;Integrated Security=True";
            sqlcon.Open();

            sqlcmd.Connection = sqlcon;
            sqlcmd.CommandText = inputCmd;
            sqldata.SelectCommand = sqlcmd;
            sqldata.Fill(data);

            return data;

        }
        public static bool Exe(string inputCmd)
        {
            SqlConnection sqlcon = new SqlConnection();
            SqlCommand sqlcmd = new SqlCommand();

            sqlcon.ConnectionString = @"Data Source=.;Initial Catalog=db_Library;Integrated Security=True";
            sqlcon.Open();

            sqlcmd.Connection = sqlcon;
            sqlcmd.CommandText = inputCmd;
            try
            {
                sqlcmd.ExecuteNonQuery();
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                sqlcon.Close();
            }
        }
        public static DataTable TableFiller(string commandText,SqlConnection connection)
        {
            if (connection.State == ConnectionState.Closed)
                connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = commandText;

            SqlDataAdapter adaptor = new SqlDataAdapter(command);
            DataTable table = new DataTable();
            adaptor.Fill(table);
            
            return table;
        }
    }
}
