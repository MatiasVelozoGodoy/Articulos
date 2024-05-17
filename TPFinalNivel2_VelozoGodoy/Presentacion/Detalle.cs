using Dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Presentacion
{
    public partial class Detalle : Form
    {
        Articulos seleccionado;
        public Detalle(Articulos seleccionado)
        {
            InitializeComponent();
            this.seleccionado = seleccionado;
        }

        private void Detalle_Load(object sender, EventArgs e)
        {
            cargarIMG(seleccionado.UrlImagen);
            lblIdArticulo.Text = seleccionado.Id.ToString();
            lblCodigoArticulo.Text = seleccionado.Codigo;
            lblNombreArticulo.Text = seleccionado.Nombre;
            lblDescipcionArticulo.Text = seleccionado.Descripcion;
            lblMarcaArticulo.Text = seleccionado.Marca.Descripcion;
            lblCategoriaArticulo.Text = seleccionado.Categoria.Descripcion;
            lblPrecioArticulo.Text = seleccionado.Precio.ToString("$ .00");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }
        public void cargarIMG(string imagen)
        {
            try
            {
                pbxIMG.Load(imagen);

            }
            catch (Exception)
            {

                pbxIMG.Load("https://upload.wikimedia.org/wikipedia/commons/thumb/6/65/No-Image-Placeholder.svg/1665px-No-Image-Placeholder.svg.png");
            }
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Close();
        }
        [DllImport("user32.dll", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();

        [DllImport("user32.dll", EntryPoint = "SendMessage")]
        private extern static IntPtr SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);


        private void panelCerrar_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }
    }
}
