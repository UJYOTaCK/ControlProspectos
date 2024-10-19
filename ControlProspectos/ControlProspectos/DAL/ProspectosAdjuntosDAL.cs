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
    public class ProspectosAdjuntosDAL
    {

        public static void GuardarProspectoAdjunto(ProspectoAdjunto prospectoAdjunto)
        {
            SqlConnection connection = null;
            try
            {                
                DataSet ds = new DataSet();
                connection = Utils.ObtenerConexionSQL();
                SqlCommand command = new SqlCommand();
                SqlParameter param = null;

                command.Connection = connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "Catprospectosadjunto_Save";

                param = new SqlParameter("@ProspectoadjuntoId", prospectoAdjunto.prospectoAdjuntoId);
                param.Direction = ParameterDirection.Output;
                param.DbType = DbType.Int32;
                command.Parameters.Add(param);

                param = new SqlParameter("@ProspectoId", prospectoAdjunto.prospectoId);
                param.Direction = ParameterDirection.Input;
                param.DbType = DbType.String;
                command.Parameters.Add(param);

                param = new SqlParameter("@TipodocumentoId", Int32.MinValue);
                param.Direction = ParameterDirection.Input;
                param.DbType = DbType.Int32;
                command.Parameters.Add(param);

                param = new SqlParameter("@Tipodocumento", prospectoAdjunto.tipoDocumento);
                param.Direction = ParameterDirection.Input;
                param.DbType = DbType.String;
                command.Parameters.Add(param);

                param = new SqlParameter("@PathFile", prospectoAdjunto.pathFile);
                param.Direction = ParameterDirection.Input;
                param.DbType = DbType.String;
                command.Parameters.Add(param);

                param = new SqlParameter("@Estatus", prospectoAdjunto.estatus);
                param.Direction = ParameterDirection.Input;
                param.DbType = DbType.Int32;
                command.Parameters.Add(param);

                param = new SqlParameter("@FechaCreacion", prospectoAdjunto.fechaCreacion);
                param.Direction = ParameterDirection.Input;
                param.DbType = DbType.DateTime;
                command.Parameters.Add(param);

                param = new SqlParameter("@FileUpload", prospectoAdjunto.fileUpload);
                param.Direction = ParameterDirection.Input;
                param.DbType = DbType.Binary;
                command.Parameters.Add(param);

                param = new SqlParameter("@Ultimaact", prospectoAdjunto.ultimaAct);
                param.Direction = ParameterDirection.Output;
                param.DbType = DbType.Int32;
                command.Parameters.Add(param);

                command.ExecuteNonQuery();


                prospectoAdjunto.prospectoAdjuntoId = Convert.ToInt32(command.Parameters["@ProspectoadjuntoId"].Value);
                prospectoAdjunto.ultimaAct = Convert.ToInt32(command.Parameters["@Ultimaact"].Value);



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


        public static void EliminarProspectoAdjunto(int prospectoid)
        {
            SqlConnection connection = null;
            try
            {
                DataSet ds = new DataSet();
                connection = Utils.ObtenerConexionSQL();
                SqlCommand command = new SqlCommand();
                SqlParameter param = null;

                command.Connection = connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "Catprospectosadjunto_Delete";

               

                param = new SqlParameter("@PROSPECTOID", prospectoid);
                param.Direction = ParameterDirection.Input;
                param.DbType = DbType.Int32;
                command.Parameters.Add(param);

                command.ExecuteNonQuery();


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



        public static List<ProspectoAdjunto> TraerAdjuntosPorProspecto(Int32 prospectoId)
        {
            SqlConnection connection = null;
            try
            {

                List<ProspectoAdjunto> Respuesta = new List<ProspectoAdjunto>();
                DataSet ds = new DataSet();
                connection = Utils.ObtenerConexionSQL();
                SqlCommand command = new SqlCommand();
                SqlParameter param = null;

                command.Connection = connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "Catprospectosadjunto_Select";

               
                param = new SqlParameter("@PROSPECTOID", prospectoId);                
                param.Direction = ParameterDirection.Input;
                param.DbType = DbType.Int32;
                command.Parameters.Add(param);

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(ds);

                Respuesta = (from a in ds.Tables[0].AsEnumerable()
                             select new ProspectoAdjunto
                             {
                                 prospectoAdjuntoId = a.Field<Int32>("PROSPECTOADJUNTO_ID"),
                                 prospectoId = a.Field<Int32>("PROSPECTO_ID"),
                                 tipoDocumento = a.Field<string>("TIPODOCUMENTO").Trim(),
                                 pathFile = a.Field<string>("PATH_FILE").Trim(),
                                 estatus = a.Field<Int32>("ESTATUS"),
                                 fechaCreacion = a.Field<DateTime>("FECHA_CREACION"),
                                 fileUpload = a.Field<byte[]>("FILE_UPLOAD"),
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
