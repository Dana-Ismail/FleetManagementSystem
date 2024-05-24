using FPro;
using Npgsql;
using System;
using System.Data;
using System.Linq;
using Newtonsoft.Json;

namespace FMSCore.SQLqueries
{
    public class DriverQueries
    {
        private readonly DatabaseConnection _dbConnection;

        public DriverQueries()
        {
            _dbConnection = new DatabaseConnection();
        }

        public GVAR GetDrivers()
        {
            string query = "SELECT * FROM \"Driver\"";
            NpgsqlDataReader reader = _dbConnection.ExecuteReader(query);
            DataTable drivers = new DataTable();
            drivers.Load(reader);

            DataTable stringDriversData = drivers.Clone();
            foreach (DataRow row in drivers.Rows)
            { stringDriversData.Rows.Add(row.ItemArray.Select(item => item.ToString()).ToArray()); }

            GVAR Gvar = new GVAR();
            Gvar.DicOfDT["Drivers"] = stringDriversData;

            string gvarJson = JsonConvert.SerializeObject(Gvar);
            Console.WriteLine(gvarJson);

            return Gvar;
        }

        public GVAR AddDriver(GVAR data)
        {
            var driverName = data.DicOfDic["Tags"]["DriverName"];
            var phoneNumber = Convert.ToInt64(data.DicOfDic["Tags"]["PhoneNumber"]);

            string query = $"INSERT INTO \"Driver\" (\"DriverName\", \"PhoneNumber\") VALUES ('{driverName}', {phoneNumber})";
            Console.WriteLine("Success");
            _dbConnection.ExecuteNonQuery(query);
            data.DicOfDic["Tags"]["STS"] = "1";
            return data;
        }

        public GVAR UpdateDriver(GVAR data)
        {
            var driverId = Convert.ToInt64(data.DicOfDic["Tags"]["DriverID"]);
            var driverName = data.DicOfDic["Tags"]["DriverName"];
            var phoneNumber = Convert.ToInt64(data.DicOfDic["Tags"]["PhoneNumber"]);
            string query = $"UPDATE \"Driver\" SET \"DriverName\" = '{driverName}', \"PhoneNumber\" = {phoneNumber} WHERE \"DriverID\" = {driverId}";
            _dbConnection.ExecuteNonQuery(query);
            data.DicOfDic["Tags"]["STS"] = "1";
            return data;
        }

        public GVAR DeleteDriver(GVAR data)
        {
            var driverId = Convert.ToInt64(data.DicOfDic["Tags"]["DriverID"]);
            // for reseting id
            string checkSequenceQuery = @"
            DO $$
            BEGIN
                IF NOT EXISTS (SELECT 1 FROM pg_class WHERE relname = 'temp_driverid_seq') THEN
                    CREATE SEQUENCE temp_driverid_seq START 1;
                END IF;
            END
            $$;
            ";
            _dbConnection.ExecuteNonQuery(checkSequenceQuery);
            string renumberDriversQuery = @"
            WITH numbered_drivers AS (
                SELECT ""DriverID"", nextval('temp_driverid_seq') as new_id
                FROM ""Driver""
                ORDER BY ""DriverID""
            )
            UPDATE ""Driver""
            SET ""DriverID"" = nd.new_id
            FROM numbered_drivers nd
            WHERE ""Driver"".""DriverID"" = nd.""DriverID"";
            ";
            _dbConnection.ExecuteNonQuery(renumberDriversQuery);
            string dropSequenceQuery = "DROP SEQUENCE IF EXISTS temp_driverid_seq;";
            _dbConnection.ExecuteNonQuery(dropSequenceQuery);
            string deleteDriverQuery = $"DELETE FROM \"Driver\" WHERE \"DriverID\" = {driverId};";
            _dbConnection.ExecuteNonQuery(deleteDriverQuery);
            data.DicOfDic["Tags"]["STS"] = "1";
            return data;
        }

    }
}