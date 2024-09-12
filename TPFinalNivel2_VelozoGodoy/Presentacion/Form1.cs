using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Negocio;
using Dominio;
using Helpers;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.IO;
using System.Configuration;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace Presentacion
{
    public partial class gestionArticulos : Form
    {
        private int boton;

        public void moverPanel(Button btn, Panel panel)
        {
            panel.Height = btn.Height;
            panel.Top = btn.Top;
        }

        private void btn_MouseEnter(object sender, EventArgs e)
        {

        }
        public gestionArticulos()
        {
            InitializeComponent();
            panelMovible.Height = btnAgregar.Height;
            panelMovible.Top = btnAgregar.Top;
            dgvLector.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvLector.RowsDefaultCellStyle.ForeColor = Color.Gainsboro;
            dgvLector.RowsDefaultCellStyle.BackColor = Color.FromArgb(38, 40, 47);
            this.Size = new System.Drawing.Size(1060, 580);
            boton = btnBuscar.Location.Y;

            //Para que se mueva el panel chiquito entre los botones
            btnModificar.MouseEnter += btn_MouseEnter;
            btnAgregar.MouseEnter += btn_MouseEnter;
            btnEliminar.MouseEnter += btn_MouseEnter;
            btnDetalles.MouseEnter += btn_MouseEnter;
            btnEliminados.MouseEnter += btn_MouseEnter;
            btnRestaurar.MouseEnter += btn_MouseEnter;
            btnEliminarFisico.MouseEnter += btn_MouseEnter;
            btnVolver.MouseEnter += btn_MouseEnter;

            //Para que se mueva todo el formulario
            Control[] controles = { panel1 };
            Ayuda.AsociarEventoMouseDown(controles, Control_MouseDown);

            dgvLector.TabIndex = 0;
        }
        private ArticuloNegocio negocio = new ArticuloNegocio();
        private Ayuda helper = new Ayuda();
        private void Control_MouseDown(object sender, MouseEventArgs e)
        {
            helper.moverFormulario(this);

        }

        private void gestionArticulos_Load(object sender, EventArgs e)
        {
            helper.mostrarLector(dgvLector, pbxIMG);
            cbxCampo.Items.Add("Nombre");
            cbxCampo.Items.Add("Marca");
            cbxCampo.Items.Add("Categoria");
            cbxCampo.Items.Add("Precio");
        }

        private void dgvLector_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvLector.CurrentRow != null)
            {
                Articulos seleccionado = (Articulos)dgvLector.CurrentRow.DataBoundItem;
                helper.mostrarImagen(pbxIMG, seleccionado.UrlImagen);
            }
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            if (dgvLector.CurrentRow != null)
            {
                Articulos seleccionado = (Articulos)dgvLector.CurrentRow.DataBoundItem;
                Agregar_Modificar modificar = new Agregar_Modificar(seleccionado);
                modificar.ShowDialog();
                helper.mostrarLector(dgvLector, pbxIMG);
            }
            else
                MessageBox.Show("No hay nada seleccionado");
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {

            if (dgvLector.CurrentRow != null)
            {
                Articulos seleccionado = (Articulos)dgvLector.CurrentRow.DataBoundItem;
                borrarLogico(seleccionado);
            }
            else
                MessageBox.Show("No hay nada seleccionado");
        }

        private void btnAgregar_Click_1(object sender, EventArgs e)
        {
            Agregar_Modificar agregar = new Agregar_Modificar();
            agregar.ShowDialog();
            helper.mostrarLector(dgvLector, pbxIMG);
        }

        private void btnDetalles_Click(object sender, EventArgs e)
        {
            if (dgvLector.CurrentRow != null)
            {
                Articulos seleccionado = (Articulos)dgvLector.CurrentRow.DataBoundItem;
                Detalle detalles = new Detalle(seleccionado);
                detalles.ShowDialog();
            }
            else
                MessageBox.Show("No hay nada seleccionado");
        }

        private void btnMinimizar_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }


        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }


        private void btnMas_Click(object sender, EventArgs e)
        {

            //hago modificaciones en el formulario
            this.Size = new Size(1060, 630);
            btnMenos.Visible = true;
            btnMas.Visible = false;
            btnBuscar.Location = new Point(753, 75);
            cbxCampo2.Enabled = true;
            cbxCriterio2.Enabled = true;
            btnMenos.Enabled = true;
            txtBuscarAvz2.Enabled = true;
            cbxCampo2.Items.Clear();
            cbxCampo2.Items.Add("Nombre");
            cbxCampo2.Items.Add("Marca");
            cbxCampo2.Items.Add("Categoria");
            cbxCampo2.Items.Add("Precio");
            btnBuscar.TabIndex = 29;

        }
        private void btnMenos_Click(object sender, EventArgs e)
        {

            //vuelvo el formulario como nace
            this.Size = new Size(1060, 580);
            btnBuscar.Location = new Point(753, 50);

            btnMas.Visible = true;
            btnMenos.Visible = false;
            btnBuscar.TabIndex = 24;
            cbxCampo2.Enabled = false;
            cbxCriterio2.Enabled = false;
            btnMenos.Enabled = false;
            txtBuscarAvz2.Enabled = false;
        }

        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            List<Articulos> listaFiltrada;
            if (txtBuscar.Text != "")
                listaFiltrada = helper.listaArticulos.FindAll(x => x.Nombre.ToUpper().Contains(txtBuscar.Text.ToUpper()) || x.Marca.ToString().ToUpper().Contains(txtBuscar.Text.ToUpper()) || x.Categoria.ToString().ToUpper().Contains(txtBuscar.Text.ToUpper()));
            else
                listaFiltrada = helper.listaArticulos;
            dgvLector.DataSource = null;
            dgvLector.DataSource = listaFiltrada;
            helper.ocultarColumnas(dgvLector);
            helper.ocultarPbxIMG(dgvLector, pbxIMG);
        }

        private void cbxCampo_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbxCriterio.Items.Clear();
            cbxCriterio.TabIndex = 0;
            if (cbxCampo.Text == "Precio")
            {
                cbxCriterio.Items.Add("Mayor que");
                cbxCriterio.Items.Add("Menor que");
                cbxCriterio.Items.Add("Igual a");
            }
            else
            {
                cbxCriterio.Items.Add("Empieza por");
                cbxCriterio.Items.Add("Termina con");
                cbxCriterio.Items.Add("Contiene");
            }
        }

        private void cbxCampo2_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbxCriterio2.Items.Clear();
            if (cbxCampo2.Text == "Precio")
            {
                cbxCriterio2.Items.Add("Mayor que");
                cbxCriterio2.Items.Add("Menor que");
                cbxCriterio2.Items.Add("Igual a");
            }
            else
            {
                cbxCriterio2.Items.Add("Empieza por");
                cbxCriterio2.Items.Add("Termina con");
                cbxCriterio2.Items.Add("Contiene");
            }
        }
        private void btnBuscar_Click(object sender, EventArgs e)
        {
            if (boton == btnBuscar.Location.Y)
            {
                if (helper.validarFiltro(cbxCampo, cbxCriterio, txtBuscarAvz))
                    return;

                string campo = cbxCampo.SelectedItem.ToString();
                string criterio = cbxCriterio.SelectedItem.ToString();
                string filtro = txtBuscarAvz.Text;
                dgvLector.DataSource = negocio.buscar(campo, criterio, filtro);
                helper.ocultarPbxIMG(dgvLector, pbxIMG);
            }
            else
            {
                if (helper.validarFitros(cbxCampo, cbxCriterio, txtBuscarAvz, cbxCampo2, cbxCriterio2, txtBuscarAvz2))
                    return;
                string campo = cbxCampo.SelectedItem.ToString();
                string criterio = cbxCriterio.SelectedItem.ToString();
                string filtro = txtBuscarAvz.Text; ;
                string campo2 = cbxCampo2.SelectedItem.ToString();
                string criterio2 = cbxCriterio2.SelectedItem.ToString();
                string filtro2 = txtBuscarAvz2.Text;
                dgvLector.DataSource = negocio.buscarMas(campo, campo2, criterio, criterio2, filtro, filtro2);
                helper.ocultarPbxIMG(dgvLector, pbxIMG);
            }

        }

        public void ocultarEnEliminados()
        {
            lblBuscar.Visible = false;
            lblBuscarAvz.Visible = false;
            lblBuscarAvz2.Visible = false;
            lblCampo.Visible = false;
            lblCampo2.Visible = false;
            lblCriterio.Visible = false;
            lblCriterio2.Visible = false;
            cbxCampo.Visible = false;
            cbxCriterio2.Visible = false;
            txtBuscarAvz2.Visible = false;
            txtBuscar.Visible = false;
            txtBuscarAvz2.Visible = false;
            lblCriterio.Visible = false;
            cbxCampo.Visible = false;
            btnAgregar.Visible = false;
            btnBuscar.Visible = false;
            btnEliminar.Visible = false;
            btnMas.Visible = false;
            btnModificar.Visible = false;
            cbxCriterio.Visible = false;
            txtBuscarAvz.Visible = false;
        }
        public void mostrarTodo()
        {
            lblBuscar.Visible = true;
            lblBuscarAvz.Visible = true;
            lblBuscarAvz2.Visible = true;
            lblCampo.Visible = true;
            lblCampo2.Visible = true;
            lblCriterio.Visible = true;
            lblCriterio2.Visible = true;
            cbxCampo.Visible = true;
            cbxCriterio2.Visible = true;
            txtBuscarAvz2.Visible = true;
            txtBuscar.Visible = true;
            txtBuscarAvz2.Visible = true;
            lblCriterio.Visible = true;
            cbxCampo.Visible = true;
            btnAgregar.Visible = true;
            btnBuscar.Visible = true;
            btnEliminar.Visible = true;
            btnMas.Visible = true;
            btnModificar.Visible = true;
            cbxCriterio.Visible = true;
            txtBuscarAvz.Visible = true;
        }

        private void btnEliminados_Click(object sender, EventArgs e)
        {
            ArticuloNegocio eliminados = new ArticuloNegocio();
            dgvLector.DataSource = null;
            dgvLector.DataSource = eliminados.eliminados();
            ocultarEnEliminados();
            btnRestaurar.Visible = true;
            btnDetalles.Location = new Point(4, 139);
            btnEliminados.Location = new Point(4, 212);
            btnRestaurar.Location = new Point(4, 285);
            btnEliminarFisico.Visible = true;
            helper.ocultarColumnas(dgvLector);
            this.Size = new System.Drawing.Size(1060, 452);
            panelMovible.Top = btnDetalles.Top;
            btnVolver.Visible = true;
            btnEliminados.Visible = false;
            helper.ocultarPbxIMG(dgvLector, pbxIMG);
            if (dgvLector.RowCount == 0)
            {
                dgvLector.Visible = false;
                MessageBox.Show("No hay nada para mostrar");
                sinNadaEnElLector();
            }

        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
            sinNadaEnElLector();

        }

        private void btnRestaurar_Click(object sender, EventArgs e)
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            if (dgvLector.CurrentRow != null)
            {
                Articulos seleccionado = (Articulos)dgvLector.CurrentRow.DataBoundItem;
                negocio.restaurar(seleccionado.Id);
                dgvLector.DataSource = null;
                dgvLector.DataSource = negocio.eliminados();
                dgvLector.Columns["Id"].Visible = false;
                dgvLector.Columns["Codigo"].Visible = false;
                dgvLector.Columns["UrlImagen"].Visible = false;
                MessageBox.Show("Restaurado con exito");
                if (dgvLector.RowCount == 0)
                {
                    dgvLector.Visible = false;
                    MessageBox.Show("No hay nada para mostrar");
                    sinNadaEnElLector();
                }
            }
        }
        public void sinNadaEnElLector()
        {
            dgvLector.Visible = true;
            helper.mostrarLector(dgvLector, pbxIMG);
            mostrarTodo();
            btnRestaurar.Visible = false;
            btnDetalles.Location = new Point(4, 358);
            btnEliminados.Location = new Point(4, 431);
            btnRestaurar.Location = new Point(4, 285);
            helper.ocultarColumnas(dgvLector);
            this.Size = new System.Drawing.Size(1060, 580);
            panelMovible.Top = btnDetalles.Top;
            btnVolver.Visible = false;
            btnEliminarFisico.Visible = false;
            btnEliminados.Visible = true;
        }

        private void btnEliminarFisico_Click(object sender, EventArgs e)
        {
            Articulos seleccionado = (Articulos)dgvLector.CurrentRow.DataBoundItem;
            if (dgvLector.CurrentRow != null)
            {
                borrarFisico(seleccionado);
            }
            else
                MessageBox.Show("No hay nada seleccionado");
        }

        private bool sol = true;

        private void btnModos_Click(object sender, EventArgs e)
        {

            if (sol)
            {
                //esta en modo oscuro, cambiando a modo claro
                btnModos.IconChar = FontAwesome.Sharp.IconChar.Moon;
                btnModos.IconColor = Color.FromArgb(29, 33, 38);
                sol = false;
                //formulario
                this.BackColor = Color.FromArgb(200, 200, 200);
                //paneles
                panel1.BackColor = Color.FromArgb(230, 230, 230);
                panelBusqueda.BackColor = Color.FromArgb(230, 230, 230);
                panelMenu.BackColor = Color.FromArgb(230, 230, 230);
                panelMovible.BackColor = Color.FromArgb(154, 100, 97);
                //labels
                lblBuscar.BackColor = Color.FromArgb(230, 230, 230);
                lblBuscarAvz.BackColor = Color.FromArgb(230, 230, 230);
                lblBuscarAvz2.BackColor = Color.FromArgb(230, 230, 230);
                lblCampo.BackColor = Color.FromArgb(230, 230, 230);
                lblCampo2.BackColor = Color.FromArgb(230, 230, 230);
                lblCriterio.BackColor = Color.FromArgb(230, 230, 230);
                lblCriterio2.BackColor = Color.FromArgb(230, 230, 230);
                lblNombre.ForeColor = Color.FromArgb(230, 230, 230);
                lblBuscarAvz.ForeColor = Color.FromArgb(41, 44, 51);
                lblBuscarAvz2.ForeColor = Color.FromArgb(41, 44, 51);
                lblCampo.ForeColor = Color.FromArgb(41, 44, 51);
                lblCampo2.ForeColor = Color.FromArgb(41, 44, 51);
                lblCriterio.ForeColor = Color.FromArgb(41, 44, 51);
                lblCriterio2.ForeColor = Color.FromArgb(41, 44, 51);
                lblNombre.BackColor = Color.FromArgb(154, 100, 97);
                lblBuscar.ForeColor = Color.FromArgb(41, 44, 51);
                //botones
                btnAgregar.ForeColor = Color.FromArgb(29, 33, 38);
                btnModificar.ForeColor = Color.FromArgb(29, 33, 38);
                btnEliminar.ForeColor = Color.FromArgb(29, 33, 38);
                btnDetalles.ForeColor = Color.FromArgb(29, 33, 38);
                btnRestaurar.ForeColor = Color.FromArgb(29, 33, 38);
                btnEliminarFisico.ForeColor = Color.FromArgb(29, 33, 38);
                btnEliminados.ForeColor = Color.FromArgb(29, 33, 38);
                btnModificar.IconColor = Color.FromArgb(41, 44, 51);
                btnVolver.ForeColor = Color.FromArgb(29, 33, 38);
                btnVolver.IconColor = Color.FromArgb(143, 65, 166);
                btnCerrar.BackColor = Color.FromArgb(41, 44, 51);
                btnCerrar.IconColor = Color.FromArgb(200, 200, 200);
                btnMinimizar.BackColor = Color.FromArgb(41, 44, 51);
                btnMinimizar.IconColor = Color.FromArgb(200, 200, 200);
                btnAgregar.FlatAppearance.MouseOverBackColor = Color.FromArgb(198, 217, 237);
                btnBuscar.FlatAppearance.MouseOverBackColor = Color.FromArgb(198, 217, 237);
                btnDetalles.FlatAppearance.MouseOverBackColor = Color.FromArgb(198, 217, 237);
                btnModificar.FlatAppearance.MouseOverBackColor = Color.FromArgb(198, 217, 237);
                btnEliminados.FlatAppearance.MouseOverBackColor = Color.FromArgb(198, 217, 237);
                btnEliminar.FlatAppearance.MouseOverBackColor = Color.FromArgb(198, 217, 237);
                btnEliminarFisico.FlatAppearance.MouseOverBackColor = Color.FromArgb(198, 217, 237);
                btnRestaurar.FlatAppearance.MouseOverBackColor = Color.FromArgb(198, 217, 237);
                btnVolver.FlatAppearance.MouseOverBackColor = Color.FromArgb(198, 217, 237);
                //DatagridView
                dgvLector.BackgroundColor = Color.FromArgb(200, 200, 200);
                dgvLector.RowsDefaultCellStyle.BackColor = Color.FromArgb(200, 200, 200);
                dgvLector.RowsDefaultCellStyle.ForeColor = Color.FromArgb(41, 44, 51);
                dgvLector.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(220, 220, 220);
                dgvLector.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(154, 100, 97);
                dgvLector.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(220, 220, 220);
                dgvLector.RowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(154, 100, 97);
                dgvLector.RowsDefaultCellStyle.SelectionForeColor = Color.FromArgb(220, 220, 220);
                dgvLector.GridColor = Color.FromArgb(41, 44, 51);
            }
            else
            {
                //esta en modo claro, cambiando a modo oscuro
                btnModos.IconChar = FontAwesome.Sharp.IconChar.Sun;
                btnModos.IconColor = Color.FromArgb(192, 192, 0);
                sol = true;
                //formulario
                this.BackColor = Color.FromArgb(41, 44, 51);
                //paneles
                panel1.BackColor = Color.FromArgb(29, 33, 38);
                panelMenu.BackColor = Color.FromArgb(29, 33, 38);
                panelBusqueda.BackColor = Color.FromArgb(29, 33, 38);
                panelMovible.BackColor = Color.FromArgb(192, 192, 0);
                //labeles
                lblBuscar.BackColor = Color.FromArgb(29, 33, 38);
                lblBuscarAvz.BackColor = Color.FromArgb(29, 33, 38);
                lblBuscarAvz2.BackColor = Color.FromArgb(29, 33, 38);
                lblCampo.BackColor = Color.FromArgb(29, 33, 38);
                lblCampo2.BackColor = Color.FromArgb(29, 33, 38);
                lblCriterio.BackColor = Color.FromArgb(29, 33, 38);
                lblCriterio2.BackColor = Color.FromArgb(29, 33, 38);
                lblNombre.BackColor = Color.FromArgb(192, 192, 0);
                lblNombre.ForeColor = Color.FromArgb(41, 44, 51);
                lblBuscarAvz.ForeColor = Color.FromArgb(200, 200, 200);
                lblBuscarAvz2.ForeColor = Color.FromArgb(200, 200, 200);
                lblCampo.ForeColor = Color.FromArgb(200, 200, 200);
                lblCampo2.ForeColor = Color.FromArgb(200, 200, 200);
                lblCriterio.ForeColor = Color.FromArgb(200, 200, 200);
                lblCriterio2.ForeColor = Color.FromArgb(200, 200, 200);
                lblBuscar.ForeColor = Color.FromArgb(200, 200, 200);
                //botones
                btnAgregar.ForeColor = Color.FromArgb(192, 192, 0);
                btnModificar.ForeColor = Color.FromArgb(192, 192, 0);
                btnEliminar.ForeColor = Color.FromArgb(192, 192, 0);
                btnDetalles.ForeColor = Color.FromArgb(192, 192, 0);
                btnRestaurar.ForeColor = Color.FromArgb(192, 192, 0);
                btnEliminarFisico.ForeColor = Color.FromArgb(192, 192, 0);
                btnEliminados.ForeColor = Color.FromArgb(192, 192, 0);
                btnModificar.IconColor = Color.FromArgb(155, 213, 240);
                btnVolver.ForeColor = Color.FromArgb(192, 192, 0);
                btnVolver.IconColor = Color.FromArgb(175, 238, 238);
                btnCerrar.BackColor = Color.FromArgb(200, 200, 200);
                btnMinimizar.BackColor = Color.FromArgb(200, 200, 200);
                btnMinimizar.IconColor = Color.FromArgb(41, 44, 51);
                btnCerrar.IconColor = Color.FromArgb(41, 44, 51);
                btnAgregar.FlatAppearance.MouseOverBackColor = Color.FromArgb(220, 220, 220);
                btnBuscar.FlatAppearance.MouseOverBackColor = Color.FromArgb(220, 220, 220);
                btnDetalles.FlatAppearance.MouseOverBackColor = Color.FromArgb(220, 220, 220);
                btnModificar.FlatAppearance.MouseOverBackColor = Color.FromArgb(220, 220, 220);
                btnEliminados.FlatAppearance.MouseOverBackColor = Color.FromArgb(220, 220, 220);
                btnEliminar.FlatAppearance.MouseOverBackColor = Color.FromArgb(220, 220, 220);
                btnEliminarFisico.FlatAppearance.MouseOverBackColor = Color.FromArgb(220, 220, 220);
                btnRestaurar.FlatAppearance.MouseOverBackColor = Color.FromArgb(220, 220, 220);
                btnVolver.FlatAppearance.MouseOverBackColor = Color.FromArgb(220, 220, 220);
                //DatagridView
                dgvLector.BackgroundColor = Color.FromArgb(41, 44, 51);
                dgvLector.RowsDefaultCellStyle.BackColor = Color.FromArgb(41, 44, 51);
                dgvLector.RowsDefaultCellStyle.ForeColor = Color.FromArgb(220, 220, 220);
                dgvLector.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(50, 54, 65);
                dgvLector.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(192, 192, 0);
                dgvLector.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(50, 54, 65);
                dgvLector.RowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(192, 192, 0);
                dgvLector.RowsDefaultCellStyle.SelectionForeColor = Color.FromArgb(41, 44, 51);
                dgvLector.GridColor = Color.FromArgb(220, 220, 220);
            }
        }

        private void dgvLector_KeyDown(object sender, KeyEventArgs e)
        {
            if (dgvLector.CurrentRow != null)
            {
                Articulos seleccionado = (Articulos)dgvLector.CurrentRow.DataBoundItem;
                if (e.KeyCode == Keys.Delete)
                {
                    if (seleccionado.Codigo.Contains("0x000"))
                    {
                        borrarFisico(seleccionado);
                    }
                    else
                    {
                        borrarLogico(seleccionado);
                    }


                }
                else if (e.KeyCode == Keys.Enter)
                {
                    Detalle detalles = new Detalle(seleccionado);
                    detalles.ShowDialog();
                }

            }
            else if (!dgvLector.Focused)
                MessageBox.Show("No hay nada seleccionado");

        }
        private void borrarFisico(Articulos seleccionado)
        {
            seleccionado = (Articulos)dgvLector.CurrentRow.DataBoundItem;
            DialogResult resultado = MessageBox.Show("¿Este articulo se eliminara de forma definitiva, desea continuar?", "Eliminar", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (resultado == DialogResult.Yes)
            {
                string carpeta = ConfigurationManager.AppSettings["Articulos-App"];
                string nombreDelArchivo = string.Empty;
                string rutaArchivo = string.Empty;
                string rutaArchivoCodigo = string.Empty;

                if (!string.IsNullOrEmpty(seleccionado.UrlImagen))
                {
                    nombreDelArchivo = Path.GetFileName(new Uri(seleccionado.UrlImagen).LocalPath);
                    rutaArchivo = Path.Combine(carpeta, nombreDelArchivo);
                    string codigo = seleccionado.Codigo.Replace("0x000", "");
                    rutaArchivoCodigo = Path.Combine(carpeta, codigo + "-" + nombreDelArchivo);

                    // Elimina el archivo si existe
                    if (File.Exists(rutaArchivo))
                    {
                        File.Delete(rutaArchivo);
                    }
                    else if (File.Exists(rutaArchivoCodigo))
                    {
                        File.Delete(rutaArchivoCodigo);
                    }
                }

                // Elimina el artículo de la base de datos
                negocio.eliminarDefinitivo(seleccionado.Id);

                // Mensaje de confirmación
                MessageBox.Show("Artículo eliminado con éxito.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            dgvLector.DataSource = negocio.eliminados();
            if (dgvLector.RowCount == 0)
            {
                dgvLector.Visible = false;
                MessageBox.Show("No hay nada para mostrar");
                sinNadaEnElLector();
            }
        }
        private void borrarLogico(Articulos seleccionado)
        {
            seleccionado = (Articulos)dgvLector.CurrentRow.DataBoundItem;
            DialogResult resultado = MessageBox.Show("¿Seguro que quieres eliminar este articulo?", "Eliminar", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (resultado == DialogResult.Yes)
            {
                negocio.eliminar(seleccionado.Id);
            }
            helper.mostrarLector(dgvLector, pbxIMG);
        }

        private void gestionArticulos_KeyDown(object sender, KeyEventArgs e)
        {
            if (!txtBuscar.Visible == false)
            {
                if (e.KeyCode == Keys.F1)
                {
                    Agregar_Modificar agregar = new Agregar_Modificar();
                    agregar.ShowDialog();
                }
                else if (e.KeyCode == Keys.F5)
                {
                    if (dgvLector.CurrentRow != null)
                    {
                        Articulos seleccionado = (Articulos)dgvLector.CurrentRow.DataBoundItem;
                        Agregar_Modificar modificar = new Agregar_Modificar(seleccionado);
                        modificar.ShowDialog();
                        helper.mostrarLector(dgvLector, pbxIMG);
                    }
                    else
                        MessageBox.Show("No hay nada seleccionado");
                }


            }

            if (e.KeyCode == Keys.F2)
            {
                ArticuloNegocio eliminados = new ArticuloNegocio();
                dgvLector.DataSource = null;
                dgvLector.DataSource = eliminados.eliminados();
                ocultarEnEliminados();
                btnRestaurar.Visible = true;
                btnDetalles.Location = new Point(4, 139);
                btnEliminados.Location = new Point(4, 212);
                btnRestaurar.Location = new Point(4, 285);
                btnEliminarFisico.Visible = true;
                helper.ocultarColumnas(dgvLector);
                this.Size = new System.Drawing.Size(1060, 452);
                panelMovible.Top = btnDetalles.Top;
                btnVolver.Visible = true;
                btnEliminados.Visible = false;
                helper.ocultarPbxIMG(dgvLector, pbxIMG);
                if (dgvLector.RowCount == 0)
                {
                    dgvLector.Visible = false;
                    MessageBox.Show("No hay nada para mostrar");
                    sinNadaEnElLector();
                }
            }

            if (txtBuscar.Visible == false)
            {
                if (e.KeyCode == Keys.Back)
                {
                    sinNadaEnElLector();
                }
                if (e.KeyCode == Keys.F5)
                {
                    ArticuloNegocio negocio = new ArticuloNegocio();
                    if (dgvLector.CurrentRow != null)
                    {
                        Articulos seleccionado = (Articulos)dgvLector.CurrentRow.DataBoundItem;
                        negocio.restaurar(seleccionado.Id);
                        dgvLector.DataSource = null;
                        dgvLector.DataSource = negocio.eliminados();
                        dgvLector.Columns["Id"].Visible = false;
                        dgvLector.Columns["Codigo"].Visible = false;
                        dgvLector.Columns["UrlImagen"].Visible = false;
                        MessageBox.Show("Restaurado con exito");
                        if (dgvLector.RowCount == 0)
                        {
                            dgvLector.Visible = false;
                            MessageBox.Show("No hay nada para mostrar");
                            sinNadaEnElLector();
                        }
                    }
                }
            }
        }

        private void btnAgregar_MouseEnter(object sender, EventArgs e)
        {
            ToolTip toolTip = new ToolTip();
            toolTip.SetToolTip(btnAgregar, "F1");
        }
    }
}





