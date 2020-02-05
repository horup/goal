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
        private GoalDB dB;
        public GoalsController(GoalDB db)
        {
            this.dB = db;
        }

        [HttpGet]
        public List<Data.Goal> Get()
        {
            return dB.GetEntries();
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
                Description = newg.Description
            };
            dB.Insert(g);
        }

        [HttpDelete]
        public void Delete([FromBody]int id)
        {
            dB.Delete(id);
        }
    }
}