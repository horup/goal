using Microsoft.AspNetCore.Mvc;
using Goal.Server.Data;
using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;

namespace Goal.Server.Controllers
{
    [ApiController]
    [Authorize]
    [Route("login")]
    public class LoginController : ControllerBase
    {
        public LoginController()
        {
        }

        [HttpGet]
        public void Get()
        {
            HttpContext.Response.Redirect("/");
        }
       
    }
}