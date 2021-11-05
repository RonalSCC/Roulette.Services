using Roulette.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Roulette.Access
{
    public static class AccessLogWebApi
    {
        public static string connectionString = string.Empty;
        public static bool RegisterLogWebAPI(EntityLogWebApi logWebApi)
        {
            try
            {
                SqlConnection cn = new SqlConnection(connectionString);
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
