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
    public partial class UbahKamar : Form
    {
        public UbahKamar()
        {
            InitializeComponent();
        }

        SqlCommand cmd;
        SqlDataReader reader;

        int idKamar;
        public void idNoKamar(int noKamar)
        {
            configconn.conn.Open();
            idKamar = noKamar;
            cmd = new SqlCommand("select kamar_no, kamar_tipe, kamar_kapasitas, jumlah_tamu from Kamar km, Kamar_Kapasitas kk, Kamar_Tipe kt where km.kamar_no = " + idKamar + " and km.kamar_tipe_id = kt.kamar_tipe_id and km.kamar_kapasitas_id = kk.kamar_kapasitas_id", configconn.conn);
            reader = cmd.ExecuteReader();
            reader.Read();
            textBox1.Text = idKamar.ToString();
            textBox3.Text = reader.GetString(1);
            textBox2.Text = idKamar.ToString();
            textBox4.Text = reader.GetString(2);
            configconn.conn.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            configconn.conn.Open();
            int kamarKapasitasID;
            int kamarTipeID;
            cmd = new SqlCommand("select kk.kamar_kapasitas_id from Kamar_Kapasitas kk where kk.kamar_kapasitas ='" + textBox4.Text + "'", configconn.conn);
            reader = cmd.ExecuteReader();
            reader.Read();
            kamarKapasitasID = reader.GetInt32(0);
            configconn.conn.Close();

            configconn.conn.Open();
            cmd = new SqlCommand("select kt.kamar_tipe_id from Kamar_Tipe kt where kt.kamar_tipe ='" + textBox3.Text + "'", configconn.conn);
            reader = cmd.ExecuteReader();
            reader.Read();
            kamarTipeID = reader.GetInt32(0);
            configconn.conn.Close();

            using (SqlCommand dataCommand = configconn.conn.CreateCommand())
            {
                configconn.conn.Open();
                dataCommand.CommandText = "update Kamar set kamar_no = "+Convert.ToInt32(textBox2.Text)+
                                                         ", kamar_tipe_id = "+kamarTipeID+
                                                         ", kamar_kapasitas_id = "+kamarKapasitasID+
                                                      " where kamar_no = "+Convert.ToInt32(textBox1.Text)+";";

                //dataCommand.Parameters.AddWithValue("@val1", Convert.ToInt32(textBox1.Text));
                //dataCommand.Parameters.AddWithValue("@param2", kamarTipeID);
                //dataCommand.Parameters.AddWithValue("@param3", kamarKapasitasID);


                dataCommand.ExecuteNonQuery();
                configconn.conn.Close();
                MessageBox.Show("kamar " + textBox1.Text + " telah diubah");
                this.Close();
            }
        }
    }
}
