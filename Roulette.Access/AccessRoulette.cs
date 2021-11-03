using System;
using System.Collections.Generic;
using System.Text;
using Roulette.Entity;
using System.Data;
using System.Data.SqlClient;
using Newtonsoft.Json;

namespace Roulette.Access
{
    public class AccessRoulette
    {
        public static int RegisterRoulette(EntityRoulette roulette)
        {
            string nomapi = System.Reflection.MethodBase.GetCurrentMethod().Name;
            int RouletteID = 0;
            //string IP = HttpContext.Connection.RemoteIpAddress.ToString()
            try
            {
                string cadenaConexion = "Data Source=(localdb)\\MSSQLLocalDB;DataBase=MasivePrueba;Integrated Security=true";
                SqlConnection cn = new SqlConnection(cadenaConexion);
                cn.Open();
                SqlCommand cmd = new SqlCommand("RUL.RegisterRoulette", cn);
                cmd.Parameters.Add(new SqlParameter("@Name", SqlDbType.VarChar, 100)).Value = !string.IsNullOrEmpty(roulette.Name) ? roulette.Name: "Nueva Ruleta";
                cmd.Parameters.Add(new SqlParameter("@Open", SqlDbType.Bit, 100)).Value = true;
                cmd.Parameters.Add(new SqlParameter("@OpenningDate", SqlDbType.DateTime, 200)).Value = DateTime.Now;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
                var IDReturn = cmd.ExecuteScalar();
                RouletteID = Convert.ToInt32(IDReturn);
                return RouletteID;
            }
            catch (Exception ex)
            {
                RegisterLogWebAPI(new EntityLogWebApi
                {
                    API = nomapi,
                    Input = JsonConvert.SerializeObject(roulette),
                    Output = JsonConvert.SerializeObject(ex),
                    RegistrationDate = DateTime.Now
                });
                return 0;
            }
        }
        public static bool RegisterLogWebAPI(EntityLogWebApi logWebApi)
        {
            try
            {
                string cadenaConexion = "Data Source=(localdb)\\MSSQLLocalDB;DataBase=MasivePrueba;Integrated Security=true";
                SqlConnection cn = new SqlConnection(cadenaConexion);
                cn.Open();
                SqlCommand cmd = new SqlCommand("LOGRUL.RegisterLogWebApi", cn);
                cmd.Parameters.Add(new SqlParameter("@API", SqlDbType.VarChar, int.MaxValue)).Value = logWebApi.API;
                cmd.Parameters.Add(new SqlParameter("@RegistrationDate", SqlDbType.DateTime, 100)).Value = logWebApi.RegistrationDate;
                cmd.Parameters.Add(new SqlParameter("@IP", SqlDbType.VarChar, int.MaxValue)).Value = !string.IsNullOrEmpty(logWebApi.IP) ? logWebApi.IP : string.Empty;
                cmd.Parameters.Add(new SqlParameter("@Input", SqlDbType.VarChar, int.MaxValue)).Value = logWebApi.Input;
                cmd.Parameters.Add(new SqlParameter("@Output", SqlDbType.VarChar, int.MaxValue)).Value = logWebApi.Output;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
