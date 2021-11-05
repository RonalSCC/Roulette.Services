using Newtonsoft.Json;
using Roulette.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Roulette.Access
{
    public static class AccessBetUser
    {
        public static string connectionString = string.Empty;
        /// <summary>
        /// Method access DB to registers a new bet USER
        /// </summary>
        /// <param name="bet">Object EntityBetUser in case of specifying with creation parameters</param>
        public static bool Register_BetUser(EntityBetUser betUser)
        {
            string nomapi = System.Reflection.MethodBase.GetCurrentMethod().Name;
            try
            {
                if (betUser != null)
                {
                    SqlConnection cn = new SqlConnection(connectionString);
                    cn.Open();
                    SqlCommand cmd = new SqlCommand("RUL.CreateBetUser", cn);
                    cmd.Parameters.Add(new SqlParameter("@UserID", SqlDbType.UniqueIdentifier)).Value = betUser.UserID;
                    cmd.Parameters.Add(new SqlParameter("@UserName", SqlDbType.VarChar)).Value = betUser.UserName;
                    cmd.Parameters.Add(new SqlParameter("@Amount", SqlDbType.Decimal)).Value = betUser.Amount;
                    cmd.Parameters.Add(new SqlParameter("@Active", SqlDbType.Bit)).Value = betUser.Active;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                    cn.Close();
                    return true;
                }
                else {
                    return false;
                }
            }
            catch (Exception ex)
            {
                AccessLogWebApi.connectionString = connectionString;
                AccessLogWebApi.RegisterLogWebAPI(new EntityLogWebApi
                {
                    API = nomapi,
                    Input = JsonConvert.SerializeObject(betUser),
                    Output = JsonConvert.SerializeObject(ex),
                    RegistrationDate = DateTime.Now
                });
                return false;
            }
        }
        /// <summary>
        /// Method access DB to search specific bet user
        /// </summary>
        /// <param name="betUserID">ID of the betUser that will be consulted</param>
        public static EntityBetUser Read_BetUserById(Guid? betUserID)
        {
            string nomapi = System.Reflection.MethodBase.GetCurrentMethod().Name;
            EntityBetUser item_BetUser = new EntityBetUser();
            try
            {
                SqlConnection cn = new SqlConnection(connectionString);
                cn.Open();
                SqlCommand cmd = new SqlCommand("RUL.ListBetUser", cn);
                cmd.Parameters.Add(new SqlParameter("@UserID", SqlDbType.UniqueIdentifier)).Value = betUserID;
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataReader storeReader = cmd.ExecuteReader();
                while (storeReader.Read())
                {
                    item_BetUser.UserID = Guid.Parse(storeReader["UserID"].ToString());
                    item_BetUser.UserName = storeReader["RouletteID"].ToString();
                    item_BetUser.Amount = Convert.ToDecimal(storeReader["UserID"].ToString());
                    item_BetUser.Active = Convert.ToBoolean(storeReader["BetValue"]);
                }
                cn.Close();
                return item_BetUser;
            }
            catch (Exception ex)
            {
                AccessLogWebApi.connectionString = connectionString;
                AccessLogWebApi.RegisterLogWebAPI(new EntityLogWebApi
                {
                    API = nomapi,
                    Input = string.Empty,
                    Output = JsonConvert.SerializeObject(ex),
                    RegistrationDate = DateTime.Now
                });
                return item_BetUser;
            }
        }
    }
}
