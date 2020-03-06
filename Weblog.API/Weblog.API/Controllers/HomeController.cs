using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Weblog.API.DbContexts;

namespace Weblog.API.Controllers
{
    [Route("api")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly WeblogContext _ctx;

        public HomeController(WeblogContext context)
        {
            this._ctx = context ?? throw new ArgumentNullException(nameof(context));
        }

        [HttpGet]
        public IActionResult TestDB()
        {
            return Ok();
        }
    }
}
