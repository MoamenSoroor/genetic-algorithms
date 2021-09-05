namespace SalesManProblem.Algorithms.GNA
{


    public enum InitGenerationChoice
    {
        Default
    }


    public enum FitnessChoice
    {
        Default,
        Fitness2
    }

    public enum ElitismChoice
    {
        Default
    }

    public enum SelectionChoice
    {
        Default, // RouletteWheelSelection
        RandomSelection
    }

    public enum CrossOverChoice
    {
        Default
    }

    public enum MutationChoice
    {
        Default
    }


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
