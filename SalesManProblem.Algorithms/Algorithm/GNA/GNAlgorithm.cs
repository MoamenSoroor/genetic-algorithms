using Ardalis.GuardClauses;
using SalesManProblem.Algorithms.Algorithms.GNA;
using SalesManProblem.Algorithms.Configurations;
using SalesManProblem.Algorithms.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;

namespace SalesManProblem.Algorithms.GNA
{
    /// <summary>
    /// a type that contains the Configured Execute Function that is a result of 
    /// the configuration process <see cref="GNAConfigurations.Configure"/>
    /// </summary>
    public class GNAlgorithm
    {

        private readonly Func<Map,GNAResult> execute;


        /// <summary>
        /// constructor that is used get the configured execute function
        /// </summary>
        /// <param name="choices">the Choices that are used to configure the algorithm</param>
        /// <param name="options">the algorithm options that are used to configure the algorithm</param>
        public GNAlgorithm(GNAChoices choices,GNAOptions options)
        {

            execute = GNAConfigurations.Configure(choices, options);
        }


        /// <summary>
        /// the Execute Function that is used to execute Genetic Algorithm over the passed Map
        /// it returns <see cref="GNAResult"/> that encapsulate the best path and another logs.
        /// </summary>
        /// <param name="map">the map to </param>
        /// <returns></returns>
        public GNAResult Execute(Map map)
        {
            return execute(map);
                
        }


    }


    


}
