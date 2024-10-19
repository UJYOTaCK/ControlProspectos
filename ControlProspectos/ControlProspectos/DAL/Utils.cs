using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlProspectos.DAL
{
    public class Utils
    {

        private static string strConexionBase = "Data Source=149.56.143.19;Initial Catalog=prospectos; User Id=prospecto; Password=prospecto;";

        public static SqlConnection ObtenerConexionSQL()
        {
            try
            {
                SqlConnection con = new SqlConnection(strConexionBase);
                con.Open();
                return con;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
