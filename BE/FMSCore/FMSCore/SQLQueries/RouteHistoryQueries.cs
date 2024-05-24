using System;
using System.Data;
using FPro;
using Newtonsoft.Json;
using System.Linq;

namespace FMSCore.SQLqueries
{
    public class RouteHistoryQueries
    {
        private readonly DatabaseConnection _dbConnection;

        public RouteHistoryQueries() {
            _dbConnection = new DatabaseConnection();
        }

        public GVAR AddHistoricalPoint(GVAR data) {
            var vehicleId = Convert.ToInt64(data.DicOfDic["Tags"]["VehicleID"]);
            var vehicleDirection = Convert.ToInt64(data.DicOfDic["Tags"]["VehicleDirection"]);
            var status = data.DicOfDic["Tags"]["Status"];
            var vehicleSpeed = data.DicOfDic["Tags"]["VehicleSpeed"];
            var epoch = Convert.ToInt64(data.DicOfDic["Tags"]["Epoch"]);
            var address = data.DicOfDic["Tags"]["Address"];
            var latitude = Convert.ToDouble(data.DicOfDic["Tags"]["Latitude"]);
            var longitude = Convert.ToDouble(data.DicOfDic["Tags"]["Longitude"]);

            string query = $"INSERT INTO \"RouteHistory\" (\"VehicleID\", \"VehicleDirection\", \"Status\", \"VehicleSpeed\", \"Epoch\", \"Address\", \"Latitude\", \"Longitude\") " +
               $"VALUES ({vehicleId}, {vehicleDirection}, '{status}', '{vehicleSpeed}', {epoch}, '{address}', {latitude}, {longitude})";

            _dbConnection.ExecuteNonQuery(query);
            data.DicOfDic["Tags"]["STS"] = "1";
            return data;
        }

        public GVAR RetrieveVehicleRouteHistory(GVAR data) {
            var vehicleId = Convert.ToInt64(data.DicOfDic["Tags"]["VehicleID"]);
            var startTimeEpoch = Convert.ToInt64(data.DicOfDic["Tags"]["StartTimeEpoch"]);
            var endTimeEpoch = Convert.ToInt64(data.DicOfDic["Tags"]["EndTimeEpoch"]);

            string query = $"SELECT rh.\"VehicleID\", v.\"VehicleNumber\", rh.\"Address\", rh.\"Status\", " +
                           $"rh.\"Latitude\", rh.\"Longitude\", rh.\"VehicleDirection\", " +
                           $"rh.\"VehicleSpeed\", rh.\"Epoch\" AS \"GPSTime\" " +
                           $"FROM \"RouteHistory\" rh " +
                           $"JOIN \"Vehicles\" v ON rh.\"VehicleID\" = v.\"VehicleID\" " +
                           $"WHERE rh.\"VehicleID\" = {vehicleId} AND rh.\"Epoch\" " +
                           $"BETWEEN {startTimeEpoch} AND {endTimeEpoch} " +
                           $"ORDER BY rh.\"Epoch\" ASC";

            DataTable routeHistoryData = new DataTable();
            using (var reader = _dbConnection.ExecuteReader(query)) { routeHistoryData.Load(reader); }

            DataTable stringRouteHistoryData = routeHistoryData.Clone();
            foreach (DataRow row in routeHistoryData.Rows)
            { stringRouteHistoryData.Rows.Add(row.ItemArray.Select(item => item.ToString()).ToArray()); }

            GVAR Gvar = new GVAR();
            Gvar.DicOfDT["RouteHistory"] = stringRouteHistoryData;

            string gvarJson = JsonConvert.SerializeObject(Gvar);
            Console.WriteLine(gvarJson);

            return Gvar;
        }
    }
}