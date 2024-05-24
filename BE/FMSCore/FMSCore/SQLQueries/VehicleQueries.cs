using FPro;
using Npgsql;
using System;
using System.Data;
using System.Linq;
using Newtonsoft.Json;

namespace FMSCore.SQLqueries
{
    public class VehicleQueries
    {
        private readonly DatabaseConnection _dbConnection;

        public VehicleQueries()
        {
            _dbConnection = new DatabaseConnection();
        }

        public GVAR GetVehicles() {
            string query = "SELECT * FROM \"Vehicles\"";
            NpgsqlDataReader reader = _dbConnection.ExecuteReader(query);
            DataTable vehicles = new DataTable();
            vehicles.Load(reader);

            DataTable stringVehiclesData = vehicles.Clone();
            foreach (DataRow row in vehicles.Rows)
            { stringVehiclesData.Rows.Add(row.ItemArray.Select(item => item.ToString()).ToArray()); }

            GVAR Gvar = new GVAR();
            Gvar.DicOfDT["Vehicles"] = stringVehiclesData;

            string gvarJson = JsonConvert.SerializeObject(Gvar);
            Console.WriteLine(gvarJson);

            return Gvar;
        }

        public GVAR AddVehicle(GVAR data) {
            var vehicleNumber = Convert.ToInt64(data.DicOfDic["Tags"]["VehicleNumber"]);
            var vehicleType = data.DicOfDic["Tags"]["VehicleType"];
            string query = $"INSERT INTO \"Vehicles\" (\"VehicleNumber\", \"VehicleType\") VALUES ('{vehicleNumber}', '{vehicleType}')";
            data.DicOfDic["Tags"]["STS"] = "1";
            _dbConnection.ExecuteNonQuery(query);
            return data;
        }

        public GVAR UpdateVehicle(GVAR data) {
            var vehicleId = Convert.ToInt64(data.DicOfDic["Tags"]["VehicleID"]);
            var vehicleNumber = Convert.ToInt64(data.DicOfDic["Tags"]["VehicleNumber"]);
            var vehicleType = data.DicOfDic["Tags"]["VehicleType"];
            string query = $"UPDATE \"Vehicles\" SET \"VehicleNumber\" = '{vehicleNumber}', \"VehicleType\" = '{vehicleType}' WHERE \"VehicleID\" = {vehicleId}";
            _dbConnection.ExecuteNonQuery(query);
            data.DicOfDic["Tags"]["STS"] = "1";
            return data;
        }

        public GVAR DeleteVehicle(GVAR data) {
            var vehicleId = Convert.ToInt64(data.DicOfDic["Tags"]["VehicleID"]);
            // for updating the ids
            _dbConnection.ExecuteNonQuery("CREATE SEQUENCE temp_vehicleid_seq START 1");
            _dbConnection.ExecuteNonQuery(@"
            WITH numbered_vehicles AS (
            SELECT ""VehicleID"", nextval('temp_vehicleid_seq') as new_id
            FROM ""Vehicles""
            ORDER BY ""VehicleID"" )
            UPDATE ""Vehicles""
            SET ""VehicleID"" = nv.new_id
            FROM numbered_vehicles nv
            WHERE ""Vehicles"".""VehicleID"" = nv.""VehicleID"""
            );
            _dbConnection.ExecuteNonQuery("DROP SEQUENCE temp_vehicleid_seq");

            string query = $"DELETE FROM \"Vehicles\" WHERE \"VehicleID\" = {vehicleId}";
            _dbConnection.ExecuteNonQuery(query);
            data.DicOfDic["Tags"]["STS"] = "1";
            return data;
        }


    }
}
