// Dependencies
using System.Collections.Generic;
// Custom Dependencies
using YannickSCF.LSTournaments.Common.Models;

namespace YannickSCF.LSTournaments.Common.Tools.Poule.Builder {
    public class RandomPoulesBuilder : PoulesBuilder {
        public RandomPoulesBuilder(int pouleMaxSize) : base(pouleMaxSize) { }

        protected override List<AthleteInfoModel> GetSpecificAthletesList(List<AthleteInfoModel> athletes) {
            Randomizer.ShuffleList(athletes);
            return athletes;
        }
    }
}
