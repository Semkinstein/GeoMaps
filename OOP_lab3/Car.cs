using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Windows.Controls;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using GMap.NET;
using GMap.NET.WindowsPresentation;

namespace OOP_lab3
{
    class Car : MapObject
    {
        private PointLatLng point;

        public Car(string title, PointLatLng point) : base(title)
        {
            this.point = point;
        }

        public override double getDistance(PointLatLng p2)
        {
            GeoCoordinate c1 = new GeoCoordinate(point.Lat, point.Lng);
            GeoCoordinate c2 = new GeoCoordinate(p2.Lat, p2.Lng);
            
            double distance = c1.GetDistanceTo(c2);
            return distance;
        }

        public override PointLatLng getFocus()
        {
            return point;
        }

        public override GMapMarker getMarker()
        {
            GMapMarker marker = new GMapMarker(point)
            {
                Shape = new Image
                {
                    Width = 40, 
                    Height = 32, 
                    ToolTip = this.getTitle(), 
                    Source = new BitmapImage(new Uri("pack://application:,,,/Resources/car.png")) 
                }
            };
            marker.Position = point;
            return marker;
        }
    }
}
