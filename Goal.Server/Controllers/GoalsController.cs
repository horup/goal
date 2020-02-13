using Microsoft.AspNetCore.Mvc;
using Goal.Server.Data;
using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;

namespace Goal.Server.Controllers
{
    [ApiController]
    [Route("api/1/goals")]
    [Authorize]
    public class GoalsController : ControllerBase
    {
        private GoalDB db;
        public GoalsController(GoalDB db)
        {
            this.db = db;
        }

        public string Nameidentifier
        {
            get
            {
                return User.FindFirst(p=>p.Type.Contains("nameidentifier")).Value;
            }
        }

        [HttpGet]
        public List<Data.Goal> Get()
        {
            return db.GetEntries(Nameidentifier);
        }

        public class PostGoal
        {
            [Required]
            public string Description {get;set;}
        }

        [HttpPost]
        public void Post([FromBody]PostGoal newg)
        {
            var g = new Data.Goal()
            {
                Timestamp = DateTimeOffset.Now,
                Description = newg.Description,
                Owner = Nameidentifier
            };
            db.Insert(g, Nameidentifier);
        }

        [HttpDelete]
        public void Delete([FromBody]int id)
        {
            db.Delete(id, Nameidentifier);
        }
    }
}