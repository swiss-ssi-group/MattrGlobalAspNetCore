using Microsoft.AspNetCore.Mvc;
using System;

namespace BoInsurance.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VerificationController : Controller
    {
        private readonly BoInsuranceDbService _boInsuranceDbService;

        public VerificationController(BoInsuranceDbService boInsuranceDbService)
        {
            _boInsuranceDbService = boInsuranceDbService;
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult DrivingLicenseCallback([FromBody]object body)
        {
            // api/Verification/DrivingLicenseCallback
            Console.WriteLine(body);
            // TODO verify using challenge
            // TODO send event to update UI with the data and 
            return Ok();
        }
    }
}