using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsPresentation;
using System.Device.Location;

namespace OOP_lab3
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MapLoaded(object sender, RoutedEventArgs e)
        {
            GMaps.Instance.Mode = AccessMode.ServerAndCache;
            Map.MapProvider = GMapProviders.GoogleMap;
            Map.MinZoom = 2;
            Map.MaxZoom = 17;
            Map.Zoom = 15;
            Map.Position = new PointLatLng(55.012823, 82.950359);

            // настройка взаимодействия с картой
         
            Map.MouseWheelZoomType = MouseWheelZoomType.MousePositionAndCenter;
            Map.CanDragMap = true;
            Map.DragButton = MouseButton.Left;
            //
            comboBox.SelectedIndex = 0;
        }

        List<MapObject> mapObjects = new List<MapObject>();
        List<PointLatLng> activePoints = new List<PointLatLng>();

        void CreateMapObjects(PointLatLng point)
        {
            if(comboBox.SelectedIndex == 0)
            {
                Car car = new Car(textBoxName.Text, point);
                mapObjects.Add(car);
                Map.Markers.Add(car.getMarker());
                activePoints.Clear();
            }
            if (comboBox.SelectedIndex == 1)
            {
                Location location = new Location(textBoxName.Text, point);
                mapObjects.Add(location);
                Map.Markers.Add(location.getMarker());
                activePoints.Clear();
            }
            if (comboBox.SelectedIndex == 2)
            {
                Human human = new Human(textBoxName.Text, point);
                mapObjects.Add(human);
                Map.Markers.Add(human.getMarker());
                activePoints.Clear();
            }
            if (comboBox.SelectedIndex == 3)
            {
                Route route = new Route(textBoxName.Text, activePoints);
                mapObjects.Add(route);
                Map.Markers.Add(route.getMarker());
                activePoints.Clear();
            }
            if (comboBox.SelectedIndex == 4)
            {
                Area area = new Area(textBoxName.Text, activePoints);
                mapObjects.Add(area);
                Map.Markers.Add(area.getMarker());
                activePoints.Clear();
            }
        }

        void SearchOnMap(PointLatLng point)
        {
            listBox.Items.Clear();
            var mapObjectsSorted = mapObjects.OrderBy(mo => mo.getDistance(point));
            foreach(MapObject mo in mapObjectsSorted)
            {
                if(mo.getTitle().Contains(textBoxSearch.Text))
                    listBox.Items.Add((int)mo.getDistance(point) + "m " + mo.getTitle() + " ");
            }
        }

        void FocusOnMarker(int selectedIndex)
        {
            Map.Position = mapObjects[selectedIndex].getFocus();
        } 
        

        private void Map_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            PointLatLng point = Map.FromLocalToLatLng((int)e.GetPosition(Map).X, (int)e.GetPosition(Map).Y);
            activePoints.Add(point);
            
        }

        private void ButtonAddOk_Click(object sender, RoutedEventArgs e)
        {
            if (radioButtonCreate.IsChecked == true)
            {
                if (activePoints.Count != 0)
                {
                    CreateMapObjects(activePoints.Last());
                }
            }
        }

        private void ButtonSearchOk_Click(object sender, RoutedEventArgs e)
        {
            if(radioButtonSearch.IsChecked == true)
            {
                if (activePoints.Count != 0)
                {
                    SearchOnMap(activePoints.Last());
                }
            }
        }

        private void ButtonAddCancel_Click(object sender, RoutedEventArgs e)
        {
            activePoints.Clear();
        }

        private void ListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            FocusOnMarker(listBox.SelectedIndex);
        }
    }
}
