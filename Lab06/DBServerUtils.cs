using System.Data.SqlClient;
using System.Data;
using System;

namespace Lab06
{
    class DBServerUtils
    {
        public static string source = @"WIN-5UUGETFTJ1E\SQLEXPRESS"; //сервер
        public static string catalog = "library"; //база данных
        //подключение к бд
        public static bool connectingDB(int d)
        {
            try
            {
                if (d == 0)
                {
                    SqlConnection connect = getDBсonnection();
                    connect.Open();
                }
                else if (d == 1)
                {
                    SqlConnection connect = getDBсonnection();
                    connect.Close();
                }
                return true;
            }
            catch
            {
                return false;
            }

        }
        //получение соединения
        public static SqlConnection getDBсonnection()
        {
            string connString = "Data Source=" + source + ";Initial Catalog=" + catalog + ";Integrated Security=True";
            SqlConnection conn = new SqlConnection(connString);
            return conn;
        }
        //получение данных с бд
        public static DataSet getData(string sql)
        {
            SqlConnection connection = getDBсonnection();
            SqlDataAdapter adapter = new SqlDataAdapter(sql, connection);
            DataSet data = new DataSet();
            adapter.Fill(data);
            return data;
        }
    }
}
