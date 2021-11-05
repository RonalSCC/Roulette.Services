using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Roulette.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Roulette.Access
{
    public static class AccessBet
    {
        public static string connectionString = string.Empty;
        /// <summary>
        /// Method access DB to registers a new bet
        /// </summary>
        /// <param name="bet">Object EntityBet in case of specifying with creation parameters</param>
        public static int Register_Bet(EntityBet bet)
        {
            string nomapi = System.Reflection.MethodBase.GetCurrentMethod().Name;
            int BetID = 0;
            try
            {
                if (bet != null)
                {
                    SqlConnection cn = new SqlConnection(connectionString);
                    cn.Open();
                    SqlCommand cmd = new SqlCommand("RUL.CreateBet", cn);
                    cmd.Parameters.Add(new SqlParameter("@RouletteID", SqlDbType.Int, 32)).Value = bet.RouletteID;
                    cmd.Parameters.Add(new SqlParameter("@UserID", SqlDbType.UniqueIdentifier)).Value = bet.UserID;
                    cmd.Parameters.Add(new SqlParameter("@BetValue", SqlDbType.Decimal)).Value = bet.BetValue;
                    cmd.Parameters.Add(new SqlParameter("@Number", SqlDbType.Int, 32)).Value = bet.Number;
                    cmd.Parameters.Add(new SqlParameter("@Finished", SqlDbType.Bit)).Value = bet.Finished;
                    cmd.Parameters.Add(new SqlParameter("@Winner", SqlDbType.Bit)).Value = bet.Winner;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                    var IDReturn = cmd.ExecuteScalar();
                    BetID = Convert.ToInt32(IDReturn);
                    cn.Close();
                }
                return BetID;

            }
            catch (Exception ex)
            {
                AccessLogWebApi.connectionString = connectionString;
                AccessLogWebApi.RegisterLogWebAPI(new EntityLogWebApi
                {
                    API = nomapi,
                    Input = JsonConvert.SerializeObject(bet),
                    Output = JsonConvert.SerializeObject(ex),
                    RegistrationDate = DateTime.Now
                });
                return 0;
            }
        }
        /// <summary>
        /// Method access DB to search specific bet
        /// </summary>
        /// <param name="betID">ID of the bet that will be consulted</param>
        public static EntityBet Read_BetById(int? betID)
        {
            string nomapi = System.Reflection.MethodBase.GetCurrentMethod().Name;
            EntityBet item_Bet = new EntityBet();
            try
            {
                SqlConnection cn = new SqlConnection(connectionString);
                cn.Open();
                SqlCommand cmd = new SqlCommand("RUL.ListBets", cn);
                cmd.Parameters.Add(new SqlParameter("@BetID", SqlDbType.Int, 32)).Value = betID;
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataReader storeReader = cmd.ExecuteReader();
                while (storeReader.Read())
                {
                    item_Bet.BetID = Convert.ToInt32(storeReader["BetID"]);
                    item_Bet.RouletteID = Convert.ToInt32(storeReader["RouletteID"]);
                    item_Bet.UserID = Guid.Parse(storeReader["UserID"].ToString());
                    item_Bet.BetValue = Convert.ToDecimal(storeReader["BetValue"]);
                    item_Bet.Number = Convert.ToInt32(storeReader["Number"]);
                    item_Bet.Finished = Convert.ToBoolean(storeReader["Finished"]);
                }
                cn.Close();
                return item_Bet;
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
                return item_Bet;
            }
        }
        /// <summary>
        /// Method access DB to search all bet
        /// </summary>
        public static List<EntityBet> Read_Bets()
        {
            string nomapi = System.Reflection.MethodBase.GetCurrentMethod().Name;
            List<EntityBet> list_Bets = new List<EntityBet>();
            try
            {
                SqlConnection cn = new SqlConnection(connectionString);
                cn.Open();
                SqlCommand cmd = new SqlCommand("RUL.ListBets", cn);
                cmd.Parameters.Add(new SqlParameter("@BetID", SqlDbType.Int, 32)).Value = null;
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataReader storeReader = cmd.ExecuteReader();

                while (storeReader.Read())
                {
                    EntityBet entityReaderRoulette = new EntityBet();
                    entityReaderRoulette.BetID = Convert.ToInt32(storeReader["BetID"]);
                    entityReaderRoulette.RouletteID = Convert.ToInt32(storeReader["RouletteID"]);
                    entityReaderRoulette.UserID = Guid.Parse(storeReader["UserID"].ToString());
                    entityReaderRoulette.BetValue = Convert.ToDecimal(storeReader["BetValue"]);
                    entityReaderRoulette.Number = Convert.ToInt32(storeReader["Number"]);
                    entityReaderRoulette.Finished = Convert.ToBoolean(storeReader["Finished"]);
                    entityReaderRoulette.Winner = Convert.ToBoolean(storeReader["Winner"]);
                    list_Bets.Add(entityReaderRoulette);
                }
                cn.Close();
                return list_Bets;
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
                return list_Bets;
            }
        }
    }
}
