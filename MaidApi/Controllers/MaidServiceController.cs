using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MaidApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaidServiceController : ControllerBase
    {
        [HttpGet("LoginPage/Message")]
        public async Task<string> GetLoginMessage()
        {
            return await Task.FromResult("Welcome to the Maid Service App!");
        }
    }
}
