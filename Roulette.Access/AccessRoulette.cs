using System;
using System.Collections.Generic;
using System.Text;
using Roulette.Entity;
using System.Data;
using System.Data.SqlClient;
using Newtonsoft.Json;
using System.Linq;
using Roulette.Helpers.General.Input;

namespace Roulette.Access
{
    public static class AccessRoulette
    {
        public static string connectionString = string.Empty;
        /// <summary>
        /// Method access DB to registers a new roulette
        /// </summary>
        /// <param name="roulette">Object EntityRoulette in case of specifying creation parameters</param>
        public static int Register_Roulette(EntityRoulette roulette)
        {
            string nomapi = System.Reflection.MethodBase.GetCurrentMethod().Name;
            int RouletteID = 0;
            //string IP = HttpContext.Connection.RemoteIpAddress.ToString()
            try
            {
                if (roulette != null)
                {
                    SqlConnection cn = new SqlConnection(connectionString);
                    cn.Open();
                    SqlCommand cmd = new SqlCommand("RUL.RegisterRoulette", cn);
                    cmd.Parameters.Add(new SqlParameter("@Name", SqlDbType.VarChar, 100)).Value = !string.IsNullOrEmpty(roulette.Name) ? roulette.Name : "Nueva Ruleta";
                    cmd.Parameters.Add(new SqlParameter("@Open", SqlDbType.Bit, 100)).Value = false;
                    cmd.Parameters.Add(new SqlParameter("@OpenningDate", SqlDbType.DateTime, 200)).Value = DateTime.Now;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                    var IDReturn = cmd.ExecuteScalar();
                    RouletteID = Convert.ToInt32(IDReturn);
                    cn.Close();
                }
                return RouletteID;

            }
            catch (Exception ex)
            {
                AccessLogWebApi.connectionString = connectionString;
                AccessLogWebApi.RegisterLogWebAPI(new EntityLogWebApi
                {
                    API = nomapi,
                    Input = JsonConvert.SerializeObject(roulette),
                    Output = JsonConvert.SerializeObject(ex),
                    RegistrationDate = DateTime.Now
                });
                return 0;
            }
        }
        /// <summary>
        /// Method access DB to update a Roulette
        /// </summary>
        /// <param name="roulette">Object EntityRoulette with modified parameters</param>
        public static EntityRoulette Update_Roulette(EntityRoulette roulette)
        {
            string nomapi = System.Reflection.MethodBase.GetCurrentMethod().Name;
            EntityRoulette rouletteUpdated = null;
            try
            {
                if (roulette != null)
                {
                    SqlConnection cn = new SqlConnection(connectionString);
                    cn.Open();
                    SqlCommand cmd = new SqlCommand("RUL.UpdateRoulette", cn);
                    cmd.Parameters.Add(new SqlParameter("@RouletteID", SqlDbType.Int, 32)).Value = roulette.RouletteID;
                    cmd.Parameters.Add(new SqlParameter("@Name", SqlDbType.VarChar, 100)).Value = roulette.Name;
                    cmd.Parameters.Add(new SqlParameter("@Open", SqlDbType.Bit, 100)).Value = roulette.Open;
                    cmd.Parameters.Add(new SqlParameter("@WinningNumber", SqlDbType.Bit, 100)).Value = roulette.WinningNumber;
                    cmd.Parameters.Add(new SqlParameter("@OpenningDate", SqlDbType.DateTime, 200)).Value = DateTime.Now;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                    cn.Close();

                    var searchRoulette = Read_RouletteById(roulette.RouletteID);
                    if (searchRoulette != null)
                    {
                        rouletteUpdated = searchRoulette;
                    }
                }
                return rouletteUpdated;
            }
            catch (Exception ex)
            {
                AccessLogWebApi.connectionString = connectionString;
                AccessLogWebApi.RegisterLogWebAPI(new EntityLogWebApi
                {
                    API = nomapi,
                    Input = JsonConvert.SerializeObject(roulette),
                    Output = JsonConvert.SerializeObject(ex),
                    RegistrationDate = DateTime.Now
                });
                return rouletteUpdated;
            }
        }
        /// <summary>
        /// Method access DB to search all Roulettes
        /// </summary>
        public static List<EntityRoulette> Read_Roulettes()
        {
            string nomapi = System.Reflection.MethodBase.GetCurrentMethod().Name;
            List<EntityRoulette> list_Roulettes = new List<EntityRoulette>();
            try
            {
                SqlConnection cn = new SqlConnection(connectionString);
                cn.Open();
                SqlCommand cmd = new SqlCommand("RUL.ListRoulettes", cn);
                cmd.Parameters.Add(new SqlParameter("@RouletteID", SqlDbType.Int, 32)).Value = null;
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataReader storeReader = cmd.ExecuteReader();

                while (storeReader.Read())
                {
                    EntityRoulette entityReaderRoulette = new EntityRoulette();
                    entityReaderRoulette.RouletteID = Convert.ToInt32(storeReader["RouletteID"]);
                    entityReaderRoulette.Name = storeReader["Name"].ToString();
                    entityReaderRoulette.Open = Convert.ToBoolean(storeReader["Open"]);
                    entityReaderRoulette.OpenningDate = Convert.ToDateTime(storeReader["OpenningDate"]);
                    list_Roulettes.Add(entityReaderRoulette);
                }
                cn.Close();
                return list_Roulettes;
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
                return list_Roulettes;
            }
        }
        /// <summary>
        /// Method access DB to search a specific Roulette
        /// </summary>
        /// <param name="rouletteID">ID of the roulette that will be consulted</param>
        public static EntityRoulette Read_RouletteById(int? rouletteID)
        {
            string nomapi = System.Reflection.MethodBase.GetCurrentMethod().Name;
            EntityRoulette item_Roulette = new EntityRoulette();
            try
            {
                SqlConnection cn = new SqlConnection(connectionString);
                cn.Open();
                SqlCommand cmd = new SqlCommand("RUL.ListRoulettes", cn);
                cmd.Parameters.Add(new SqlParameter("@RouletteID", SqlDbType.Int, 32)).Value = rouletteID;
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataReader storeReader = cmd.ExecuteReader();

                while (storeReader.Read())
                {
                    item_Roulette.RouletteID = Convert.ToInt32(storeReader["RouletteID"]);
                    item_Roulette.Name = storeReader["Name"].ToString();
                    item_Roulette.Open = Convert.ToBoolean(storeReader["Open"]);
                    item_Roulette.OpenningDate = Convert.ToDateTime(storeReader["OpenningDate"]);
                }
                cn.Close();
                return item_Roulette;
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
                return item_Roulette;
            }
        }

        public static bool Finish_Roulette(InputFinish_Roulette info)
        {
            string nomapi = System.Reflection.MethodBase.GetCurrentMethod().Name;
            try
            {
                if (info == null)
                {
                    return false;
                }

                SqlConnection cn = new SqlConnection(connectionString);
                cn.Open();
                SqlCommand cmd = new SqlCommand("RUL.FinishRoulette", cn);
                cmd.Parameters.Add(new SqlParameter("@RouletteID", SqlDbType.Int, 32)).Value = info.RouletteID;
                cmd.Parameters.Add(new SqlParameter("@BetWinnerID", SqlDbType.Int, 32)).Value = info.RouletteID;
                cmd.Parameters.Add(new SqlParameter("@BetWinnerColorIDs", SqlDbType.VarChar)).Value = info.BetWinnerColorIDs;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
                cn.Close();
                return true;
            }
            catch (Exception ex)
            {
                AccessLogWebApi.connectionString = connectionString;
                AccessLogWebApi.RegisterLogWebAPI(new EntityLogWebApi
                {
                    API = nomapi,
                    Input = JsonConvert.SerializeObject(info),
                    Output = JsonConvert.SerializeObject(ex),
                    RegistrationDate = DateTime.Now
                });
                return false;
            }
        }
    }
}
