using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Dominio;

namespace Negocio
{
    public class ArticuloNegocio
    {
        private AccesoDatos data = new AccesoDatos();
        public List<Articulos> listar()
        {
            List<Articulos> lista = new List<Articulos>();
            try
            {
                data.setearConsulta("select A.Id, Codigo, Nombre, A.Descripcion, M.Descripcion Marca, C.Descripcion Categoria, A.IdCategoria, A.IdMarca, ImagenUrl, Precio from ARTICULOS A, CATEGORIAS C, MARCAS M  where A.IdCategoria = C.Id and A.IdMarca = M.Id and Codigo not like '%0x000%'");
                data.ejecutarLectura();
                while (data.Lector.Read())
                {
                    Articulos aux = new Articulos();
                    aux.Id = (int)data.Lector["Id"];
                    aux.Codigo = (string)data.Lector["Codigo"];
                    aux.Nombre = (string)data.Lector["Nombre"];
                    aux.Descripcion = (string)data.Lector["Descripcion"];
                    if (!(data.Lector["ImagenUrl"] is DBNull))
                        aux.UrlImagen = (string)data.Lector["ImagenUrl"];
                    aux.Marca = new Marcas();
                    aux.Marca.Id = (int)data.Lector["IdMarca"];
                    aux.Marca.Descripcion = (string)data.Lector["Marca"];
                    aux.Categoria = new Categorias();
                    aux.Categoria.Id = (int)data.Lector["IdCategoria"];
                    aux.Categoria.Descripcion = (string)data.Lector["Categoria"];
                    aux.Precio = (decimal)data.Lector["Precio"];
                    lista.Add(aux);
                }
                return lista;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                data.cerrarConexion();
            }
        }
        public void agregar(Articulos nuevo)
        {
            try
            {
                data.setearConsulta("insert into ARTICULOS(Codigo, Nombre, Descripcion, ImagenUrl, IdCategoria, IdMarca, Precio) values (@codigo, @nombre, @descripcion, @url, @idCategoria, @idMarca, @precio)");
                data.limpiarParametros();
                data.setearParametro("@codigo", nuevo.Codigo);
                data.setearParametro("@nombre", nuevo.Nombre);
                data.setearParametro("@descripcion", nuevo.Descripcion);
                data.setearParametro("@url", nuevo.UrlImagen);
                data.setearParametro("@idCategoria", nuevo.Categoria.Id);
                data.setearParametro("@idMarca", nuevo.Marca.Id);
                data.setearParametro("@precio", nuevo.Precio);
                
                data.ejecutarAccion();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                data.cerrarConexion();
            }
        }
        public void modificar(Articulos modifica)
        {
            try
            {
                data.setearConsulta("update ARTICULOS set Codigo = @codigo, Nombre = @nombre, Descripcion = @desc, ImagenUrl = @img ,IdMarca = @idMarca, IdCategoria= @idCategoria, Precio = @precio where Id = @id");
                data.limpiarParametros();
                data.setearParametro("@codigo", modifica.Codigo);
                data.setearParametro("@nombre", modifica.Nombre);
                data.setearParametro("@desc", modifica.Descripcion);
                data.setearParametro("@img", modifica.UrlImagen);
                data.setearParametro("@idMarca", modifica.Marca.Id);
                data.setearParametro("@idCategoria", modifica.Categoria.Id);
                data.setearParametro("@id", modifica.Id);
                data.setearParametro("@precio", modifica.Precio);
                data.ejecutarAccion();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                data.cerrarConexion();
            }
        }

        public void eliminar(int id)
        {
            try
            {
                data.setearConsulta("Update ARTICULOS set Codigo = Codigo + '0x000' where Id = @id");
                data.limpiarParametros();
                data.setearParametro("@id", id);
                data.ejecutarAccion();
            }
            catch (Exception ex)
            {
                throw ex;
            }finally
            { 
                data.cerrarConexion();
            }
        }
        public string busqueda(string campo, string criterio, string filtro)
        {
            string consulta = "select A.Id, Codigo, Nombre, A.Descripcion, M.Descripcion Marca, C.Descripcion Categoria, A.IdCategoria, A.IdMarca, ImagenUrl, Precio from ARTICULOS A, CATEGORIAS C, MARCAS M  where A.IdCategoria = C.Id and A.IdMarca = M.Id and Codigo != '0' and ";
            switch (campo)
            {
                case "Precio":
                    switch (criterio)
                    {
                        case "Mayor que":
                            consulta += campo + " > " + filtro;
                            break;
                        case "Menor que":
                            consulta += campo + " < " + filtro;
                            break;
                        case "Igual a":
                            consulta += campo + " = " + filtro;
                            break;
                    }
                    break;
                case "Nombre":
                    switch (criterio)
                    {
                        case "Empieza por":
                            consulta += campo + " Like '" + filtro + "%'";
                            break;
                        case "Termina con":
                            consulta += campo + " Like '%" + filtro + "'";
                            break;
                        case "Contiene":
                            consulta += campo + " Like " + "'%" + filtro + "%'";
                            break;
                    }
                    break;
                case "Marca":
                    switch (criterio)
                    {
                        case "Empieza por":
                            consulta += " M.Descripcion Like '" + filtro + "%'";
                            break;
                        case "Termina con":
                            consulta += " M.Descripcion Like '%" + filtro + "'";
                            break;
                        case "Contiene":
                            consulta += " M.Descripcion Like " + "'%" + filtro + "%'";
                            break;
                    }
                    break;
                case "Categoria":
                    switch (criterio)
                    {
                        case "Empieza por":
                            consulta += " C.Descripcion Like '" + filtro + "%'";
                            break;
                        case "Termina con":
                            consulta += " C.Descripcion Like '%" + filtro + "'";
                            break;
                        case "Contiene":
                            consulta += " C.Descripcion Like " + "'%" + filtro + "%'";
                            break;
                    }
                    break;
            }
            return consulta;
        }

        public List<Articulos> buscar(string campo, string criterio, string filtro)
        {
            List<Articulos> lista = new List<Articulos>();
            try
            {                
                data.setearConsulta(busqueda(campo,criterio,filtro));
                data.ejecutarLectura();
                while (data.Lector.Read())
                {
                    Articulos aux = new Articulos();
                    aux.Id = (int)data.Lector["Id"];
                    aux.Codigo = (string)data.Lector["Codigo"];
                    aux.Nombre = (string)data.Lector["Nombre"];
                    aux.Descripcion = (string)data.Lector["Descripcion"];
                    if (!(data.Lector["ImagenUrl"] is DBNull))
                        aux.UrlImagen = (string)data.Lector["ImagenUrl"];
                    aux.Marca = new Marcas();
                    aux.Marca.Id = (int)data.Lector["IdMarca"];
                    aux.Marca.Descripcion = (string)data.Lector["Marca"];
                    aux.Categoria = new Categorias();
                    aux.Categoria.Id = (int)data.Lector["IdCategoria"];
                    aux.Categoria.Descripcion = (string)data.Lector["Categoria"];
                    aux.Precio = (decimal)data.Lector["Precio"];
                    lista.Add(aux);
                }
                return lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                data.cerrarConexion();
            }
        }

        public List<Articulos> buscarMas(string campo, string campo2, string criterio, string criterio2, string filtro, string filtro2)
        {
            List<Articulos> lista = new List<Articulos>();
            try
            {
                
                string consulta2 = busqueda(campo, criterio, filtro);
                consulta2 = consulta2 + " and ";
                switch (campo2)
                {
                    case "Precio":
                        switch (criterio2)
                        {
                            case "Mayor que":
                                consulta2 += campo2 + " > " + filtro2;
                                break;
                            case "Menor que":
                                consulta2 += campo2 + " < " + filtro2;
                                break;
                            case "Igual a":
                                consulta2 += campo2 + " = " + filtro2;
                                break;
                        }
                        break;
                    case "Nombre":
                        switch (criterio2)
                        {
                            case "Empieza por":
                                consulta2 += campo2 + " Like '" + filtro2 + "%'";
                                break;
                            case "Termina con":
                                consulta2 += campo2 + " Like '%" + filtro2 + "'";
                                break;
                            case "Contiene":
                                consulta2 += campo2 + " Like " + "'%" + filtro2 + "%'";
                                break;
                        }
                        break;
                    case "Marca":
                        switch (criterio2)
                        {
                            case "Empieza por":
                                consulta2 += " M.Descripcion Like '" + filtro2 + "%'";
                                break;
                            case "Termina con":
                                consulta2 += " M.Descripcion Like '%" + filtro2 + "'";
                                break;
                            case "Contiene":
                                consulta2 += " M.Descripcion Like " + "'%" + filtro2 + "%'";
                                break;
                        }
                        break;
                    case "Categoria":
                        switch (criterio2)
                        {
                            case "Empieza por":
                                consulta2 += " C.Descripcion Like '" + filtro2 + "%'";
                                break;
                            case "Termina con":
                                consulta2 += " C.Descripcion Like '%" + filtro2 + "'";
                                break;
                            case "Contiene":
                                consulta2 += " C.Descripcion Like " + "'%" + filtro2 + "%'";
                                break;
                        }
                        break;
                }
                data.setearConsulta(consulta2);
                data.ejecutarLectura();
                while (data.Lector.Read())
                {
                    Articulos aux = new Articulos();
                    aux.Id = (int)data.Lector["Id"];
                    aux.Codigo = (string)data.Lector["Codigo"];
                    aux.Nombre = (string)data.Lector["Nombre"];
                    aux.Descripcion = (string)data.Lector["Descripcion"];
                    if (!(data.Lector["ImagenUrl"] is DBNull))
                        aux.UrlImagen = (string)data.Lector["ImagenUrl"];
                    aux.Marca = new Marcas();
                    aux.Marca.Id = (int)data.Lector["IdMarca"];
                    aux.Marca.Descripcion = (string)data.Lector["Marca"];
                    aux.Categoria = new Categorias();
                    aux.Categoria.Id = (int)data.Lector["IdCategoria"];
                    aux.Categoria.Descripcion = (string)data.Lector["Categoria"];
                    aux.Precio = (decimal)data.Lector["Precio"];
                    lista.Add(aux);
                }
                return lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                data.cerrarConexion();
            }
        
        }
        public List<Articulos> eliminados()
        {
            List<Articulos> lista = new List<Articulos>();
            try
            {
                data.setearConsulta("select A.Id, Codigo, Nombre, A.Descripcion, M.Descripcion Marca, C.Descripcion Categoria, A.IdCategoria, A.IdMarca, ImagenUrl, Precio from ARTICULOS A, CATEGORIAS C, MARCAS M  where A.IdCategoria = C.Id and A.IdMarca = M.Id and Codigo like '%0x000%'");
                data.ejecutarLectura();
                while (data.Lector.Read())
                {
                    Articulos aux = new Articulos();
                    aux.Id = (int)data.Lector["Id"];
                    aux.Codigo = (string)data.Lector["Codigo"];
                    aux.Nombre = (string)data.Lector["Nombre"];
                    aux.Descripcion = (string)data.Lector["Descripcion"];
                    if (!(data.Lector["ImagenUrl"] is DBNull))
                        aux.UrlImagen = (string)data.Lector["ImagenUrl"];
                    aux.Marca = new Marcas();
                    aux.Marca.Id = (int)data.Lector["IdMarca"];
                    aux.Marca.Descripcion = (string)data.Lector["Marca"];
                    aux.Categoria = new Categorias();
                    aux.Categoria.Id = (int)data.Lector["IdCategoria"];
                    aux.Categoria.Descripcion = (string)data.Lector["Categoria"];
                    aux.Precio = (decimal)data.Lector["Precio"];
                    lista.Add(aux);
                }
                return lista;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                data.cerrarConexion();
            }
        }
        public void restaurar(int id)
        {
            try
            {
                data.setearConsulta("Update ARTICULOS set Codigo =  REPLACE(Codigo, '0x000' , '') where Id = @id");
                data.limpiarParametros();
                data.setearParametro("@id", id);
                data.ejecutarAccion();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                data.cerrarConexion();
            }
        }
        public void eliminarDefinitivo(int id)
        {
            try
            {
                data.setearConsulta("Delete from ARTICULOS where Id = @id");
                data.limpiarParametros();
                data.setearParametro("@id", id);
                data.ejecutarAccion();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                data.cerrarConexion();
            }
        }
    }
}
