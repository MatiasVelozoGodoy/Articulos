﻿using Dominio;
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
        }
        public Agregar_Modificar(Articulos articulo)
        {
            InitializeComponent();
            this.articulo = articulo;
            Text = "Modificar";
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
                    MessageBox.Show("Por favor rellene el campo");
                    return;
                }
                foreach (char caracter in txtPrecio.Text)
                {
                    if (!(char.IsNumber(caracter)))
                    {
                        MessageBox.Show("Solo numero por favor");
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
                archivo = new OpenFileDialog();
                archivo.Filter = "jpg|*.jpg|png|*.png";
                if(archivo.ShowDialog() == DialogResult.OK)
                {
                    txtIMG.Text = archivo.FileName;
                    helper.mostrarImagen(pbxIMG, archivo.FileName);
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
                        
                        //pregunto si existe el archivo
                        if (File.Exists(nombreDelArchivo))
                        {
                            //si existe lo elimino(ya agregado el codigo)
                            File.Delete(ConfigurationManager.AppSettings["Articulos-App"] + articulo.Codigo + "-" + archivo.SafeFileName);
                        }
                        else

                            //si no existe lo creo con el nombre original
                            File.Copy(archivo.FileName, ConfigurationManager.AppSettings["Articulos-App"] + archivo.SafeFileName);

                    //si ya hay una imagen que se llama igual le agrego el codigo
                    File.Copy(archivo.FileName, ConfigurationManager.AppSettings["Articulos-App"] + articulo.Codigo + "-" + archivo.SafeFileName);

                    //primero ve si no existe el archivo y lo crea, en segunda instacia si ya existe le agrega su codigo,
                    //y por tercero si ya existe el mismo nombre con el codigo incluido, lo borra y crea uno con el mismo nombre
                    
                    }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        
        }

        private void txtCodigo_Leave(object sender, EventArgs e)
        {
            if(txtCodigo.Text == "")
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
    }
}
