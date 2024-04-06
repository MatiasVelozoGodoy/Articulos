using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio;

namespace Negocio
{
    public class CategoriaNegocio
    {
        private AccesoDatos data = new AccesoDatos();
        public List<Categorias> listar()
        {
            List<Categorias> lista = new List<Categorias>();
            try
            {
                data.setearConsulta("Select Id, Descripcion From CATEGORIAS");
                data.ejecutarLectura();
                while (data.Lector.Read())
                {
                    Categorias aux = new Categorias();
                    aux.Id = (int)data.Lector["Id"];
                    aux.Descripcion = (string)data.Lector["Descripcion"];
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
    }
}
