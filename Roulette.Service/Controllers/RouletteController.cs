using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Roulette.Domain;
using Roulette.Entity;
using Roulette.Helpers.General;
using Roulette.Helpers.General.Input;
using Roulette.Helpers.General.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Roulette.Service.Controllers
{
    public class RouletteController : ControllerBase
    {
        private readonly IConfiguration m_config;
        DomainRoulette oRouletteDomain = new DomainRoulette();
        DomainBet oBetDomain = new DomainBet();
        public RouletteController(IConfiguration config)
        {
            m_config = config;
            oRouletteDomain.connectionString = m_config.GetConnectionString("MyConnection");
            oBetDomain.connectionString = m_config.GetConnectionString("MyConnection");
        }
        public ResponseRegisterRoulette RegisterRoulette(EntityRoulette entidad)
        {
            ResponseRegisterRoulette objReturn = new ResponseRegisterRoulette();
            try
            {
                var IDRoulette = oRouletteDomain.RegisterRoulette(entidad);
                if (IDRoulette != 0)
                {
                    var objListRoulettes = GetRouletteItem(new InputGetRouletteItem { rouletteID = IDRoulette });
                    if (objListRoulettes != null && objListRoulettes.Success == true && objListRoulettes.ItemRoulette != null)
                    {
                        objReturn.Success = true;
                        objReturn.Message = "Roulette registered correctly";
                        objReturn.Roulette = objListRoulettes.ItemRoulette;
                    }
                    else
                    {
                        objReturn.Success = false;
                        objReturn.Message = "No saved roulette information found";
                    }
                }
                else {
                    objReturn.Success = false;
                    objReturn.Message = "There was a problem registering the roulette, try again";
                }

                return objReturn;
            }
            catch (Exception ex)
            {
                objReturn.Success = false;
                objReturn.Message = "There was a problem registering the roulette, try again";
                return objReturn;
            }
        }
        public ResponseUpdateRoulette OpenRoulette(InputOpenRoulette infoRoulette)
        {
            ResponseUpdateRoulette objReturn = new ResponseUpdateRoulette();
            try
            {
                if (infoRoulette?.RouletteID != 0)
                {
                    var objResponseListRoulette = GetRouletteItem(new InputGetRouletteItem { rouletteID = infoRoulette.RouletteID });
                    if (objResponseListRoulette != null && objResponseListRoulette.Success == true && objResponseListRoulette.ItemRoulette != null)
                    {
                        EntityRoulette rouletteOpen = objResponseListRoulette.ItemRoulette;
                        if (rouletteOpen?.Open == false)
                        {
                            rouletteOpen.Open = true;
                            rouletteOpen.OpenningDate = DateTime.Now;
                            EntityRoulette responseUpdate = oRouletteDomain.UpdateRoulette(rouletteOpen);
                            if (responseUpdate != null)
                            {
                                objReturn.Success = true;
                                objReturn.Message = "Roulette open correctly";
                                objReturn.StateRoulette = responseUpdate.Open;
                            }
                            else
                            {
                                objReturn.Success = false;
                                objReturn.Message = "An error occurred in the roulette update";
                            }
                        }
                        else {
                            objReturn.StateRoulette = rouletteOpen.Open;
                            objReturn.Success = true;
                            objReturn.Message = "Roulette is already open";
                        }
                        
                    }
                    else {
                        objReturn.Success = false;
                        objReturn.Message = "Please verify the roulette number, no information found";
                    }
                }
                else
                {
                    objReturn.Success = false;
                    objReturn.Message = "Please verify the roulette number, no information found";
                }
                return objReturn;
            }
            catch (Exception ex)
            {
                objReturn.Success = false;
                objReturn.Message = "An error occurred in the roulette update";
                return objReturn;
            }
        }
        public ResponseListRoulettes ListRoulettes()
        {
            ResponseListRoulettes objReturn = new ResponseListRoulettes();
            try
            {
                objReturn.ListRoulettes = oRouletteDomain.ListRoulette();
                objReturn.Success = true;
                if (objReturn.ListRoulettes != null && objReturn.ListRoulettes.Count > 0)
                {
                    objReturn.Message = "List consulted correctly";
                }
                else { 
                    objReturn.Message = "No roulette information found";
                }
                return objReturn;
            }
            catch (Exception ex)
            {
                objReturn.Success = false;
                objReturn.Message = "An error occurred in the roulette update";
                return objReturn;
            }
        }
        public ResponseGetRouletteItem GetRouletteItem(InputGetRouletteItem conditionalData)
        {
            ResponseGetRouletteItem objReturn = new ResponseGetRouletteItem();
            try
            {
                var objListRoulettes = oRouletteDomain.GetRouletteByID(conditionalData?.rouletteID);
                if (objListRoulettes != null)
                {
                    objReturn.Success = true;
                    objReturn.ItemRoulette = objListRoulettes;
                    objReturn.Message = "List consulted correctly";
                }
                else
                {
                    objReturn.Message = "No roulette information found";
                }
                return objReturn;
            }
            catch (Exception ex)
            {
                objReturn.Success = false;
                objReturn.Message = "An error occurred in the roulette update";
                return objReturn;
            }
        }
        public ResponseCloseRoulette CloseRoulette([FromBody]InputCloseRoulette infoRoulette)
        {
            ResponseCloseRoulette objReturn = new ResponseCloseRoulette();
            try
            {
                if (infoRoulette?.RouletteID == 0)
                {
                    objReturn.Success = false;
                    objReturn.Message = "Please verify the roulette number, no information found";
                    return objReturn;
                }
                var responseRouletteSearch = GetRouletteItem(new InputGetRouletteItem { rouletteID = infoRoulette?.RouletteID });
                if (responseRouletteSearch == null || responseRouletteSearch.ItemRoulette == null)
                {
                    objReturn.Success = false;
                    objReturn.Message = "You have specified a non-existent roulette";
                    return objReturn;
                }
                var responseListBets = oBetDomain.ListBets();
                if (responseListBets == null || responseListBets.Count == 0)
                {
                    objReturn.Success = false;
                    objReturn.Message = "No bets created yet";
                    return objReturn;
                }
                var objRoulete = responseRouletteSearch.ItemRoulette;
                var listBets = responseListBets.Where(x => x.Finished != true && x.RouletteID == objRoulete.RouletteID).ToList();
                if (listBets == null || listBets.Count == 0)
                {
                    objReturn.Success = false;
                    objReturn.Message = "There are no bets for roulette";
                    return objReturn;
                }
                Random random = new Random();
                int WinningNumber = random.Next(0, 36);

                var objSendFinish = new InputFinish_Roulette();
                objSendFinish.RouletteID = objRoulete.RouletteID;
                var betWinner = listBets.FirstOrDefault(x => x.Number == WinningNumber);
                if (betWinner != null)
                {
                    objSendFinish.BetWinnerID = betWinner.BetID;
                }
                var OddOrEven = WinningNumber % 2;
                var listWinnersWithColor = listBets.Where(x => x.Number % 2 == OddOrEven).ToList();
                if (listWinnersWithColor != null && listWinnersWithColor.Count > 0)
                {
                    objSendFinish.BetWinnerColorIDs = string.Join(',', listWinnersWithColor.Select(y=> y.BetID));
                }
                var resultFinish = oRouletteDomain.Finish_Roulette(objSendFinish);
                if (resultFinish == false)
                {
                    objReturn.Success = false;
                    objReturn.Message = "We could not finish the roulette, try again";
                    return objReturn;
                }
                objReturn.WinningNumber = WinningNumber;
                objReturn.Success = true;
                objReturn.Message = "Roulette successfully completed";
                var updateListBets = oBetDomain.ListBets();
                if (updateListBets != null && updateListBets.Count > 0)
                {
                    objReturn.ListBetsFinished = updateListBets.Where(x=> x.RouletteID == objRoulete.RouletteID && x.Finished == true).ToList();
                }
                return objReturn;
            }
            catch (Exception ex)
            {
                objReturn.Success = false;
                objReturn.Message = "An error occurred in the roulette update";
                return objReturn;
            }
        }
    }
}
