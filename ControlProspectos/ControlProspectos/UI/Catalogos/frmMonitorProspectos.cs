using ControlProspectos.Entidades;
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
    public partial class frmMonitorProspectos : Form
    {

        private List<Prospecto> listaProspectos = new List<Prospecto>();
        private DataTable table;
        private int activos = 1, autorizado = 2, rechazado = 3, estatusselected = 0;

        
        public frmMonitorProspectos()
        {
            InitializeComponent();
            TraerProspectos(activos);
        }

        private void inicializaGrid()
        {
            table = new DataTable();
            table.Columns.Add("id", typeof(int));
            table.Columns.Add("Nombre Completo", typeof(string));


            if (dataGridView1.DataSource != null)
            {
                dataGridView1.DataSource = null;
                dataGridView1.Rows.Clear();
                dataGridView1.Columns.Clear();
                dataGridView1.Refresh();
            }

            foreach (var prospecto in listaProspectos) {
                DataRow row = table.NewRow();
                row["id"] = prospecto.prospectoId;
                row["Nombre Completo"] = prospecto.nombre + " " + prospecto.apellidoPaterno + " " + prospecto.apellidoMaterno;
                table.Rows.Add(row);
            }

            dataGridView1.DataSource = table;
            dataGridView1.ScrollBars = ScrollBars.Both;



            DataGridViewButtonColumn btn = new DataGridViewButtonColumn();
            dataGridView1.Columns.Add(btn);
            btn.HeaderText = "Detalle";
            btn.Text = "Ver";
            btn.Name = "btnVerDetalle";
            btn.UseColumnTextForButtonValue = true;
            btn.FlatStyle = FlatStyle.Popup;


            //DataGridViewButtonColumn btn2 = new DataGridViewButtonColumn();
            //dataGridView1.Columns.Add(btn2);
            //btn2.HeaderText = "Autorizar";
            //btn2.Text = "Autorizar";
            //btn2.Name = "btnAutorizar";
            //btn2.UseColumnTextForButtonValue = true;
            //btn2.FlatStyle = FlatStyle.Popup;

            dataGridView1.Refresh();

        }

        private void radioButton1_Click(object sender, EventArgs e)
        {
            TraerProspectos(activos);
        }

        private void radioButton2_Click(object sender, EventArgs e)
        {
            TraerProspectos(autorizado);
        }

        private void radioButton3_Click(object sender, EventArgs e)
        {
            TraerProspectos(rechazado);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    if (e.ColumnIndex == dataGridView1.Columns["btnVerDetalle"].Index)
                    {
                        if (dataGridView1.Rows[e.RowIndex].Cells["id"].Value.ToString() != string.Empty)
                        {
                            Prospecto item = listaProspectos.Find(x => x.prospectoId == (int) dataGridView1.Rows[e.RowIndex].Cells["id"].Value);

                            frmProspectos formulario = new frmProspectos(item);
                            formulario.Owner = this;
                            formulario.StartPosition = FormStartPosition.CenterParent;
                            formulario.ShowDialog();
                            TraerProspectos(estatusselected);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrio un error al cargar los elementos del detalle, Error: " + ex.Message);
            }
        }

        public void TraerProspectos(int estatus) {
            listaProspectos = DAL.ProspectosDAL.TraerProspectos(null, estatus);
            estatusselected = estatus;
            inicializaGrid();

        }

        private void frmMonitorProspectos_Load(object sender, EventArgs e)
        {

        }
    }
}
