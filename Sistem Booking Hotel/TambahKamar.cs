using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Sistem_Booking_Hotel
{
    public partial class TambahKamar : Form
    {
        public TambahKamar()
        {
            InitializeComponent();
        }

        SqlCommand cmd;
        SqlDataReader reader;
        //SqlDataReader reader1;

        private void button1_Click(object sender, EventArgs e)
        {
            configconn.conn.Open();
            int kamarKapasitasID;
            int kamarTipeID;
            cmd = new SqlCommand("select kk.kamar_kapasitas_id from Kamar_Kapasitas kk where kk.kamar_kapasitas ='" + textBox3.Text + "'", configconn.conn);
            reader = cmd.ExecuteReader();
            reader.Read();
            kamarKapasitasID = reader.GetInt32(0);
            configconn.conn.Close();

            configconn.conn.Open();
            cmd = new SqlCommand("select kt.kamar_tipe_id from Kamar_Tipe kt where kt.kamar_tipe ='" + textBox2.Text + "'", configconn.conn);
            reader = cmd.ExecuteReader();
            reader.Read();
            kamarTipeID = reader.GetInt32(0);
            configconn.conn.Close();

            /*
            cmd = new SqlCommand("insert into Kamar(kamar_no, kamar_tipe_id, kamar_kapasitas_id) values(@param1, @param2, @param3)", Form3.conn);
            
            Form3.conn.Open();
            cmd.ExecuteNonQuery();
            Form3.conn.Close();*/

            using (SqlCommand dataCommand = configconn.conn.CreateCommand())
            {
                configconn.conn.Open();
                dataCommand.CommandText = "INSERT INTO Kamar (kamar_no, kamar_tipe_id, kamar_kapasitas_id) values(@val1, @param2, @param3)";
                                          
                dataCommand.Parameters.AddWithValue("@val1", Convert.ToInt32(textBox1.Text));
                dataCommand.Parameters.AddWithValue("@param2", kamarTipeID);
                dataCommand.Parameters.AddWithValue("@param3", kamarKapasitasID); 
                
                
                dataCommand.ExecuteNonQuery();
                configconn.conn.Close();
                MessageBox.Show("kamar " + textBox1.Text + " telah tersedia");
            }
        }
    }
}
