using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Negocio
{
    public class AccesoDatos
    {
        private SqlConnection conexion;
        private SqlCommand comando;
        private SqlDataReader lector;
        public SqlDataReader Lector
        {
            get { return lector; }
        }
        public AccesoDatos()
        {
            conexion = new SqlConnection("server=.\\SQLEXPRESS; database = CATALOGO_DB; integrated security = true");
            comando = new SqlCommand();
        }
        public void setearConsulta(string consulta)
        {
            try
            {
                comando.CommandType = System.Data.CommandType.Text;
                comando.CommandText = consulta;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void setearParametro(string nombre, object valor) 
        {
            try
            {
                comando.Parameters.AddWithValue(nombre, valor);    
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void ejecutarLectura()
        {
            try
            {
                comando.Connection= conexion;
                conexion.Open();
                lector = comando.ExecuteReader();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void ejecutarAccion()
        {
            try
            {
                comando.Connection= conexion;
                conexion.Open();
                comando.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void cerrarConexion()
        {
            if(lector != null)
            {
                lector.Close();
            }
            conexion.Close();
        }
        public void limpiarParametros()
        {
            if (comando != null)
            {
                comando.Parameters.Clear();
            }
        }
    }
}
