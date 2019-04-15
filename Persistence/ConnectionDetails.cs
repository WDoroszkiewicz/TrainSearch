using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence
{
    public class ConnectionDetails
    {
        public static string GetInitialDetails()
        {
            string path = @"D:\_DEV\C#\MVC fundamentals\TrainSearch\Persistence\RequiredParams.txt";
            string details = "";
            try
            {
                using (StreamReader streamReader = new StreamReader(path))
                {
                    string line;
                    while ((line = streamReader.ReadLine()) != null)
                    {
                        details += details == "" || details.EndsWith("&") ? line : $"&{line}";
                    }
                }

            }
            catch (Exception e)
            {
                throw new Exception($"Unable to read file: {path}, {Environment.NewLine}{e.StackTrace}");
            }
            return details;
        }
    }
}
