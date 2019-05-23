using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Persistence;
using System.Net;
using HtmlAgilityPack;
using Common;

namespace Domain
{
    public enum Station
    {
        WroclawMuchobor = 5104130,
        WroclawGlowny = 5100069
    }
    public class ConnectionPeeker
    {
        public enum Routes
        {
            GlownyToMuchobor,
            MuchoborToGlowny
        }
        const string parameterFromStation = "REQ0JourneyStopsS0G";
        const string parameterToStation = "REQ0JourneyStopsZ0G";
        const string parameterDate = "date"; //dd.mm.yy 
        const string parameterTime = "time"; //hh:mm
        const string wroclawMuchoborId = "5104130";
        const string wroclawGlownyId = "5100069";

        private string ConnectionString = "";
        private string InitialQuery = "";
        private Station FromStation;
        private Station ToStation;

        public ConnectionPeeker()
        {
            ConnectionString = InitialQuery = ConnectionDetails.GetInitialDetails();
        }
        public List<Time> GetTimes(Routes route, DateTime dateTime)
        {
            switch (route)
            {
                case Routes.GlownyToMuchobor:
                    ToWroclawMuchobor(dateTime);
                    break;
                case Routes.MuchoborToGlowny:
                    ToWroclawGlowny(dateTime);
                    break;
                default:
                    break;
            }
            return runHtmlQuery();
        }
        private List<Time> runHtmlQuery()
        {
            using (WebClient client = new WebClient())
            {
                try
                {
                    string htmlCode = client.DownloadString(connectionString);
                    List<Time> parsedResult = parseHtmlResults(htmlCode);
                    return parsedResult;
                }
                catch (Exception e)
                {
                    throw new Exception($"Error when quering PKP page {e.StackTrace}");
                }

            }
        }

        private List<Time> parseHtmlResults(string htmlCode)
        {
            try
            {
                //<td data-value="19041423:58" />
                List<Time> times = new List<Time>();
                var htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(htmlCode);
                var selectedNodes = htmlDocument.DocumentNode.SelectNodes("//td[@data-value]");
                foreach (var node in selectedNodes)
                {
                    string dateTime = node.Attributes["data-value"].Value;
                    if (tryFindTime(dateTime, out Time time))
                    {
                        times.Add(time);
                    }
                }
                return times;
            }
            catch (Exception e)
            {
                throw new Exception($"Error when parsing result of HTML query {Environment.NewLine}{e.StackTrace}");
            }
        }
        public bool tryFindTime(string time, out Time parsedTime)
        {
            if (time != null && time.Trim().Length == 11)
            {
                var potentialDate = time.Substring(6, 5);
                var hourAndMinutes = potentialDate.Split(':');
                if (hourAndMinutes.Length == 2)
                {
                    parsedTime = new Time(int.Parse(hourAndMinutes[0]), int.Parse(hourAndMinutes[1]));
                    return true;
                }
            }
            parsedTime = new Time();
            return false;
        }

        public void ToWroclawGlowny(DateTime dateTime)
        {
            ConnectionString = InitialQuery;
            setFromStation(Station.WroclawMuchobor);
            setTostation(Station.WroclawGlowny);
            setDateTime(dateTime);
        }

        public void ToWroclawMuchobor(DateTime dateTime)
        {
            ConnectionString = InitialQuery;
            setFromStation(Station.WroclawGlowny);
            setTostation(Station.WroclawMuchobor);
            setDateTime(dateTime);
        }

        public void setFromStation(Station stationId)
        {
            addParameter(parameterFromStation, (int) stationId);
            FromStation = stationId;
        }
        public void setTostation(Station stationId)
        {
            addParameter(parameterToStation, (int) stationId);
            ToStation = stationId;
        }
        public void setDateTime(DateTime dateTime)
        {
            string formattedYear = dateTime.Year.ToString()[2] + "" + dateTime.Year.ToString()[3];
            addParameter(parameterDate, $"{dateTime.Day}.{dateTime.Month}.{formattedYear}");
            addParameter(parameterTime, $"{dateTime.Hour}:{dateTime.Minute}");
        }

        void addParameter(string parameterName, string parameterBody)
        {
            if (ConnectionString == "")
            {
                throw new Exception("Please fill initial connection string first !");
            }
            if (!ConnectionString.EndsWith("&"))
            {
                ConnectionString += "&";
            }
            ConnectionString += $"{parameterName}={parameterBody}";
        }
        void addParameter(string parameterName, int parameterBody)
        {
            addParameter(parameterName, parameterBody.ToString());
        }
    }
}
