using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Roulette.Domain;
using Roulette.Entity;
using Roulette.Helpers.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Roulette.Service.Controllers
{
    public class RouletteController : ControllerBase
    {
        DomainRoulette oRouletteDomain = new DomainRoulette();
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

        private static void ReadCustomHeader(out string customHeader, string nameHeader, HttpContext context)
        {
            customHeader = string.Empty;
            if (context.Request.Headers.TryGetValue("thecodebuzz", out var traceValue))
            {
                customHeader = traceValue;
            }
        }
    }
}
