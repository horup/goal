using Microsoft.AspNetCore.Mvc;
using Goal.Server.Data;

namespace Goal.Server.Controllers
{
    [ApiController]
    [Route("api/1/goal")]
    public class GoalController
    {
        private GoalDB dB;
        public GoalController(GoalDB db)
        {
            this.dB = db;
        }
        
        [HttpGet]
        public string Get()
        {
            return dB.ListEntries().Count.ToString();
        }
    }
}