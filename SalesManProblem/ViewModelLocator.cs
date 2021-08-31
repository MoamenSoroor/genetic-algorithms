using SalesManProblem.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesManProblem
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {

        }

        private IServiceProvider services = App.Current.Services;

        public MainWindowViewModel MainWindowVM { get => services.GetService<MainWindowViewModel>(); }
        
        //public MapCanvasViewModel MapCanvasVM { get => services.GetService<MapCanvasViewModel>(); }
    
        
    }
}
