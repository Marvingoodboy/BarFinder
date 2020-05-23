using BarFinder.Models;
using GMap.NET;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using NativeWifi;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Drawing;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace BarFinder
{
    public partial class Form1 : Form
    {
        private double userX = 0;
        private double userY = 0;
        private const string GEOLOCATION_API_HOST = "https://api.mylnikov.org/geolocation/wifi?bssid=";

        public Form1()
        {
            InitializeComponent();
        }

        private void gMapControl1_Load(object sender, EventArgs e)
        {

            GetUserCoordinates();
            ////HardCode user coordinates
            //userX = 0;
            //userY = 0;

            //GMap settings
            gmap.Bearing = 0;
            gmap.CanDragMap = true;
            gmap.DragButton = MouseButtons.Left;
            gmap.GrayScaleMode = true;
            gmap.MarkersEnabled = true;
            gmap.MaxZoom = 18;
            gmap.MinZoom = 2;
            gmap.MouseWheelZoomType = GMap.NET.MouseWheelZoomType.MousePositionWithoutCenter;
            gmap.NegativeMode = false;
            gmap.PolygonsEnabled = true;
            gmap.RoutesEnabled = true;
            gmap.ShowTileGridLines = false;
            gmap.Zoom = 16;

            gmap.MapProvider = GMap.NET.MapProviders.GMapProviders.GoogleMap;
            GMap.NET.GMaps.Instance.Mode = GMap.NET.AccessMode.ServerOnly;
            gmap.Position = new GMap.NET.PointLatLng(userX, userY);

            var point = DeserializeJsonFile();

            PutBarMarkersOnMap(point);

            PutUserMarkerOnMap();
        }
        private void GetUserCoordinates()
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
            var test = JsonConvert.DeserializeObject<MyLocation>(coodrinates);
            userX = test.Data.Lat;
            userY = test.Data.Lon;
        }
        private List<Point> DeserializeJsonFile()
        {
            string json = File.ReadAllText(@"C:\features.json");
            var bar = JsonConvert.DeserializeObject<Institutions>(json);
            List<Point> point = new List<Point>();
            foreach (Feature feature in bar.Features)
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
        private void PutBarMarkersOnMap(List<Point> point)
        {
            GMapOverlay markersOverlay = new GMapOverlay("Bar");
            foreach (Point p in point)
            {
                GeoCoordinate P1 = new GeoCoordinate(userX, userY);
                GeoCoordinate P2 = new GeoCoordinate(p.X, p.Y);
                double distanse = P1.GetDistanceTo(P2);
                distanse = Math.Ceiling(distanse);
                if (distanse < 300)
                {
                    GMarkerGoogle marker = new GMarkerGoogle(new PointLatLng(p.X, p.Y), new Bitmap(Properties.Resources.yes));
                    marker.ToolTip = new GMap.NET.WindowsForms.ToolTips.GMapRoundedToolTip(marker);
                    marker.ToolTipText = p.Name;
                    markersOverlay.Markers.Add(marker);

                }
                else
                {
                    GMarkerGoogle marker = new GMarkerGoogle(new PointLatLng(p.X, p.Y), new Bitmap(Properties.Resources.no));
                    marker.ToolTip = new GMap.NET.WindowsForms.ToolTips.GMapRoundedToolTip(marker);
                    marker.ToolTipText = p.Name;
                    markersOverlay.Markers.Add(marker);
                }
            }
            gmap.Overlays.Add(markersOverlay);
        }
        private void PutUserMarkerOnMap()
        {
            GMapOverlay markersOverlay = new GMapOverlay("Drunk Man");
            GMarkerGoogle UserMarker = new GMarkerGoogle(new PointLatLng(userX, userY), new Bitmap(Properties.Resources.me));
            UserMarker.ToolTip = new GMap.NET.WindowsForms.ToolTips.GMapRoundedToolTip(UserMarker);
            UserMarker.ToolTipText = "Me";
            markersOverlay.Markers.Add(UserMarker);
            gmap.Overlays.Add(markersOverlay);
        }
    }
}
