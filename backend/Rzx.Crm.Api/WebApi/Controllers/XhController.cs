using Microsoft.AspNetCore.Mvc;
using Rzx.Crm.Api.WebApi.Models;

namespace Rzx.Crm.Api.WebApi.Controllers
{
    [Route("xh")]
    [ApiController]
    public class XhController : ControllerBase
    {
        [HttpGet]
        [Route("authStatus")]
        public IActionResult GetAuthStatus()
        {
            return Ok(new { authenticated = true });
        }

        [HttpGet]
        [Route("getIdentity")]
        public IActionResult GetIdentity()
        {
            return Ok(new { user = new { userName = "user", email = "user@crm.com", displayName = "user", active = true }, roles = new string[] { "USER" } });
        }

        [HttpGet]
        [Route("getConfig")]
        public IActionResult GetConfig()
        {
            return Ok(new XhGetConfigResponse());
        }

        [HttpGet]
        [Route("environment")]
        public IActionResult GetEnvironment()
        {
            return Ok(new XhEnvironmentResponse());
        }

        [HttpGet]
        [Route("environmentPoll")]
        public IActionResult GetEnvironmentPoll()
        {
            return Ok(new XhEnvironmentPollResponse());
        }

        [HttpPost]
        [Route("getPrefs")]
        public IActionResult GetPrefs()
        {
            return Ok(new XhGetPrefsResponse());
        }
    }
}
