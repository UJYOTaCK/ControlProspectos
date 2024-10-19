using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ControlProspectos.UI.Menu
{
    public partial class MenuPrincipal : Form
    {
        public MenuPrincipal()
        {
            InitializeComponent();
        }

        private void prospectosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {

                ControlProspectos.UI.Catalogos.frmProspectos frmComponent = new ControlProspectos.UI.Catalogos.frmProspectos(null);
                frmComponent.MdiParent = this;
                frmComponent.StartPosition = FormStartPosition.Manual;
                frmComponent.Location = new Point((this.ClientSize.Width - frmComponent.Width) / 2,
                       (this.ClientSize.Height - frmComponent.Height) / 2);
                frmComponent.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al instanciar el menu: Revisor versiones, Error: " + ex.Message);
            }
        }

        private void documentacionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {

                ControlProspectos.UI.Catalogos.frmMonitorProspectos frmComponent = new ControlProspectos.UI.Catalogos.frmMonitorProspectos();
                frmComponent.MdiParent = this;
                frmComponent.StartPosition = FormStartPosition.Manual;
                frmComponent.Location = new Point((this.ClientSize.Width - frmComponent.Width) / 2,
                       (this.ClientSize.Height - frmComponent.Height) / 2);
                frmComponent.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al instanciar el menu: Revisor versiones, Error: " + ex.Message);
            }
        }
    }
}
