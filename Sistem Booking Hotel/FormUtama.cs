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
    public partial class FormUtama : Form
    {   
        
        // irwan tambahkan 
        DataTable dKamarPesan = new DataTable();
        ComboboxItem item = new ComboboxItem();
        Boolean cekPilih = false;
        int dataCustomer = 0; int rowSelect = 0; int columnSelect;
        int TglBulan = 0; int Tgltahun = 0;
        int totalBiaya = 0;
        // end irwan

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
            panelKamarDibooking.Enabled = true;
            panelKamar.Controls.Clear();
            //conn = koneksi.KoneksiDB();
            //conn.Open();
            SqlCommand cmd = new SqlCommand((@"select count(*) from Kamar"), koneksi.KoneksiDB());

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
                Kamar[x].MouseEnter += new EventHandler(button1_MouseEnter_2);
                Kamar[x].MouseLeave += new EventHandler(button1_MouseLeave_1);

                panelKamar.Controls.Add(Kamar[x]);
                x += 1;
                //Kamar[x].MouseEnter += button1_MouseEnter_2;// Kamar_Tips;//new EventHandler(Kamar_Tips);
                
            }
            //conn.Close();
            koneksi.KoneksiDB().Close();
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
            dKamarPesan.Columns.Add("NO Kamar".ToString());
            dKamarPesan.Columns.Add("Checkin", typeof(DateTime));
            dKamarPesan.Columns.Add("Checkout", typeof(DateTime));
            dKamarPesan.Columns.Add("Tamu".ToString());
            dKamarPesan.Columns.Add("Harga".ToString());
            isiCombobox();
            isCombobox3();
            // end irwan
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
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
            panelCheckinDate.Visible = false;
            panelCheckoutDate.Visible = false;
            //irwan tambahkan
            cekPilih = true;
            flowLayoutPanel1.Visible = true;
            dataGridView3.BringToFront();
            //setLoad(7, 2014);
            setLoad(DateTime.Now.Month,DateTime.Now.Year);
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
	            combine.kamar_no no,DATEPART(dd,tanggal_id) tanggal,booking_id
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
		            booking_id,kamar_no,DATEADD(dd,hari,checkin) tanggal
		            from
		            (
			            select
			            ROW_NUMBER() over(partition by reservasi_id,kamar_no order by checkin,tamu_id)-1 hari
			            ,booking_id,kamar_no,checkin
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
	            max(Booking_id) for tanggal in ([1],[2],[3],[4],[5],[6],[7],[8],[9],[10],[11],[12],[13],[14],[15],[16],[17],[18],[19],[20],[21],[22],[23],[24],[25],[26],[27],[28],[29],[30],[31])
            )as piv";
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
            dataGridView3.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader;
            int row = 0;
            foreach (DataGridViewRow rw in this.dataGridView3.Rows)
            {
                //row++;
                for (int i = 0; i < rw.Cells.Count; i++)
                {
                    if (Convert.ToString(rw.Cells[i].Value) != string.Empty && i > 0)

                    //if (dataGridView3.Rows[row].Cells[i].Value.ToString().Length >= 1)//rw.Cells[i].Value != null || rw.Cells[i].Value != DBNull.Value )
                    {
                        //rw.Cells[i].Style.BackColor = Color.Red;
                        dataGridView3.Rows[row].Cells[i].Style.BackColor = Color.Red;
                    }
                } row++;
            }
        }

        //private void setLoad(int bulan, int tahun)
        //{
        //    TglBulan = bulan;
        //    Tgltahun = tahun;

        //    string select = "SELECT * FROM reservasi";
        //    //Connection c = new Connection();
        //    SqlDataAdapter dataAdapter = new SqlDataAdapter(select, koneksi.KoneksiDB()); //c.con is the connection string

        //    SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dataAdapter);
        //    DataSet ds = new DataSet();
        //    dataAdapter.Fill(ds);
        //    dataGridView3.ReadOnly = true;
        //    dataGridView3.DataSource = ds;

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

        //    for (int i = 1; i < dataGridView3.ColumnCount - 1; i++)
        //    {
        //        tanggalPesan = Convert.ToDateTime(bulan + "/" + dataGridView3.Columns[i].Name.ToString() + "/" + tahun);
        //        sql = new SqlCommand("select kamar_no, status from Reservasi where checkin <=@id and checkout >= @id and (status='booking' or status='checkin')", koneksi.KoneksiDB());
        //        sql.Parameters.AddWithValue("@id", tanggalPesan);
        //        reader = sql.ExecuteReader();
        //        while (reader.Read())
        //        {
        //            int nilai = 0;
        //            foreach (DataGridViewRow row in this.dataGridView3.Rows)
        //            {
        //                if (row.Cells[0].Value.ToString().Equals(reader["kamar_no"].ToString()) && reader["status"].ToString().Equals("booking"))
        //                {
        //                    dataGridView3.Rows[nilai].Cells[i].Style.BackColor = Color.Red;
        //                }
        //                else if (row.Cells[0].Value.ToString().Equals(reader["kamar_no"].ToString()) && reader["status"].ToString().Equals("checkin"))
        //                {
        //                    dataGridView3.Rows[nilai].Cells[i].Style.BackColor = Color.Green;

        //                }
        //                nilai += 1;
        //            }
        //        }
        //        koneksi.KoneksiDB().Close();
        //    }


        //}

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
                "                        Dipesan oleh "+ reader.GetString(0) +"\r\n" +
                "----------------------------------------------------------------------------\r\n" +
                "Kamar " + NoKamarInfo.ToString() +" Checkin " + reader.GetDateTime(1).ToString("dd/MMM/yyyy") + " Checkout " + reader.GetDateTime(2).ToString("dd/MMM/yyyy") + "\r\n" +
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

             DateTime tanggalPesan1 = Convert.ToDateTime(TglBulan + "/" + dataGridView3.Columns[columnSelect].Name.ToString() + "/" + Tgltahun);
                
             int NoKamarInfo = Int32.Parse(dataGridView3.Rows[rowSelect].Cells[0].Value.ToString());
                    SqlCommand sqlq = new SqlCommand("select Reservasi.reservasi_id from Reservasi, Tamu where Tamu.tamu_id = Reservasi.tamu_id and Reservasi.checkin <=@id and Reservasi.checkout >= @id and Reservasi.kamar_no=@nok and (Reservasi.status='booking')", koneksi.KoneksiDB());
                    sqlq.Parameters.AddWithValue("@id", tanggalPesan1);
                    sqlq.Parameters.AddWithValue("@nok", NoKamarInfo);
            
                    string reservasiKamar = sqlq.ExecuteScalar().ToString();
            
            SqlCommand sql = new SqlCommand("update Reservasi set status= 'checkin' where reservasi_id =@id", koneksi.KoneksiDB());
            sql.Parameters.AddWithValue("@id",reservasiKamar);
            sql.ExecuteNonQuery();
            koneksi.KoneksiDB().Close();

            setLoad(TglBulan, Tgltahun);
            
        }

        private void btnDaftarTamu_Click_1(object sender, EventArgs e)
        {

        }
        DataTable dtPesan = new DataTable();
        
        private void MunculKan(object sender, EventArgs e)
        {

            Button btn = sender as Button;
            contextMenuStrip2.Show(Cursor.Position);
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
            panel1.BringToFront();
        }

        private void lblClose_Click(object sender, EventArgs e)
        {
            PanelPesan.BringToFront();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label21_Click(object sender, EventArgs e)
        {
            panel2.BringToFront();
            SqlDataAdapter da = new SqlDataAdapter("select Item.item_id, Item.item, Item_Tipe.item_tipe, Item.harga from Item, Item_Tipe where Item.item_tipe_id = Item_Tipe.item_tipe_id", koneksi.KoneksiDB());
            DataTable ds = new DataTable();
            da.Fill(ds);
            dataGridView5.DataSource = ds;
            koneksi.KoneksiDB().Close();
        }

        private void label26_Click(object sender, EventArgs e)
        {

            SqlDataAdapter da = new SqlDataAdapter("select Item.item_id, Item.item, Item_Tipe.item_tipe, Item.harga from Item, Item_Tipe where Item.item_tipe_id = Item_Tipe.item_tipe_id and Item.item like @nama ", koneksi.KoneksiDB());
            da.SelectCommand.Parameters.Add(new SqlParameter("@nama", string.Format("%{0}%", TxtCust.Text)));
            DataTable ds = new DataTable();
            da.Fill(ds);
            dataGridView5.DataSource = ds;
            koneksi.KoneksiDB().Close();
        }

        private void label25_Click(object sender, EventArgs e)
        {
            panel1.BringToFront();
        }

        private void dataGridView5_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
           xPenang.Text = dataGridView5.Rows[dataGridView5.CurrentRow.Index].Cells[0].Value.ToString();
           label15.Text = dataGridView5.Rows[dataGridView5.CurrentRow.Index].Cells[1].Value.ToString();
           label3.Text = dataGridView5.Rows[dataGridView5.CurrentRow.Index].Cells[3].Value.ToString();
           panel1.BringToFront();
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
            //// Compare the column to the column you want to format
            //if (this.dataGridView3.Columns[e.ColumnIndex].Name == "ColumnName")
            //{
            //    //get the IChessitem you are currently binding, using the index of the current row to access the datasource
            //    //check the condition
            //    if (item == condition)
            //    {
            //        e.CellStyle.BackColor = Color.Green;
            //    }
            //}
        }

        private void btnPengaturanKamar_Click(object sender, EventArgs e)
        {
            DataKamar dataKamar = new DataKamar();
            dataKamar.TopLevel = false;
            //panelPengaturanKamar.BringToFront();
            splitContainer2.Panel1.Controls.Clear();
            splitContainer2.Panel1.Controls.Add(dataKamar);
            dataKamar.Show();
            dataKamar.Dock = DockStyle.Fill;
            dataKamar.BringToFront();
        }
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