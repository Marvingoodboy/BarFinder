using BarFinder.Models;
using GMap.NET;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Drawing;
using System.Windows.Forms;

namespace BarFinder
{
    public partial class MapProcessor : Form
    {
        private double userX;
        private double userY;

        private List<Point> barCoordinates;

        public MapProcessor()
        {
            InitializeComponent();
            MyLocation currentLocation = CoordinateProcessor.GetUserCoordinates();

            this.userX = currentLocation.Data.Lat;
            this.userY = currentLocation.Data.Lon;

            List<Institutions> dataFromBarDataBase = CoordinateProcessor.DeserializeJsonFile();

            this.barCoordinates = CoordinateProcessor.GetBarPoints(dataFromBarDataBase);
        }

        private void gMapControl1_Load(object sender, EventArgs e)
        {
            ////HardCode user coordinates
            userX = 55.752160;
            userY = 37.598279;

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

            PutBarMarkersOnMap(barCoordinates);

            PutUserMarkerOnMap();
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
