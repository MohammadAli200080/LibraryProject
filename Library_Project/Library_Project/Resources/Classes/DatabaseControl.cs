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
