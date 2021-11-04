using Newtonsoft.Json;
using Roulette.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Roulette.Access
{
    public class AccessBet
    {
        public static int Register_Bet(EntityBet bet)
        {
            string nomapi = System.Reflection.MethodBase.GetCurrentMethod().Name;
            int BetID = 0;
            //string IP = HttpContext.Connection.RemoteIpAddress.ToString()
            try
            {
                if (bet != null)
                {
                    string cadenaConexion = "Data Source=(localdb)\\MSSQLLocalDB;DataBase=MasivePrueba;Integrated Security=true";
                    SqlConnection cn = new SqlConnection(cadenaConexion);
                    cn.Open();
                    SqlCommand cmd = new SqlCommand("RUL.CreateBet", cn);
                    cmd.Parameters.Add(new SqlParameter("@RouletteID", SqlDbType.Int, 32)).Value = bet.RouletteID;
                    cmd.Parameters.Add(new SqlParameter("@UserID", SqlDbType.UniqueIdentifier)).Value = bet.UserID;
                    cmd.Parameters.Add(new SqlParameter("@BetValue", SqlDbType.Decimal)).Value = bet.BetValue;
                    cmd.Parameters.Add(new SqlParameter("@Number", SqlDbType.Int, 32)).Value = bet.Number;
                    cmd.Parameters.Add(new SqlParameter("@Finished", SqlDbType.Bit)).Value = bet.Finished;
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

        public static EntityBet Read_BetById(int? betID)
        {
            string nomapi = System.Reflection.MethodBase.GetCurrentMethod().Name;
            EntityBet item_Bet = new EntityBet();
            try
            {
                string cadenaConexion = "Data Source=(localdb)\\MSSQLLocalDB;DataBase=MasivePrueba;Integrated Security=true";
                SqlConnection cn = new SqlConnection(cadenaConexion);
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
    }
}
