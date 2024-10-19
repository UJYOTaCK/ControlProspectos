using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ControlProspectos.UI.Catalogos
{
    public partial class frmObservaciones : Form
    {

        public string observaciones = "";
        public frmObservaciones(string obs)
        {
            InitializeComponent();
            observaciones = obs;
            textBox1.Text = observaciones;
        }

        private void button1_Click(object sender, EventArgs e)
        {

            observaciones = textBox1.Text;
            this.Close();

        }
    }
}
