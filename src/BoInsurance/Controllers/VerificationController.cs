using Microsoft.AspNetCore.Mvc;
using System;

namespace Insurance.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VerificationController : Controller
    {
        [HttpPost]
        [Route("[action]")]
        public IActionResult DrivingLicenseCallback([FromBody]object body)
        {
            // api/Verification/DrivingLicenseCallback
            Console.WriteLine(body);
            // TODO verify
            // TODO send event to update UI with the data and 
            return Ok();
        }
    }
}