using Microsoft.AspNetCore.Mvc;
using Goal.Server.Data;
using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using System.Security.Principal;

namespace Goal.Server.Controllers
{
    public class Me
    {
        public string Id {get;set;}
        public string Fullname {get;set;}
        public string Email {get;set;}
    }
    [ApiController]
    [Authorize]
    [Route("api/1/me")]
    public class MeController : ControllerBase
    {
        public MeController()
        {
        }

        [HttpGet]
        public Me Get()
        {

            return new Me()
            {
                Id = User.FindFirst(c=>c.Type.Contains("nameidentifier"))?.Value,
                Fullname = User.Identity.Name,
                Email = User.FindFirst(c=>c.Type.Contains("email"))?.Value
            };
        }
       
    }
}