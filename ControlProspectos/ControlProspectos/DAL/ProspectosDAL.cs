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
    public class ProspectosDAL
    {


        public static void GuardarProspecto(Prospecto prospecto)
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
                command.CommandText = "Catprospecto_Save";

                Int32 prospectoid = prospecto.prospectoId;
                int ultimaact = prospecto.ultimaAct;

                param = new SqlParameter("@ProspectoId", prospectoid);
                param.Direction = ParameterDirection.Input;
                param.DbType = DbType.Int32;
                command.Parameters.Add(param);

                param = new SqlParameter("@Nombre", prospecto.nombre);
                param.Direction = ParameterDirection.Input;
                param.DbType = DbType.String;
                command.Parameters.Add(param);

                param = new SqlParameter("@ApellidoPaterno", prospecto.apellidoPaterno);
                param.Direction = ParameterDirection.Input;
                param.DbType = DbType.String;
                command.Parameters.Add(param);

                param = new SqlParameter("@ApellidoMaterno", prospecto.apellidoMaterno);
                param.Direction = ParameterDirection.Input;
                param.DbType = DbType.String;
                command.Parameters.Add(param);

                param = new SqlParameter("@Calle", prospecto.calle);
                param.Direction = ParameterDirection.Input;
                param.DbType = DbType.String;
                command.Parameters.Add(param);

                param = new SqlParameter("@Numero", prospecto.numero);
                param.Direction = ParameterDirection.Input;
                param.DbType = DbType.String;
                command.Parameters.Add(param);

                param = new SqlParameter("@Colonia", prospecto.colonia);
                param.Direction = ParameterDirection.Input;
                param.DbType = DbType.String;
                command.Parameters.Add(param);

                param = new SqlParameter("@CodigoPostal", prospecto.codigoPostal);
                param.Direction = ParameterDirection.Input;
                param.DbType = DbType.String;
                command.Parameters.Add(param);

                param = new SqlParameter("@Telefono", prospecto.telefono);
                param.Direction = ParameterDirection.Input;
                param.DbType = DbType.String;
                command.Parameters.Add(param);

                param = new SqlParameter("@Rfc", prospecto.rfc);
                param.Direction = ParameterDirection.Input;
                param.DbType = DbType.String;
                command.Parameters.Add(param);


                param = new SqlParameter("@Estatus", prospecto.estatus);
                param.Direction = ParameterDirection.Input;
                param.DbType = DbType.Int32;
                command.Parameters.Add(param);


                param = new SqlParameter("@Observaciones", prospecto.observaciones);
                param.Direction = ParameterDirection.Input;
                param.DbType = DbType.String;
                command.Parameters.Add(param);


                param = new SqlParameter("@Ultimaact", ultimaact);
                param.Direction = ParameterDirection.Input;
                param.DbType = DbType.Int32;
                command.Parameters.Add(param);

                param = new SqlParameter("@ProspectoIdR", ultimaact);
                param.Direction = ParameterDirection.Output;
                param.DbType = DbType.Int32;
                command.Parameters.Add(param);

                param = new SqlParameter("@UltimaactR", ultimaact);
                param.Direction = ParameterDirection.Output;
                param.DbType = DbType.Int32;
                command.Parameters.Add(param);

                command.ExecuteNonQuery();


                prospecto.prospectoId = Convert.ToInt32(command.Parameters["@ProspectoIdR"].Value);
                prospecto.ultimaAct = Convert.ToInt32(command.Parameters["@UltimaactR"].Value);

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



        public static List<Prospecto> TraerProspectos(Int32? prospectoid, Int32? estatus)
        {
            SqlConnection connection = null;
            try
            {

                List<Prospecto> Respuesta = new List<Prospecto>();

                DataSet ds = new DataSet();
                connection = Utils.ObtenerConexionSQL();
                SqlCommand command = new SqlCommand();
                SqlParameter param = null;

                command.Connection = connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "Catprospecto_Select";

                if (prospectoid == null)
                {
                    param = new SqlParameter("@ProspectoId", Int32.MinValue);
                }
                else {
                    param = new SqlParameter("@ProspectoId", prospectoid);
                }
                param.Direction = ParameterDirection.Input;
                param.DbType = DbType.Int32;
                command.Parameters.Add(param);

                if (estatus == null)
                {
                    param = new SqlParameter("@estatus", Int32.MinValue);
                }
                else
                {
                    param = new SqlParameter("@estatus", estatus);
                }
                param.Direction = ParameterDirection.Input;
                param.DbType = DbType.Int32;
                command.Parameters.Add(param);


                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(ds);

                Respuesta = (from a in ds.Tables[0].AsEnumerable()
                             select new Prospecto
                             {
                                 prospectoId = a.Field<Int32>("PROSPECTO_ID"),
                                 nombre = a.Field<string>("NOMBRE").Trim(),
                                 apellidoPaterno = a.Field<string>("APELLIDO_PATERNO").Trim(),
                                 apellidoMaterno = a.Field<string>("APELLIDO_MATERNO").Trim(),
                                 calle = a.Field<string>("CALLE").Trim(),
                                 numero = a.Field<string>("NUMERO").Trim(),
                                 colonia = a.Field<string>("COLONIA").Trim(),
                                 codigoPostal = a.Field<string>("CODIGO_POSTAL").Trim(),
                                 telefono = a.Field<string>("TELEFONO").Trim(),
                                 rfc = a.Field<string>("RFC").Trim(),
                                 estatus = a.Field<Int32>("ESTATUS"),
                                 observaciones = a.Field<string>("OBSERVACIONES").Trim(),                               
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
