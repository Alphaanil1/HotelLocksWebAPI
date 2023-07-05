using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelLock.DAL
{
    public class DBConnection
    {
        public static string connectionString = @"Data Source =64.202.191.110;Initial Catalog = HotelLocks; User Id=sa;Password=Alpha%03;Trusted_Connection=False;MultipleActiveResultSets=true;";
        //public static string connectionString = @"Data Source =ALPHA002\SQLGODREJ;Initial Catalog = HotelLocks; User Id=sa;Password=Alpha%03;Trusted_Connection=False;MultipleActiveResultSets=true;";
        public IDbConnection connection;
        public DBConnection()
        {
            connection = new SqlConnection(connectionString);
        }
        public void CloseConnection()
        {
            connection.Dispose();
        }
    }
}
