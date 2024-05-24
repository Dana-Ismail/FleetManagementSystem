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
    public class RouteHistoryController : ControllerBase
    {
        private readonly RouteHistoryQueries _routeHistoryQueries;

        public RouteHistoryController()
        {
            _routeHistoryQueries = new RouteHistoryQueries();
        }

        [HttpPost("add")]
        public IActionResult AddHistoricalPoint([FromBody] GVAR data)
        {
            GVAR routeHistoryData = _routeHistoryQueries.AddHistoricalPoint(data);
            string gvarJson = JsonConvert.SerializeObject(routeHistoryData);

            // Broadcast the new route history to all connected clients
            WebSocketServer.Broadcast(gvarJson);

            return Ok(gvarJson);
        }

        [HttpPost("history")]
        public IActionResult RetrieveVehicleRouteHistory([FromBody] GVAR data)
        {
            GVAR routeHistoryData = _routeHistoryQueries.RetrieveVehicleRouteHistory(data);
            string gvarJson = JsonConvert.SerializeObject(routeHistoryData);
            return Ok(gvarJson);
        }
    }
}
