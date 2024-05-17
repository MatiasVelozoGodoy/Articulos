using Dominio;
using Negocio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Helpers;
using System.IO;
using System.Configuration;
using System.Security.AccessControl;
using System.Runtime.InteropServices;

namespace Presentacion
{
    public partial class Agregar_Modificar : Form
    {
        private Ayuda helper = new Ayuda();
        private Articulos articulo = null;
        private OpenFileDialog archivo = null;
        private ArticuloNegocio negocio = new ArticuloNegocio();
        
        public Agregar_Modificar()
        {
            InitializeComponent();
            Text = "Agregar";
            this.ActiveControl = lblAsteriscoCodigo;
        }
        public Agregar_Modificar(Articulos articulo)
        {
            InitializeComponent();
            this.articulo = articulo;
            Text = "Modificar";
            lblAgregarModificar.Text = "Modificar";
            lblAsteriscoCodigo.Visible = false;
            lblAsteriscoNombre.Visible = false;
            lblAsteriscoPrecio.Visible = false;
        }
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            try
            {
                if(articulo == null)
                {
                    articulo= new Articulos();
                }
                articulo.Codigo = txtCodigo.Text;
                articulo.Nombre = txtNombre.Text;
                articulo.Descripcion = txtDescripcion.Text;
                helper.mostrarImagen(pbxIMG, txtIMG.Text);
                articulo.UrlImagen = txtIMG.Text;
                articulo.Marca = (Marcas)cbxMarca.SelectedItem;
                articulo.Categoria = (Categorias)cbxCategoria.SelectedItem;
                if(txtPrecio.Text == "" || txtCodigo.Text == "" || txtNombre.Text == "")
                {
                    
                    if (txtNombre.Text == "")
                    {
                        txtNombre.BackColor = Color.FromArgb(255, 160, 160);
                        lblAsteriscoNombre.Text = "*";
                    }
                    else
                    {
                        txtNombre.BackColor = SystemColors.Window;
                        lblAsteriscoNombre.Text = "✓";
                    }
                    if (txtCodigo.Text == "")
                    {
                        txtCodigo.BackColor = Color.FromArgb(255, 160, 160);
                        lblAsteriscoCodigo.Text = "*";
                    }
                    else
                    {
                        txtCodigo.BackColor = SystemColors.Window;
                        lblAsteriscoCodigo.Text = "✓";
                    }
                    if (txtPrecio.Text == "")
                    {
                        txtPrecio.BackColor = Color.FromArgb(255, 160, 160);
                        lblAsteriscoPrecio.Text = "*";
                    }
                    else
                    {
                        txtPrecio.BackColor = SystemColors.Window;
                        lblAsteriscoPrecio.Text = "✓";
                    }

                    MessageBox.Show("Por favor rellene el campo marcado");
                    return;
                }
                foreach (char caracter in txtPrecio.Text)
                {
                    if (!(char.IsNumber(caracter)))
                    {
                        MessageBox.Show("Solo numero por favor sin coma por favor");
                        lblAsteriscoPrecio.Text = "*";

                        return;
                    }
                }
                articulo.Precio = decimal.Parse(txtPrecio.Text);
                if(articulo.Id != 0)
                {
                    MessageBox.Show("Modificado exitosamente");
                    negocio.modificar(articulo);
                    borrarImagenExistente();
                }
                else
                {
                    MessageBox.Show("Agregado correctamente");
                    negocio.agregar(articulo);
                    borrarImagenExistente();
                }

                Close();

         }
         catch (Exception ex)
         {
             throw ex;
         }
    }

        private void Agregar_Modificar_Load(object sender, EventArgs e)
        {
            try
            {
                MarcaNegocio marca = new MarcaNegocio();
                CategoriaNegocio categoria = new CategoriaNegocio();
                cbxMarca.DataSource = marca.listar();
                cbxMarca.DisplayMember = "Descripcion";
                cbxMarca.ValueMember = "Id";
                cbxCategoria.DataSource = categoria.listar();
                cbxCategoria.DisplayMember = "Descripcion";
                cbxCategoria.ValueMember = "Id";

                if(articulo != null)
                {
                    txtCodigo.Text = articulo.Codigo;
                    txtNombre.Text = articulo.Nombre;
                    txtDescripcion.Text = articulo.Descripcion;
                    txtIMG.Text = articulo.UrlImagen;
                    helper.mostrarImagen(pbxIMG, txtIMG.Text);
                    txtPrecio.Text = articulo.Precio.ToString("0.00");
                    cbxMarca.SelectedValue = articulo.Marca.Id;
                    cbxCategoria.SelectedValue = articulo.Categoria.Id;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void txtIMG_Leave(object sender, EventArgs e)
        {
            helper.mostrarImagen(pbxIMG, txtIMG.Text);
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnAgregarIMG_Click(object sender, EventArgs e)
        {
            try
            {
                string carpeta = ConfigurationManager.AppSettings["Articulos-App"];

                if (!(Directory.Exists(carpeta)))
                {
                    Directory.CreateDirectory(carpeta);
                    archivo = new OpenFileDialog();
                    archivo.Filter = "jpg|*.jpg|png|*.png";
                    if(archivo.ShowDialog() == DialogResult.OK)
                    {
                        txtIMG.Text = archivo.FileName;
                        helper.mostrarImagen(pbxIMG, archivo.FileName);
                    }
                }
                else
                {
                    archivo = new OpenFileDialog();
                    archivo.Filter = "jpg|*.jpg|png|*.png";
                    if (archivo.ShowDialog() == DialogResult.OK)
                    {
                        txtIMG.Text = archivo.FileName;
                        helper.mostrarImagen(pbxIMG, archivo.FileName);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void borrarImagenExistente()
        {
            try
            {                
                if (archivo != null && !(txtIMG.Text.ToLower().Contains("http")))
                {
                    string nombreDelArchivo = ConfigurationManager.AppSettings["Articulos-App"] + archivo.SafeFileName;
                    string nombreConCodigo = ConfigurationManager.AppSettings["Articulos-App"] + articulo.Codigo + "-" + archivo.SafeFileName;

                    //pregunto si existe el archivo
                    if (!(File.Exists(nombreDelArchivo)))
                    {
                        //si no existe lo creo con el nombre original + codigo
                        File.Copy(archivo.FileName, ConfigurationManager.AppSettings["Articulos-App"] + archivo.SafeFileName);                        
                    }
                    else if(!(File.Exists(nombreConCodigo)))
                    {
                        //si ya existe uno creo uno nuevo agregandole el codigo
                        File.Copy(archivo.FileName, ConfigurationManager.AppSettings["Articulos-App"] + articulo.Codigo + "-" + archivo.SafeFileName);    
                    }
                    else
                    {
                        //    //si existe lo elimino
                        File.Delete(ConfigurationManager.AppSettings["Articulos-App"] + articulo.Codigo + "-" + archivo.SafeFileName);
                        //lo vuelvo agregar
                        File.Copy(archivo.FileName, ConfigurationManager.AppSettings["Articulos-App"]+  articulo.Codigo + "-" + archivo.SafeFileName);
                    }
                    //primero se fija si existe el archivo, si no existe lo crea
                    //segundo se fije si ya existe el archivo, si existe le agrega su codigo
                    //por ultimo se fija si ya existe uno con codigo, si existe lo borra y lo vuelve a crear
                
                }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        
        }

        private void txtCodigo_Leave(object sender, EventArgs e)
        {
            if (txtCodigo.Text == "")
            {
                txtCodigo.BackColor = Color.FromArgb(255, 160, 160);
                lblAsteriscoCodigo.Text = "*";
            }
            else
            {
                txtCodigo.BackColor = SystemColors.Window;
                lblAsteriscoCodigo.Text = "✓";

            }
        }

        private void txtNombre_Leave(object sender, EventArgs e)
        {
            if (txtNombre.Text == "")
            {
                txtNombre.BackColor = Color.FromArgb(255, 160, 160);
                lblAsteriscoNombre.Text = "*";
            }
            else
            {
                txtNombre.BackColor = SystemColors.Window;
                lblAsteriscoNombre.Text = "✓";
            }
        }

        private void txtPrecio_Leave(object sender, EventArgs e)
        {
            if (txtPrecio.Text == "")
            {
                txtPrecio.BackColor = Color.FromArgb(255, 160, 160);
                lblAsteriscoPrecio.Text = "*";
            }
            else
            {
                txtPrecio.BackColor = SystemColors.Window;
                lblAsteriscoPrecio.Text= "✓";
            }
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
