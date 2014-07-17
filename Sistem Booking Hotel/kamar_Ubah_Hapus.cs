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
    public partial class kamar_Ubah_Hapus : Form
    {
        public kamar_Ubah_Hapus()
        {
            InitializeComponent();
        }
        int nomorKamarX;
        public void passNoKamar(int noKamar)
        {
            nomorKamarX = noKamar;
        }

        private void kamar_Ubah_Hapus_Load(object sender, EventArgs e)
        {

        }

        private void btn_Kamar_Ubah_Click(object sender, EventArgs e)
        {
            UbahKamar ubahKamar = new UbahKamar();
            ubahKamar.idNoKamar(nomorKamarX);
            ubahKamar.Show();
            this.Close();
        }

        SqlCommand cmd;
        private void btn_Kamar_Hapus_Click(object sender, EventArgs e)
        {
            //HapusKamar hapusKamar = new HapusKamar();
            //hapusKamar.Show();
            this.Close();
            configconn.conn.Open();
            cmd = new SqlCommand("delete from Kamar where kamar_no = @paramkamar", configconn.conn);
            cmd.Parameters.AddWithValue("@paramkamar", nomorKamarX);
            cmd.ExecuteNonQuery();
            configconn.conn.Close();
            MessageBox.Show("kamar " + nomorKamarX.ToString() + " telah terhapus");
        }
    }
}
