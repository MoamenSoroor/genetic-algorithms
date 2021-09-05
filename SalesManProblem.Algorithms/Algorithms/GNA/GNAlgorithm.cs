using Ardalis.GuardClauses;
using SalesManProblem.Algorithms.Algorithms.GNA;
using SalesManProblem.Algorithms.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;

namespace SalesManProblem.Algorithms.GNA
{

    public class GNAlgorithm
    {

        private readonly Func<Map,GNAResult> execute;
        
        public GNAlgorithm(GNAChoices choices,GNAOptions options)
        {

            execute = GNAConfigurations.Configure(choices, options);
        }

        public GNAResult Execute(Map map)
        {
            return execute(map);
                
        }


    }


    


}
