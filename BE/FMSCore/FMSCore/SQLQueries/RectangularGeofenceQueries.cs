using FPro;
using Npgsql;
using FMSCore;
using System.Data;
using Newtonsoft.Json;
using System.Linq;
using System;

namespace FMSCore.SQLqueries
{
    public class RectangularGeofenceQueries
    {
        private readonly DatabaseConnection _dbConnection;

        public RectangularGeofenceQueries()
        {
            _dbConnection = new DatabaseConnection();
        }

        public GVAR RetrieveRectangularGeofencesCoordinates() {
            string query = "SELECT \"GeofenceID\", \"North\", \"East\", \"West\", \"South\" FROM \"RectangleGeofence\"";

            DataTable rectangularGeofencesData = new DataTable();
            using (var reader = _dbConnection.ExecuteReader(query)) { rectangularGeofencesData.Load(reader); }

            DataTable stringRectangularGeofencesData = rectangularGeofencesData.Clone();

            foreach (DataRow row in rectangularGeofencesData.Rows) { 
                stringRectangularGeofencesData.Rows.Add(row.ItemArray.Select(item => item.ToString()).ToArray()); 
            }
            GVAR Gvar = new GVAR();
            Gvar.DicOfDT["RectangularGeofences"] = stringRectangularGeofencesData;

            string gvarJson = JsonConvert.SerializeObject(Gvar);
            Console.WriteLine(gvarJson);

            return Gvar;
        }

    }
}
