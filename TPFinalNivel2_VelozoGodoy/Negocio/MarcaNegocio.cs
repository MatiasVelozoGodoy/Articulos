using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio;

namespace Negocio
{
    public class MarcaNegocio
    {
        private AccesoDatos data = new AccesoDatos();
        public List<Marcas> listar()
        {
            List<Marcas> lista= new List<Marcas>();
            try
            {
                data.setearConsulta("Select Id, Descripcion From MARCAS");
                data.ejecutarLectura();
                while (data.Lector.Read())
                {
                    Marcas aux = new Marcas();
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
