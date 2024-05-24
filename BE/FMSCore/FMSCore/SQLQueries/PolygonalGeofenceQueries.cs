using FPro;
using Npgsql;
using FMSCore;
using System.Data;
using Newtonsoft.Json;
using System.Linq;
using System;

namespace FMSCore.SQLqueries
{
    public class PolygonalGeofenceQueries
    {
        private readonly DatabaseConnection _dbConnection;

        public PolygonalGeofenceQueries() {
            _dbConnection = new DatabaseConnection();
        }

        public GVAR RetrievePolygonalGeofencesCoordinates() {
            string query = "SELECT \"GeofenceID\", \"Latitude\", \"Longitude\" FROM \"PolygonGeofence\"";

            DataTable polygonalGeofencesData = new DataTable();
            using (var reader = _dbConnection.ExecuteReader(query)) { polygonalGeofencesData.Load(reader); }

            DataTable stringPolygonalGeofencesData = polygonalGeofencesData.Clone();

            foreach (DataRow row in polygonalGeofencesData.Rows) {
                stringPolygonalGeofencesData.Rows.Add(row.ItemArray.Select(item => item.ToString()).ToArray());
            }

            GVAR Gvar = new GVAR();
            Gvar.DicOfDT["PolygonalGeofences"] = stringPolygonalGeofencesData;

            string gvarJson = JsonConvert.SerializeObject(Gvar);
            Console.WriteLine(gvarJson);

            return Gvar;
        }

    }
}
