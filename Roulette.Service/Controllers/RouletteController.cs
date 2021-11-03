using Microsoft.AspNetCore.Mvc;
using Roulette.Domain;
using Roulette.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Roulette.Service.Controllers
{
    public class RouletteController : ControllerBase
    {
        public int RegisterRoulette(EntityRoulette entidad)
        {
            DomainRoulette oEmpleadoDomain = new DomainRoulette();
            return oEmpleadoDomain.RegisterRoulette(entidad);
        }
    }
}
