using System;
using FPro;
using System.Data;
using Npgsql;
using FMSCore;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace FMSCore.SQLqueries
{
    public class VehiclesInformationsQueries
    {
        private readonly DatabaseConnection _dbConnection;

        public VehiclesInformationsQueries() {
            _dbConnection = new DatabaseConnection();
        }

        public GVAR AddVehicleInformation(GVAR data) {
            var vehicleId = Convert.ToInt64(data.DicOfDic["Tags"]["VehicleID"]);
            var driverId = Convert.ToInt64(data.DicOfDic["Tags"]["DriverID"]);
            var vehicleMake = data.DicOfDic["Tags"]["VehicleMake"];
            var vehicleModel = data.DicOfDic["Tags"]["VehicleModel"];
            var purchaseDate = Convert.ToInt64(data.DicOfDic["Tags"]["PurchaseDate"]);

            string query = $"INSERT INTO \"VehiclesInformations\" (\"VehicleID\", \"DriverID\", \"VehicleMake\", \"VehicleModel\", \"PurchaseDate\") VALUES ({vehicleId}, '{driverId}', '{vehicleMake}', '{vehicleModel}', {purchaseDate})";
            _dbConnection.ExecuteNonQuery(query);
            data.DicOfDic["Tags"]["STS"] = "1";
            return data;
        }

        public GVAR UpdateVehicleInformation(GVAR data) {
            var vehicleId = Convert.ToInt64(data.DicOfDic["Tags"]["VehicleID"]);
            var driverId = Convert.ToInt64(data.DicOfDic["Tags"]["DriverID"]);
            var vehicleMake = data.DicOfDic["Tags"]["VehicleMake"];
            var vehicleModel = data.DicOfDic["Tags"]["VehicleModel"];
            var purchaseDate = Convert.ToInt64(data.DicOfDic["Tags"]["PurchaseDate"]);

            string query = $"UPDATE \"VehiclesInformations\" SET \"DriverID\" = '{driverId}', \"VehicleMake\" = '{vehicleMake}', \"VehicleModel\" = '{vehicleModel}', \"PurchaseDate\" = {purchaseDate} WHERE \"VehicleID\" = {vehicleId}";
            _dbConnection.ExecuteNonQuery(query);
            data.DicOfDic["Tags"]["STS"] = "1";
            return data;
        }

        public GVAR DeleteVehicleInformation(GVAR data) {
            var vehicleId = Convert.ToInt64(data.DicOfDic["Tags"]["VehicleID"]);
            string query = $"DELETE FROM \"VehiclesInformations\" WHERE \"VehicleID\" = {vehicleId}";
            _dbConnection.ExecuteNonQuery(query);
            data.DicOfDic["Tags"]["STS"] = "1";
            return data;
        }
        public GVAR GetBasicVehicle()
        {
            string query = @"
            SELECT
                VI.""ID"",
                VI.""VehicleID"",
                V.""VehicleNumber"",
                VI.""DriverID"",
                D.""DriverName"",
                VI.""VehicleMake"",
                VI.""VehicleModel"",
                VI.""PurchaseDate""
            FROM
                ""VehiclesInformations"" VI
            JOIN
                ""Vehicles"" V ON VI.""VehicleID"" = V.""VehicleID""
            JOIN
                ""Driver"" D ON VI.""DriverID"" = D.""DriverID"";
            ";

            DataTable basicVehicleData = new DataTable();
            using (var reader = _dbConnection.ExecuteReader(query))
            {
                basicVehicleData.Load(reader);
            }

            DataTable stringBasicVehiclesData = basicVehicleData.Clone();
            foreach (DataRow row in basicVehicleData.Rows)
            {
                string id = row["ID"].ToString();
                string vehicleId = row["VehicleID"].ToString();
                string vehicleNumber = row["VehicleNumber"] == DBNull.Value ? null : row["VehicleNumber"].ToString();
                string driverId = row["DriverID"].ToString();
                string driverName = row["DriverName"] == DBNull.Value ? null : row["DriverName"].ToString();
                string vehicleMake = row["VehicleMake"] == DBNull.Value ? null : row["VehicleMake"].ToString();
                string vehicleModel = row["VehicleModel"] == DBNull.Value ? null : row["VehicleModel"].ToString();
                string purchaseDate = row["PurchaseDate"].ToString();

                stringBasicVehiclesData.Rows.Add(id, vehicleId, vehicleNumber, driverId, driverName, vehicleMake, vehicleModel, purchaseDate);
            }

            GVAR gvar = new GVAR();
            gvar.DicOfDT["VehicleInformation"] = stringBasicVehiclesData;

            return gvar;
        }

        public GVAR GetVehiclesInformation()
        {
            string query = @"
            SELECT
                V.""VehicleID"",
                V.""VehicleNumber"",
                V.""VehicleType"",
                RH.""VehicleDirection"" AS ""LastDirection"",
                RH.""Status"" AS ""LastStatus"",
                RH.""Address"" AS ""LastAddress"",
                RH.""Latitude"" AS ""LastLatitude"",
                RH.""Longitude"" AS ""LastLongitude""
            FROM
                ""Vehicles"" V
            JOIN
                (
                    SELECT
                        ""VehicleID"",
                        MAX(""Epoch"") AS ""MaxEpoch""
                    FROM
                        ""RouteHistory""
                    GROUP BY
                        ""VehicleID""
                ) MaxEpochs ON V.""VehicleID"" = MaxEpochs.""VehicleID""
            JOIN
                ""RouteHistory"" RH ON V.""VehicleID"" = RH.""VehicleID"" AND RH.""Epoch"" = MaxEpochs.""MaxEpoch""
            ORDER BY
                V.""VehicleID"";";


            DataTable vehiclesData = new DataTable();
            using (var reader = _dbConnection.ExecuteReader(query))
            {
                vehiclesData.Load(reader);
            }

            DataTable stringVehiclesData = vehiclesData.Clone();
            foreach (DataRow row in vehiclesData.Rows)
            {
                string lastDirection = row["LastDirection"] == DBNull.Value ? null : row["LastDirection"].ToString();
                stringVehiclesData.Rows.Add(row["VehicleID"], row["VehicleNumber"], row["VehicleType"],
                                             lastDirection, row["LastStatus"], row["LastAddress"],
                                             row["LastLatitude"], row["LastLongitude"]);
            }

            GVAR gvar = new GVAR();
            gvar.DicOfDT["VehicleInformation"] = stringVehiclesData;

            return gvar;
        }



        public GVAR GetDetailedVehicleInformation(GVAR data)
        {
            var vehicleId = Convert.ToInt64(data.DicOfDic["Tags"]["VehicleID"]);
            string query = $@"SELECT V.""VehicleNumber"", V.""VehicleType"", D.""DriverName"", D.""PhoneNumber"",
            CONCAT(R.""Latitude"", ',', R.""Longitude"") AS ""LastPosition"", VI.""VehicleMake"", VI.""VehicleModel"", R.""Epoch"" AS ""LastGPSTime"", R.""VehicleSpeed"" AS ""LastGPSSpeed"",
            R.""Address"" AS ""LastAddress"" FROM ""Vehicles"" V JOIN ""VehiclesInformations"" VI ON V.""VehicleID"" = VI.""VehicleID""
            JOIN ""Driver"" D ON VI.""DriverID"" = D.""DriverID"" JOIN (
            SELECT ""VehicleID"", ""Latitude"", ""Longitude"", ""Epoch"", ""VehicleSpeed"", ""Address""
            FROM ""RouteHistory"" WHERE ""VehicleID"" = {vehicleId}
            ORDER BY ""Epoch"" DESC LIMIT 1 ) R ON V.""VehicleID"" = R.""VehicleID""";

            DataTable detailedVehicleData = new DataTable();
            using (var reader = _dbConnection.ExecuteReader(query)) { detailedVehicleData.Load(reader); }

            DataTable stringDetailedVehicleData = detailedVehicleData.Clone();
            foreach (DataRow row in detailedVehicleData.Rows)
            {
                string vehicleNumber = row["VehicleNumber"] == DBNull.Value ? null : row["VehicleNumber"].ToString();
                string vehicleType = row["VehicleType"] == DBNull.Value ? null : row["VehicleType"].ToString();
                string driverName = row["DriverName"] == DBNull.Value ? null : row["DriverName"].ToString();
                string phoneNumber = row["PhoneNumber"] == DBNull.Value ? null : row["PhoneNumber"].ToString();
                string lastPosition = row["LastPosition"] == DBNull.Value ? null : row["LastPosition"].ToString();
                string vehicleMake = row["VehicleMake"] == DBNull.Value ? null : row["VehicleMake"].ToString();
                string vehicleModel = row["VehicleModel"] == DBNull.Value ? null : row["VehicleModel"].ToString();
                string lastGPSTime = row["LastGPSTime"] == DBNull.Value ? null : row["LastGPSTime"].ToString();
                string lastGPSSpeed = row["LastGPSSpeed"] == DBNull.Value ? null : row["LastGPSSpeed"].ToString();
                string lastAddress = row["LastAddress"] == DBNull.Value ? null : row["LastAddress"].ToString();

                stringDetailedVehicleData.Rows.Add(vehicleNumber, vehicleType, driverName, phoneNumber,
                                                   lastPosition, vehicleMake, vehicleModel,
                                                   lastGPSTime, lastGPSSpeed, lastAddress);
            }

            GVAR Gvar = new GVAR();
            Gvar.DicOfDT["DetailedVehicleInformation"] = stringDetailedVehicleData;

            string gvarJson = JsonConvert.SerializeObject(Gvar);
            Console.WriteLine(gvarJson);

            return Gvar;
        }

    }
}
