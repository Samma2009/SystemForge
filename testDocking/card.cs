using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace testDocking
{
    public partial class card : Form
    {
        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HTCAPTION = 0x2;
        private const int HTBOTTOMRIGHT = 17;

        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        private static extern bool ReleaseCapture();

        public card()
        {
            InitializeComponent();
            TransparencyKey = Color.Black;
            Size = new Size(1007/2, 623/2);
            Opacity = 0;
        }

        private void card_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(this.Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Opacity += 0.1f;
            if (Opacity >= 1)
            {
                timer1.Stop();
                Thread.Sleep(1000);
                Hide();
                new Form1().ShowDialog();
                Close();
            }
        }
    }
}
