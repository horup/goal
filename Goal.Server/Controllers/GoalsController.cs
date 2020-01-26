using Microsoft.AspNetCore.Mvc;
using Goal.Server.Data;
using System.Collections.Generic;
using Goal.Common;
using System;
using System.ComponentModel.DataAnnotations;

namespace Goal.Server.Controllers
{
    [ApiController]
    [Route("api/1/goals")]
    public class GoalsController
    {
        private GoalDB dB;
        public GoalsController(GoalDB db)
        {
            this.dB = db;
        }

        [HttpGet]
        public List<GoalEntry> Get()
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
            var g = new GoalEntry()
            {
                Timestamp = DateTimeOffset.Now,
                Description = newg.Description
            };
            dB.Insert(g);
        }
    }
}