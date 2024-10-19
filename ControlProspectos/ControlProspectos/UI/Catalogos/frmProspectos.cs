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
    public partial class frmProspectos : Form
    {

        private List<TiposDocumentos> TiposDeDocumentos = new List<TiposDocumentos>();

        private Prospecto prospecto = null;

        private List<ProspectoAdjunto> listaAdjuntos = new List<ProspectoAdjunto>();

        

        private DataTable table;
        public frmProspectos(Prospecto prospecto)
        {
            InitializeComponent();
            inicializarControles();
            habilitarControles(false);
            if (prospecto != null) {
                this.prospecto = prospecto;
                muestraDatosConsulta();
            }
        }



        public void inicializarControles() {
            TiposDeDocumentos = ControlProspectos.DAL.TiposDocumentosDAL.TraerTiposDocumentos();
            cmbTipoDocumento.DataSource = TiposDeDocumentos;
            cmbTipoDocumento.ValueMember = "tipoDocumento";
            cmbTipoDocumento.SelectedValue = "tipoDocumentoId";
            cmbTipoDocumento.Refresh();
        }


        public void muestraDatosConsulta()
        {

            txtNombre.Text = prospecto.nombre;
            txtApellidoPaterno.Text = prospecto.apellidoPaterno;
            txtApellidoMaterno.Text = prospecto.apellidoMaterno;
            txtRFC.Text = prospecto.rfc;
            txtTelefono.Text = prospecto.telefono;
            txtCalle.Text = prospecto.calle;
            txtNumero.Text = prospecto.numero;
            txtColonia.Text = prospecto.colonia;
            txtCodigoPostal.Text = prospecto.codigoPostal;

            btnAgregar.Visible = false;
            btnEnviar.Visible = false;
            btnLimpiar.Visible = false;
            btnNuevaCaptura.Visible = false;

            btnAutorizar.Visible = false;
            btnRechazar.Visible = false;


            if (prospecto.estatus == 1) {

                btnAutorizar.Visible = true;
                btnRechazar.Visible = true;
                
            }

            lblEstatus.Visible = true;
            switch (prospecto.estatus) {
                case 1: 
                    lblEstatus.Text = "Estatus: Pendiente de autorizar";
                    break;
                case 2:
                    lblEstatus.Text = "Estatus: Autorizado";
                    break;

                case 3:
                    lblEstatus.Text = "Estatus: Rechazado" + Environment.NewLine + "Observaciones: " + prospecto.observaciones;
                    break;


            }

            listaAdjuntos = DAL.ProspectosAdjuntosDAL.TraerAdjuntosPorProspecto(prospecto.prospectoId);

            inicializaGridDocumentos();

            foreach (var adjunto in listaAdjuntos) {
                DataRow row = table.NewRow();
                row["Documento"] = adjunto.tipoDocumento;
                row["Ruta"] = adjunto.pathFile;
                table.Rows.Add(row);
            }

            

            if (dtgDocumentos.DataSource != null)
            {
                dtgDocumentos.DataSource = null;
                dtgDocumentos.Rows.Clear();
                dtgDocumentos.Columns.Clear();
                dtgDocumentos.Refresh();
            }


            dtgDocumentos.DataSource = table;
            dtgDocumentos.ScrollBars = ScrollBars.Both;

            dtgDocumentos.Refresh();

        }

        public void habilitarControles(bool enabled) {
            txtNombre.Enabled = enabled;
            txtApellidoPaterno.Enabled = enabled;
            txtApellidoMaterno.Enabled = enabled;
            txtRFC.Enabled = enabled;
            txtTelefono.Enabled = enabled;
            txtCalle.Enabled = enabled;
            txtNumero.Enabled = enabled;
            txtColonia.Enabled = enabled;
            txtCodigoPostal.Enabled = enabled;       
            cmbTipoDocumento.Enabled = enabled;
            btnAgregar.Enabled = enabled;
            btnEnviar.Enabled = enabled;

        }

        private void inicializaGridDocumentos()
        {
            table = new DataTable();
            table.Columns.Add("Documento", typeof(string));
            table.Columns.Add("Ruta", typeof(string));


            if (dtgDocumentos.DataSource != null)
            {
                dtgDocumentos.DataSource = null;
                dtgDocumentos.Rows.Clear();
                dtgDocumentos.Columns.Clear();
                dtgDocumentos.Refresh();
            }

        }

        public void limpiarPantalla()
        {
            txtNombre.Text = "";
            txtApellidoPaterno.Text = "";
            txtApellidoMaterno.Text = "";
            txtRFC.Text = "";
            txtTelefono.Text = "";
            txtCalle.Text = "";
            txtNumero.Text = "";
            txtColonia.Text = "";
            txtCodigoPostal.Text = "";
            btnAgregar.Enabled = false;
            btnEnviar.Enabled = false;
            prospecto = null;
            listaAdjuntos = new List<ProspectoAdjunto>();
            table = null;
            inicializaGridDocumentos();
            btnNuevaCaptura.Visible = true;
            cmbTipoDocumento.SelectedIndex = -1;
            btnLimpiar.Enabled = false;
            habilitarControles(false);


    }



        private void btnNuevaCaptura_Click(object sender, EventArgs e)
        {
            habilitarControles(true);
            inicializaGridDocumentos();
            txtNombre.Focus();
            btnNuevaCaptura.Hide();
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {

            string tipoDocumento = cmbTipoDocumento.SelectedValue.ToString();
            
            bool documentoAgregado = false;
            foreach (DataRow doc in table.Rows) {
                if (doc["Documento"].ToString().Trim().Equals(tipoDocumento)) {
                    documentoAgregado = true;
                }
            }

            if (documentoAgregado) {
                MessageBox.Show ("El tipo de documento ya fue agregado");
                return;
            }



            openFileDialog1.ShowDialog();
            string filename = openFileDialog1.FileName;
            byte[] bytesfileUpload = System.IO.File.ReadAllBytes(filename);
            string extension = filename.Split('.')[filename.Split('.').Length - 1];
            string fileNameSave = Guid.NewGuid().ToString();
            fileNameSave += "." + extension;



            DataRow row = table.NewRow();
            row["Documento"] = tipoDocumento;
            row["Ruta"] = filename;
            table.Rows.Add(row);

            if (dtgDocumentos.DataSource != null)
            {
                dtgDocumentos.DataSource = null;
                dtgDocumentos.Rows.Clear();
                dtgDocumentos.Columns.Clear();
                dtgDocumentos.Refresh();
            }


            dtgDocumentos.DataSource = table;
            dtgDocumentos.ScrollBars = ScrollBars.Both;
       
            DataGridViewButtonColumn btn = new DataGridViewButtonColumn();
            dtgDocumentos.Columns.Add(btn);
            btn.HeaderText = "Eliminar";
            btn.Text = "X";
            btn.Name = "btnEliminar";
            btn.UseColumnTextForButtonValue = true;
            btn.FlatStyle = FlatStyle.Popup;

            dtgDocumentos.Refresh();
        }

        private void btnEnviar_Click(object sender, EventArgs e)
        {
            if (isDataProspectoCorrect()) {

                setDataProspecto();                
                DAL.ProspectosDAL.GuardarProspecto(prospecto);
                setDataAdjuntos();
                DAL.ProspectosAdjuntosDAL.EliminarProspectoAdjunto(prospecto.prospectoId);

                foreach (var prospectoAdjunto in listaAdjuntos) {
                    DAL.ProspectosAdjuntosDAL.GuardarProspectoAdjunto(prospectoAdjunto);
                }
                
                MessageBox.Show("Datos Guardados");

                limpiarPantalla();
            }

            

        }

        public bool isDataProspectoCorrect() {


            if (txtNombre.Text.Trim().Length == 0) {
                MessageBox.Show("Capture el nombre del prospecto");
                txtNombre.Focus();
                return false;
            }

            if (txtApellidoPaterno.Text.Trim().Length == 0)
            {
                MessageBox.Show("Capture el apellido paterno del prospecto");
                txtApellidoPaterno.Focus();
                return false;
            }

            if (txtCalle.Text.Trim().Length == 0)
            {
                MessageBox.Show("Capture la calle del prospecto");
                txtCalle.Focus();
                return false;
            }

            if (txtTelefono.Text.Trim().Length == 0)
            {
                MessageBox.Show("Capture el telefono del prospecto");
                txtTelefono.Focus();
                return false;
            }

            if (txtRFC.Text.Trim().Length == 0)
            {
                MessageBox.Show("Capture el RFC del prospecto");
                txtRFC.Focus();
                return false;
            }


            if (txtNumero.Text.Trim().Length == 0)
            {
                MessageBox.Show("Capture el numero del prospecto");
                txtNumero.Focus();
                return false;
            }

            if (txtColonia.Text.Trim().Length == 0)
            {
                MessageBox.Show("Capture la colonia del prospecto");
                txtColonia.Focus();
                return false;
            }

            if (txtCodigoPostal.Text.Trim().Length == 0)
            {
                MessageBox.Show("Capture el codigo postal del prospecto");
                txtCodigoPostal.Focus();
                return false;
            }


            if (dtgDocumentos.Rows.Count == 0) {
                MessageBox.Show("Debe capturar al menos 1 documento oficial del prospecto");
                cmbTipoDocumento.Focus();
                return false;
            }

           

            return true;
        
        }

        public void setDataProspecto() {
            if (prospecto == null) {
                prospecto = new Prospecto();
                prospecto.prospectoId = 0;
                prospecto.ultimaAct = 0;
                prospecto.observaciones = "";
            }
            prospecto.nombre = txtNombre.Text;
            prospecto.apellidoPaterno = txtApellidoPaterno.Text;
            prospecto.apellidoMaterno = txtApellidoMaterno.Text;
            prospecto.rfc = txtRFC.Text;
            prospecto.telefono = txtTelefono.Text;
            prospecto.calle = txtCalle.Text;
            prospecto.numero = txtNumero.Text;
            prospecto.colonia = txtColonia.Text;
            prospecto.codigoPostal = txtCodigoPostal.Text;
            prospecto.estatus = 1;
        }

        public void setDataAdjuntos() {

            listaAdjuntos = new List<ProspectoAdjunto>();

            foreach (DataRow doc in table.Rows) {

                string filename = doc["Ruta"].ToString();
                byte[] bytesfileUpload = System.IO.File.ReadAllBytes(filename);
                string extension = filename.Split('.')[filename.Split('.').Length - 1];
                string fileNameSave = Guid.NewGuid().ToString();
                fileNameSave += "." + extension;

                ProspectoAdjunto prospectoAdjunto = new ProspectoAdjunto();
                prospectoAdjunto.prospectoAdjuntoId = 0;
                prospectoAdjunto.prospectoId = prospecto.prospectoId;
                prospectoAdjunto.tipoDocumento = doc["Documento"].ToString(); 
                prospectoAdjunto.pathFile = fileNameSave;
                prospectoAdjunto.estatus = 1;
                prospectoAdjunto.fechaCreacion = DateTime.Now;
                prospectoAdjunto.fileUpload = bytesfileUpload;
                prospectoAdjunto.ultimaAct = 0;

                listaAdjuntos.Add(prospectoAdjunto);
            }

            


            
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            limpiarPantalla();
        }

        private void btnAutorizar_Click(object sender, EventArgs e)
        {
            var confirmResult = MessageBox.Show(
                    "                        ¿Desea realizar la autorizacion del prospecto?"
                    , "Confirmar", MessageBoxButtons.YesNo);
            if (confirmResult == DialogResult.Yes)
            {

                prospecto.estatus = 2;

                DAL.ProspectosDAL.GuardarProspecto(prospecto);
                MessageBox.Show("Autorizado");
                this.Close();


            }
        }

        private void btnRechazar_Click(object sender, EventArgs e)
        {
            var confirmResult = MessageBox.Show(
                    "                        ¿Desea realizar el rechazo del prospecto?"
                    , "Confirmar", MessageBoxButtons.YesNo);
            if (confirmResult == DialogResult.Yes)
            {


                frmObservaciones formulario = new frmObservaciones(prospecto.observaciones);
                formulario.Owner = this;
                formulario.StartPosition = FormStartPosition.CenterParent;
                formulario.ShowDialog();
                prospecto.observaciones = formulario.observaciones;
                prospecto.estatus = 3;



                DAL.ProspectosDAL.GuardarProspecto(prospecto);
                MessageBox.Show("Rechazado");
                this.Close();


            }
        }
    }
}
