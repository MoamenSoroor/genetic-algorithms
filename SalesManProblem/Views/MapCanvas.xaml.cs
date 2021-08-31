using Microsoft.Toolkit.Mvvm.Messaging;
using SalesManProblem.Algorithms;
using SalesManProblem.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace SalesManProblem.Views
{
    /// <summary>
    /// Interaction logic for MapCanvas.xaml
    /// </summary>
    public partial class MapCanvas : UserControl
    {
        public MapCanvas()
        {
            InitializeComponent();

            WeakReferenceMessenger.Default
                .Register<List<City>>(this, HandleCitites);


            WeakReferenceMessenger.Default
                .Register<MapPath>(this, HandlePath);

        }



        private Polyline path = new Polyline();
        private List<City> cities;

        private void HandlePath(object recipient, MapPath message)
        {
            Map.Children.Clear();

            Polyline path = new Polyline();
            path.Stroke = Brushes.Black;
            path.StrokeThickness = 2;
            path.StrokeEndLineCap = PenLineCap.Round;
            path.StrokeLineJoin = PenLineJoin.Round;
            path.StrokeStartLineCap = PenLineCap.Round;

            path.Points = new PointCollection(message.Positions
                .Select(p=> new System.Windows.Point(p.X,p.Y)));

            this.Map.Children.Add(path);

            DrawCities();
        }

        private void HandleCitites(object recipient, List<City> message)
        {
            this.Map.Children.Clear();
            cities = message;
            DrawCities();
        }

        private void DrawCities()
        {
            int size = 15;
            bool first = true;
            cities.ForEach((city) =>
            {
                var el = new Ellipse() { Width = size, Height = size, Fill = first? Brushes.Red : Brushes.Green };
                Canvas.SetLeft(el, city.Position.X - size/2);
                Canvas.SetTop(el, city.Position.Y - size/2);

                var name = new TextBlock() { 
                    Text = city.Name, 
                    Foreground=Brushes.DarkBlue,
                    FontSize = 15,
                    FontWeight=FontWeights.Bold
                };
                Canvas.SetLeft(name, city.Position.X+10 );
                Canvas.SetTop(name, city.Position.Y+10 );

                this.Map.Children.Add(el);
                this.Map.Children.Add(name);
                first = false;
            });

        }

    }
}
