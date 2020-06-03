using BarFinder.Models;
using NativeWifi;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace BarFinder
{
    class CordinateProcessor
    {
        private const string GEOLOCATION_API_HOST = "https://api.mylnikov.org/geolocation/wifi?bssid=";
        private const string BAR_DATABASE_PATH = @"C:\features.json";

        private static Institutions DeserializeJsonFile()
        {
            string json = File.ReadAllText(BAR_DATABASE_PATH);
            return JsonConvert.DeserializeObject<Institutions>(json);
        }

        private static List<Point> GetBarPoints(Institutions barCoordinates)
        {
            List<Point> point = new List<Point>();
            foreach (Feature feature in barCoordinates.Features)
            {
                if (feature.Properties.Attributes.TypeObject.Equals("бар"))
                {
                    point.Add(new Point()
                    {
                        Name = feature.Properties.Attributes.Name,
                        X = feature.Geometry.Coordinates[1],
                        Y = feature.Geometry.Coordinates[0]
                    });
                }
            }
            return point;
        }

        private static MyLocation GetUserCoordinates()
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
