using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge.Video;
using AForge.Video.DirectShow;
using ZXing;


namespace Qrcode
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            dataGridViewList.ColumnCount = 5;
            dataGridViewList.Columns[0].Name = "ID";
            dataGridViewList.Columns[1].Name = "MachineName";
            dataGridViewList.Columns[2].Name = "Operator";
            dataGridViewList.Columns[3].Name = "StartTime";
            dataGridViewList.Columns[4].Name = "EndTime";

            dataGridViewList.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewList.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewList.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewList.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewList.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;


        }

        FilterInfoCollection filterInfoCollection;
        VideoCaptureDevice videoCaptureDevice;

     


        //start button
        private void button1_Click(object sender, EventArgs e)
        {
            videoCaptureDevice = new VideoCaptureDevice(filterInfoCollection[cbCamera.SelectedIndex].MonikerString);

            videoCaptureDevice.NewFrame += VideoCaptureDevice_NewFrame;
            videoCaptureDevice.Start();

        }

        private void VideoCaptureDevice_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            //throw new NotImplementedException();
            //
            Bitmap bitmap = (Bitmap)eventArgs.Frame.Clone();
            BarcodeReader reader = new BarcodeReader(); // tao doi tuong doc barcode
            var result = reader.Decode(bitmap); //doc qr code
            if (result != null)
            {
                txbCode.Invoke(new MethodInvoker(delegate ()
                {
                    txbCode.Text = result.ToString(); // hien thi ket qua doc duoc len textbox

                }
                ));

            }
            pictureBox1.Image = bitmap; // hien thi frame hinh anh len tren pictureBox

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (videoCaptureDevice != null)
            {
                if (videoCaptureDevice.IsRunning)
                {
                    videoCaptureDevice.Stop();
                    pictureBox1.Image = null;
                }
            }

        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (videoCaptureDevice != null)
            {
                if (videoCaptureDevice.IsRunning)
                {
                    videoCaptureDevice.Stop();
                    pictureBox1.Image = null;
                }
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //lay ds tat ca cac thiet bi video dang dc ket noi
            filterInfoCollection = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            //them ten cua moi thiet bi vao dropdow cua combobox
            foreach (FilterInfo device in filterInfoCollection)
                cbCamera.Items.Add(device.Name);
            cbCamera.SelectedIndex = 0;

        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Lấy nội dung từ TextBox1
            string inputText = txbCode.Text;

            // Tách chuỗi đầu vào thành các phần
            string[] parts = inputText.Split(',');
            if (parts.Length != 5)
            {
                MessageBox.Show("Dữ liệu đầu vào không hợp lệ!");
                return;
            }

            // Xử lý dữ liệu
            string id = parts[0].Trim();
            string machine = parts[1].Trim();
            string name = parts[2].Trim();
            string start = parts[3].Trim();
            string end = parts[4].Trim();


            int yearS = int.Parse(start.Substring(0, 4));
            int monthS = int.Parse(start.Substring(4, 2));
            int dayS = int.Parse(start.Substring(6, 2));
            int hourS = int.Parse(start.Substring(8, 2));
            int minuteS = int.Parse(start.Substring(10, 2));
            int secondS = int.Parse(start.Substring(12, 2));

            DateTime datetimeStart = new DateTime(yearS, monthS, dayS, hourS, minuteS, secondS);
            string datetimeStartString = datetimeStart.ToString("dd/MM/yyyy HH:mm:ss");



            int yearE = int.Parse(end.Substring(0, 4));
            int monthE = int.Parse(end.Substring(4, 2));
            int dayE = int.Parse(end.Substring(6, 2));
            int hourE = int.Parse(end.Substring(8, 2));
            int minuteE = int.Parse(end.Substring(10, 2));
            int secondE = int.Parse(end.Substring(12, 2));

            DateTime datetimeEnd = new DateTime(yearE, monthE, dayE, hourE, minuteE, secondE);
            string datetimeEndString = datetimeEnd.ToString("dd/MM/yyyy HH:mm:ss");

         



            // Tạo chuỗi kết quả
            //string result = $"ID: {id}\nMáy: {machine}\nTên: {name}";

            // Hiển thị kết quả trong TextBox2
            //display.Text = result;

            //
            //addRow(id, machine, name, datetimeStartString, datetimeEndString);
            addRow(id, machine, name, datetimeStartString, datetimeEndString);


        }

        private void addRow(string id, string machine, string name, string datetimeStartString, string datetimeEndString)
        {
            String[] row = { id, machine, name, datetimeStartString, datetimeEndString };
            dataGridViewList.Rows.Add(row);   

        }

        private void cbCamera_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void txbCode_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
