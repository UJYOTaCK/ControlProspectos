using ControlProspectos.Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlProspectos.DAL
{
    public class TiposDocumentosDAL
    {

        public static List<TiposDocumentos> TraerTiposDocumentos()
        {
            SqlConnection connection = null;
            try
            {

                List<TiposDocumentos> Respuesta = new List<TiposDocumentos>();

                DataSet ds = new DataSet();
                connection = Utils.ObtenerConexionSQL();
                SqlCommand command = new SqlCommand();
                SqlParameter param = null;

                command.Connection = connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "Cattiposdocumento_Select";

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(ds);

                Respuesta = (from a in ds.Tables[0].AsEnumerable()
                             select new TiposDocumentos
                             {

                                 tipoDocumentoId = a.Field<Int32>("TIPODOCUMENTO_ID"),
                                 tipoDocumento = a.Field<string>("TIPODOCUMENTO").Trim(),
                                 ultimaAct = a.Field<Int32>("UltimaAct")
                             }).ToList();

                return Respuesta;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                    connection.Dispose();
                }
            }
        }
    }
}
