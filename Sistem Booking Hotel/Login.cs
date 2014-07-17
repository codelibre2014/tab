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
    public partial class Login : Form
    {
        public static int idS = 0;
        public int cekNilai;

        public Login()
        {
            InitializeComponent();
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.BackColor = Color.Transparent;

        }

        
           
        //protected override void OnPaintBackground(PaintEventArgs e)
        //{
        //    //empty implementation<
        //}

        private void Login_Load(object sender, EventArgs e)
        {
           // Login.back .FromArgb(100, 88, 44, 55);
            inputIdentitas.Focus();
            inputIdentitas.Select(inputIdentitas.Text.Length, 0);
            
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            validate();
        }

        private void inputSandi_Enter(object sender, EventArgs e)
        {
            //validate();
        }

        //private void validate()
        //{
        //    if ((inputIdentitas.Text == inputSandi.Text) && ((inputIdentitas.Text == "admin") || (inputIdentitas.Text == "resepsionis")))
        //    {
        //        FormUtama FormUtama = new FormUtama();

        //        FormUtama.Show();
        //    }
        //    else
        //        MessageBox.Show("Login Gagal");
        //}

        private void validate()
        {
            configconn config = new configconn();
            config.KoneksiDB();
            //configconn.conn.Open();
            SqlCommand cmd = new SqlCommand("select Id_jabatan,staff_id from Staff where username=@nama and password=@pass", configconn.conn);
            cmd.Parameters.AddWithValue("@nama", inputIdentitas.Text);
            cmd.Parameters.AddWithValue("@pass", inputSandi.Text);
            SqlDataReader reader;
            reader = cmd.ExecuteReader();
            cekNilai = 0;
            while (reader.Read())
            {
                cekNilai = (reader.GetInt32(0));
                idS = reader.GetInt32(1);
            }
            /*
            if ((inputIdentitas.Text == inputSandi.Text) && ((inputIdentitas.Text == "admin") || (inputIdentitas.Text == "resepsionis")))
            {
                FormUtama FormUtama = new FormUtama();

                FormUtama.Show();
            }
             */
            if (cekNilai > 1)
            {
                //string admin = "staff";
                FormUtama FormUtama = new FormUtama();
                FormUtama.getAdmin = "staff";
                FormUtama.Show();
                this.Hide();
            }

            else if (cekNilai == 1)
            {

                //AdminForm administratorForm = new AdminForm();
                //administratorForm.Show();
                //string admin = "admin";
                FormUtama FormUtama = new FormUtama();
                FormUtama.getAdmin = "admin";
                Button btnPengaturanKamar = ((Button)FormUtama.Controls.Find("btnPengaturanKamar", true)[0]);
                //if (removeaddBooking != null) panelKamarDibooking.Controls.Remove(removeaddBooking);
                //FormUtama.Controls.btn
                FormUtama.Show();
                
                this.Hide();
                //Form2 newForm = new Form2();
                //newForm.TheValue = value;
                //newForm.ShowDialog();
                /*
                cobaForm coba = new cobaForm();
                coba.Show();
                this.Hide();
                 */
            }
            else
                MessageBox.Show("Login Gagal");

            configconn.conn.Close();
        }

        private void Login_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) validate();
        }

        private void keluarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
