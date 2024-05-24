using FPro;
using Newtonsoft.Json;
using Npgsql;
using System;
using System.Data;
using System.Linq;

namespace FMSCore.SQLqueries
{
    public class CircularGeofenceQueries
    {
        private readonly DatabaseConnection _dbConnection;

        public CircularGeofenceQueries() {
            _dbConnection = new DatabaseConnection();
        }

        public GVAR RetrieveCircularGeofencesCoordinates() {
            string query = "SELECT \"GeofenceID\", \"Radius\", \"Latitude\", \"Longitude\" FROM \"CircleGeofence\"";

            DataTable circularGeofencesData = new DataTable();
            using (var reader = _dbConnection.ExecuteReader(query)) { circularGeofencesData.Load(reader); }

            DataTable stringCircularGeofencesData = circularGeofencesData.Clone();

            foreach (DataRow row in circularGeofencesData.Rows)
            { stringCircularGeofencesData.Rows.Add(row.ItemArray.Select(item => item.ToString()).ToArray()); }

            GVAR Gvar = new GVAR();
            Gvar.DicOfDT["CircularGeofences"] = stringCircularGeofencesData;

            string gvarJson = JsonConvert.SerializeObject(Gvar);
            Console.WriteLine(gvarJson);

            return Gvar;
        }

    }
}
