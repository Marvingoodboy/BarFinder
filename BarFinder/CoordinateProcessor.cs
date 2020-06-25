using BarFinder.Models;
using NativeWifi;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace BarFinder
{
    class CoordinateProcessor
    {
        private const string GEOLOCATION_API_HOST = "https://api.mylnikov.org/geolocation/wifi?bssid=";


        public static List<Institutions> DeserializeJsonFile()
        {
            string json = new DataProcessor().UnZipToMemory();

            return JsonConvert.DeserializeObject<List<Institutions>>(json);
        }

        public static List<Point> GetBarPoints(List<Institutions> barCoordinates)
        {
            List<Point> point = new List<Point>();
            foreach (Institutions feature in barCoordinates)
            {
                if (feature.TypeObject.Equals("бар"))
                {
                    point.Add(new Point()
                    {
                        Name = feature.Name,
                        X = feature.GeoData.Coordinates[1],
                        Y = feature.GeoData.Coordinates[0]
                    });
                }
            }
            return point;
        }

        public static MyLocation GetUserCoordinates()
        {
            string[] BSSID = null;
            foreach (WlanClient.WlanInterface wlanInterface in new WlanClient().Interfaces)
            {
                foreach (Wlan.WlanBssEntry wlanBssEntry in wlanInterface.GetNetworkBssList())
                {
                    byte[] macAddr = wlanBssEntry.dot11Bssid;
                    UInt32 macAddrLen = (uint)macAddr.Length;
                    BSSID = new string[(int)macAddrLen];
                    for (int i = 0; i < macAddrLen; i++)
                    {
                        BSSID[i] = macAddr[i].ToString("x2");
                    }
                }
            }
            string coodrinates = "";
            WebRequest request = WebRequest.Create(GEOLOCATION_API_HOST + string.Join("", BSSID));
            WebResponse response = request.GetResponse();
            using (Stream stream = response.GetResponseStream())
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    coodrinates = reader.ReadLine();
                }

            }
            response.Close();
            return JsonConvert.DeserializeObject<MyLocation>(coodrinates);
        }
    }


}
