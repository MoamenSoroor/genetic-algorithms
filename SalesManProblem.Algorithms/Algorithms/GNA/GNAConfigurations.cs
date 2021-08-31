using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesManProblem.Algorithms.GNA
{
    public class GNAChoices
    {
        //private int iterations;
        //private int populationSize;
        //private double crossoverRate;
        //private double mutationRate;
        //private double elitismRate;

        private InitGenerationChoice initGenerationChoice;
        private FitnessChoice fitnessChoice;
        private ElitismChoice elitismChoice;
        private SelectionChoice selectionChoice;
        private CrossoverChoice crossoverChoice;
        private MutationChoice mutationChoice;





    }


    public class GNAConfigurations
    {
        //private int iterations;
        //private int populationSize;
        //private double crossoverRate;
        //private double mutationRate;
        //private double elitismRate;

        private InitGenerationChoice initGenerationChoice;
        private FitnessChoice        fitnessChoice;
        private ElitismChoice        elitismChoice;
        private SelectionChoice      selectionChoice;
        private CrossoverChoice      crossoverChoice;
        private MutationChoice       mutationChoice;


        // initGeneration Methods
        private ImmutableList<Candidate> InitRandomCandidates(int populationSize,ImmutableList<Point> allPositions)
        {
            return default;

        }



    }


    public enum InitGenerationChoice
    {
        Default
    }


    public enum FitnessChoice
    {
        Default
    }

    public enum ElitismChoice
    {
        Default
    }

    public enum SelectionChoice
    {
        Default
    }

    public enum CrossoverChoice
    {
        Default
    }

    public enum MutationChoice
    {
        Default
    }







}
