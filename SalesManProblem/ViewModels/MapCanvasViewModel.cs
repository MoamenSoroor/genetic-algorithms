using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Messaging;
using SalesManProblem.Algorithms;
using SalesManProblem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SalesManProblem.ViewModels
{
    //public class MapCanvasViewModel : ObservableRecipient
    //{

    //    public MapCanvasViewModel()
    //    {
    //        WeakReferenceMessenger.Default
    //            .Register<List<MapCity>>(this, HandleCitites);


    //        WeakReferenceMessenger.Default
    //            .Register<MapPath>(this, HandlePath);
    //    }

    //    private void HandlePath(object recipient, MapPath message)
    //    {
    //        MessageBox.Show("recieved path");
    //        //throw new NotImplementedException();
    //    }

    //    private void HandleCitites(object recipient, List<MapCity> message)
    //    {

    //        //MessageBox.Show("recieved map city");

    //        var result = message.Select(v =>
    //        {
    //            var el = new Ellipse() { Width = 5, Height = 5, Fill = Brushes.Green };
    //            Canvas.SetLeft(el, v.Position.X);
    //            Canvas.SetTop(el, v.Position.Y);
    //            return el;
    //        });
            

    //    }

    //}
}
