using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Persistence;

namespace Domain
{
    public class ConnectionPeeker
    {
        const string parameterFromStation = "REQ0JourneyStopsS0G";
        const string parameterToStation = "REQ0JourneyStopsZ0G";
        const string parameterDate = "date"; //dd.mm.yy 
        const string parameterTime = "time"; //hh:mm
        const string wroclawMuchoborId = "5104130";
        const string wroclawGlownyId = "5100069";

        private string connectionString = "";
        private string initialQuery = "";

        public ConnectionPeeker()
        {
            connectionString = initialQuery = ConnectionDetails.GetInitialDetails();
        }
        public void ToWroclawGlowny(DateTime dateTime)
        {
            connectionString = initialQuery;
            setFromStation(wroclawMuchoborId);
            setTostation(wroclawGlownyId);
            setDateTime(dateTime);
        }

        public void ToWroclawMuchobor(DateTime dateTime)
        {
            connectionString = initialQuery;
            setFromStation(wroclawGlownyId);
            setTostation(wroclawMuchoborId);
            setDateTime(dateTime);
        }

        public void setFromStation(string stationId)
        {
            addParameter(parameterFromStation, stationId);
        }
        public void setTostation(string stationId)
        {
            addParameter(parameterToStation, stationId);
        }
        public void setDateTime(DateTime dateTime)
        {
            string formattedYear = dateTime.Year.ToString()[2] +""+ dateTime.Year.ToString()[3];
            addParameter(parameterDate, $"{dateTime.Day}.{dateTime.Month}.{formattedYear}");
            addParameter(parameterTime, $"{dateTime.Hour}:{dateTime.Minute}");
        }

        void addParameter(string parameterName, string parameterBody)
        {
            if (connectionString == "")
            {
                throw new Exception("Please fill initial connection string first !");
            }
            if (!connectionString.EndsWith("&"))
            {
                connectionString += "&";
            }
            connectionString += $"{parameterName}={parameterBody}";
        }
    }
}
