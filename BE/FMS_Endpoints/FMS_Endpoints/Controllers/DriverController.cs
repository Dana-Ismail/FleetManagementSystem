using FMSCore.SQLqueries;
using Microsoft.AspNetCore.Mvc;
using FPro;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Cors;

namespace FMS_Endpoints.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("AllowSpecificOrigin")]
    public class DriverController : ControllerBase
    {
        private readonly DriverQueries _driverQueries;

        public DriverController() {
            _driverQueries = new DriverQueries();
        }

        [HttpGet("all")]
        public IActionResult GetDrivers()
        {
            GVAR driversData = _driverQueries.GetDrivers();
            string gvarJson = JsonConvert.SerializeObject(driversData);
            return Ok(gvarJson);
        }

        [HttpPost]
        [Route("add")]
        public IActionResult AddDriver([FromBody] GVAR data) {
            GVAR gvar = _driverQueries.AddDriver(data);
            return Ok(gvar);
        }

        [HttpPut]
        [Route("update")]
        public IActionResult UpdateDriver([FromBody] GVAR data) {
            GVAR gvar = _driverQueries.UpdateDriver(data);
            return Ok(gvar);
        }
    }
}
