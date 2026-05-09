using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SafeY0u
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            

            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            Form1 f = new Form1();
            f.Show();
            this.Hide();
        }
    }
}
