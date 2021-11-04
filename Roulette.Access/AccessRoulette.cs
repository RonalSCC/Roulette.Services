using System;
using System.Collections.Generic;
using System.Text;
using Roulette.Entity;
using System.Data;
using System.Data.SqlClient;
using Newtonsoft.Json;
using System.Linq;

namespace Roulette.Access
{
    public class AccessRoulette
    {
        public static int Register_Roulette(EntityRoulette roulette)
        {
            string nomapi = System.Reflection.MethodBase.GetCurrentMethod().Name;
            int RouletteID = 0;
            //string IP = HttpContext.Connection.RemoteIpAddress.ToString()
            try
            {
                if (roulette != null)
                {
                    string cadenaConexion = "Data Source=(localdb)\\MSSQLLocalDB;DataBase=MasivePrueba;Integrated Security=true";
                    SqlConnection cn = new SqlConnection(cadenaConexion);
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

        public static EntityRoulette Update_Roulette(EntityRoulette roulette)
        {
            string nomapi = System.Reflection.MethodBase.GetCurrentMethod().Name;
            EntityRoulette rouletteUpdated = null;
            try
            {
                if (roulette != null)
                {
                    string cadenaConexion = "Data Source=(localdb)\\MSSQLLocalDB;DataBase=MasivePrueba;Integrated Security=true";
                    SqlConnection cn = new SqlConnection(cadenaConexion);
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
                    if (searchRoulette != null )
                    {
                        rouletteUpdated = searchRoulette;
                    }
                }
                return rouletteUpdated;
            }
            catch (Exception ex)
            {
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

        public static List<EntityRoulette> Read_Roulettes()
        {
            string nomapi = System.Reflection.MethodBase.GetCurrentMethod().Name;
            List<EntityRoulette> list_Roulettes = new List<EntityRoulette>();
            try
            {
                string cadenaConexion = "Data Source=(localdb)\\MSSQLLocalDB;DataBase=MasivePrueba;Integrated Security=true";
                SqlConnection cn = new SqlConnection(cadenaConexion);
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

        public static EntityRoulette Read_RouletteById(int? rouletteID)
        {
            string nomapi = System.Reflection.MethodBase.GetCurrentMethod().Name;
            EntityRoulette item_Roulette = new EntityRoulette();
            try
            {
                string cadenaConexion = "Data Source=(localdb)\\MSSQLLocalDB;DataBase=MasivePrueba;Integrated Security=true";
                SqlConnection cn = new SqlConnection(cadenaConexion);
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

    }
}
