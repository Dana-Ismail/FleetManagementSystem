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
    public class GeofenceController : ControllerBase
    {
        private readonly GeofenceQueries _geofenceQueries;
        private readonly CircularGeofenceQueries _circularGeofenceQueries;
        private readonly PolygonalGeofenceQueries _polygonalGeofenceQueries;
        private readonly RectangularGeofenceQueries _rectangularGeofenceQueries;

        public GeofenceController() {
            _geofenceQueries = new GeofenceQueries();
            _circularGeofenceQueries = new CircularGeofenceQueries();
            _polygonalGeofenceQueries = new PolygonalGeofenceQueries();
            _rectangularGeofenceQueries = new RectangularGeofenceQueries();
        }

        [HttpGet("all")]
        public IActionResult GetGeofenceInformation() {
            GVAR geofencesData = _geofenceQueries.GetGeofenceInformation();
            string gvarJson = JsonConvert.SerializeObject(geofencesData);
            return Ok(gvarJson);
        }

        [HttpGet("circular")]
        public IActionResult RetrieveCircularGeofencesCoordinates() {
            GVAR circularGeofencesData = _circularGeofenceQueries.RetrieveCircularGeofencesCoordinates();
            string gvarJson = JsonConvert.SerializeObject(circularGeofencesData);
            return Ok(gvarJson);
        }

        [HttpGet("rectangular")]
        public IActionResult RetrieveRectangularGeofencesCoordinates() {
            GVAR rectangularGeofencesData = _rectangularGeofenceQueries.RetrieveRectangularGeofencesCoordinates();
            string gvarJson = JsonConvert.SerializeObject(rectangularGeofencesData);
            return Ok(gvarJson);
        }

        [HttpGet("polygonal")]
        public IActionResult RetrievePolygonalGeofencesCoordinates() {
            GVAR polygonalGeofencesData = _polygonalGeofenceQueries.RetrievePolygonalGeofencesCoordinates();
            string gvarJson = JsonConvert.SerializeObject(polygonalGeofencesData);
            return Ok(gvarJson);
        }
    }
}
