using Microsoft.AspNetCore.Mvc;
using Goal.Server.Data;
using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Goal.Server.Controllers
{
    [ApiController]
    [Route("logout")]
    public class LogoutController : ControllerBase
    {
        public LogoutController()
        {
        }

        [HttpGet]
        public void Get()
        {
            foreach (var cookie in Request.Cookies.Keys)
            {
                Response.Cookies.Delete(cookie);
            }

            Response.Redirect("/");
        }
       
    }
}