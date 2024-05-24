using FPro;
using FMSCore.SQLqueries;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Cors;

namespace FMS_Endpoints.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("AllowSpecificOrigin")]
    public class VehiclesController : ControllerBase
    {
        private readonly VehicleQueries _vehicleQueries;
        private readonly VehiclesInformationsQueries _vehicleInfoQueries;

        public VehiclesController() {
            _vehicleQueries = new VehicleQueries();
            _vehicleInfoQueries = new VehiclesInformationsQueries();
        }

        [HttpGet("all")]
        public IActionResult GetVehicles() {
            GVAR vehiclesData = _vehicleQueries.GetVehicles();
            string gvarJson = JsonConvert.SerializeObject(vehiclesData);
            return Ok(gvarJson);
        }

        [HttpPost("add")]
        public IActionResult AddVehicle([FromBody] GVAR data) {
            GVAR gvar = _vehicleQueries.AddVehicle(data);
            return Ok(gvar);
        }

        [HttpPut("update")]
        public IActionResult UpdateVehicle([FromBody] GVAR data) {
            GVAR gvar = _vehicleQueries.UpdateVehicle(data);
            return Ok(gvar);
        }

        [HttpPost("delete")]
        public IActionResult DeleteVehicle([FromBody] GVAR data) {
            GVAR gvar = _vehicleQueries.DeleteVehicle(data);
            return Ok(gvar);
        }
        [HttpGet("basicInfo/all")]
        public IActionResult GetBasicVehicle()
        {
            GVAR vehiclesData = _vehicleInfoQueries.GetBasicVehicle();
            string gvarJson = JsonConvert.SerializeObject(vehiclesData);
            return Ok(gvarJson);
        }
        [HttpGet("vehiclesinformation/all")]
        public IActionResult GetVehiclesInformation() {
            GVAR vehicleInfo = _vehicleInfoQueries.GetVehiclesInformation();
            string gvarJson = JsonConvert.SerializeObject(vehicleInfo);
            return Ok(gvarJson);
        }

        [HttpGet("vehiclesinformation/details")]
        public IActionResult GetDetailedVehicleInformation([FromQuery] GVAR data) {
            var detailedVehicleInfo = _vehicleInfoQueries.GetDetailedVehicleInformation(data);
            string gvarJson = JsonConvert.SerializeObject(detailedVehicleInfo);
            return Ok(gvarJson);
        }

        [HttpPost("vehiclesinformation/add")]
        public IActionResult AddVehicleInformation([FromBody] GVAR data) {
            var detailedVehicleInfo = _vehicleInfoQueries.AddVehicleInformation(data);
            string gvarJson = JsonConvert.SerializeObject(detailedVehicleInfo);
            return Ok(gvarJson); ;
        }

        [HttpPut("vehiclesinformation/update")]
        public IActionResult UpdateVehicleInformation([FromBody] GVAR data) {
            var detailedVehicleInfo = _vehicleInfoQueries.UpdateVehicleInformation(data);
            string gvarJson = JsonConvert.SerializeObject(detailedVehicleInfo);
            return Ok(gvarJson);
        }

        [HttpPost("vehiclesinformation/delete")]
        public IActionResult DeleteVehicleInformation([FromBody] GVAR data) {
            var detailedVehicleInfo = _vehicleInfoQueries.DeleteVehicleInformation(data);
            string gvarJson = JsonConvert.SerializeObject(detailedVehicleInfo);
            return Ok(gvarJson);
        }
    }
}
