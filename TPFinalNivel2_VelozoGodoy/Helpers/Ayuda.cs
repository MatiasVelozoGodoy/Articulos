using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Negocio;
using Dominio;
using System.Runtime.InteropServices;

namespace Helpers
{
    public class Ayuda
    {
        public List<Articulos> listaArticulos = new List<Articulos>();
        public void mostrarLector(DataGridView dgvLector, PictureBox pbxIMG)
        {
            ArticuloNegocio aux = new ArticuloNegocio();
            listaArticulos = aux.listar();
            dgvLector.DataSource = listaArticulos;
            ocultarColumnas(dgvLector);
            ocultarPbxIMG(dgvLector, pbxIMG);
            mostrarImagen(pbxIMG, aux.listar()[0].UrlImagen);
        }
        public void mostrarImagen(PictureBox pbxIMG, string imagen)
        {
            try
            {
                pbxIMG.Load(imagen);
            }
            catch (Exception)
            {

                pbxIMG.Load("https://www.pulsecarshalton.co.uk/wp-content/uploads/2016/08/jk-placeholder-image.jpg");
            }
        }
        public void ocultarColumnas(DataGridView dgvLector)
        {
            dgvLector.Columns["Id"].Visible = false;
            dgvLector.Columns["Codigo"].Visible = false;
            dgvLector.Columns["UrlImagen"].Visible = false;
            dgvLector.Columns["Precio"].DefaultCellStyle.Format = "$ .00";
        }
        public void ocultarPbxIMG(DataGridView dgvLector, PictureBox pbxIMG)
        {
            if (dgvLector.Rows.Count <= 0)
            {
                pbxIMG.Visible = false;
            }
            else
            {
                pbxIMG.Visible = true;
            }
        }
        [DllImport("user32.dll")]
        public static extern int ReleaseCapture();

        // metodo para mover el formulario
        public void moverFormulario(Form formulario)
        {
            ReleaseCapture();
            SendMessage(formulario.Handle, 0x112, 0xf012, 0);
        }

        // importa la función SendMessage de la API de windows para que pueda mover el formulario
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        public static void AsociarEventoMouseDown(Control[] controles, MouseEventHandler metodo)
        {
            foreach (Control control in controles)
            {
                control.MouseDown += metodo;
            }
        }
        public bool validarFiltro(ComboBox cbxCampo, ComboBox cbxCriterio, TextBox txtBuscarAvz)
        {
            if (cbxCampo.SelectedIndex < 0)
            {
                MessageBox.Show("Seleccione un campo por favor.");
                return true;
            }
            if (cbxCriterio.SelectedIndex < 0)
            {
                MessageBox.Show("Seleccione el criterio por favor.");
                return true;
            }
            if (cbxCampo.SelectedItem.ToString() == "Precio")
            {
                if (string.IsNullOrEmpty(txtBuscarAvz.Text))
                {
                    MessageBox.Show("Por favor carga un valor");
                    return true;
                }
                if (!(validarNumero(txtBuscarAvz.Text)))
                {
                    MessageBox.Show("Solo numeros por favor");
                    return true;
                }

            }
            return false;
        }
        public bool validarNumero(string cadena)
        {
            foreach (char caracter in cadena)
            {
                if (!(char.IsNumber(caracter)))
                {
                    return false;
                }
            }
            return true;
        }
        public bool validarFitros(ComboBox cbxCampo, ComboBox cbxCriterio, TextBox txtBuscarAvz, ComboBox cbxCampo2, ComboBox cbxCriterio2, TextBox txtBuscarAvz2)
        {
            if (cbxCampo.SelectedIndex < 0)
            {
                MessageBox.Show("Seleccione un campo por favor.");
                return true;
            }
            if (cbxCriterio.SelectedIndex < 0)
            {
                MessageBox.Show("Seleccione el criterio por favor.");
                return true;
            }
            if (cbxCampo.SelectedItem.ToString() == "Precio")
            {
                if (string.IsNullOrEmpty(txtBuscarAvz.Text))
                {
                    MessageBox.Show("Por favor carga un valor");
                    return true;
                }
                if (!(validarNumero(txtBuscarAvz.Text)))
                {
                    MessageBox.Show("Solo numeros por favor");
                    return true;
                }
            }

            if (cbxCampo2.SelectedIndex < 0)
            {
                MessageBox.Show("Por favor cargar el segundo campo");
                return true;
            }
            if (cbxCriterio2.SelectedIndex < 0)
            {
                MessageBox.Show("Por favor cargar el segundo criterio");
                return true;
            }
            if (cbxCampo2.SelectedItem.ToString() == "Precio")
            {
                if (string.IsNullOrEmpty(txtBuscarAvz2.Text))
                {
                    MessageBox.Show("Por favor carga un valor en el segundo buscador");
                    return true;
                }
                if (!(validarNumero(txtBuscarAvz2.Text)))
                {
                    MessageBox.Show("Solo numeros por favor");
                    return true;
                }

            }
            return false;
        }

    }
}

