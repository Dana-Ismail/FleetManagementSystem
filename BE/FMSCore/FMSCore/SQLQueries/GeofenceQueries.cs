using System;
using FPro;
using System.Data;
using Npgsql;
using FMSCore;
using System.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace FMSCore.SQLqueries
{
    public class GeofenceQueries
    {
        private readonly DatabaseConnection _dbConnection;

        public GeofenceQueries() {
            _dbConnection = new DatabaseConnection();
        }

        public GVAR GetGeofenceInformation() {
            string query = "SELECT * FROM \"Geofences\"";
            NpgsqlDataReader reader = _dbConnection.ExecuteReader(query);
            DataTable geofencesData = new DataTable();
            geofencesData.Load(reader);

            DataTable stringGeofencesData = geofencesData.Clone();
            foreach (DataRow row in geofencesData.Rows)
            { stringGeofencesData.Rows.Add(row.ItemArray.Select(item => item.ToString()).ToArray()); }

            GVAR Gvar = new GVAR();
            Gvar.DicOfDT["Geofences"] = stringGeofencesData;

            string gvarJson = JsonConvert.SerializeObject(Gvar);
            Console.WriteLine(gvarJson);

            return Gvar;
        }
        

    }

}
