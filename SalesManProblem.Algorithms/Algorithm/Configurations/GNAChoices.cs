using SalesManProblem.Algorithms.GNA;

namespace SalesManProblem.Algorithms.Configurations
{

    /// <summary>
    /// type used to pass GNA Configuration Choices to the <see cref="GNAConfigurations.Configure"/>
    /// </summary>
    public class GNAChoices
    {
        public GNAChoices(FitnessChoice fitnessChoice, ElitismChoice elitismChoice, SelectionChoice selectionChoice, CrossOverChoice crossoverChoice, MutationChoice mutationChoice)
        {
            //InitGenerationChoice = initGenerationChoice;
            FitnessChoice = fitnessChoice;
            ElitismChoice = elitismChoice;
            SelectionChoice = selectionChoice;
            CrossOverChoice = crossoverChoice;
            MutationChoice = mutationChoice;
        }

        public GNAChoices()
        {

        }

        //public InitGenerationChoice InitGenerationChoice { get; init; }
        public FitnessChoice FitnessChoice { get; init; }
        public ElitismChoice ElitismChoice { get; init; }
        public SelectionChoice SelectionChoice { get; init; }
        public CrossOverChoice CrossOverChoice { get; init; }
        public MutationChoice MutationChoice { get; init; }




    }







}
