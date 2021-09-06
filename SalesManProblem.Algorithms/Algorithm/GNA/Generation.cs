using MoreLinq;
using System.Collections.Immutable;
using System.Linq;

namespace SalesManProblem.Algorithms.GNA
{
    public class Generation
    {

        public static Generation Create(ImmutableList<Candidate> candidates)
        {
            return new Generation(candidates);
        }


        private Generation(ImmutableList<Candidate> candidates)
        {
            Candidates = candidates;
            BestPathLength = (int)Candidates.Min(c => c.Path.PathLength);
            BestPath = Candidates.FirstOrDefault(v => v.Path.PathLength >= BestPathLength).Path;
        }


        public ImmutableList<Candidate> Candidates { get; }
        public int GenerationNumber { get; }
        public int BestPathLength { get; }
        public MapPath BestPath { get; }

        
    }


    


}
