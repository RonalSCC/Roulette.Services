using Microsoft.AspNetCore.Mvc;
using Roulette.Domain;
using Roulette.Entity;
using Roulette.Helpers.General;
using Roulette.Helpers.General.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Roulette.Service.Controllers
{
    public class BetController : ControllerBase
    {
        DomainBet oBetDomain = new DomainBet();
        DomainRoulette oRouletteDomain = new DomainRoulette();
        public ResponseRegisterBet RegisterBet(EntityBet entidad)
        {
            ResponseRegisterBet objReturn = new ResponseRegisterBet();
            try
            {
                var IDRoulette = oBetDomain.RegisterBet(entidad);
                if (IDRoulette != 0)
                {
                    var objListRoulettes = GetBetItem(new InputGetRouletteItem { rouletteID = IDRoulette });
                    if (objListRoulettes != null && objListRoulettes.Success == true && objListRoulettes.ItemBet != null)
                    {
                        objReturn.Success = true;
                        objReturn.Message = "Bet registered correctly";
                        objReturn.BetRegister = objListRoulettes.ItemBet;
                    }
                    else
                    {
                        objReturn.Success = false;
                        objReturn.Message = "No saved bet information found";
                    }
                }
                else
                {
                    objReturn.Success = false;
                    objReturn.Message = "There was a problem registering the bet, try again";
                }

                return objReturn;
            }
            catch (Exception ex)
            {
                objReturn.Success = false;
                objReturn.Message = "There was a problem registering the bet, try again";
                return objReturn;
            }
        }

        public ResponseGetBetItem GetBetItem(InputGetRouletteItem conditionalData)
        {
            ResponseGetBetItem objReturn = new ResponseGetBetItem();
            try
            {
                var objListRoulettes = oBetDomain.GetBetByID(conditionalData?.rouletteID);
                if (objListRoulettes != null)
                {
                    objReturn.Success = true;
                    objReturn.ItemBet = objListRoulettes;
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
    }
}
