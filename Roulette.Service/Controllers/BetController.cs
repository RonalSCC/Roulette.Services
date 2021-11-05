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
    public class BetController : ControllerBase
    {
        private readonly IConfiguration m_config;
        DomainBet oBetDomain = new DomainBet();
        DomainRoulette oRouletteDomain = new DomainRoulette();
        DomainBetUser oBetUserDomain = new DomainBetUser();
        LoginController oLoginController = new LoginController();
        object maxValueBet = new object();
        object minNumberBet = new object();
        object maxNumberBet = new object();
        public BetController(IConfiguration config)
        {
            m_config = config;
            oRouletteDomain.connectionString = m_config.GetConnectionString("MyConnection");
            oBetDomain.connectionString = m_config.GetConnectionString("MyConnection");
            oBetUserDomain.connectionString = m_config.GetConnectionString("MyConnection");
            maxValueBet = m_config.GetValue(typeof(String),"maxValueBet");
            minNumberBet = m_config.GetValue(typeof(String), "minNumberBet");
            maxNumberBet = m_config.GetValue(typeof(String), "maxNumberBet");
        }
        public ResponseRegisterBet RegisterBet([FromBody]EntityBet entidad)
        {
            ResponseRegisterBet objReturn = new ResponseRegisterBet();
            try
            {
                if (entidad == null || entidad.RouletteID == 0)
                {
                    objReturn.Success = false;
                    objReturn.Message = "Roulette not found";
                    return objReturn;
                }
                if (!ValidateBetValue(entidad))
                {
                    objReturn.Success = false;
                    objReturn.Message = $"You cannot place bets higher than {maxValueBet.ToString()}";
                    return objReturn;
                }
                if (!ValidateBetNumber(entidad))
                {
                    objReturn.Success = false;
                    objReturn.Message = $"You can only bet numbers from {minNumberBet} to {maxNumberBet}";
                    return objReturn;
                }
                var nGuidUser = Guid.NewGuid();
                var successRegister = oBetUserDomain.RegisterBetUser(new EntityBetUser
                {
                    UserID = nGuidUser,
                    Active = true,
                    Amount = entidad.BetValue,
                    UserName = "New user"
                });

                if (successRegister == false)
                {
                    objReturn.Success = false;
                    objReturn.Message = $"The user could not be created to place the bet";
                    return objReturn;
                }
                entidad.UserID = nGuidUser;
                var IDBet = oBetDomain.RegisterBet(entidad);
                var objListRoulettes = GetBetItem(new InputGetBetItem { betID = IDBet });
                if (objListRoulettes != null && objListRoulettes.Success == true && objListRoulettes.ItemBet != null)
                {
                    objReturn.Success = true;
                    objReturn.Message = "Bet registered correctly";
                    objReturn.BetRegister = objListRoulettes.ItemBet;
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
        public ResponseListBets ListBets()
        {
            ResponseListBets objReturn = new ResponseListBets();
            try
            {
                objReturn.ListBets = oBetDomain.ListBets();
                objReturn.Success = true;
                if (objReturn.ListBets != null && objReturn.ListBets.Count > 0)
                {
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
        public ResponseGetBetItem GetBetItem(InputGetBetItem conditionalData)
        {
            ResponseGetBetItem objReturn = new ResponseGetBetItem();
            try
            {
                var objListBet = oBetDomain.GetBetByID(conditionalData?.betID);
                if (objListBet != null)
                {
                    objReturn.Success = true;
                    objReturn.ItemBet = objListBet;
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
        public bool ValidateBetValue(EntityBet bet) {
            try
            {
                decimal maxValueValidate = Convert.ToDecimal(maxValueBet);
                if (bet.BetValue <= maxValueValidate)
                {
                    return true;
                }
                else {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool ValidateBetNumber(EntityBet bet)
        {
            try
            {
                int maxNumberValidate = Convert.ToInt32(maxNumberBet);
                int minNumberValidate = Convert.ToInt32(minNumberBet);
                if (bet.Number >= minNumberValidate && bet.Number <= maxNumberValidate)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
