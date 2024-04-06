using Dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
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

                pbxIMG.Load("https://www.pulsecarshalton.co.uk/wp-content/uploads/2016/08/jk-placeholder-image.jpg");
            }
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
