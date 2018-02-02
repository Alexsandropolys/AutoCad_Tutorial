using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ConsoleApi
{
    public class Getter
    {
       const string RequestBeginString = "http://apilayer.net/api/historical?access_key=e5d34f2b8f962721980f27abb0d02313&source=USD&date=";
       public static double GetValue(DateTime date)
        {
            string line = string.Empty;
            string dateForRequest = date.ToString("yyyy-MM-dd");
            WebRequest request = WebRequest.Create(RequestBeginString + dateForRequest);
            WebResponse response = request.GetResponse();
            Stream stream = response.GetResponseStream();
            using (StreamReader sr = new StreamReader(stream))
            {
                line = sr.ReadToEnd();
            }
            JObject resp = JsonConvert.DeserializeObject(line) as JObject;
            double usd = Convert.ToDouble(resp["quotes"]["USDALL"]);

            return usd;
        }
        
        public static double[] RequestForMonth(DateTime date)
        {
            double[] result = new double[25];
            for(int i = 0; i < 25; i++)
            {
                DateTime currentDate = date.AddDays(i);
                result[i] = GetValue(currentDate);
            }
            return result;
        }
    }
}
