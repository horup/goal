using Microsoft.AspNetCore.Mvc;

namespace Goal.Server.Controllers
{
    [ApiController]
    [Route("api/1/goal")]
    public class GoalController
    {
        public string Get()
        {
            return "hello world";
        }
    }
}