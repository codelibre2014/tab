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
using Microsoft.VisualBasic;

namespace Sistem_Booking_Hotel
{
    public partial class FormUtama : Form
    {
        SqlCommand cmd;
        
        // irwan tambahkan 
        DataTable dKamarPesan = new DataTable();
        ComboboxItem item = new ComboboxItem();
        Boolean cekPilih = false;
        int dataCustomer = 0; int rowSelect = 0; int columnSelect;
        int TglBulan = 0; int Tgltahun = 0;
        int totalBiaya = 0;
        DataSet ds;
        SqlDataAdapter da;
        
        int dataKamarCh = 0;
        int noIDdatatamu = 0;
        
        // end irwan

        public string boxJbt = "";
        public string boxCheck = "";
        public string boxBook = "";
        public string boxTamu = "";
        public string boxKalender = "";
        public string boxDafBook = "";
        public string boxUpdBook = "";
        public string boxPrint = "";
        public string boxUser = "";
        public string boxRights = "";
        public int boxId;

        public FormUtama()
        {
            InitializeComponent();                    
        }

        SqlDataReader reader;
        int x = 0;
        configconn koneksi = new configconn();
        //SqlConnection conn = new SqlConnection();



        private string isAdmin;
        public string getAdmin
        {
            get
            {
                return isAdmin;
            }
            set
            {
                isAdmin = value;
                // do something with _theValue so that it
                // appears in the UI

            }
        }

        public void refresh_kamar()
        {

            dKamarPesan.Reset();
            dKamarPesan.Columns.Add("NO Kamar".ToString());
            dKamarPesan.Columns.Add("Checkin", typeof(DateTime));
            dKamarPesan.Columns.Add("Checkout", typeof(DateTime));
            dKamarPesan.Columns.Add("Tamu".ToString());
            dKamarPesan.Columns.Add("Harga".ToString());
            
            panelKamarDibooking.Enabled = true;
            panelKamar.Controls.Clear();
            //conn = koneksi.KoneksiDB();
            //conn.Open();
            cmd = new SqlCommand((@"select count(*) from Kamar"), koneksi.KoneksiDB());

            int jumKamar = (int)cmd.ExecuteScalar();

            ///button1.Text = jumKamar.ToString();
            Button[] Kamar;

            //command.Parameters.AddWithValue("@Username", username);
            //command.Parameters.AddWithValue("@Password", password);

            cmd = new SqlCommand(
            (@"
            select k.kamar_no,kamar_tipe,
            case when DATENAME(dw,tanggal_id) in ('Saturday','Sunday') then harga_weekend else harga end harga 
            from 
            (
	            select distinct kamar_no
	            from 
	            Reservasi r
                where 
	                (
                        (r.checkin >= @checkindate
	                    and
	                    r.checkout <=@checkoutdate
	                    )
	                    or
	                    (
	                    r.checkin <= @checkindate
	                    and
	                    r.checkout >=@checkoutdate
	                    )
	                    or 
	                    (
	                    r.checkin >= @checkindate
	                    and
	                    r.checkin <=@checkoutdate
	                    )
	                    or 
	                    (
	                    r.checkout >= @checkindate
	                    and
	                    r.checkout <=@checkoutdate
	                    )
                    )
                    and r.status in ('booking','checkin') 
            )a
            full join
            Kamar k
            on a.kamar_no = k.kamar_no 
            inner join 
            kamar_tipe kt on k.kamar_tipe_id = kt.kamar_tipe_id inner join harga h on h.tanggal_id = '2008-7-1'
            and kt.kamar_tipe_id = h.kamar_tipe_id
			where a.kamar_no is null
            "), koneksi.KoneksiDB());
            cmd.Parameters.AddWithValue("@checkindate",checkinDate.Value.ToString("yyyy-M-d"));
            cmd.Parameters.AddWithValue("@checkoutdate", checkoutDate.Value.ToString("yyyy-M-d"));
            /*
             
             cmd = new SqlCommand(
            (@"select
            k.kamar_no,
            k.kamar_tipe_id,
            case when DATENAME(dw,tanggal_id) in ('Saturday','Sunday') then harga_weekend else harga end harga
            from            
            Kamar k
            inner join kamar_tipe kt on k.kamar_tipe_id = kt.kamar_tipe_id 
            inner join harga h on h.tanggal_id = '2008-7-1'
            and kt.kamar_tipe_id = h.kamar_tipe_id"), koneksi.KoneksiDB());
            //cmd.Parameters.AddWithValue("@checkindate",checkinDate.Value.ToString("yyyy-M-d"));
            //cmd.Parameters.AddWithValue("@checkoutdate",checkoutDate.Value.ToString("yyyy-M-d"));
             */


            reader = cmd.ExecuteReader();
            Kamar = new Button[jumKamar];
            x = 0;
            while (reader.Read())
            {
                Kamar[x] = new Button();
                Kamar[x].Text = reader.GetInt32(0).ToString() + "\n\r" + reader.GetString(1);
                Kamar[x].Name = reader.GetInt32(0).ToString();
                Kamar[x].Visible = true;
                Kamar[x].Height = 35;
                Kamar[x].Tag = reader.GetDouble(2).ToString();
                Kamar[x].BackColor = Color.FromName(reader.GetString(1));
                Kamar[x].Click += new EventHandler(tambah_kamar);
                //Kamar[x].MouseEnter += new EventHandler(button1_MouseEnter_2);
                //Kamar[x].MouseLeave += new EventHandler(button1_MouseLeave_1);

                panelKamar.Controls.Add(Kamar[x]);
                x += 1;
                //Kamar[x].MouseEnter += button1_MouseEnter_2;// Kamar_Tips;//new EventHandler(Kamar_Tips);
                
            }
            //conn.Close();
            koneksi.KoneksiDB().Close();
        }


        public void refresh_pengaturankamar()
        {
            configconn.conn.Open();
            SqlCommand cmd = new SqlCommand((@"select count(*) from Kamar"), configconn.conn);
            int jumKamar = (int)cmd.ExecuteScalar();

            ///button1.Text = jumKamar.ToString();
            Button[] Kamar;
            cmd = new SqlCommand("select kamar_no from Kamar", configconn.conn);
            reader = cmd.ExecuteReader();
            Kamar = new Button[jumKamar + 1];
            x = 0;
            while (reader.Read())
            {
                Kamar[x] = new Button();
                Kamar[x].Text = reader.GetInt32(0).ToString(); //+ "\n\r" + reader.GetString(1);
                Kamar[x].Name = reader.GetInt32(0).ToString();
                Kamar[x].Visible = true;
                Kamar[x].Height = 35;
                //Kamar[x].Tag = reader.GetDouble(2).ToString();
                //Kamar[x].BackColor = Color.FromName(reader.GetString(1));
                Kamar[x].Click += new EventHandler(load_Ubah_Hapus);
                //Kamar[x].MouseEnter += new EventHandler(button1_MouseEnter_2);
                //Kamar[x].MouseLeave += new EventHandler(button1_MouseLeave_1);

                flowLayoutPanel1.Controls.Add(Kamar[x]);
                x += 1;
                //MessageBox.Show("ok");
                //Kamar[x].MouseEnter += button1_MouseEnter_2;// Kamar_Tips;//new EventHandler(Kamar_Tips);

            }
            Kamar[x] = new Button();
            //Kamar[x].Text = reader.GetInt32(0).ToString() + "\n\r" + reader.GetString(1);
            //Kamar[x].Name = reader.GetInt32(0).ToString();
            Kamar[x].Text = "+";
            Kamar[x].Visible = true;
            Kamar[x].Height = 35;
            //Kamar[x].Tag = reader.GetDouble(2).ToString();
            Kamar[x].BackColor = Color.Aqua;
            Kamar[x].Click += new EventHandler(tambah_Kamar);
            flowLayoutPanel1.Controls.Add(Kamar[x]);
            //conn.Close();
            configconn.conn.Close();
        }
        //kamar_Ubah_Hapus loadUbahHapus = new kamar_Ubah_Hapus();
        protected void load_Ubah_Hapus(object sender, EventArgs e)
        {
            kamar_Ubah_Hapus loadUbahHapus = new kamar_Ubah_Hapus();
            Button btn = sender as Button;
            loadUbahHapus.passNoKamar(Convert.ToInt32(btn.Text));
            loadUbahHapus.Show();
            loadUbahHapus.Location = new Point(btn.Location.X + (btn.Width) + loadUbahHapus.Width, btn.Location.Y);
        }

        protected void tambah_Kamar(object sender, EventArgs e)
        {
            TambahKamar tambahKamar = new TambahKamar();
            tambahKamar.Show();
        }



        private void Form2_Load(object sender, EventArgs e)
        {
            if (isAdmin == "admin") btnPengaturanKamar.Visible = true; else btnPengaturanKamar.Visible = false;
            //MessageBox.Show(isAdmin.ToString());
            // TODO: This line of code loads data into the 'tabHotelDataSet.Tamu' table. You can move, or remove it, as needed.
            this.tamuTableAdapter.Fill(this.tabHotelDataSet.Tamu);
            // TODO: This line of code loads data into the 'tabHotelDataSet1.Tamu' table. You can move, or remove it, as needed.
            //this.tamuTableAdapter.Fill(this.tabHotelDataSet1.Tamu);
            // TODO: This line of code loads data into the 'tabHotelDataSet.Booking' table. You can move, or remove it, as needed.
            //this.bookingTableAdapter.Fill(this.tabHotelDataSet.Booking);
            //this.TopMost = true;
           
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            checkoutDate.Value = checkinDate.Value.AddDays(1);
            panelKamar.BringToFront();
            panelCheckinDate.Visible = true;
            panelCheckoutDate.Visible = true;
            refresh_kamar();
            //irwan tambahkan
            isiCombobox();
            isCombobox3();
            // end irwan



            //connecting();
            //conn.Open();

            cmbJbtUsr.Refresh();
            cmbJbtR.Refresh();
            cmd = new SqlCommand("select id_jabatan,jabatan from jabatan", koneksi.KoneksiDB());
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                boxId = reader.GetInt32(0);
                cmbJbtR.Items.Add(reader.GetString(1));
                cmbJbtUsr.Items.Add(reader.GetString(1));
                //a++;
            }
//            conn.Close();

  //          connecting();
    //        conn.Open();
            cmd = new SqlCommand("select nama from staff", koneksi.KoneksiDB());
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                cmbNmUsr.Items.Add(reader.GetString(0));
                //a++;
            }
//            conn.Close();

            string strQ = "select * from jabatan";
            createTblNoParam(strQ);

            string strQUsr = "select nama,password,username,telp,email,Jabatan " +
                "from staff a,Jabatan b where a.id_jabatan=b.id_jabatan";
            createTblNoParamUsr(strQUsr);

        }


        private void createTblNoParam(string strQuery)
        {

//            connecting();
//            conn.Open();
            cmd = new SqlCommand(strQuery, koneksi.KoneksiDB());
            ds = new DataSet();
            da = new SqlDataAdapter(cmd);
            da.Fill(ds, "User Rights");
            dgR.DataSource = ds;
            dgR.DataMember = "User Rights";
            dgR.Refresh();
            koneksi.KoneksiDB().Close();

        }

        private void createTblNoParamUsr(string strQuery)
        {

            //connecting();
            //conn.Open();
            cmd = new SqlCommand(strQuery, koneksi.KoneksiDB());
            ds = new DataSet();
            da = new SqlDataAdapter(cmd);
            da.Fill(ds, "User");
            dgUsr.DataSource = ds;
            dgUsr.DataMember = "User";
            dgUsr.Refresh();
            koneksi.KoneksiDB().Close();

        }

        private void createTbl1Param(string strQuery, string strParam)
        {

            //connecting();
            //conn.Open();
            cmd = new SqlCommand(strQuery, koneksi.KoneksiDB());
            cmd.Parameters.AddWithValue("@nm", strParam);
            ds = new DataSet();
            da = new SqlDataAdapter(cmd);
            da.Fill(ds, "User Rights");
            dgR.DataSource = ds;
            dgR.DataMember = "User Rights";
            dgR.Refresh();
            koneksi.KoneksiDB().Close();

        }

        private void createTbl1ParamUsr(string strQuery, string strParam)
        {

            //connecting();
            //conn.Open();
            cmd = new SqlCommand(strQuery, koneksi.KoneksiDB());
            cmd.Parameters.AddWithValue("@nm", strParam);
            ds = new DataSet();
            da = new SqlDataAdapter(cmd);
            da.Fill(ds, "User");
            dgUsr.DataSource = ds;
            dgUsr.DataMember = "User";
            dgUsr.Refresh();
            koneksi.KoneksiDB().Close();

        }

        private void clearJabatan()
        {
            //connecting();
            //conn.Open();
            cmbJbtUsr.Items.Clear();
            //cmbJbtUsr.ResetText();
            string strQ2 = "select jabatan from Jabatan";
            cmd = new SqlCommand(strQ2, koneksi.KoneksiDB());
            //cmd.Parameters.AddWithValue("@nm", cmbJbtUsr.SelectedItem.ToString());
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                // boxId = reader.GetInt32(0);
                //    lblHideJabUsr.Text = reader.GetInt32(0).ToString();
                //    //cmbJbtR.Items.Add(reader.GetString(1));
                cmbJbtUsr.Items.Add(reader.GetString(0));
                //    //a++;
            }
            koneksi.KoneksiDB().Close();
        }


        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Enter(object sender, EventArgs e)
        {

            
        }

        private void button1_MouseEnter(object sender, EventArgs e)
        {
            toolTip1.Show("Tooltip text goes here\r\nTestingtesting\r\n", btnBooking);
        }

        private void button1_MouseLeave(object sender, EventArgs e)
        {
            toolTip1.Hide(btnBooking);
        }

        private void keluarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        protected void Kamar_Tips(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            //toolTip1.Show(
            //    "----------------------------------------------------------------------------\r\n" +
            //    "                       Booking ##########\r\n Dipesan oleh ##########\r\n" +
            //    "----------------------------------------------------------------------------\r\n" +
            //    "Kamar ## Tamu ########### Checkin ##-##-## Checkout ##-##-##\r\n" +
            //    "Kamar ## Tamu ########### Checkin ##-##-## Checkout ##-##-##\r\n" +
            //    "----------------------------------------------------------------------------\r\n"
            //    , btn);

        }


        //private void button1_Click(object sender, EventArgs e)
        //{
        //    //if (panelCheckinDate.Visible == false)
        //    //{
        //    //    panelCheckinDate.Visible = true;
        //    //    panelCheckoutDate.Visible = true;
        //    //    panelKamar.BringToFront();
        //    //}else
        //    //{
        //    //    panelCheckinDate.Visible = false;
        //    //    panelCheckoutDate.Visible = false;
        //    //}

        //    refresh_kamar();
        //    panelCheckinDate.Visible = true;
        //    panelCheckoutDate.Visible = true;
        //    panelKamar.BringToFront();
        //    panelKamarDibooking.Controls.Clear();
        //    groupBukuTamu.SendToBack();
        //    panelDataTamu.Enabled = true;
        //}

        private void button1_MouseEnter_1(object sender, EventArgs e)
        {
            toolTip1.Show(
                "----------------------------------------------------------------------------\r\n" +
                "                       Booking ##########\r\n Dipesan oleh ##########\r\n" +
                "----------------------------------------------------------------------------\r\n" +
                "Kamar ## Tamu ########### Checkin ##-##-## Checkout ##-##-##\r\n" +
                "Kamar ## Tamu ########### Checkin ##-##-## Checkout ##-##-##\r\n" +
                "----------------------------------------------------------------------------\r\n"              
                , btnBooking);

        }

        protected void button1_MouseLeave_1(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            toolTip1.Hide(btn);
        }

        private void toolStripTextBox1_Click(object sender, EventArgs e)
        {

        }

        //private void button1_Click_1(object sender, EventArgs e)
        //{
        //    if (button1.FlatAppearance.BorderSize == 1)
        //    {
        //        button1.FlatAppearance.BorderSize = 2;
        //        button1.Font = new Font(button1.Font, FontStyle.Bold);
        //    }else
        //    {
        //        button1.FlatAppearance.BorderSize = 1;
        //        button1.Font = new Font(button1.Font, FontStyle.Regular);

        //    }
        //}

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnKalender_Click(object sender, EventArgs e)
        {
             
            panelKamarDibooking.Controls.Clear();
            //irwan tambahkan
            DataTamuKalender.Visible = false;
            hidepanelPengaturanKamar();
            panelPengaturanKamar.SendToBack();
            //irwan tambahkan
            cekPilih = true;
            flowLayoutPanel1.Visible = true;
            dataGridView3.BringToFront();
            //setLoad(7, 2014);
            setLoad(DateTime.Now.Month,DateTime.Now.Year);
            panelKalender.BringToFront();
            //setLoad(,);
            //comboBox2.Text = "Juli";//comboBox2.Items[0].ToString();
            //MessageBox.Show(DateTime.Now.Month.ToString());
            switch (DateTime.Now.Month)
            {
                case 1: comboBox2.Text = "Januari"; break;
                case 2: comboBox2.Text = "Februari"; break;
                case 3: comboBox2.Text = "Maret"; break;
                case 4: comboBox2.Text = "April"; break;
                case 5: comboBox2.Text = "Mei"; break;
                case 6: comboBox2.Text = "Juni"; break;
                case 7: comboBox2.Text = "Juli"; break;
                case 8: comboBox2.Text = "Agustus"; break;
                case 9: comboBox2.Text = "September"; break;
                case 10: comboBox2.Text = "Oktober"; break;
                case 11: comboBox2.Text = "November"; break;
                default: comboBox2.Text = "Desember"; break;
            }
            comboBox3.Text = DateTime.Now.Year.ToString();
            dataGridView3.ReadOnly = true;
           //end irwan
            panelCheckinDate.Visible = false;
            panelCheckoutDate.Visible = false;
        }


        private void btnDaftarBooking_Click(object sender, EventArgs e)
        {
            panelDaftarBooking.BringToFront();
            panelCheckinDate.Visible = false;
            panelCheckoutDate.Visible = false;
        }

        private void btnDaftarTamu_Click(object sender, EventArgs e)
        {
             
            panelKamarDibooking.Controls.Clear();
            panelDaftarTamu.BringToFront();
            panelCheckinDate.Visible = false;
            panelCheckoutDate.Visible = false;
        }

        private void statusStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        protected void button1_MouseEnter_2(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            toolTip1.Show(
                "----------------------------------------------------------------------------\r\n" +
                "                       Booking ########## Dipesan oleh ##########\r\n" +
                "----------------------------------------------------------------------------\r\n" +
                "Kamar ## Tamu ########### Checkin ##-##-## Checkout ##-##-##\r\n" +
                "Kamar ## Tamu ########### Checkin ##-##-## Checkout ##-##-##\r\n" +
                "----------------------------------------------------------------------------\r\n"
                , btn);

        }

        private void checkinDate_ValueChanged(object sender, EventArgs e)
        {
            checkoutDate.Value = checkinDate.Value.AddDays(1);
            cekBooking();
            refresh_kamar();
        }

        private void tambah_kamar(object sender, EventArgs e)
        {            
            Button btn = sender as Button;
            //irwan tambahkan

            DataRow dr = dKamarPesan.NewRow();
            dr["NO Kamar"] = btn.Name;
            dr["Checkin"] = checkinDate.Value.Date;
            dr["Checkout"] = checkoutDate.Value.Date;
            dr["Tamu"] = "";
            dr["Harga"] = ((Convert.ToInt32(btn.Tag)) * (checkoutDate.Value.Date - checkinDate.Value.Date).TotalDays).ToString();
            dKamarPesan.Rows.Add(dr);
            item.Text = btn.Name;
            item.Value = btn.Name;
            comboBox4.Items.Add(item);
            comboBox4.Text = btn.Name;
            //end irwan


            //
            btn.Visible = false;
            Button kamarDibooking = new Button();
            kamarDibooking.Name = btn.Name + "Dibooking";
            kamarDibooking.Text = btn.Text;
            kamarDibooking.Margin = new Padding(3,3,3,3);
            kamarDibooking.BackColor = btn.BackColor;
            kamarDibooking.Height = btn.Height;
            panelKamarDibooking.Controls.Add(kamarDibooking);

            Label lblcheckin = new Label();
            lblcheckin.Name = btn.Name + "Checkin";
            lblcheckin.Width = 200;
            lblcheckin.Height = 70;
            lblcheckin.Text = 
                "Checkin                    : " + checkinDate.Value.ToString("yyyy-M-d")+"\n\r"
               + "Checkout                 : " + checkoutDate.Value.ToString("yyyy-M-d")+"\n\r"
               +"Jumlah hari               : " + (checkoutDate.Value.Date - checkinDate.Value.Date).TotalDays.ToString() + " hari\n\r"
               +"Harga kamar satuan : Rp. " +btn.Tag.ToString()+ ",00\n\r"
               + "Harga kamar total     : Rp. " + ((Convert.ToInt32(btn.Tag)) * (checkoutDate.Value.Date - checkinDate.Value.Date).TotalDays).ToString() + ",00\n\r"
               +lblcheckin.Text;

            panelKamarDibooking.Controls.Add(lblcheckin);
            Int32 totalBooking=0;

            try
            {
                Button removeaddBooking = ((Button)panelKamarDibooking.Controls.Find("addBooking", true)[0]);
                if (removeaddBooking != null) panelKamarDibooking.Controls.Remove(removeaddBooking);               
            }
            catch { 
            }


            try
            {
               Label removelblTotalBooking = ((Label)panelKamarDibooking.Controls.Find("lblTotalBooking", true)[0]);
               if (removelblTotalBooking != null)
                {
                    totalBooking = +Convert.ToInt32(removelblTotalBooking.Tag);
                    panelKamarDibooking.Controls.Remove(removelblTotalBooking);
                }
            }
            catch
            {
            }
            
            Label lblTotalBooking = new Label();
            lblTotalBooking.Name = "lblTotalBooking";
            lblTotalBooking.Height = 50;
            lblTotalBooking.Width = 300;
            lblTotalBooking.Margin = new Padding(3, 10, 3, 3);
            
            lblTotalBooking.Tag = (((Convert.ToInt32(btn.Tag)) * (checkoutDate.Value.Date - checkinDate.Value.Date).TotalDays) + totalBooking).ToString();
            lblTotalBooking.Text = 
                "----------------------------------------------------\n\r"
                + "Grand Total : Rp." + lblTotalBooking.Tag.ToString() + ",00\n\r"
                +"----------------------------------------------------";
            panelKamarDibooking.Controls.Add(lblTotalBooking);
            lblTotalBooking.Text = lblTotalBooking.Text;
            //irwan tambahkan
            totalBiaya = Int32.Parse(lblTotalBooking.Tag.ToString());
            //end irwan
            Button addBooking = new Button();
            addBooking.Name = "addBooking";
            //panelKamarDibooking.Controls.Remove("addBooking");
            //LinkLabel lbls = (LinkLabel)sender;
            //this.Controls.Remove((LinkLabel)sender);
            //this.Controls.Remove((TextBox)lbls.Tag);
            addBooking.Width = 150;
            addBooking.Text = "Booking Kamar";
            addBooking.Click += new EventHandler(addBooking_DataTamu);
            panelKamarDibooking.Controls.Add(addBooking);

            //panelKamarDibooking.Controls.Find("addBooking").Visible = false;
            //check51.Visible = false;
            //check51.remov
            //((TextBox)frm.Controls.Find("controlName", true)[0]).Text = "yay";

        }

        private void hidepanelPengaturanKamar()
        {
            try
            {
                Form pengaturanKamarInner = ((Form)splitContainer2.Panel1.Controls.Find("panelPengaturanKamarInnerForm", true)[0]);
                pengaturanKamarInner.Visible = false;
                splitContainer2.Panel1.BringToFront();
            }
            catch
            { }
        }

        private void showpanelPengaturanKamar()
        {
            try
            {
                Form pengaturanKamarInner = ((Form)splitContainer2.Panel1.Controls.Find("panelPengaturanKamarInnerForm", true)[0]);
                pengaturanKamarInner.Visible = true;
            }
            catch
            { }

        }

        private void addBooking_DataTamu(object sender, EventArgs e)
        {
            Button addBooking = sender as Button;
            addBooking.Visible = false;
            //panelKamarDibooking.Enabled = false;
            panelDataTamu.BringToFront();
            panelKamarDibooking.Enabled = false;
        }

        private void checkoutDate_ValueChanged(object sender, EventArgs e)
        {
            cekBooking();
            refresh_kamar();
        }

        public void cekBooking()
        {
            ////btnBooking.Text = (checkoutDate.Value.Date - checkinDate.Value.Date).ToString();//.ToString();
            //string query = "select kamar_no from Reservasi"; //where (@chIn <= checkin and @chOt >= checkin) or (@chIn <= checkout and @chOt >= checkout) ";
            //////Form3.
            //conn.Open();
            //cmd = new SqlCommand(query, conn);
            ////cmd.Parameters.AddWithValue("@chIn", checkinDate.Value.Date);
            ////cmd.Parameters.AddWithValue("@chOt", checkoutDate.Value.Date);
            //reader = cmd.ExecuteReader();
            //conn.Close();   
        }

        private void panelDataTamu_Paint(object sender, PaintEventArgs e)
        {

        }

        static string switchHari(int hari)
        {
            switch (hari)
            {
                case 0: return "Minggu"; break;
                case 1: return "Senin"; break;
                case 2: return "Selasa"; break;
                case 3: return "Rabu"; break;
                case 4: return "Kamis"; break;
                case 5: return "Jum'at"; break;
                default: return "Sabtu"; break;                
            }            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            statusTime.Text = "Hari " + 
                switchHari((int)DateTime.Now.DayOfWeek)
                    + " Tanggal " + 
                DateTime.Now.ToString("d-M-yyyy") + " Jam " + DateTime.Now.ToString("hh:mm");
            //Tanggal ##-##-#### Jam ##:##
        }

        private void groupBukuTamu_Paint(object sender, PaintEventArgs e)
        {
            groupBukuTamu.Visible = false;
        }

        private void btnCariTamu_Click(object sender, EventArgs e)
        {
            groupBukuTamu.Visible = true;
            groupBukuTamu.Height = 500;
            //groupBukuTamu.Dock = DockStyle.Top;
            groupBukuTamu.BringToFront();
            //panelDataTamu.Enabled = false;
            btnKonfirmasiBooking.Enabled = false;
        }

        private void groupBukuTamu_Leave(object sender, EventArgs e)
        {
            
        }

        private void panelDataTamu_Enter(object sender, EventArgs e)
        {
            
        }

        private void panelDataTamu_MouseClick(object sender, MouseEventArgs e)
        {
            groupBukuTamu.Visible = false;
            groupBukuTamu.SendToBack();
            btnKonfirmasiBooking.Enabled = true;
        }

        private void dataGridView3_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            ////if (groupBukuTamu.Visible == true)
            ////{
            //    inputNamaTamu.Text = datagridTamu[1, e.RowIndex].Value.ToString();
            ////}
            //groupBukuTamu.Visible = false;
            //groupBukuTamu.SendToBack();
            
            //    btnKonfirmasiBooking.Enabled = true;
        }

        private void KosongkanInput()
        {
            Action<Control.ControlCollection> func = null;

            func = (controls) =>
            {
                foreach (Control control in controls)
                    if (control is TextBox)
                        (control as TextBox).Clear();
                    else
                        func(control.Controls);
            };

            func(Controls);
        }

        private void btnBooking_Click(object sender, EventArgs e)
        {
             
            refresh_kamar();
            panelCheckinDate.Visible = true;
            panelCheckoutDate.Visible = true;
            panelKamar.BringToFront();
            panelPengaturanKamar.SendToBack();
            panelKamarDibooking.Controls.Clear();
            flowLayoutPanel1.Visible = false;
            groupBukuTamu.SendToBack();
            //panelDataTamu.Enabled = true;
            groupBukuTamu.Refresh();
            panelDataTamu.Refresh();
            groupBukuTamu.Visible = false;
            //groupBox2.Refresh();
            //inputNamaTamu.Text = "";
            //groupBukuTamu.Invalidate();
            //groupBukuTamu.Update();
            //groupBukuTamu.Refresh();
            //Application.DoEvents();
            KosongkanInput();
            hidepanelPengaturanKamar();
        }

        private void datagridTamu_SelectionChanged(object sender, EventArgs e)
        {

        }

        private void datagridTamu_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //if (groupBukuTamu.Visible == true)
            //{
            inputNamaTamu.Text = datagridTamu[1, e.RowIndex].Value.ToString();
            inputEmail.Text = datagridTamu[5, e.RowIndex].Value.ToString();
            inputAlamat.Text = datagridTamu[2, e.RowIndex].Value.ToString();
            inputKota.Text = datagridTamu[3, e.RowIndex].Value.ToString();
            //irwan tambahkan
            dataCustomer = Int32.Parse(datagridTamu[0, e.RowIndex].Value.ToString());
            //end irwan
            //}
            groupBukuTamu.Visible = false;
            groupBukuTamu.SendToBack();

            btnKonfirmasiBooking.Enabled = true;

        }
        //irwan tambahkan
        private void isCombobox3()
        {
            ComboboxItem item = new ComboboxItem();
          //  MessageBox.Show("A");
            int tahunTampil = 2008;
            for (int i = 0; i < 10; i++)
            {
                item = new ComboboxItem();
                item.Text = tahunTampil.ToString();
                item.Value = tahunTampil;
                comboBox3.Items.Add(item);

                tahunTampil = tahunTampil + 1;
            }
            comboBox3.Text = comboBox3.Items[0].ToString();
        
        }
        private void isiCombobox()
        {

            ComboboxItem item = new ComboboxItem();

            item.Text = "Januari";
            item.Value = 1;
            comboBox2.Items.Add(item);
            item = new ComboboxItem();
            item.Text = "Februari";
            item.Value = 2;
            comboBox2.Items.Add(item);
            item = new ComboboxItem();
            item.Text = "Maret";
            item.Value = 3;
            comboBox2.Items.Add(item);
            item = new ComboboxItem();
            item.Text = "April";
            item.Value = 4;
            comboBox2.Items.Add(item);
            item = new ComboboxItem();
            item.Text = "Mei";
            item.Value = 5;
            comboBox2.Items.Add(item);
            item = new ComboboxItem();
            item.Text = "Juni";
            item.Value = 6;
            comboBox2.Items.Add(item);
            item = new ComboboxItem();
            item.Text = "Juli";
            item.Value = 7;
            comboBox2.Items.Add(item);
            item = new ComboboxItem();
            item.Text = "Agustus";
            item.Value = 8;
            comboBox2.Items.Add(item);
            item = new ComboboxItem();
            item.Text = "September";
            item.Value = 9;
            comboBox2.Items.Add(item);
            item = new ComboboxItem();
            item.Text = "Oktober";
            item.Value = 10;
            comboBox2.Items.Add(item);
            item = new ComboboxItem();
            item.Text = "November";
            item.Value = 11;
            comboBox2.Items.Add(item);
            item = new ComboboxItem();
            item.Text = "Desember";
            item.Value = 12;
            comboBox2.Items.Add(item);
            comboBox2.Text = comboBox2.Items[0].ToString();
        }
        DataTable dt;
        //private void setLoad(int bulan, int tahun)
        //{
        //    TglBulan = bulan;
        //    Tgltahun = tahun;
        //    dt = new DataTable();
        //    SqlCommand sql = new SqlCommand("select tanggal from Tanggal where bulan = @bln and tahun = @thn", koneksi.KoneksiDB());
        //    sql.Parameters.AddWithValue("@bln", bulan);
        //    sql.Parameters.AddWithValue("@thn", tahun);

        //    dataGridView3.DataSource = dt;
        //    dataGridView3.AllowUserToAddRows = false;
        //    dataGridView3.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

        //    dt.Columns.Add("NO_KAMAR".ToString());
        //    reader = sql.ExecuteReader();
        //    while (reader.Read())
        //    {
        //        dt.Columns.Add(reader["tanggal"].ToString());
        //    }

        //    dataGridView3.DataSource = dt;
        //    koneksi.KoneksiDB().Close();

        //    sql = new SqlCommand("select kamar_no from Kamar", koneksi.KoneksiDB());
        //    reader = sql.ExecuteReader();
        //    while (reader.Read())
        //    {
        //        DataRow dr = dt.NewRow();
        //        dr["NO_Kamar"] = reader["kamar_no"].ToString();
        //        dt.Rows.Add(dr);

        //    }
        //    koneksi.KoneksiDB().Close();
        //    dataGridView3.Columns[0].Width = 50;
        //    DateTime tanggalPesan;

        //    ComboboxItem selectedCar = (ComboboxItem)comboBox2.SelectedItem;

        //    for (int i = 1; i <= dataGridView3.ColumnCount-1; i++)
        //    {
        //        dataGridView3.Columns[i].Width = 20;            
        //        tanggalPesan = Convert.ToDateTime(bulan + "/" + dataGridView3.Columns[i].Name.ToString() + "/" + tahun);
        //        sql = new SqlCommand("select kamar_no, status from Reservasi where checkin <@id and checkout >= @id and (status='booking' or status='checkin')", koneksi.KoneksiDB());
        //        sql.Parameters.AddWithValue("@id", tanggalPesan);
        //        reader = sql.ExecuteReader();
        //        while (reader.Read())
        //        {
        //            int nilai = 0;
        //            foreach (DataGridViewRow row in this.dataGridView3.Rows)
        //            {
        //                if (row.Cells[0].Value.ToString().Equals(reader["kamar_no"].ToString()) && reader["status"].ToString().Equals("booking"))
        //                {
        //                    dataGridView3.Rows[nilai].Cells[i-1].Style.BackColor = Color.Red;
        //                }
        //                else if (row.Cells[0].Value.ToString().Equals(reader["kamar_no"].ToString()) && reader["status"].ToString().Equals("checkin"))
        //                {
        //                    dataGridView3.Rows[nilai].Cells[i-1].Style.BackColor = Color.Green;

        //                }
        //                nilai += 1;
        //            }
        //        }
        //        koneksi.KoneksiDB().Close();
        //    }


        //}


        private void setLoad(int bulan, int tahun)
        {
            TglBulan = bulan;
            Tgltahun = tahun;

            /*string select = "SELECT * FROM reservasi";
            //Connection c = new Connection();
            SqlDataAdapter dataAdapter = new SqlDataAdapter(select, koneksi.KoneksiDB()); //c.con is the connection string

            SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dataAdapter);
            DataSet ds = new DataSet();
            dataAdapter.Fill(ds);
            dataGridView3.ReadOnly = true;
            dataGridView3.DataSource = ds.table[0];
            */
            //            string connectionString = "Data Source=.;Initial Catalog=pubs;Integrated Security=True";
            string sql = @"select
            *
            from
            (
	            select
	            combine.kamar_no no,DATEPART(dd,tanggal_id) tanggal,case when status = 'checkin' then 'IN' when status = 'booking' then 'BOK' else '' end status
	            from
	            (
		            select 
		            kamar_no,tanggal_id
		            from 
		            Tanggal t
		            cross join 
		            Kamar
		            WHERE 
		            bulan  = " + DateTime.Now.Month + @" and tahun = " + DateTime.Now.Year + @"
	            )combine
	            left join 
	            (
		            select 
		            booking_id,kamar_no,DATEADD(dd,hari,checkin) tanggal, status
		            from
		            (
			            select
			            ROW_NUMBER() over(partition by reservasi_id,kamar_no order by checkin,tamu_id)-1 hari
			            ,booking_id,kamar_no,checkin,r.status
			            from 
			            Reservasi r
			            join master..spt_values v on v.type='P'
			            and v.number between 1 
			            and datediff(dd, checkin, checkout)
			            where
			            (year(r.checkin) = " + DateTime.Now.Year + @" and month(r.checkin) = " + DateTime.Now.Month + @")
			            or
			            (year(r.checkout) = " + DateTime.Now.Year + @" and month(r.checkout) = " + DateTime.Now.Month + @")
		            )a
	            )a
	            on 
	            combine.kamar_no = a.kamar_no
	            and
	            combine.tanggal_id = a.tanggal
            )s
            pivot
            (
	            max(status) for tanggal in ([1],[2],[3],[4],[5],[6],[7],[8],[9],[10],[11],[12],[13],[14],[15],[16],[17],[18],[19],[20],[21],[22],[23],[24],[25],[26],[27],[28],[29],[30],[31])
            )as piv
            order by no
            ";
            //sql.Parameters.AddWithValue("@bln", bulan);
            //sql.Parameters.AddWithValue("@thn", tahun);

            //SqlConnection connection = new SqlConnection(connectionString);
            SqlDataAdapter dataadapter = new SqlDataAdapter(sql, koneksi.KoneksiDB());
            DataSet ds = new DataSet();
            //connection.Open();
            dataadapter.Fill(ds, "reservasi");
            //connection.Close();
            koneksi.KoneksiDB().Close();
            dataGridView3.DataSource = ds;
            dataGridView3.DataMember = "reservasi";
            int row = 0;
            foreach (DataGridViewRow rw in this.dataGridView3.Rows)
            {
                //row++;
                for (int i = 0; i < rw.Cells.Count; i++)
                {
                    if (Convert.ToString(rw.Cells[i].Value) != string.Empty && i > 0 && Convert.ToString(rw.Cells[i].Value) == "BOK")

                    //if (dataGridView3.Rows[row].Cells[i].Value.ToString().Length >= 1)//rw.Cells[i].Value != null || rw.Cells[i].Value != DBNull.Value )
                    {
                        //rw.Cells[i].Style.BackColor = Color.Red;
                        dataGridView3.Rows[row].Cells[i].Style.BackColor = Color.Red;
                    }

                    if (Convert.ToString(rw.Cells[i].Value) != string.Empty && i > 0 && Convert.ToString(rw.Cells[i].Value) == "IN")

                    //if (dataGridView3.Rows[row].Cells[i].Value.ToString().Length >= 1)//rw.Cells[i].Value != null || rw.Cells[i].Value != DBNull.Value )
                    {
                        //rw.Cells[i].Style.BackColor = Color.Red;
                        dataGridView3.Rows[row].Cells[i].Style.BackColor = Color.Green;
                    }
                    //dataGridView3.Rows[row].Cells[i].Value = DBNull;
                } row++;
            }
            //for (int i = 1; i <= dataGridView3.ColumnCount - 1; i++)
            //{
            //    dataGridView3.Columns[i].Width = 20;
            //}
            dataGridView3.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader;
            dataGridView3.ReadOnly = true;
        }

        private void dataGridView3_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {

            try
            {
                DateTime tanggalPesan1;
                ComboboxItem selectedCar = (ComboboxItem)comboBox2.SelectedItem;
                int NoKamarInfo;
                tanggalPesan1 = Convert.ToDateTime(TglBulan + "/" + dataGridView3.Columns[e.ColumnIndex].Name.ToString() + "/" + Tgltahun);

                if (dataGridView3.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor == Color.Red || dataGridView3.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor == Color.Green)
                {
                    NoKamarInfo = Int32.Parse(dataGridView3.Rows[e.RowIndex].Cells[0].Value.ToString());
                    SqlCommand sqlq = new SqlCommand("select Tamu.tamu, Reservasi.checkin, Reservasi.checkout from Reservasi, Tamu where Tamu.tamu_id = Reservasi.tamu_id and Reservasi.checkin <=@id and Reservasi.checkout >= @id and Reservasi.kamar_no=@nok and (Reservasi.status='booking' or Reservasi.status='checkin')", koneksi.KoneksiDB());
                    sqlq.Parameters.AddWithValue("@id", tanggalPesan1);
                    sqlq.Parameters.AddWithValue("@nok", NoKamarInfo);
                    reader = sqlq.ExecuteReader();

                    while (reader.Read())
                    {

                        var cell = dataGridView3.CurrentCell;
                        var cellDisplayRect = dataGridView3.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
                        //reader.GetString(0), reader.GetDateTime(1).ToString("dd/MMM/yyyy"), reader.GetDateTime(2).ToString("dd/MMM/yyyy")
                        toolTip1.Show("----------------------------------------------------------------------------\r\n" +
                "                        Dipesan oleh " + reader.GetString(0) + "\r\n" +
                "----------------------------------------------------------------------------\r\n" +
                "Kamar " + NoKamarInfo.ToString() + " Checkin " + reader.GetDateTime(1).ToString("dd/MMM/yyyy") + " Checkout " + reader.GetDateTime(2).ToString("dd/MMM/yyyy") + "\r\n" +
                "----------------------------------------------------------------------------\r\n"
                ,
                        dataGridView3,
                        cellDisplayRect.X + cell.Size.Width / 2,
                        cellDisplayRect.Y + cell.Size.Height / 2,
                        5000);

                        dataGridView3.ShowCellToolTips = false;
                    }
                    koneksi.KoneksiDB().Close();
                }
                else
                {
                    //hide
                    toolTip1.Hide(this);
                }

            }
            catch
            {
            }
        
        }

        private void dataGridView3_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            // MessageBox.Show(e.ColumnIndex.ToString()+ " " + e.RowIndex.ToString());
            if (dataGridView3.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor == Color.Red)
            {
                contextMenuStrip1.Show(Cursor.Position);
                rowSelect = e.RowIndex;
                columnSelect = e.ColumnIndex;
            }
        }


        private void btnKonfirmasiBooking_Click(object sender, EventArgs e)
        {
            SqlCommand sql;
            if (dataCustomer < 1)
            {
                sql = new SqlCommand("insert into Tamu(tamu,alamat,kota,telepon,email,perusahaan,tanggallahir,sebutan,gelar) values (@a,@b,@c,@d,@e,@f,@g,@h,@i)", koneksi.KoneksiDB());
                sql.Parameters.AddWithValue("@a", inputNamaTamu.Text);
                sql.Parameters.AddWithValue("@b", inputAlamat.Text);
                sql.Parameters.AddWithValue("@c", inputKota.Text);
                sql.Parameters.AddWithValue("@d", inputTelepon.Text);
                sql.Parameters.AddWithValue("@e", inputEmail.Text);
                sql.Parameters.AddWithValue("@f", textBox2.Text);
                sql.Parameters.AddWithValue("@g", inputUlangTahun.Value);
                sql.Parameters.AddWithValue("@h", inputSebutan.Text);
                sql.Parameters.AddWithValue("@i", inputGelar.Text);

                sql.ExecuteNonQuery();
                koneksi.KoneksiDB().Close();

                sql = new SqlCommand("select max(tamu_id) from Tamu", koneksi.KoneksiDB());
                dataCustomer = Int32.Parse(sql.ExecuteScalar().ToString());
                koneksi.KoneksiDB().Close();
                //btnKonfirmasiBooking.Text = "Booking Telah Dilakukan";
                //btnKonfirmasiBooking.Enabled = false;
            }


            sql = new SqlCommand("insert into Booking(tamu_id, tgl_booking, checkin, checkout, uang_muka, tag_kamar,tag_restoran,tag_transport,status,grand_total,payment,balance_due,note,booking_diskon_id,staff_id) values (@a,@b,@c,@d,@e,@f,@g,@h,@i,@j,@k,@l,@m,@n,@o)", koneksi.KoneksiDB());
            sql.Parameters.AddWithValue("@a", dataCustomer);
            sql.Parameters.AddWithValue("@b", DateTime.Now.Date);
            sql.Parameters.AddWithValue("@c", DateTime.Now.Date);
            sql.Parameters.AddWithValue("@d", DateTime.Now.Date);
            sql.Parameters.AddWithValue("@e", 0);
            sql.Parameters.AddWithValue("@f", totalBiaya);
            sql.Parameters.AddWithValue("@g", 0);
            sql.Parameters.AddWithValue("@h", 0);
            sql.Parameters.AddWithValue("@i", "NO");
            sql.Parameters.AddWithValue("@j", totalBiaya);
            sql.Parameters.AddWithValue("@k", 1);
            sql.Parameters.AddWithValue("@l", totalBiaya - Int32.Parse(inputPembayaran.Text));
            sql.Parameters.AddWithValue("@m", "");
            sql.Parameters.AddWithValue("@n", 1);
            sql.Parameters.AddWithValue("@o", 1);
            sql.ExecuteNonQuery();
            koneksi.KoneksiDB().Close();


            sql = new SqlCommand("select max(booking_id) from Booking", koneksi.KoneksiDB());
            int nilaimax = Int32.Parse(sql.ExecuteScalar().ToString());

            koneksi.KoneksiDB().Close();
           
            //simpan reservasi
            List<DataRow> rd = new List<DataRow>();
            foreach (DataRow dr in dKamarPesan.Rows)
            {
                sql = new SqlCommand("insert into Reservasi(booking_id, checkin, checkout, tamu_id, kamar_no, tag_kamar,tag_restoran,tag_transport,harga_id,status,downpayment) values (@a,@b,@c,@d,@e,@f,@g,@h,@i,'booking',@j)", koneksi.KoneksiDB());
                sql.Parameters.AddWithValue("@a", nilaimax);
                sql.Parameters.AddWithValue("@b", dr["Checkin"]);
                sql.Parameters.AddWithValue("@c", dr["Checkout"]);
                sql.Parameters.AddWithValue("@d", dataCustomer);
                sql.Parameters.AddWithValue("@e", dr["NO Kamar"]);
                sql.Parameters.AddWithValue("@f", dr["Harga"]);
                sql.Parameters.AddWithValue("@g", 0);
                sql.Parameters.AddWithValue("@h", 0);
                sql.Parameters.AddWithValue("@i", 1);
                if (comboBox4.Text.Equals(dr["NO Kamar"].ToString()))
                {
                    sql.Parameters.AddWithValue("@j", Int32.Parse(inputPembayaran.Text));
                }
                else
                {
                    sql.Parameters.AddWithValue("@j", 0);
                }

                sql.ExecuteNonQuery();
                koneksi.KoneksiDB().Close();
            }
            dKamarPesan.Clear();
            comboBox4.Items.Clear();
            dataCustomer = 0;

            btnKonfirmasiBooking.Text = "Booking Telah Disimpan";
            btnKonfirmasiBooking.Enabled = false;
        }

        private void comboBox2_SelectedValueChanged(object sender, EventArgs e)
        {
        
        }

        private void comboBox3_SelectedValueChanged(object sender, EventArgs e)
        {
        
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
        
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
        
        }

        private void comboBox3_TextChanged(object sender, EventArgs e)
        {
            if (cekPilih == true)
            {
                ComboboxItem selectedCar = (ComboboxItem)comboBox2.SelectedItem;
                setLoad(Convert.ToInt32(selectedCar.Value), Int32.Parse(comboBox3.Text));
            }
        }

        private void comboBox2_TextChanged(object sender, EventArgs e)
        {
            if (cekPilih == true)
            {
                ComboboxItem selectedCar = (ComboboxItem)comboBox2.SelectedItem;
                setLoad(Convert.ToInt32(selectedCar.Value), Int32.Parse(comboBox3.Text));
                
            }
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }

        private void checkInToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //string nOKamar = dataGridView3.Rows[rowSelect].Cells[0].Value.ToString();
            //MessageBox.Show(nOKamar);
            //Boolean cekKiri=false;
            //int kiriGanti=columnSelect;

            //while(!cekKiri){
            //    if (dataGridView3.Rows[rowSelect].Cells[kiriGanti].Style.BackColor == Color.Red)
            //    {
            //        dataGridView3.Rows[rowSelect].Cells[kiriGanti].Style.BackColor = Color.Green;
            //        kiriGanti = kiriGanti - 1;
            //    }
            //    else
            //    {
            //        cekKiri = true;   
            //    }
            //}
            //cekKiri = false;
            //kiriGanti = columnSelect+1;
            //while (!cekKiri)
            //{
            //    if (dataGridView3.Rows[rowSelect].Cells[kiriGanti].Style.BackColor == Color.Red)
            //    {
            //        dataGridView3.Rows[rowSelect].Cells[kiriGanti].Style.BackColor = Color.Green;
            //        kiriGanti = kiriGanti + 1;
            //    }
            //    else
            //    {
            //        cekKiri = true;
            //    }
            //}
            DataTamuKalender.Visible = true;
            SqlDataAdapter da = new SqlDataAdapter("select tamu_id, tamu, alamat, kota, telepon from Tamu", koneksi.KoneksiDB());
            DataTable dset = new DataTable();
            da.Fill(dset);
            dataGridView6.DataSource = dset;
            koneksi.KoneksiDB().Close();
        }

        private void btnDaftarTamu_Click_1(object sender, EventArgs e)
        {
            panelPengaturanKamar.SendToBack();
        }
        DataTable dtPesan = new DataTable();
        
        private void MunculKan(object sender, EventArgs e)
        {

            Button btn = sender as Button;
            contextMenuStrip2.Show(Cursor.Position);
            dataKamarCh = Int32.Parse(btn.Text);
            SqlCommand sql = new SqlCommand("select reservasi_id from Reservasi where kamar_no =@noKamar and @tnggal>=checkin and @tnggal <= checkout and status='checkin' ", koneksi.KoneksiDB());
            sql.Parameters.AddWithValue("@noKamar", Int32.Parse(btn.Text));
            sql.Parameters.AddWithValue("@tnggal", DateTime.Now.Date);
            string idReservasi = "-";

            try
            {
                idReservasi = sql.ExecuteScalar().ToString();
            }
            catch
            {
                idReservasi = "-";
            }
            koneksi.KoneksiDB().Close();
            label22.Text = idReservasi;
            dtPesan = new DataTable();

            dataGridView4.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dtPesan.Columns.Add("ID_ITEM".ToString());
            dtPesan.Columns.Add("RESERVASI_ID".ToString());
            dtPesan.Columns.Add("TANGGAL", typeof(DateTime));
            dtPesan.Columns.Add("HARGA".ToString());
            dataGridView4.DataSource = dtPesan;
        
        }
        private void btnPesan_Click(object sender, EventArgs e)
        {
             
            panelKamarDibooking.Controls.Clear();
            hidepanelPengaturanKamar();
            PanelPesan.BringToFront();
            PanelPesan.Controls.Clear();
            SqlCommand cmd = new SqlCommand((@"select count(*) from Kamar, Reservasi where Kamar.kamar_no = Reservasi.kamar_no and Reservasi.status = 'checkin'"), koneksi.KoneksiDB());

            int jumKamar = (int)cmd.ExecuteScalar();

            ///button1.Text = jumKamar.ToString();
            Button[] Kamar;

            //command.Parameters.AddWithValue("@Username", username);
            //command.Parameters.AddWithValue("@Password", password);

            cmd = new SqlCommand(
            (@"
            select Kamar.kamar_no 
            from Kamar, Reservasi 
            where Kamar.kamar_no = Reservasi.kamar_no 
            and Reservasi.status = 'checkin'
            order by kamar.kamar_no
            "), koneksi.KoneksiDB());

            /*
             
             cmd = new SqlCommand(
            (@"select
            k.kamar_no,
            k.kamar_tipe_id,
            case when DATENAME(dw,tanggal_id) in ('Saturday','Sunday') then harga_weekend else harga end harga
            from            
            Kamar k
            inner join kamar_tipe kt on k.kamar_tipe_id = kt.kamar_tipe_id 
            inner join harga h on h.tanggal_id = '2008-7-1'
            and kt.kamar_tipe_id = h.kamar_tipe_id"), koneksi.KoneksiDB());
            //cmd.Parameters.AddWithValue("@checkindate",checkinDate.Value.ToString("yyyy-M-d"));
            //cmd.Parameters.AddWithValue("@checkoutdate",checkoutDate.Value.ToString("yyyy-M-d"));
             */


            reader = cmd.ExecuteReader();
            Kamar = new Button[jumKamar];
            x = 0;
            while (reader.Read())
            {
                Kamar[x] = new Button();
                Kamar[x].Text = reader.GetInt32(0).ToString();
                Kamar[x].Name = reader.GetInt32(0).ToString();
                Kamar[x].Visible = true;
                Kamar[x].Height = 35;
                //Kamar[x].Tag = 0;
                //Kamar[x].BackColor = Color.FromName(reader.GetString(1));
                Kamar[x].Click += new EventHandler(MunculKan);
                //Kamar[x].MouseEnter += new EventHandler(button1_MouseEnter_2);
                //Kamar[x].MouseLeave += new EventHandler(button1_MouseLeave_1);

                PanelPesan.Controls.Add(Kamar[x]);
                x += 1;
                //Kamar[x].MouseEnter += button1_MouseEnter_2;// Kamar_Tips;//new EventHandler(Kamar_Tips);

            }
            //conn.Close();
            koneksi.KoneksiDB().Close();
        }

        private void contextMenuStrip2_Opening(object sender, CancelEventArgs e)
        {

        }

        private void pesanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panelPesanItem.BringToFront();
        }

        private void lblClose_Click(object sender, EventArgs e)
        {
            PanelPesan.BringToFront();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        //private void label21_Click(object sender, EventArgs e)
        //{
        //    panel2.BringToFront();
        //    SqlDataAdapter da = new SqlDataAdapter("select Item.item_id, Item.item, Item_Tipe.item_tipe, Item.harga from Item, Item_Tipe where Item.item_tipe_id = Item_Tipe.item_tipe_id", koneksi.KoneksiDB());
        //    DataTable ds = new DataTable();
        //    da.Fill(ds);
        //    dataGridView5.DataSource = ds;
        //    koneksi.KoneksiDB().Close();
        //}

        //private void label26_Click(object sender, EventArgs e)
        //{

        //    SqlDataAdapter da = new SqlDataAdapter("select Item.item_id, Item.item, Item_Tipe.item_tipe, Item.harga from Item, Item_Tipe where Item.item_tipe_id = Item_Tipe.item_tipe_id and Item.item like @nama ", koneksi.KoneksiDB());
        //    da.SelectCommand.Parameters.Add(new SqlParameter("@nama", string.Format("%{0}%", TxtCust.Text)));
        //    DataTable ds = new DataTable();
        //    da.Fill(ds);
        //    dataGridView5.DataSource = ds;
        //    koneksi.KoneksiDB().Close();
        //}

        //private void label25_Click(object sender, EventArgs e)
        //{
        //    panel1.BringToFront();
        //}

        private void dataGridView5_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
           xPenang.Text = dataGridView5.Rows[dataGridView5.CurrentRow.Index].Cells[0].Value.ToString();
           label15.Text = dataGridView5.Rows[dataGridView5.CurrentRow.Index].Cells[1].Value.ToString();
           label3.Text = dataGridView5.Rows[dataGridView5.CurrentRow.Index].Cells[3].Value.ToString();
           panelPesanItem.BringToFront();
        }

        private void dataGridView4_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label20_Click(object sender, EventArgs e)
        {
            DataRow dr = dtPesan.NewRow();
            dr["ID_ITEM"] = xPenang.Text;
            dr["RESERVASI_ID"] = label22.Text;
            dr["TANGGAL"] = DateTime.Now.Date;
            dr["HARGA"] = label3.Text;
            dtPesan.Rows.Add(dr);
            dataGridView1.DataSource = dtPesan;
        }

        private void lblClear_Click(object sender, EventArgs e)
        {
            dtPesan.Clear();
        }

        private void lblRemove_Click(object sender, EventArgs e)
        {
            List<DataRow> rd = new List<DataRow>();
            int index = 0;
            foreach (DataRow dr in dtPesan.Rows)
            {
                if (index == dataGridView4.CurrentRow.Index)
                {
                    rd.Add(dr);
                }

                index += 1;
            }

            foreach (var r in rd)
            {
                dtPesan.Rows.Remove(r);
            }
            dtPesan.AcceptChanges();
            dataGridView4.DataSource = dtPesan;

        }

        private void label19_Click(object sender, EventArgs e)
        {
            List<DataRow> rd = new List<DataRow>();
            foreach (DataRow dr in dtPesan.Rows)
            {

                SqlCommand sql = new SqlCommand("insert into Pemesanan(item_id, reservasi_id, tgl_pemesanan, harga) values (@a,@b,@c,@d)", koneksi.KoneksiDB());
                sql.Parameters.AddWithValue("@a", dr["ID_ITEM"]);
                sql.Parameters.AddWithValue("@b", dr["RESERVASI_ID"]);
                sql.Parameters.AddWithValue("@c", dr["TANGGAL"]);
                sql.Parameters.AddWithValue("@d", dr["HARGA"]);

                sql.ExecuteNonQuery();
                koneksi.KoneksiDB().Close();

                sql = new SqlCommand("select booking_id from Reservasi where reservasi_id = @id", koneksi.KoneksiDB());
                sql.Parameters.AddWithValue("@id", dr["RESERVASI_ID"]);
                string databooking = (sql.ExecuteScalar().ToString());

                koneksi.KoneksiDB().Close();

                sql = new SqlCommand("update Booking set grand_total=grand_total+@biaya, balance_due = balance_due+@biaya  where booking_id=@id", koneksi.KoneksiDB());
                sql.Parameters.AddWithValue("@biaya", dr["HARGA"]);
                sql.Parameters.AddWithValue("@id", Int32.Parse(databooking));

                sql.ExecuteNonQuery();
                koneksi.KoneksiDB().Close();
                sql = new SqlCommand("update Reservasi set tag_restoran=tag_restoran+@biaya where booking_id=@id and reservasi_id=@idr", koneksi.KoneksiDB());
                sql.Parameters.AddWithValue("@biaya", dr["HARGA"]);
                sql.Parameters.AddWithValue("@idr", dr["RESERVASI_ID"]);
                sql.Parameters.AddWithValue("@id", Int32.Parse(databooking));

                sql.ExecuteNonQuery();
            koneksi.KoneksiDB().Close();
            }
            dtPesan.Clear();
            label22.Text = "-";
            label3.Text = "-";
            label15.Text = "-";
            xPenang.Text = "";
            PanelPesan.BringToFront();
            
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void dataGridView3_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            
        }

        private void btnPengaturanKamar_Click(object sender, EventArgs e)
        {
             
            panelKamarDibooking.Controls.Clear();
            DataKamar dataKamar = new DataKamar();
            dataKamar.TopLevel = false;
            dataKamar.Name = "panelPengaturanKamarInnerForm";
            //panelPengaturanKamar.BringToFront();
            //splitContainer2.Panel1.Controls.Clear();
            splitContainer2.Panel1.Controls.Add(dataKamar);
            dataKamar.Show();
            dataKamar.Dock = DockStyle.Fill;
            dataKamar.BringToFront();
            //panelPengaturanKamarInner.Show();
            //panelPengaturanKamarInner.BringToFront();
            //refresh_pengaturankamar();
        }

        private void panelPengaturanKamarInner_Paint(object sender, PaintEventArgs e)
        {
            refresh_pengaturankamar();
        }

        /// <summary>
        //Suhendro Update
        /// </summary>
        /// 

        private void cmbJbtR_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnSAllR_Click(object sender, EventArgs e)
        {

        }

        private void btnSaveR_Click(object sender, EventArgs e)
        {
            string strTemp1 = "";
            string strTemp2 = "";
            string strTemp3 = "";
            string strTemp4 = "";
            string strTemp5 = "";
            string strTemp6 = "";
            string strTemp7 = "";
            string strTemp8 = "";
            string strTemp9 = "";

            if (cmbJbtR.SelectedText == "")
            {
                if (cKamarR.Checked == true)
                {
                    strTemp1 = "On";
                }
                else
                {
                    strTemp1 = "Off";
                }

                if (cBookingR.Checked == true)
                {
                    strTemp2 = "On";
                }
                else
                {
                    strTemp2 = "Off";
                }

                if (cTamuR.Checked == true)
                {
                    strTemp3 = "On";
                }
                else
                {
                    strTemp3 = "Off";
                }

                if (cKalenderR.Checked == true)
                {
                    strTemp4 = "On";
                }
                else
                {
                    strTemp4 = "Off";
                }

                if (cDafBookingR.Checked == true)
                {
                    strTemp5 = "On";
                }
                else
                {
                    strTemp5 = "Off";
                }

                if (cUpdBookR.Checked == true)
                {
                    strTemp6 = "On";
                }
                else
                {
                    strTemp6 = "Off";
                }

                if (cPrintR.Checked == true)
                {
                    strTemp7 = "On";
                }
                else
                {
                    strTemp7 = "Off";
                }

                if (cUserR.Checked == true)
                {
                    strTemp8 = "On";
                }
                else
                {
                    strTemp8 = "Off";
                }

                if (cRights.Checked == true)
                {
                    strTemp9 = "On";
                }
                else
                {
                    strTemp9 = "Off";
                }
                string strJabatan = cmbJbtR.Text;
                //connecting();
                //conn.Open();
                string strQueryIns = "insert into jabatan(Jabatan,Rights_Check_Kamar,Rights_Booking_Kamar,Rights_Data_Tamu," +
                    "Rights_Kalender,Rights_Daftar_Booking,Rights_Update_Booking,Rights_Print_Invoice,Rights_Daftar_User," +
                    "Rights_Rights) values (@jab,@rcheck,@rbook,@rtamu,@rkal,@rdaf,@rupd,@rprint,@ruser,@rrig)";
                SqlCommand cmd = new SqlCommand(strQueryIns, koneksi.KoneksiDB());
                cmd.Parameters.AddWithValue("@jab", strJabatan);
                cmd.Parameters.AddWithValue("@rcheck", strTemp1);
                cmd.Parameters.AddWithValue("@rbook", strTemp2);
                cmd.Parameters.AddWithValue("@rtamu", strTemp3);
                cmd.Parameters.AddWithValue("@rkal", strTemp4);
                cmd.Parameters.AddWithValue("@rdaf", strTemp5);
                cmd.Parameters.AddWithValue("@rupd", strTemp6);
                cmd.Parameters.AddWithValue("@rprint", strTemp7);
                cmd.Parameters.AddWithValue("@ruser", strTemp8);
                cmd.Parameters.AddWithValue("@rrig", strTemp9);
                cmd.ExecuteNonQuery();
                //conn.Close();
                MessageBox.Show("Data Rights Telah Disimpan");
                string strQ = "select * from jabatan";
                createTblNoParam(strQ);
            }
        }

        private void btnUpdR_Click(object sender, EventArgs e)
        {

        }

        private void dgR_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void panelKamarDibooking_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnSaveR_Click_1(object sender, EventArgs e)
        {
            cmbJbtR.Items.Add(cmbJbtR.Text);
            string strTemp1 = "";
            string strTemp2 = "";
            string strTemp3 = "";
            string strTemp4 = "";
            string strTemp5 = "";
            string strTemp6 = "";
            string strTemp7 = "";
            string strTemp8 = "";
            string strTemp9 = "";

            if (cmbJbtR.SelectedText == "")
            {
                if (cKamarR.Checked == true)
                {
                    strTemp1 = "On";
                }
                else
                {
                    strTemp1 = "Off";
                }

                if (cBookingR.Checked == true)
                {
                    strTemp2 = "On";
                }
                else
                {
                    strTemp2 = "Off";
                }

                if (cTamuR.Checked == true)
                {
                    strTemp3 = "On";
                }
                else
                {
                    strTemp3 = "Off";
                }

                if (cKalenderR.Checked == true)
                {
                    strTemp4 = "On";
                }
                else
                {
                    strTemp4 = "Off";
                }

                if (cDafBookingR.Checked == true)
                {
                    strTemp5 = "On";
                }
                else
                {
                    strTemp5 = "Off";
                }

                if (cUpdBookR.Checked == true)
                {
                    strTemp6 = "On";
                }
                else
                {
                    strTemp6 = "Off";
                }

                if (cPrintR.Checked == true)
                {
                    strTemp7 = "On";
                }
                else
                {
                    strTemp7 = "Off";
                }

                if (cUserR.Checked == true)
                {
                    strTemp8 = "On";
                }
                else
                {
                    strTemp8 = "Off";
                }

                if (cRights.Checked == true)
                {
                    strTemp9 = "On";
                }
                else
                {
                    strTemp9 = "Off";
                }
                string strJabatan = cmbJbtR.Text;
                //connecting();
                //conn.Open();
                string strQueryIns = "insert into jabatan(Jabatan,Rights_Check_Kamar,Rights_Booking_Kamar,Rights_Data_Tamu," +
                    "Rights_Kalender,Rights_Daftar_Booking,Rights_Update_Booking,Rights_Print_Invoice,Rights_Daftar_User," +
                    "Rights_Rights) values (@jab,@rcheck,@rbook,@rtamu,@rkal,@rdaf,@rupd,@rprint,@ruser,@rrig)";
                SqlCommand cmd = new SqlCommand(strQueryIns, koneksi.KoneksiDB());
                cmd.Parameters.AddWithValue("@jab", strJabatan);
                cmd.Parameters.AddWithValue("@rcheck", strTemp1);
                cmd.Parameters.AddWithValue("@rbook", strTemp2);
                cmd.Parameters.AddWithValue("@rtamu", strTemp3);
                cmd.Parameters.AddWithValue("@rkal", strTemp4);
                cmd.Parameters.AddWithValue("@rdaf", strTemp5);
                cmd.Parameters.AddWithValue("@rupd", strTemp6);
                cmd.Parameters.AddWithValue("@rprint", strTemp7);
                cmd.Parameters.AddWithValue("@ruser", strTemp8);
                cmd.Parameters.AddWithValue("@rrig", strTemp9);
                cmd.ExecuteNonQuery();
                //conn.Close();
                MessageBox.Show("Data Rights Telah Disimpan");
                string strQ = "select * from jabatan";
                createTblNoParam(strQ);
            }
        }

        private void btnUpdR_Click_1(object sender, EventArgs e)
        {
            string strTemp1 = "";
            string strTemp2 = "";
            string strTemp3 = "";
            string strTemp4 = "";
            string strTemp5 = "";
            string strTemp6 = "";
            string strTemp7 = "";
            string strTemp8 = "";
            string strTemp9 = "";

            if (cKamarR.Checked == true)
            {
                strTemp1 = "On";
            }
            else
            {
                strTemp1 = "Off";
            }

            if (cBookingR.Checked == true)
            {
                strTemp2 = "On";
            }
            else
            {
                strTemp2 = "Off";
            }

            if (cTamuR.Checked == true)
            {
                strTemp3 = "On";
            }
            else
            {
                strTemp3 = "Off";
            }

            if (cKalenderR.Checked == true)
            {
                strTemp4 = "On";
            }
            else
            {
                strTemp4 = "Off";
            }

            if (cDafBookingR.Checked == true)
            {
                strTemp5 = "On";
            }
            else
            {
                strTemp5 = "Off";
            }

            if (cUpdBookR.Checked == true)
            {
                strTemp6 = "On";
            }
            else
            {
                strTemp6 = "Off";
            }

            if (cPrintR.Checked == true)
            {
                strTemp7 = "On";
            }
            else
            {
                strTemp7 = "Off";
            }

            if (cUserR.Checked == true)
            {
                strTemp8 = "On";
            }
            else
            {
                strTemp8 = "Off";
            }

            if (cRights.Checked == true)
            {
                strTemp9 = "On";
            }
            else
            {
                strTemp9 = "Off";
            }

            string strJabatan = cmbJbtR.Text;
            //connecting();
            //conn.Open();
            string strQueryUpd = "update jabatan set Rights_Check_Kamar = @rcheck,Rights_Booking_Kamar = @rbook," +
                "Rights_Data_Tamu = @rtamu,Rights_Kalender = @rkal,Rights_Daftar_Booking = @rdaf,Rights_Update_Booking = @rupd," +
                "Rights_Print_Invoice = @rprint,Rights_Daftar_User = @ruser,Rights_Rights = @rrig where jabatan = @jab";
            SqlCommand cmd = new SqlCommand(strQueryUpd, koneksi.KoneksiDB());
            cmd.Parameters.AddWithValue("@jab", strJabatan);
            cmd.Parameters.AddWithValue("@rcheck", strTemp1);
            cmd.Parameters.AddWithValue("@rbook", strTemp2);
            cmd.Parameters.AddWithValue("@rtamu", strTemp3);
            cmd.Parameters.AddWithValue("@rkal", strTemp4);
            cmd.Parameters.AddWithValue("@rdaf", strTemp5);
            cmd.Parameters.AddWithValue("@rupd", strTemp6);
            cmd.Parameters.AddWithValue("@rprint", strTemp7);
            cmd.Parameters.AddWithValue("@ruser", strTemp8);
            cmd.Parameters.AddWithValue("@rrig", strTemp9);
            cmd.ExecuteNonQuery();
            koneksi.KoneksiDB().Close();
            MessageBox.Show("Data Rights Telah Diupdate");
            string strQ = "select * from jabatan";
            createTblNoParam(strQ);
        }

        private void btnDelR_Click_1(object sender, EventArgs e)
        {
            string strJabatan = cmbJbtR.Text;
            //connecting();
            //conn.Open();
            string strQueryDel = "delete from jabatan where jabatan = @jab";
            SqlCommand cmd = new SqlCommand(strQueryDel, koneksi.KoneksiDB());
            cmd.Parameters.AddWithValue("@jab", strJabatan);
            cmd.ExecuteNonQuery();
            koneksi.KoneksiDB().Close();
            MessageBox.Show("Data Rights Telah Dihapus");
            string strQ = "select * from jabatan";
            createTblNoParam(strQ);
        }

        private void btnSAllR_Click_1(object sender, EventArgs e)
        {
            //connecting();
            //conn.Open();
            string strQ = "select * from jabatan";
            createTblNoParam(strQ);
        }

        private void dgR_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            // boxId = dgR.Rows[dgR.CurrentRow.Index].Cells[0].Value.GetType(Int32());
            boxJbt = dgR.Rows[dgR.CurrentRow.Index].Cells[1].Value.ToString();
            boxCheck = dgR.Rows[dgR.CurrentRow.Index].Cells[2].Value.ToString();
            boxBook = dgR.Rows[dgR.CurrentRow.Index].Cells[3].Value.ToString();
            boxTamu = dgR.Rows[dgR.CurrentRow.Index].Cells[4].Value.ToString();
            boxKalender = dgR.Rows[dgR.CurrentRow.Index].Cells[5].Value.ToString();
            boxDafBook = dgR.Rows[dgR.CurrentRow.Index].Cells[6].Value.ToString();
            boxUpdBook = dgR.Rows[dgR.CurrentRow.Index].Cells[7].Value.ToString();
            boxPrint = dgR.Rows[dgR.CurrentRow.Index].Cells[8].Value.ToString();
            boxUser = dgR.Rows[dgR.CurrentRow.Index].Cells[9].Value.ToString();
            boxRights = dgR.Rows[dgR.CurrentRow.Index].Cells[10].Value.ToString();

            cmbJbtR.Refresh();
            cmbJbtR.Text = "";
            cmbJbtR.SelectedText = boxJbt;
            if (boxCheck == "On")
            {
                cKamarR.Checked = true;
            }
            else
            {
                cKamarR.Checked = false;
            }

            if (boxBook == "On")
            {
                cBookingR.Checked = true;
            }
            else
            {
                cBookingR.Checked = false;
            }

            if (boxTamu == "On")
            {
                cTamuR.Checked = true;
            }
            else
            {
                cTamuR.Checked = false;
            }

            if (boxKalender == "On")
            {
                cKalenderR.Checked = true;
            }
            else
            {
                cKalenderR.Checked = false;
            }

            if (boxDafBook == "On")
            {
                cDafBookingR.Checked = true;
            }
            else
            {
                cDafBookingR.Checked = false;
            }

            if (boxUpdBook == "On")
            {
                cUpdBookR.Checked = true;
            }
            else
            {
                cUpdBookR.Checked = false;
            }

            if (boxPrint == "On")
            {
                cPrintR.Checked = true;
            }
            else
            {
                cPrintR.Checked = false;
            }

            if (boxUser == "On")
            {
                cUserR.Checked = true;
            }
            else
            {
                cUserR.Checked = false;
            }

            if (boxRights == "On")
            {
                cRights.Checked = true;
            }
            else
            {
                cRights.Checked = false;
            }
        }

        private void cmbJbtR_SelectedIndexChanged_1(object sender, EventArgs e)
        {


            string tCheck = "";
            string tBook = "";
            string tTamu = "";
            string tKalender = "";
            string tDafBook = "";
            string tUpdBook = "";
            string tPrint = "";
            string tUser = "";
            string tRights = "";


            //connecting();
            //conn.Open();
            string strQ = "select * from Jabatan where jabatan = @nm";
            cmd = new SqlCommand(strQ, koneksi.KoneksiDB());
            cmd.Parameters.AddWithValue("@nm", cmbJbtR.SelectedItem.ToString());
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                tCheck = reader.GetString(2);
                tBook = reader.GetString(3);
                tTamu = reader.GetString(4);
                tKalender = reader.GetString(5);
                tDafBook = reader.GetString(6);
                tUpdBook = reader.GetString(7);
                tPrint = reader.GetString(8);
                tUser = reader.GetString(9);
                tRights = reader.GetString(10);
                //a++;
            }
            koneksi.KoneksiDB().Close();

            if (tCheck.Equals("On"))
            {
                cKamarR.Checked = true;
            }
            else
            {
                cKamarR.Checked = false;
            }

            if (tBook.Equals("On"))
            {
                cBookingR.Checked = true;
            }
            else
            {
                cBookingR.Checked = false;
            }

            if (tTamu.Equals("On"))
            {
                cTamuR.Checked = true;
            }
            else
            {
                cTamuR.Checked = false;
            }

            if (tKalender.Equals("On"))
            {
                cKalenderR.Checked = true;
            }
            else
            {
                cKalenderR.Checked = false;
            }

            if (tDafBook.Equals("On"))
            {
                cDafBookingR.Checked = true;
            }
            else
            {
                cDafBookingR.Checked = false;
            }

            if (tUpdBook.Equals("On"))
            {
                cUpdBookR.Checked = true;
            }
            else
            {
                cUpdBookR.Checked = false;
            }

            if (tPrint.Equals("On"))
            {
                cPrintR.Checked = true;
            }
            else
            {
                cPrintR.Checked = false;
            }

            if (tUser.Equals("On"))
            {
                cUserR.Checked = true;
            }
            else
            {
                cUserR.Checked = false;
            }

            if (tRights.Equals("On"))
            {
                cRights.Checked = true;
            }
            else
            {
                cRights.Checked = false;
            }

            createTbl1Param(strQ, cmbJbtR.SelectedItem.ToString());
        }

        private void groupBox4_Enter(object sender, EventArgs e)
        {

        }

        private void panelRights1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnUser_Click(object sender, EventArgs e)
        {
             
            panelKamarDibooking.Controls.Clear();
            panelUser.BringToFront();
            panelCheckinDate.Visible = false;
            panelCheckoutDate.Visible = false;

        }

        private void splitContainer2_SplitterMoved(object sender, SplitterEventArgs e)
        {

        }

        private void groupBox5_Enter(object sender, EventArgs e)
        {

        }

        private void btnSaveUsr_Click(object sender, EventArgs e)
        {
            //connecting();
            //conn.Open();

            if (cmbNmUsr.Text != "")
            {
                cmbNmUsr.Items.Add(cmbNmUsr.Text);
                string strNama = cmbNmUsr.Text;
                string strQueryIns = "insert into staff(nama,password,username,id_jabatan," +
                    "telp,email) values (@nm,@pass,@usrnm,@idjab,@telp,@email)";
                SqlCommand cmd = new SqlCommand(strQueryIns, koneksi.KoneksiDB());
                cmd.Parameters.AddWithValue("@nm", strNama);
                cmd.Parameters.AddWithValue("@pass", txtPassUsr.Text);
                cmd.Parameters.AddWithValue("@usrnm", txtUserNmUsr.Text);
                cmd.Parameters.AddWithValue("@idjab", boxId);
                cmd.Parameters.AddWithValue("@telp", txtTelpUsr.Text);
                cmd.Parameters.AddWithValue("@email", txtEmailUsr.Text);
                cmd.ExecuteNonQuery();
                koneksi.KoneksiDB().Close();
                MessageBox.Show("Data User Telah Disimpan");
                string strQ = "select nama,password,username,telp,email,Jabatan " +
                   "from staff a,Jabatan b where a.id_jabatan=b.id_jabatan";
                createTblNoParamUsr(strQ);
                clearJabatan();
            }
            else
            {
                MessageBox.Show("Masukkan Nama !");
            }


        }

        private void cmbJbtUsr_SelectedIndexChanged(object sender, EventArgs e)
        {
            //connecting();
            //conn.Open();
            //lblHideJabUsr.Refresh();
            //lblHideJabUsr.Text = "";
            cmbJbtUsr.Refresh();
            string strQ = "select id_jabatan from Jabatan where jabatan = @nm";
            cmd = new SqlCommand(strQ, koneksi.KoneksiDB());
            cmd.Parameters.AddWithValue("@nm", cmbJbtUsr.SelectedItem.ToString());
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                boxId = reader.GetInt32(0);
            }
            koneksi.KoneksiDB().Close();
            //clearJabatan();

        }

        private void cmbNmUsr_SelectedIndexChanged(object sender, EventArgs e)
        {

            //connecting();
            //conn.Open();
            cmbNmUsr.Text = "";
            cmbJbtUsr.Text = "";
            cmbJbtUsr.Refresh();
            //cmbNmUsr.Refresh();
            //string strNama = cmbNmUsr.SelectedItem.ToString();
            cmbJbtUsr.Items.Clear();
            string strQ = "select nama,password,username,telp,email,Jabatan " +
                "from staff a,Jabatan b where a.id_jabatan=b.id_jabatan and nama = @nm";
            cmd = new SqlCommand(strQ, koneksi.KoneksiDB());
            cmd.Parameters.AddWithValue("@nm", cmbNmUsr.SelectedItem.ToString());
            //cmd.Parameters.AddWithValue("@nm", strNama);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                txtPassUsr.Text = reader.GetString(1);
                txtUserNmUsr.Text = reader.GetString(2);
                txtTelpUsr.Text = reader.GetString(3);
                txtEmailUsr.Text = reader.GetString(4);
                cmbJbtUsr.Text = reader.GetString(5);
                //a++;
            }
            koneksi.KoneksiDB().Close();
            createTbl1ParamUsr(strQ, cmbNmUsr.SelectedItem.ToString());
            clearJabatan();
        }

        private void btnUpdUsr_Click(object sender, EventArgs e)
        {
            if (cmbNmUsr.Text != "")
            {
                string strUserNm = cmbNmUsr.Text;
                //connecting();
                //koneksi.KoneksiDB().Open();
                string strQueryUpd = "update staff set password = @pass,username = @usrname," +
                    "id_jabatan = @idjabat,telp = @telp,email = @email where nama = @nama";
                SqlCommand cmd = new SqlCommand(strQueryUpd, koneksi.KoneksiDB());
                cmd.Parameters.AddWithValue("@pass", txtPassUsr.Text);
                cmd.Parameters.AddWithValue("@usrname", txtUserNmUsr.Text);
                cmd.Parameters.AddWithValue("@idjabat", boxId);
                cmd.Parameters.AddWithValue("@telp", txtTelpUsr.Text);
                cmd.Parameters.AddWithValue("@email", txtEmailUsr.Text);
                cmd.Parameters.AddWithValue("@nama", strUserNm);
                cmd.ExecuteNonQuery();
                koneksi.KoneksiDB().Close();
                MessageBox.Show("Data User Telah Diupdate");
                string strQ = "select nama,password,username,telp,email,Jabatan " +
                    "from staff a,Jabatan b where a.id_jabatan=b.id_jabatan";
                createTblNoParamUsr(strQ);
            }
            else
            {
                MessageBox.Show("Masukkan Nama !");
            }

        }

        private void btnDelUsr_Click(object sender, EventArgs e)
        {
            if (cmbNmUsr.Text != "")
            {
                string strNmUsr = cmbNmUsr.Text;
                //connecting();
                //conn.Open();
                string strQueryDel = "delete from staff where nama = @nm";
                SqlCommand cmd = new SqlCommand(strQueryDel, koneksi.KoneksiDB());
                cmd.Parameters.AddWithValue("@nm", strNmUsr);
                cmd.ExecuteNonQuery();
                koneksi.KoneksiDB().Close();
                MessageBox.Show("Data User Telah Dihapus");
                string strQ = "select nama,password,username,telp,email,Jabatan " +
                "from staff a,Jabatan b where a.id_jabatan=b.id_jabatan";
                createTblNoParamUsr(strQ);
            }
            else
            {
                MessageBox.Show("Masukkan Nama !");
            }

        }

        private void btnSAllUsr_Click(object sender, EventArgs e)
        {
            string strQUsr = "select nama,password,username,telp,email,Jabatan " +
               "from staff a,Jabatan b where a.id_jabatan=b.id_jabatan";
            createTblNoParamUsr(strQUsr);


        }

        private void dgUsr_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // boxId = dgR.Rows[dgR.CurrentRow.Index].Cells[0].Value.GetType(Int32());
            cmbNmUsr.Text = dgUsr.Rows[dgUsr.CurrentRow.Index].Cells[0].Value.ToString();
            txtUserNmUsr.Text = dgUsr.Rows[dgUsr.CurrentRow.Index].Cells[1].Value.ToString();
            txtPassUsr.Text = dgUsr.Rows[dgUsr.CurrentRow.Index].Cells[2].Value.ToString();
            txtTelpUsr.Text = dgUsr.Rows[dgUsr.CurrentRow.Index].Cells[3].Value.ToString();
            txtEmailUsr.Text = dgUsr.Rows[dgUsr.CurrentRow.Index].Cells[4].Value.ToString();
            cmbJbtUsr.Text = dgUsr.Rows[dgUsr.CurrentRow.Index].Cells[5].Value.ToString();
        }


        ///
        ///Suhendro Update
        ///

        private void btnRights_Click(object sender, EventArgs e)
        {
             
            panelKamarDibooking.Controls.Clear();
            hidepanelPengaturanKamar();
            panelRights1.BringToFront();
            panelCheckinDate.Visible = false;
            panelCheckoutDate.Visible = false;

        }

        private void dgUsr_Click(object sender, EventArgs e)
        {
            // boxId = dgR.Rows[dgR.CurrentRow.Index].Cells[0].Value.GetType(Int32());
            cmbNmUsr.Text = dgUsr.Rows[dgUsr.CurrentRow.Index].Cells[0].Value.ToString();
            txtUserNmUsr.Text = dgUsr.Rows[dgUsr.CurrentRow.Index].Cells[1].Value.ToString();
            txtPassUsr.Text = dgUsr.Rows[dgUsr.CurrentRow.Index].Cells[2].Value.ToString();
            txtTelpUsr.Text = dgUsr.Rows[dgUsr.CurrentRow.Index].Cells[3].Value.ToString();
            txtEmailUsr.Text = dgUsr.Rows[dgUsr.CurrentRow.Index].Cells[4].Value.ToString();
            cmbJbtUsr.Text = dgUsr.Rows[dgUsr.CurrentRow.Index].Cells[5].Value.ToString();

        }

        private void tambahToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string lmaHari = Interaction.InputBox("Lama Hari =");
            //MessageBox.Show( DateTime.Now.Date.AddDays(Int32.Parse(lmaHari)).ToString());
            SqlCommand sql1 = new SqlCommand("select checkout from Reservasi where kamar_no =@noKamar and status = 'checkin'", koneksi.KoneksiDB());
            sql1.Parameters.AddWithValue("@noKamar", dataKamarCh);

            DateTime haricheck = Convert.ToDateTime(sql1.ExecuteScalar().ToString());
            //DATENAME(dw,tanggal_id) in ('Saturday','Sunday')
            koneksi.KoneksiDB().Close();
            SqlCommand sql = new SqlCommand(@"
                select case when DATENAME(dw,tanggal_id) in ('Saturday','Sunday') then harga_weekend else harga end harga 
                from 
                Harga 
                inner join Kamar
                on Kamar.kamar_tipe_id = Harga.kamar_tipe_id and Kamar.kamar_no=@NOKAMAR and Harga.tanggal_id=@TANGGALNOW
            ", koneksi.KoneksiDB());
            sql.Parameters.AddWithValue("@NOKAMAR", dataKamarCh);
            sql.Parameters.AddWithValue("@TANGGALNOW", haricheck.AddDays(Int32.Parse(lmaHari)));
            string hargaKamar = sql.ExecuteScalar().ToString();
            koneksi.KoneksiDB().Close();

            sql = new SqlCommand("update Reservasi set checkout = @tnggalcheck, tag_kamar = tag_kamar + @nilai where status='checkin' and kamar_no=@kamarno", koneksi.KoneksiDB());
            sql.Parameters.AddWithValue("@kamarno", dataKamarCh);
            sql.Parameters.AddWithValue("@nilai", Int32.Parse(hargaKamar) * Int32.Parse(lmaHari));
            sql.Parameters.AddWithValue("@tnggalcheck", haricheck.AddDays(Int32.Parse(lmaHari)));
            sql.ExecuteNonQuery();
            koneksi.KoneksiDB().Close();

            sql = new SqlCommand("select booking_id from Reservasi where status='checkin' and kamar_no = @kamarno", koneksi.KoneksiDB());
            sql.Parameters.AddWithValue("@kamarno", dataKamarCh);
            int kodeid = Int32.Parse(sql.ExecuteScalar().ToString());
            koneksi.KoneksiDB().Close();

            sql = new SqlCommand("update Booking set tag_kamar=tag_kamar+@nilai, balance_due=balance_due+@nilai where  booking_id=@id", koneksi.KoneksiDB());
            sql.Parameters.AddWithValue("@nilai", Int32.Parse(hargaKamar) * Int32.Parse(lmaHari));
            sql.Parameters.AddWithValue("@id", kodeid);
            sql.ExecuteNonQuery();
            koneksi.KoneksiDB().Close();

        }

        private void btnCariItemPesan_Click(object sender, EventArgs e)
        {
            panel2.Visible = true;
            panel2.BringToFront();
            SqlDataAdapter da = new SqlDataAdapter("select Item.item_id, Item.item, Item_Tipe.item_tipe, Item.harga from Item, Item_Tipe where Item.item_tipe_id = Item_Tipe.item_tipe_id", koneksi.KoneksiDB());
            DataTable ds = new DataTable();
            da.Fill(ds);
            dataGridView5.DataSource = ds;
            koneksi.KoneksiDB().Close();
        }

        private void btnRemovePesan_Click(object sender, EventArgs e)
        {
            List<DataRow> rd = new List<DataRow>();
            int index = 0;
            foreach (DataRow dr in dtPesan.Rows)
            {
                if (index == dataGridView4.CurrentRow.Index)
                {
                    rd.Add(dr);
                }

                index += 1;
            }

            foreach (var r in rd)
            {
                dtPesan.Rows.Remove(r);
            }
            dtPesan.AcceptChanges();
            dataGridView4.DataSource = dtPesan;
        }

        private void btnClearPesan_Click(object sender, EventArgs e)
        {

            dtPesan.Clear();
        }

        private void btnaddItemPesan_Click(object sender, EventArgs e)
        {
            DataRow dr = dtPesan.NewRow();
            dr["ID_ITEM"] = xPenang.Text;
            dr["RESERVASI_ID"] = label22.Text;
            dr["TANGGAL"] = DateTime.Now.Date;
            dr["HARGA"] = label3.Text;
            dtPesan.Rows.Add(dr);
            dataGridView1.DataSource = dtPesan;
        }

        private void btnSubmitPesan_Click(object sender, EventArgs e)
        {
            List<DataRow> rd = new List<DataRow>();
            foreach (DataRow dr in dtPesan.Rows)
            {

                SqlCommand sql = new SqlCommand("insert into Pemesanan(item_id, reservasi_id, tgl_pemesanan, harga) values (@a,@b,@c,@d)", koneksi.KoneksiDB());
                sql.Parameters.AddWithValue("@a", dr["ID_ITEM"]);
                sql.Parameters.AddWithValue("@b", dr["RESERVASI_ID"]);
                sql.Parameters.AddWithValue("@c", dr["TANGGAL"]);
                sql.Parameters.AddWithValue("@d", dr["HARGA"]);

                sql.ExecuteNonQuery();
                koneksi.KoneksiDB().Close();

                sql = new SqlCommand("select booking_id from Reservasi where reservasi_id = @id", koneksi.KoneksiDB());
                sql.Parameters.AddWithValue("@id", dr["RESERVASI_ID"]);
                string databooking = (sql.ExecuteScalar().ToString());

                koneksi.KoneksiDB().Close();

                sql = new SqlCommand("update Booking set grand_total=grand_total+@biaya, balance_due = balance_due+@biaya  where booking_id=@id", koneksi.KoneksiDB());
                sql.Parameters.AddWithValue("@biaya", dr["HARGA"]);
                sql.Parameters.AddWithValue("@id", Int32.Parse(databooking));

                sql.ExecuteNonQuery();
                koneksi.KoneksiDB().Close();
                sql = new SqlCommand("update Reservasi set tag_restoran=tag_restoran+@biaya where booking_id=@id and reservasi_id=@idr", koneksi.KoneksiDB());
                sql.Parameters.AddWithValue("@biaya", dr["HARGA"]);
                sql.Parameters.AddWithValue("@idr", dr["RESERVASI_ID"]);
                sql.Parameters.AddWithValue("@id", Int32.Parse(databooking));

                sql.ExecuteNonQuery();
                koneksi.KoneksiDB().Close();
            }
            dtPesan.Clear();
            label22.Text = "-";
            label3.Text = "-";
            label15.Text = "-";
            xPenang.Text = "";
            PanelPesan.BringToFront();
        }

        private void TxtCust_TextChanged(object sender, EventArgs e)
        {
            BindingSource bs = new BindingSource();
            bs.DataSource = dataGridView5.DataSource;
            bs.Filter = "item like '%" + txtcariItemPesan.Text + "%'";
            dataGridView5.DataSource = bs;
        }

        private void dataGridView5_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            xPenang.Text = dataGridView5[0, e.RowIndex].Value.ToString();
            label15.Text = dataGridView5[1, e.RowIndex].Value.ToString();
            label3.Text = dataGridView5[3, e.RowIndex].Value.ToString();

            panel2.Visible = false;
        }

        private void txtcariItemPesan_TextChanged(object sender, EventArgs e)
        {
            BindingSource bs = new BindingSource();
            bs.DataSource = dataGridView5.DataSource;
            bs.Filter = "item like '%" + txtcariItemPesan.Text + "%'";
            dataGridView5.DataSource = bs;
        }

        private void dataGridView3_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
            //string cellValue = dataGridView3.Rows[e.RowIndex].Cells[e.ColumnIndex].FormattedValue.ToString();
            //DBNull isnull;
            dataGridView3.ReadOnly = false;
           dataGridView3.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = 0;
           
        }

        private void dataGridView3_CellEnter_1(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView3.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor == Color.Red)
            {
                contextMenuStrip1.Show(Cursor.Position);
                rowSelect = e.RowIndex;
                columnSelect = e.ColumnIndex;
            }
        }

        private void dataGridView6_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataTamuKalender.Visible = false;
            noIDdatatamu = Int32.Parse(dataGridView6.Rows[dataGridView6.CurrentRow.Index].Cells[0].Value.ToString());
            DateTime tanggalPesan1 = Convert.ToDateTime(TglBulan + "/" + dataGridView3.Columns[columnSelect].Name.ToString() + "/" + Tgltahun);

            int NoKamarInfo = Int32.Parse(dataGridView3.Rows[rowSelect].Cells[0].Value.ToString());
            SqlCommand sqlq = new SqlCommand("select Reservasi.reservasi_id from Reservasi, Tamu where Tamu.tamu_id = Reservasi.tamu_id and Reservasi.checkin <=@id and Reservasi.checkout >= @id and Reservasi.kamar_no=@nok and (Reservasi.status='booking')", koneksi.KoneksiDB());
            sqlq.Parameters.AddWithValue("@id", tanggalPesan1);
            sqlq.Parameters.AddWithValue("@nok", NoKamarInfo);

            string reservasiKamar = sqlq.ExecuteScalar().ToString();
            SqlCommand sql;
            if (noIDdatatamu > 0)
            {
                sql = new SqlCommand("update Reservasi set status= 'checkin', tamu_id=@a where reservasi_id =@id", koneksi.KoneksiDB());

                sql.Parameters.AddWithValue("@a", noIDdatatamu);
            }
            else
            {
                sql = new SqlCommand("update Reservasi set status= 'checkin' where reservasi_id =@id", koneksi.KoneksiDB());

            }
            sql.Parameters.AddWithValue("@id", reservasiKamar);
            noIDdatatamu = 0;
            sql.ExecuteNonQuery();
            koneksi.KoneksiDB().Close();

            setLoad(TglBulan, Tgltahun);

        }

        public void reloaddata()
        {
            SqlCommand sql;
            //SqlDataReader reader;
            Button[] JKamar;
            int jumKamar;

            panelRestoran.Controls.Clear();

            sql = new SqlCommand("select count(*) from MejaRestaurant", koneksi.KoneksiDB());
            jumKamar = (int)sql.ExecuteScalar();
            koneksi.KoneksiDB().Close();

            sql = new SqlCommand("select NoMeja,StatusMeja from MejaRestaurant", koneksi.KoneksiDB());
            reader = sql.ExecuteReader();
            JKamar = new Button[jumKamar];
            int ctr = 0;
            while (reader.Read())
            {
                JKamar[ctr] = new Button();
                JKamar[ctr].Text = reader.GetInt32(0).ToString();
                JKamar[ctr].Name = reader.GetInt32(0).ToString();
                JKamar[ctr].Visible = true;
                JKamar[ctr].Height = 35;
                //JKamar[ctr].Click += new EventHandler(tambah_kamar);
                //JKamar[ctr].MouseEnter += new EventHandler(button1_MouseEnter_2);
                //JKamar[ctr].MouseLeave += new EventHandler(button1_MouseLeave_1);

                //JKamar[ctr] = new Label();
                //JKamar[ctr].Text = reader.GetInt32(0).ToString();
                //JKamar[ctr].Name = reader.GetInt32(0).ToString();
                //JKamar[ctr].Size = new Size(panjangLbl, lebarLbl);
                //JKamar[ctr].Font = new Font("Arial", 16);
                //JKamar[ctr].TextAlign = ContentAlignment.MiddleCenter;

                //JKamar[ctr].Location = new Point((panjangLbl * posX) + (20 * posX), (posY * lebarLbl) + (20 * posY));
                if (reader.GetString(1).ToString().Equals("S"))
                {
                    JKamar[ctr].BackColor = Color.Green;
                }
                else
                {
                    JKamar[ctr].BackColor = Color.Gray;

                }
                JKamar[ctr].Click += new EventHandler(JKClick);
                JKamar[ctr].MouseEnter += new EventHandler(JKEnter);
                JKamar[ctr].MouseLeave += new EventHandler(JKLeave);

                panelRestoran.Controls.Add(JKamar[ctr]);

                ctr += 1;

            }
            koneksi.KoneksiDB().Close();

        }



        private void JKEnter(object sender, EventArgs e)
        {
            Button enter = sender as Button;
            String noKamar = "-";
            String namaTamu = "-";
            try
            {
                SqlCommand sql = new SqlCommand("select h.NoKamar from HRestaurant h, MejaRestaurant m where m.StatusMeja = 'R' and h.flag='M' and m.NoMeja = @nMeja and h.NoMeja=m.NoMeja group by h.NoKamar ", koneksi.KoneksiDB());
                sql.Parameters.AddWithValue("@nMeja", enter.Text);
                noKamar = sql.ExecuteScalar().ToString();
                koneksi.KoneksiDB().Close();

                sql = new SqlCommand("select Tamu.tamu from Reservasi, Tamu where Reservasi.kamar_no = @kmarno and Reservasi.status = 'checkin' and Reservasi.tamu_id = Tamu.tamu_id ", koneksi.KoneksiDB());
                sql.Parameters.AddWithValue("@kmarno", noKamar);
                namaTamu = sql.ExecuteScalar().ToString();
                koneksi.KoneksiDB().Close();
            }
            catch
            {
            }

            toolTip1.Show(" No Meja : " + enter.Text + "\n No Kamar : " + noKamar + "\n Tamu : " + namaTamu, enter);
        }

        private void JKLeave(object sender, EventArgs e)
        {
            toolTip1.Hide(sender as Button);
        }

        //DataTable dt;
        private void JKClick(object sender, EventArgs e)
        {


            Button lbl = sender as Button;
            dt = new DataTable();
            if (lbl.BackColor == Color.Green)
            {
                //panelPesanRestoran.Visible = true;
                noMeja.Text = lbl.Text;

                cb_inputNoKamar.Items.Clear();
                SqlCommand sql = new SqlCommand("select kamar_no from reservasi where status='checkin'", koneksi.KoneksiDB());
                reader = sql.ExecuteReader();
                while (reader.Read())
                {
                    cb_inputNoKamar.Items.Add(reader.GetValue(0));
                }
                koneksi.KoneksiDB().Close();
                cb_inputNoKamar.Enabled = true;

                GridViewAddItem.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dt.Columns.Add("ID_ITEM".ToString());
                dt.Columns.Add("Jumlah".ToString());
                dt.Columns.Add("TANGGAL", typeof(DateTime));
                dt.Columns.Add("HARGA".ToString());
                dt.Columns.Add("SubTotal".ToString());
                GridViewAddItem.DataSource = dt;

                panelPesanRestoran.BringToFront();
                panelCariItem.Visible = false;
            }
            else
            {
                contextMenuRestoran.Show(Cursor.Position);

                noMejaPembayaran.Text = lbl.Text;
                noMeja.Text = lbl.Text;

                cb_inputNoKamar.Items.Clear();
                SqlCommand sql = new SqlCommand("select kamar_no from reservasi where status='checkin'", koneksi.KoneksiDB());
                reader = sql.ExecuteReader();
                while (reader.Read())
                {
                    cb_inputNoKamar.Items.Add(reader.GetValue(0));
                }
                koneksi.KoneksiDB().Close();
                cb_inputNoKamar.Enabled = false;

                try
                {
                    sql = new SqlCommand("select h.NoKamar from HRestaurant h, MejaRestaurant m where m.StatusMeja = 'R' and h.flag='M' and m.NoMeja = @nMeja and h.NoMeja=m.NoMeja group by h.NoKamar ", koneksi.KoneksiDB());
                    sql.Parameters.AddWithValue("@nMeja", lbl.Text);
                    cb_inputNoKamar.SelectedIndex = cb_inputNoKamar.FindStringExact(sql.ExecuteScalar().ToString());
                    koneksi.KoneksiDB().Close();
                }
                catch
                {
                }
                GridViewAddItem.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dt.Columns.Add("ID_ITEM".ToString());
                dt.Columns.Add("Jumlah".ToString());
                dt.Columns.Add("TANGGAL", typeof(DateTime));
                dt.Columns.Add("HARGA".ToString());
                dt.Columns.Add("SubTotal".ToString());
                GridViewAddItem.DataSource = dt;

            }
        }

        private void btn_restoran_Click(object sender, EventArgs e)
        {
             
            panelKamarDibooking.Controls.Clear();
            panelRestoran.BringToFront();
            panelCheckinDate.Visible = false;
            panelCheckoutDate.Visible = false;

            //panelPesanRestoran.Visible = false;
            reloaddata();
        }

        private void tambahToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            panelPesanRestoran.BringToFront();
            panelCariItem.Visible = false;
        }

        private void bayarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            comboboxPembayaran.Items.Clear();
            ComboboxItem item = new ComboboxItem();
            item.Text = "Bayar Langsung";
            item.Value = 1;
            comboboxPembayaran.Items.Add(item);
            item = new ComboboxItem();
            item.Text = "Simpan";
            item.Value = 2;
            comboboxPembayaran.Items.Add(item);
            comboboxPembayaran.Text = "Bayar Langsung";

            panelBayarRestoran.BringToFront();
        
        }

        private void btn_cari_item_Click(object sender, EventArgs e)
        {
            panelCariItem.Visible = true;

            SqlDataAdapter da = new SqlDataAdapter("select Item.item_id, Item.item, Item_Tipe.item_tipe, Item.harga from Item, Item_Tipe where Item.item_tipe_id = Item_Tipe.item_tipe_id", koneksi.KoneksiDB());
            DataTable ds = new DataTable();
            da.Fill(ds);
            GridViewItem.DataSource = ds;

            koneksi.KoneksiDB().Close();

        }

        private void btn_addItem_Click(object sender, EventArgs e)
        {
            totalHargaItem.Text = (Int32.Parse(totalHargaItem.Text) + (Int32.Parse(HargaItem.Text) * Int32.Parse(inputJumlahItem.Text))).ToString();
            DataRow dr = dt.NewRow();
            dr["ID_ITEM"] = inputIdItem.Text;
            dr["Jumlah"] = inputJumlahItem.Text;
            dr["TANGGAL"] = DateTime.Now.Date;
            dr["HARGA"] = HargaItem.Text;
            dr["SubTotal"] = (Int32.Parse(HargaItem.Text) * Int32.Parse(inputJumlahItem.Text)).ToString();
            dt.Rows.Add(dr);
            GridViewAddItem.DataSource = dt;
        
        }

        private void inputCariItem_TextChanged(object sender, EventArgs e)
        {
            BindingSource bs = new BindingSource();
            bs.DataSource = GridViewItem.DataSource;
            bs.Filter = "item like '%" + inputCariItem.Text + "%'";
            GridViewItem.DataSource = bs;
        
        }

        private void btn_submitItem_Click(object sender, EventArgs e)
        {
            if (cb_inputNoKamar.Enabled)
            {
                try
                {
                    SqlCommand sql = new SqlCommand("insert into HRestaurant(TglPesan, NoKamar,NoMeja, Biaya, flag) values (@a,@b,@c,@d,'M')", koneksi.KoneksiDB());
                    sql.Parameters.AddWithValue("@a", DateTime.Now.Date);
                    sql.Parameters.AddWithValue("@b", cb_inputNoKamar.SelectedItem);
                    sql.Parameters.AddWithValue("@c", Int32.Parse(noMeja.Text));
                    sql.Parameters.AddWithValue("@d", Int32.Parse(totalHargaItem.Text));
                    sql.ExecuteNonQuery();
                    koneksi.KoneksiDB().Close();

                    sql = new SqlCommand("update MejaRestaurant set StatusMeja = 'R' where NoMeja = @no  ", koneksi.KoneksiDB());
                    sql.Parameters.AddWithValue("@no", noMeja.Text);
                    sql.ExecuteNonQuery();
                    koneksi.KoneksiDB().Close();

                    sql = new SqlCommand("select MAX(NoPemesanan) from HRestaurant", koneksi.KoneksiDB());
                    int nopesan = Int32.Parse(sql.ExecuteScalar().ToString());
                    koneksi.KoneksiDB().Close();

                    List<DataRow> rd = new List<DataRow>();
                    foreach (DataRow dr in dt.Rows)
                    {
                        sql = new SqlCommand("insert into DRestaurant(NoPemesanan, NoItem,Jumlah, SubBiaya) values (@a,@b,@c,@d)", koneksi.KoneksiDB());
                        sql.Parameters.AddWithValue("@a", nopesan);
                        sql.Parameters.AddWithValue("@b", dr["ID_ITEM"]);
                        sql.Parameters.AddWithValue("@c", dr["Jumlah"]);
                        sql.Parameters.AddWithValue("@d", dr["SubTotal"]);

                        sql.ExecuteNonQuery();

                        koneksi.KoneksiDB().Close();
                    }
                }
                catch
                {
                }
            }
            else
            {
                try
                {
                    SqlCommand sql = new SqlCommand("Select NoPemesanan from HRestaurant where NoMeja=@nMeja and flag='M'", koneksi.KoneksiDB());
                    sql.Parameters.AddWithValue("@nMeja", noMeja.Text);
                    int nopesan = Int32.Parse(sql.ExecuteScalar().ToString());
                    koneksi.KoneksiDB().Close();

                    sql = new SqlCommand("update HRestaurant set Biaya=Biaya+@biaya where NoPemesanan=@nopesan", koneksi.KoneksiDB());
                    sql.Parameters.AddWithValue("@biaya", totalHargaItem.Text);
                    sql.Parameters.AddWithValue("@nopesan", nopesan);
                    sql.ExecuteNonQuery();
                    koneksi.KoneksiDB().Close();

                    List<DataRow> rd = new List<DataRow>();
                    foreach (DataRow dr in dt.Rows)
                    {
                        sql = new SqlCommand("insert into DRestaurant(NoPemesanan, NoItem,Jumlah, SubBiaya) values (@a,@b,@c,@d)", koneksi.KoneksiDB());
                        sql.Parameters.AddWithValue("@a", nopesan);
                        sql.Parameters.AddWithValue("@b", dr["ID_ITEM"]);
                        sql.Parameters.AddWithValue("@c", dr["Jumlah"]);
                        sql.Parameters.AddWithValue("@d", dr["SubTotal"]);

                        sql.ExecuteNonQuery();

                        koneksi.KoneksiDB().Close();
                    }

                }
                catch
                { }
            }
            btn_restoran_Click(sender, e);
        
        }

        private void btn_removeItem_Click(object sender, EventArgs e)
        {
            List<DataRow> rd = new List<DataRow>();
            int index = 0;
            foreach (DataRow dr in dt.Rows)
            {
                if (index == GridViewAddItem.CurrentRow.Index)
                {
                    rd.Add(dr);
                }

                index += 1;
            }

            foreach (var r in rd)
            {
                dt.Rows.Remove(r);
            }
            dt.AcceptChanges();
            GridViewAddItem.DataSource = dt;
        
        }

        private void btn_clearItem_Click(object sender, EventArgs e)
        {
            dt.Clear();
        }

        private void GridViewItem_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void GridViewItem_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            inputIdItem.Text = GridViewItem[0, e.RowIndex].Value.ToString();
            namaItem.Text = GridViewItem[1, e.RowIndex].Value.ToString();
            HargaItem.Text = GridViewItem[3, e.RowIndex].Value.ToString();

            panelCariItem.Visible = false;
        
        }

        private void _bayarRestoran_Click(object sender, EventArgs e)
        {
            SqlCommand sql = new SqlCommand("update MejaRestaurant set StatusMeja ='S' where NoMeja=@a", koneksi.KoneksiDB());
            sql.Parameters.AddWithValue("@a", noMejaPembayaran.Text);
            sql.ExecuteNonQuery();
            koneksi.KoneksiDB().Close();

            sql = new SqlCommand("select max(noPemesanan) from HRestaurant where noMeja =@a", koneksi.KoneksiDB());
            sql.Parameters.AddWithValue("@a", noMejaPembayaran.Text);
            int nopesan = Int32.Parse(sql.ExecuteScalar().ToString());
            koneksi.KoneksiDB().Close();

            sql = new SqlCommand("select NoKamar from HRestaurant where NoPemesanan =@a", koneksi.KoneksiDB());
            sql.Parameters.AddWithValue("@a", nopesan);
            int noKamarA = Int32.Parse(sql.ExecuteScalar().ToString());
            koneksi.KoneksiDB().Close();

            if (comboBox1.Text.Equals("Bayar Langsung"))
            {
                sql = new SqlCommand("update HRestaurant set flag ='S' where NoMeja=@a and noPemesanan = @b ", koneksi.KoneksiDB());
                sql.Parameters.AddWithValue("@a", noMejaPembayaran.Text);
                sql.Parameters.AddWithValue("@b", nopesan);
                sql.ExecuteNonQuery();
                koneksi.KoneksiDB().Close();
            }
            else
            {
                sql = new SqlCommand("update HRestaurant set flag ='B' where NoMeja=@a and noPemesanan = @b ", koneksi.KoneksiDB());
                sql.Parameters.AddWithValue("@a", noMejaPembayaran.Text);
                sql.Parameters.AddWithValue("@b", nopesan);
                sql.ExecuteNonQuery();
                koneksi.KoneksiDB().Close();

                sql = new SqlCommand("select Biaya from HRestaurant where NoPemesanan=@b ", koneksi.KoneksiDB());
                sql.Parameters.AddWithValue("@b", nopesan);
                int biayaBaru = Int32.Parse(sql.ExecuteScalar().ToString());
                koneksi.KoneksiDB().Close();

                sql = new SqlCommand("update Reservasi set tag_restoran=tag_restoran+@a where kamar_no = @b and status='checkin'", koneksi.KoneksiDB());
                sql.Parameters.AddWithValue("@a", biayaBaru);
                sql.Parameters.AddWithValue("@b", noKamarA);
                sql.ExecuteNonQuery();
                koneksi.KoneksiDB().Close();

                sql = new SqlCommand("select booking_id from Reservasi where kamar_no=@a and status='checkin'", koneksi.KoneksiDB());
                sql.Parameters.AddWithValue("@a", noKamarA);
                int idbooking = Int32.Parse(sql.ExecuteScalar().ToString());
                koneksi.KoneksiDB().Close();

                sql = new SqlCommand("update Booking set balance_due=balance_due+@a where booking_id=@b", koneksi.KoneksiDB());
                sql.Parameters.AddWithValue("@a", biayaBaru);
                sql.Parameters.AddWithValue("@b", idbooking);
                sql.ExecuteNonQuery();
                koneksi.KoneksiDB().Close();
            }

            panelReportRestoran.BringToFront();
            
        }

        private void btnSimpan_Click(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand((@"update Tamu set tamu=@nama, alamat=@alamat, kota=@kota, telepon=@telepon, email=@email, perusahaan=@perusahaan, tanggallahir=@tgllhr, sebutan=@sebutan, gelar=@gelar where tamu_id=@tamu_id"), koneksi.KoneksiDB());
            cmd.Parameters.AddWithValue("@tamu_id", tamu_id);
            cmd.Parameters.AddWithValue("@nama", inputNamaDT.Text);
            cmd.Parameters.AddWithValue("@alamat", inputAlamatDT.Text);
            cmd.Parameters.AddWithValue("@kota", inputKotaDT.Text);
            cmd.Parameters.AddWithValue("@telepon", inputTlpnDT.Text);
            cmd.Parameters.AddWithValue("@email", inputEmailDT.Text);
            cmd.Parameters.AddWithValue("@perusahaan", inputPerusahaanDT.Text);
            if (inputTglLhrDT.Value.Date.Year != 1900)
            {
                cmd.Parameters.Add("@tgllhr", SqlDbType.DateTime).Value = inputTglLhrDT.Value.Date;
            }
            else
            {
                cmd.Parameters.Add("@tgllhr", SqlDbType.DateTime).Value = DBNull.Value;
            }
            cmd.Parameters.AddWithValue("@sebutan", inputSebutanDT.Text);
            cmd.Parameters.AddWithValue("@gelar", inputGelarDT.Text);

            cmd.ExecuteNonQuery();
            
            
            tamuBindingSource1.ResetBindings(true);
            
            GridViewDaftarTamu.DataSource = null;
            GridViewDaftarTamu.Update();
            GridViewDaftarTamu.Invalidate();
            GridViewDaftarTamu.Refresh();

            tamuBindingSource1.ResetBindings(false);
            GridViewDaftarTamu.DataSource = tamuBindingSource1;
            GridViewDaftarTamu.Update();
            GridViewDaftarTamu.Invalidate();
            GridViewDaftarTamu.Refresh();

//            cmd = new SqlCommand(strQuery, koneksi.KoneksiDB());
            

            koneksi.KoneksiDB().Close();
            //GridViewDaftarTamu.DataSource.
            //GridViewDaftarTamu.Refresh();


            //cmd = new SqlCommand(strQuery, koneksi.KoneksiDB());
            //ds = new DataSet();
            //da = new SqlDataAdapter(cmd);
            //da.Fill(ds, "tamu");
            //GridViewDaftarTamu.DataSource = ds;
            //GridViewDaftarTamu.DataMember = "tamu";
            //GridViewDaftarTamu.Invalidate();
            //GridViewDaftarTamu.Refresh();
            //koneksi.KoneksiDB().Close();

        }

        private void inputSearchDT_TextChanged(object sender, EventArgs e)
        {
            if (inputSearchDT.Text.Length >= 3)
            {
                BindingSource bs = new BindingSource();
                bs.DataSource = GridViewDaftarTamu.DataSource;
                bs.Filter = "tamu like '%" + inputSearchDT.Text + "%'";
                GridViewDaftarTamu.DataSource = bs;
            }
        }

        private void inputSearchPerusahaanDT_TextChanged(object sender, EventArgs e)
        {
            if (inputSearchPerusahaanDT.Text.Length >= 3)
            {
                BindingSource bs = new BindingSource();
                bs.DataSource = GridViewDaftarTamu.DataSource;
                bs.Filter = "perusahaan like '%" + inputSearchPerusahaanDT.Text + "%'";
                GridViewDaftarTamu.DataSource = bs;
            }
        }

        string tamu_id;
        private void GridViewDaftarTamu_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            SqlCommand cmd = new SqlCommand((@"select * from Tamu where tamu_id=@tamu_id"), koneksi.KoneksiDB());
            cmd.Parameters.AddWithValue("@tamu_id", GridViewDaftarTamu[0, e.RowIndex].Value.ToString());
            tamu_id = GridViewDaftarTamu[0, e.RowIndex].Value.ToString();
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                inputNamaDT.Text = reader.GetValue(1).ToString();
                inputAlamatDT.Text = reader.GetValue(2).ToString();
                inputKotaDT.Text = reader.GetValue(3).ToString();
                inputTlpnDT.Text = reader.GetValue(4).ToString();
                inputEmailDT.Text = reader.GetValue(5).ToString();
                inputPerusahaanDT.Text = reader.GetValue(6).ToString();
                if (reader.GetValue(7) != DBNull.Value)
                {
                    //Console.WriteLine(reader.GetValue(7));
                    inputTglLhrDT.Value = Convert.ToDateTime(reader.GetValue(7));
                }
                else
                {
                    inputTglLhrDT.Value = Convert.ToDateTime("1900-1-1 16:58:00"); ;
                }
                inputSebutanDT.Text = reader.GetValue(8).ToString();
                inputGelarDT.Text = reader.GetValue(9).ToString();

            }
            koneksi.KoneksiDB().Close();

            //GridViewDaftarTamu.Visible = false;
        
        }

        private void inputCariNamaTamu_TextChanged(object sender, EventArgs e)
        {

            if (inputCariNamaTamu.Text.Length >= 3)
            {
                BindingSource bs = new BindingSource();
                bs.DataSource = datagridTamu.DataSource;
                bs.Filter = "tamu like '%" + inputCariNamaTamu.Text + "%'";
                datagridTamu.DataSource = bs;
            }
        }

        private void dataGridView6_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataTamuKalender.Visible = false;
            noIDdatatamu = Int32.Parse(dataGridView6.Rows[dataGridView6.CurrentRow.Index].Cells[0].Value.ToString());
            DateTime tanggalPesan1 = Convert.ToDateTime(TglBulan + "/" + dataGridView3.Columns[columnSelect].Name.ToString() + "/" + Tgltahun);

            int NoKamarInfo = Int32.Parse(dataGridView3.Rows[rowSelect].Cells[0].Value.ToString());
            SqlCommand sqlq = new SqlCommand("select Reservasi.reservasi_id from Reservasi, Tamu where Tamu.tamu_id = Reservasi.tamu_id and Reservasi.checkin <=@id and Reservasi.checkout >= @id and Reservasi.kamar_no=@nok and (Reservasi.status='booking')", koneksi.KoneksiDB());
            sqlq.Parameters.AddWithValue("@id", tanggalPesan1);
            sqlq.Parameters.AddWithValue("@nok", NoKamarInfo);

            string reservasiKamar = sqlq.ExecuteScalar().ToString();
            SqlCommand sql;
            if (noIDdatatamu > 0)
            {
                sql = new SqlCommand("update Reservasi set status= 'checkin', tamu_id=@a where reservasi_id =@id", koneksi.KoneksiDB());

                sql.Parameters.AddWithValue("@a", noIDdatatamu);
            }
            else
            {
                sql = new SqlCommand("update Reservasi set status= 'checkin' where reservasi_id =@id", koneksi.KoneksiDB());

            }
            sql.Parameters.AddWithValue("@id", reservasiKamar);
            noIDdatatamu = 0;
            sql.ExecuteNonQuery();
            koneksi.KoneksiDB().Close();

            setLoad(TglBulan, Tgltahun);

        }

        private void txtNamaCust_TextChanged(object sender, EventArgs e)
        {
            if (txtNamaCust.Text.Length >= 3)
            {
                BindingSource bs = new BindingSource();
                bs.DataSource = dataGridView6.DataSource;
                bs.Filter = "tamu like '%" + txtNamaCust.Text + "%'";
                dataGridView6.DataSource = bs;
            }
        }

        private void checkOutToolStripMenuItem_Click(object sender, EventArgs e)
        {

            SqlCommand sql = new SqlCommand("update Reservasi set status= 'checkout', checkout=@tggal where kamar_no = @id and checkout >= @tggal and status='checkin' ", koneksi.KoneksiDB());
            sql.Parameters.AddWithValue("@tggal", DateTime.Now.Date);
            sql.Parameters.AddWithValue("@id", dataKamarCh);
            sql.ExecuteNonQuery();
            koneksi.KoneksiDB().Close();

            int tagihankamar = 0;
            int tagihanpesanan = 0;
            int reservasiid = 0;
            int bookingid = 0; int tamuid = 0;
            sql = new SqlCommand("select reservasi_id,tag_kamar,booking_id,tamu_id from Reservasi where kamar_no = @id and checkout = @tggal", koneksi.KoneksiDB());
            sql.Parameters.AddWithValue("@tggal", DateTime.Now.Date);
            sql.Parameters.AddWithValue("@id", dataKamarCh);
            SqlDataReader reader = sql.ExecuteReader();
            while (reader.Read())
            {
                tagihankamar = Convert.ToInt32(reader["tag_kamar"]);
                reservasiid = Convert.ToInt32(reader["reservasi_id"]);
                bookingid = Convert.ToInt32(reader["booking_id"]);
                tamuid = Convert.ToInt32(reader["tamu_id"]);
                // MessageBox.Show(reader.GetInt32(1).ToString());
            }
            koneksi.KoneksiDB().Close();

            sql = new SqlCommand("select harga from Pemesanan where reservasi_id = @id", koneksi.KoneksiDB());
            sql.Parameters.AddWithValue("@id", reservasiid);
            SqlDataReader reader1 = sql.ExecuteReader();
            while (reader1.Read())
            {
                tagihanpesanan = tagihanpesanan + Convert.ToInt32(reader1["harga"]);

            }

            koneksi.KoneksiDB().Close();

            sql = new SqlCommand("select downpayment from Reservasi where reservasi_id = @id", koneksi.KoneksiDB());
            sql.Parameters.AddWithValue("@id", reservasiid);
            int diskon = 0;
            diskon = Int32.Parse(sql.ExecuteScalar().ToString());
            koneksi.KoneksiDB().Close();

            int totalbiayakamar = tagihankamar + tagihanpesanan - diskon;

            sql = new SqlCommand("update Booking set balance_due = balance_due-@total where booking_id = @id", koneksi.KoneksiDB());
            sql.Parameters.AddWithValue("@id", bookingid);
            sql.Parameters.AddWithValue("@total", totalbiayakamar);
            sql.ExecuteNonQuery();

            koneksi.KoneksiDB().Close();
            btnPesan_Click(sender, e);   
        
        }

        private void dgUsr_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {

        }

        
        //private void btnUser_Click(object sender, EventArgs e)
        //{
        //    panelUser.BringToFront();
        //    panelCheckinDate.Visible = false;
        //    panelCheckoutDate.Visible = false;
            
        //}

        //end irwan
    
    }

}




//irwan tambahkan
class ComboboxItem
{
    public string Text { get; set; }
    public object Value { get; set; }

    public override string ToString()
    {
        return Text;
    }
}
//end irwan