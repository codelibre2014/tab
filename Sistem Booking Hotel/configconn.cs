using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Sistem_Booking_Hotel
{
    public class configconn
    {
        public static SqlConnection conn;
        public SqlConnection KoneksiDB()
        {
            conn = new SqlConnection("Data Source=LYCURGUS\\DEMO3;Initial Catalog=tabHotel;Integrated Security=True");
            conn.Open();
            return conn;
        }
    }
}
