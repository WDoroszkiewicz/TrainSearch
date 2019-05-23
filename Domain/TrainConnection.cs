using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class TrainConnection
    {
        public Station From { get; set; }
        public Station To { get; set; }
        public DateTime When { get; set; }

        public TrainConnection(Station From, Station To, DateTime When)
        {
            this.From = From;
            this.To = To;
            this.When = When;
        }
        /// <param name="When"> Format: YYMMDDHH:MM</param>
        public TrainConnection(Station From, Station To, string When)
        {
            this.From = From;
            this.To = To;
            parseWhen(When);
        }
        public override string ToString()
        {
            return $"From: {From} To: {To} When: {When.ToShortDateString()}, {When.ToShortTimeString()}";
        }


        //#TODO REFRACTOR ME
        private void parseWhen(string when)
        {
            try
            {
                When = new DateTime(2_000 + int.Parse(when.Substring(0, 2)), //YY
                    int.Parse(when.Substring(2, 2)), //MM
                    int.Parse(when.Substring(4, 2)), //DD
                    int.Parse(when.Substring(6, 2)), //HH
                    int.Parse(when.Substring(9, 2)), //MM
                    0); //SS
            }
            catch (Exception e)
            {
                throw new Exception($"Unable to parse When string {Environment.NewLine} {e.Message}" +
                    $" {Environment.NewLine} {e.StackTrace}");
            }            
        }
    }
}
