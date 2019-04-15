using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Persistence;
using System.Net;
using System.Xml;
using HtmlAgilityPack;

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
        public string RunQuery()
        {
            using (WebClient client = new WebClient())
            {
                try
                {
                    string htmlCode = client.DownloadString(connectionString);
                    string parsedResult = parseHtmlResults(htmlCode);
                    return parsedResult;
                }
                catch (Exception e)
                {
                    throw new Exception($"Error when quering PKP page {e.StackTrace}");
                }

            }
        }

        private string parseHtmlResults(string htmlCode)
        {
            try
            {
                //<td data-value="19041423:58" />
                string result = $"Result of parsing: {Environment.NewLine}";
                var htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(htmlCode);
                var selectedNodes = htmlDocument.DocumentNode.SelectNodes("//td[@data-value]");
                foreach (var node in selectedNodes)
                {
                    result += $"{node.Attributes["data-value"].Value} {Environment.NewLine}";
                }
                return result;
            }
            catch (Exception e)
            {
                throw new Exception($"Error when parsing result of HTML query {Environment.NewLine}{e.StackTrace}");
            }


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
            string formattedYear = dateTime.Year.ToString()[2] + "" + dateTime.Year.ToString()[3];
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
